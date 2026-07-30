using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TowBoatSalvageWebApp.Components;
using TowBoatSalvageWebApp.Components.Account;
using TowBoatSalvageWebApp.Data;
using TowBoatSalvageWebApp.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Stripe;
using Microsoft.AspNetCore.DataProtection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(o => o.DetailedErrors = true);

//Adding Service 
builder.Services.AddScoped<SalvageTableService>();
builder.Services.AddSingleton<ToastService>();

//mudblazor
builder.Services.AddMudServices();

//cache user
builder.Services.AddScoped<UserState>();

//signal r
builder.Services.AddSignalR();

//user service to keep track of names
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<Honda500HrService>();

//stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddScoped<StripePaymentService>();

builder.Services.AddScoped<VehicleInspectionService>();
builder.Services.AddScoped<VehicleInspectionPdfService>();

builder.Services.AddScoped<FuelLogService>();
builder.Services.AddScoped<WorkOrderService>();

builder.Services.AddDbContextFactory<SalvageDbContext>(
    options => options.UseSqlite("Data Source=salvage.db"),
    ServiceLifetime.Scoped);
builder.Services.AddScoped<FuelLogReportService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<DispatchAuditLogService>();

builder.Services.AddScoped<DispatchSignedPdfService>();
builder.Services.AddScoped<WorkOrderPdfService>();
builder.Services.AddScoped<DispatchEmailService>();
builder.Services.AddScoped<DispatchSigningLinkService>();
builder.Services.AddHttpClient<MailgunEmailSender>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddDbContext<SalvageDbContext>(options => options.UseSqlite("Data Source=salvage.db"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=users.db"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
 //builder.Services.AddDbContext<ApplicationDbContext>(options =>
     //options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// keep same encryption keys so that existing cookies remain valid, users wont have to sign in again after updates and redploying
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/var/www/towboatsalvage/data-protection-keys"))
    .SetApplicationName("TowBoatSalvage");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Setup Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //serve files over wwwroot


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Stripe webhook � receives payment confirmation events
app.MapPost("/webhooks/stripe", async (
    HttpRequest request,
    IConfiguration config,
    StripePaymentService paymentService,
    ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("StripeWebhook");

    // Step 1: Read the raw body � Stripe requires this for signature verification.
    var json = await new StreamReader(request.Body).ReadToEndAsync();

    // Step 2: Verify the webhook signature.
    // This ensures the request actually came from Stripe, not a bad actor.
    var endpointSecret = config["Stripe:WebhookSecret"] ?? "";
    Event stripeEvent;

    try
    {
        stripeEvent = EventUtility.ConstructEvent(
            json,
            request.Headers["Stripe-Signature"],
            endpointSecret,
            throwOnApiVersionMismatch: false);
    }
    catch (StripeException ex)
    {
        logger.LogWarning(ex, "Stripe webhook signature verification failed.");
        return Results.BadRequest("Invalid signature.");
    }

    // Step 3: Handle the event we care about.
    if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
    {
        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
        var paymentLinkId = session?.PaymentLinkId;

        logger.LogInformation("checkout.session.completed received. SessionId={SessionId}, PaymentLinkId={PaymentLinkId}", session?.Id, paymentLinkId);

        if (!string.IsNullOrEmpty(paymentLinkId))
        {
            // The PaymentLink ID connects this event back to our PaymentRequest record.
            await paymentService.MarkAsPaidAsync(paymentLinkId, session!.Id, session.PaymentIntentId);
            logger.LogInformation("Checkout session completed. PaymentLink={LinkId}", paymentLinkId);
        }
        else
        {
            logger.LogWarning("checkout.session.completed had no PaymentLinkId. SessionId={SessionId}", session?.Id);
        }
    }

    return Results.Ok();
})
.DisableAntiforgery();

app.MapGet("/reports/fuellog/{boat}/{year:int}", async (
    string boat,
    int year,
    FuelLogReportService reportService) =>
{
    var bytes = await reportService.BuildPdfAsync(boat, year);
    var fileName = $"FuelLog_{boat}_{year}.pdf";
    return Results.File(bytes, "application/pdf", fileName);
});

app.MapPost("/webhooks/mailgun/events", async (
    HttpRequest request,
    IConfiguration config,
    SalvageDbContext db,
    ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("MailgunWebhook");

    using var json = await JsonDocument.ParseAsync(request.Body);
    var root = json.RootElement;

    var signingKey = config["Mailgun:WebhookSigningKey"] ?? "";
    if (!IsValidMailgunSignature(root, signingKey))
    {
        logger.LogWarning("Rejected Mailgun webhook due to invalid signature.");
        return Results.Unauthorized();
    }

    if (!root.TryGetProperty("event-data", out var eventData))
    {
        return Results.BadRequest("Missing event-data.");
    }

    var eventName = eventData.TryGetProperty("event", out var eventProp)
        ? eventProp.GetString()
        : null;

    var eventUtc = DateTime.UtcNow;
    if (eventData.TryGetProperty("timestamp", out var tsProp) &&
        tsProp.ValueKind == JsonValueKind.Number &&
        tsProp.TryGetDouble(out var unixSeconds))
    {
        eventUtc = DateTimeOffset.FromUnixTimeMilliseconds((long)(unixSeconds * 1000)).UtcDateTime;
    }

    string? dispatchToken = null;
    if (eventData.TryGetProperty("user-variables", out var userVars) &&
        userVars.ValueKind == JsonValueKind.Object &&
        userVars.TryGetProperty("dispatch_token", out var tokenProp))
    {
        dispatchToken = tokenProp.GetString();
    }

    if (string.IsNullOrWhiteSpace(dispatchToken))
    {
        logger.LogInformation("Mailgun event ignored: no dispatch_token.");
        return Results.Ok();
    }


    //route to correct table based on the token prefix
    if (dispatchToken.StartsWith("pay_"))
    {
        //this is a payment request email
        var paymentRequest = await db.PaymentRequests
           .FirstOrDefaultAsync(x => x.EmailTrackingToken == dispatchToken);

        if (paymentRequest is null)
        {
            logger.LogInformation("Mailgun event ignored: payment token not found. Token={Token}", dispatchToken);
            return Results.Ok();
        }

        paymentRequest.EmailLastEvent = eventName;

        switch (eventName)
        {
            case "accepted":
                paymentRequest.EmailAcceptedAtUtc ??= eventUtc;
                break;
            case "delivered":
                paymentRequest.EmailDeliveredAtUtc ??= eventUtc;
                break;
            case "opened":
                paymentRequest.EmailOpenedAtUtc ??= eventUtc;
                break;
            case "failed":
            case "bounced":
            case "complained":
                paymentRequest.EmailFailedAtUtc ??= eventUtc;
                if (eventData.TryGetProperty("reason", out var payReasonProp))
                {
                    paymentRequest.EmailFailureReason = payReasonProp.GetString();
                }
                break;
        }
    }
    else
    {
        var docRequest = await db.DocumentSignatureRequests
            .FirstOrDefaultAsync(x => x.Token == dispatchToken);

        if (docRequest is null)
        {
            logger.LogInformation("Mailgun event ignored: dispatch_token not found. Token={Token}", dispatchToken);
            return Results.Ok();
        }

        docRequest.EmailLastEvent = eventName;

        switch (eventName)
        {
            case "accepted":
                docRequest.EmailAcceptedAtUtc ??= eventUtc;
                break;
            case "delivered":
                docRequest.EmailDeliveredAtUtc ??= eventUtc;
                break;
            case "opened":
                docRequest.EmailOpenedAtUtc ??= eventUtc;
                break;
            case "failed":
            case "bounced":
            case "complained":
                docRequest.EmailFailedAtUtc ??= eventUtc;
                if (eventData.TryGetProperty("reason", out var reasonProp))
                {
                    docRequest.EmailFailureReason = reasonProp.GetString();
                }
                break;
        }
    }
        

    await db.SaveChangesAsync();
    return Results.Ok();
})
.DisableAntiforgery();

app.MapHub<SalvageHub>("/hubs/salvage");


app.Run();

static bool IsValidMailgunSignature(JsonElement root, string signingKey)
{
    if (string.IsNullOrWhiteSpace(signingKey))
    {
        return false;
    }

    if (!root.TryGetProperty("signature", out var signatureObj))
    {
        return false;
    }

    var timestamp = signatureObj.TryGetProperty("timestamp", out var timestampProp) ? timestampProp.GetString() : null;
    var token = signatureObj.TryGetProperty("token", out var tokenProp) ? tokenProp.GetString() : null;
    var signature = signatureObj.TryGetProperty("signature", out var signatureProp) ? signatureProp.GetString() : null;

    if (string.IsNullOrWhiteSpace(timestamp) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(signature))
    {
        return false;
    }

    var payload = $"{timestamp}{token}";
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingKey));
    var digest = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    var computed = Convert.ToHexString(digest).ToLowerInvariant();
    var provided = signature.ToLowerInvariant();

    return CryptographicOperations.FixedTimeEquals(
        Encoding.UTF8.GetBytes(computed),
        Encoding.UTF8.GetBytes(provided));
}