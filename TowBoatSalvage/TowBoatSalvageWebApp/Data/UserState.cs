


namespace TowBoatSalvageWebApp.Data
{
    public class UserState
    {
        public ApplicationUser? CurrentUser { get;  set;}
        public IList<string>? Roles {get; set;}
    }
}