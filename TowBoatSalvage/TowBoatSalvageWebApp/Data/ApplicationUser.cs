using Microsoft.AspNetCore.Identity;

namespace TowBoatSalvageWebApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // putting this process in place so this becomes a internal web app meant for use only within towboat
        public bool isApproved { get; set; } = false; // not approved by default
        public bool isAdmin { get; set; } = false; // this will only be true for a few, dont wanna setup roles through identity right now

        public string Name { get; set; } = string.Empty; // setting up names to identify comment in work order section

        public bool AddToFuelList { get; set; } = false;
    }

}
