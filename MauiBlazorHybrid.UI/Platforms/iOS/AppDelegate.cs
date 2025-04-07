using Foundation;
using MauiBlazorHybrid.UI;

namespace MauiBlazorHybrid
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
