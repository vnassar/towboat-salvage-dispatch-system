namespace TowBoatSalvageWebApp.Services
{
    public class ToastService
    {
        public event Func<string, string, Task>? OnShow; // (message,type)

        public async Task ShowToast(string message, string type = "info")
        {
            if (OnShow != null)
            {
                await OnShow.Invoke(message, type);
            }
        }
    }
}
