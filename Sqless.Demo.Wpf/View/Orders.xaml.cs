using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Demo.Sqless.Wpf.View
{
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            VM.Init();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
