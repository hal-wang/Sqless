using Hubery.Sqless.Api;
using System.Windows;

namespace Hubery.Sqless.Demo.Wpf
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
