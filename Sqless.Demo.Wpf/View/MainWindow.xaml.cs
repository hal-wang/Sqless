using MahApps.Metro.Controls;

namespace Demo.Sqless.Wpf.View
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameElement.Navigate(new Products());
        }
    }
}
