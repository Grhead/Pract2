using System.Windows;

namespace WpfApp1.View
{

    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminViewModel();

        }
    }
}
