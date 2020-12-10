using System.Windows.Controls;

namespace Sqless.Demo.Wpf.View
{
    public partial class Products : Page
    {
        public Products()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VM.Init();
        }

        private void Order_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Orders());
        }
    }
}
