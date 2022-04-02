using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    public partial class StuwardWindow : Window, INotifyPropertyChanged
    {
        public StuwardViewModel stuwardViewModel = new StuwardViewModel();

        public StuwardWindow()
        {
            InitializeComponent();
            DataContext = stuwardViewModel;
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
        private void ProductsList_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBox listView = sender as ListBox;
            if (listView?.SelectedItem != null)
            {
                DragDrop.DoDragDrop(listView, listView.SelectedItem as Dish,
                DragDropEffects.Copy);
            }
        }
        private void BagList_OnDrop(object sender, DragEventArgs e)
        {
            Dish product = (Dish)e.Data.GetData(e.Data.GetFormats()[0]);
            List<Dish> test = new List<Dish>(new StuwardViewModel().FinishDishes);
            bool isExists = test.Exists(x => x == product);
            if (isExists)
            {
                //test.Find(x => x == product);
            }
            else
            {
                stuwardViewModel.FinishDishes.Add(product);
                double temp = 0;
                foreach (Dish item in stuwardViewModel.FinishDishes)
                {
                    if (item != null)
                    {
                        temp += Convert.ToDouble(item.Price);
                    }
                }
                stuwardViewModel.SumOfDishes = temp;
            }
            PreOrder.ItemsSource = stuwardViewModel.FinishDishes;
            PreOrder.Items.Refresh();
            OnPropertyChanged();
        }
    }
}
