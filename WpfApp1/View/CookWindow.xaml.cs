using System.Windows;

namespace WpfApp1
{
    public partial class CookWindow : Window
    {
        public CookWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.CookViewModel();
        }
    }
}
