using System.Windows;
using System.Windows.Controls;

namespace DiaShowWpf
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class Window
    {
        public Window()
        {
            InitializeComponent();
        }

        private void SelectPath(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            box?.SelectAll();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is WindowViewModel vm)
                vm.GetResultsFromPath();
        }
    }
}
