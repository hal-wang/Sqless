using MahApps.Metro.Controls;

namespace Sqless.Demo.Wpf.View
{
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();

            VM.OnLoginFinished += OnLoginFinished;
        }

        private void OnLoginFinished()
        {
            var window = new MainWindow();
            window.Show();

            this.Close();
        }
    }
}
