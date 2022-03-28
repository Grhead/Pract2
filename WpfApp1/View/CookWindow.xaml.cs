using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CookWindow.xaml
    /// </summary>
    public partial class CookWindow : Window
    {
        public CookWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.CookViewModel();
        }
    }
}
