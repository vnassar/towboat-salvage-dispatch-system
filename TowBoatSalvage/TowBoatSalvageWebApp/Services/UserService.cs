using TowBoatSalvageWebApp.Data;

namespace TowBoatSalvageWebApp.Services
{
    public class UserService
    {
        public ApplicationUser? User { get; private set; }
        public string? Name { get; private set; }
        public bool isAdmin { get; private set; }

        public void SetUser(ApplicationUser? user)
        {
            if (user is not null) User = user;
            isAdmin = User?.isAdmin ?? false;
        }

        public void SetRoles(IList<string>? roles)
        {
            if (roles is null) return;

            if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase)) isAdmin = true;
        }

        public bool GetIsAdmin() => isAdmin;

        public void SetName(string? name) => Name = name;
    }
}
