using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class VehicleInspection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaptainsPersonalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CaptainsUSCGLicence = table.Column<bool>(type: "INTEGER", nullable: false),
                    USCGLicenseExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProofOfDrugConsortium = table.Column<bool>(type: "INTEGER", nullable: false),
                    DrugConsortiumExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AutoInflateLifeJacket = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoInflateLifeJacketExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CaptainsDitchBag = table.Column<bool>(type: "INTEGER", nullable: false),
                    PLBBatteryRegistration = table.Column<bool>(type: "INTEGER", nullable: false),
                    PLBExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ClipboardsWithInvoices = table.Column<bool>(type: "INTEGER", nullable: false),
                    USCGRulesOfTheRoadBook = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptainsPersonalItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitialInspection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FloridaDOCCertificate = table.Column<bool>(type: "INTEGER", nullable: false),
                    FloridaRegistration = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentRegistration = table.Column<bool>(type: "INTEGER", nullable: false),
                    AlcoholTestStrip = table.Column<bool>(type: "INTEGER", nullable: false),
                    AlcoholTestStripDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VesselsLogBook = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialInspection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MountedLights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Navigation = table.Column<bool>(type: "INTEGER", nullable: false),
                    Towing = table.Column<bool>(type: "INTEGER", nullable: false),
                    PublicSafety = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpotLight = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeckFlood = table.Column<bool>(type: "INTEGER", nullable: false),
                    Flashlight = table.Column<bool>(type: "INTEGER", nullable: false),
                    Horn = table.Column<bool>(type: "INTEGER", nullable: false),
                    Siren = table.Column<bool>(type: "INTEGER", nullable: false),
                    FogHorn = table.Column<bool>(type: "INTEGER", nullable: false),
                    Hailer = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountedLights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Navigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GPS = table.Column<bool>(type: "INTEGER", nullable: false),
                    BackupGPS = table.Column<bool>(type: "INTEGER", nullable: false),
                    DepthFinder = table.Column<bool>(type: "INTEGER", nullable: false),
                    Radar = table.Column<bool>(type: "INTEGER", nullable: false),
                    VHFPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    VHFSecondary = table.Column<bool>(type: "INTEGER", nullable: false),
                    DVR = table.Column<bool>(type: "INTEGER", nullable: false),
                    CheckDVRCardForRecording = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstRecording = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastRecording = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WhiskeyOrELectronicCompass = table.Column<bool>(type: "INTEGER", nullable: false),
                    Binoculars = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navigation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TwoStrokOil = table.Column<bool>(type: "INTEGER", nullable: false),
                    FourCycleOil = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationalCheck",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SalvagePump = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServicePump = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmptyFuelCarburator = table.Column<bool>(type: "INTEGER", nullable: false),
                    FillGasTank = table.Column<bool>(type: "INTEGER", nullable: false),
                    InspectWiring = table.Column<bool>(type: "INTEGER", nullable: false),
                    VerifyPumpRuns = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateOfPumpCheck = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalCheck", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Placards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DischargeOfOil = table.Column<bool>(type: "INTEGER", nullable: false),
                    DischargeOfGarbage = table.Column<bool>(type: "INTEGER", nullable: false),
                    NavigationChart = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoatUSMembershipApplications = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafetyDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CaptainCrewType1Offshore = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdultPFDs = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChildsPFDs = table.Column<bool>(type: "INTEGER", nullable: false),
                    ThrowableLifeRing = table.Column<bool>(type: "INTEGER", nullable: false),
                    ColdWaterSuite = table.Column<bool>(type: "INTEGER", nullable: false),
                    Flares = table.Column<bool>(type: "INTEGER", nullable: false),
                    FlaresExpire = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Epirb = table.Column<bool>(type: "INTEGER", nullable: false),
                    EpirbExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FireExtinguishers = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstAidKit = table.Column<bool>(type: "INTEGER", nullable: false),
                    ThermalBlanket = table.Column<bool>(type: "INTEGER", nullable: false),
                    HandheldVHF = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salvage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TwoRulePumps = table.Column<bool>(type: "INTEGER", nullable: false),
                    Bucket = table.Column<bool>(type: "INTEGER", nullable: false),
                    Pump = table.Column<bool>(type: "INTEGER", nullable: false),
                    PVCPipe = table.Column<bool>(type: "INTEGER", nullable: false),
                    SuctionHose = table.Column<bool>(type: "INTEGER", nullable: false),
                    Pads = table.Column<bool>(type: "INTEGER", nullable: false),
                    WoodenPlugs = table.Column<bool>(type: "INTEGER", nullable: false),
                    FoamFootballs = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salvage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VesselEquipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TowBit = table.Column<bool>(type: "INTEGER", nullable: false),
                    Knife = table.Column<bool>(type: "INTEGER", nullable: false),
                    PrimaryTowLine = table.Column<bool>(type: "INTEGER", nullable: false),
                    SecondaryTowLine = table.Column<bool>(type: "INTEGER", nullable: false),
                    FourDockLines = table.Column<bool>(type: "INTEGER", nullable: false),
                    FendersTwo8 = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpareBattery = table.Column<bool>(type: "INTEGER", nullable: false),
                    JumperCables = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoBilgePumps = table.Column<bool>(type: "INTEGER", nullable: false),
                    PrimaryAnchor = table.Column<bool>(type: "INTEGER", nullable: false),
                    BackupAnchor = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoatHook = table.Column<bool>(type: "INTEGER", nullable: false),
                    ToolKit = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoGasolineCans = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFuelFilters = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselEquipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInspection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VesselNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeOrCaptain = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfInspection = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InitialInspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CaptainsPersonalItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    SafetyDevicesId = table.Column<int>(type: "INTEGER", nullable: false),
                    MountedLightsId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlacardsId = table.Column<int>(type: "INTEGER", nullable: false),
                    NavigationId = table.Column<int>(type: "INTEGER", nullable: false),
                    VesselEquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalvageId = table.Column<int>(type: "INTEGER", nullable: false),
                    OilId = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationalCheckId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_CaptainsPersonalItems_CaptainsPersonalItemsId",
                        column: x => x.CaptainsPersonalItemsId,
                        principalTable: "CaptainsPersonalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_InitialInspection_InitialInspectionId",
                        column: x => x.InitialInspectionId,
                        principalTable: "InitialInspection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_MountedLights_MountedLightsId",
                        column: x => x.MountedLightsId,
                        principalTable: "MountedLights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_Navigation_NavigationId",
                        column: x => x.NavigationId,
                        principalTable: "Navigation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_Oil_OilId",
                        column: x => x.OilId,
                        principalTable: "Oil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_OperationalCheck_OperationalCheckId",
                        column: x => x.OperationalCheckId,
                        principalTable: "OperationalCheck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_Placards_PlacardsId",
                        column: x => x.PlacardsId,
                        principalTable: "Placards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_SafetyDevices_SafetyDevicesId",
                        column: x => x.SafetyDevicesId,
                        principalTable: "SafetyDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_Salvage_SalvageId",
                        column: x => x.SalvageId,
                        principalTable: "Salvage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInspection_VesselEquipment_VesselEquipmentId",
                        column: x => x.VesselEquipmentId,
                        principalTable: "VesselEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_CaptainsPersonalItemsId",
                table: "VehicleInspection",
                column: "CaptainsPersonalItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_InitialInspectionId",
                table: "VehicleInspection",
                column: "InitialInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_MountedLightsId",
                table: "VehicleInspection",
                column: "MountedLightsId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_NavigationId",
                table: "VehicleInspection",
                column: "NavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_OilId",
                table: "VehicleInspection",
                column: "OilId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_OperationalCheckId",
                table: "VehicleInspection",
                column: "OperationalCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_PlacardsId",
                table: "VehicleInspection",
                column: "PlacardsId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_SafetyDevicesId",
                table: "VehicleInspection",
                column: "SafetyDevicesId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_SalvageId",
                table: "VehicleInspection",
                column: "SalvageId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspection_VesselEquipmentId",
                table: "VehicleInspection",
                column: "VesselEquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleInspection");

            migrationBuilder.DropTable(
                name: "CaptainsPersonalItems");

            migrationBuilder.DropTable(
                name: "InitialInspection");

            migrationBuilder.DropTable(
                name: "MountedLights");

            migrationBuilder.DropTable(
                name: "Navigation");

            migrationBuilder.DropTable(
                name: "Oil");

            migrationBuilder.DropTable(
                name: "OperationalCheck");

            migrationBuilder.DropTable(
                name: "Placards");

            migrationBuilder.DropTable(
                name: "SafetyDevices");

            migrationBuilder.DropTable(
                name: "Salvage");

            migrationBuilder.DropTable(
                name: "VesselEquipment");
        }
    }
}
