using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class StuwardWindow : Window, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimeBlock.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
