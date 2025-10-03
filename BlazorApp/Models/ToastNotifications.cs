using BlazorBootstrap;
using BlazorApp;

namespace BlazorApp.Models
{

    public class ToastNotifications
    {
        public string Message { get; set; } = "Something has happened";
        public ToastType Type { get; set; } = ToastType.Info;
        public string Title { get; set; } = "Notification";
        public string HelpText { get; set; } = $"{DateTime.Now}";
        public int Delay { get; set; } = 5000;
        public bool ShowProgress { get; set; } = true;
        public bool Autohide { get; set; } = true;
        public ToastsPlacement Placement { get; set; } = ToastsPlacement.TopRight;

        public ToastMessage Create(string message, ToastType type, string title)
        {
            return new ToastMessage
            {
                Message = message,
                Type = type,
                Title = title ?? "Notification",
                HelpText = DateTime.Now.ToString(),
                AutoHide = Autohide,
            };
        }
    }
}
