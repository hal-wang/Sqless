using Sqless.Api;
using System;
using System.Windows;

namespace Demo.Sqless.Wpf {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            SqlessClient.HttpClient.BaseAddress = new Uri(WpfGlobal.ApiUrl);
        }
    }
}
