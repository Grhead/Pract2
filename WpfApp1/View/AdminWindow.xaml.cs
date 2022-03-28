using System.Windows;

namespace WpfApp1.View
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminViewModel();

        }
    }
}
