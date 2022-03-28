using System;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class StuwardWindow : Window
    {
        public StuwardWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.StuwardViewModel();
            DispatcherTimer? timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerTick;
            timer.Start();


        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimeBlock.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
