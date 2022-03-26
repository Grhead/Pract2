using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для StuwardWindow.xaml
    /// </summary>
    public partial class StuwardWindow : Window
    {
        public StuwardWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.StuwardViewModel();
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerTick;
            timer.Start();

            
        }
        void TimerTick(object sender, EventArgs e)
        {
            TimeBlock.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
