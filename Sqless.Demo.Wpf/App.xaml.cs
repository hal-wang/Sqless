using Sqless.Api;
using System.Windows;

namespace Sqless.Demo.Wpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SqlessClient.BaseUrl = WpfGlobal.ApiUrl;
        }
    }
}
