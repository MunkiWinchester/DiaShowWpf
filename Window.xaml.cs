using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;

namespace DiaShowWpf
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class Window : IViewFor<WindowViewModel>
    {
        public Window()
        {
            ViewModel = new WindowViewModel();
            InitializeComponent();

            DataContext = ViewModel;

            this.Events()
                .KeyDown.Where(x => x.Key == Key.Right || x.Key == Key.Left || x.Key == Key.Space)
                .InvokeCommand(this, x => x.ViewModel.KeyDownCommand);
            this.Events().Loaded.InvokeCommand(this, x => x.ViewModel.GetImages);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (WindowViewModel) value; }
        }

        public WindowViewModel ViewModel { get; set; }
    }
}
