using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TowBoatSalvageWebApp.Data;

namespace TowBoatSalvageWebApp.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authStateprovider;
        private readonly UserManager<ApplicationUser> _userManager;

        private bool initialized = false;

        public ApplicationUser? User { get; private set; } = null;
        public string? Name { get; private set; }
        public bool isAdmin { get; private set; }
        public IList<string> roles { get; set; } = new List<string>();

        public UserService(AuthenticationStateProvider authStateProvider, UserManager<ApplicationUser> userManager)
        {
            _authStateprovider = authStateProvider;
            _userManager = userManager;
        }

        public bool GetIsAdmin() => isAdmin;

        public async Task InitializeAsync()
        {
            if (initialized) return;

            var authState = await _authStateprovider.GetAuthenticationStateAsync();
            var user = await _userManager.GetUserAsync(authState.User);

            if (user is null) return;

            roles  = await _userManager.GetRolesAsync(user);

            User = user;
            Name = user.Name;
            isAdmin = user.isAdmin || roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

            initialized = true;
        }

        public async Task SetUserService()
        {
            var authState = await _authStateprovider.GetAuthenticationStateAsync();
            var user = await _userManager.GetUserAsync(authState.User);

            if (user is null) return;

            roles = await _userManager.GetRolesAsync(user);

            User = user;
            Name = user.Name;
            isAdmin = user.isAdmin || roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);
        }
    }
}
