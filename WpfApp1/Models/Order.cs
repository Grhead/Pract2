using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Order : ViewModels.StaticViewModel
    {
        public Order()
        {
            DishesInOrders = new HashSet<DishesInOrder>();
        }

        public int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public DateTime time;
        public DateTime Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        public int IdClient { get; set; }
        public int Status { get; set; }
        public int Sum { get; set; }

        public Client IdClientNavigation { get; set; } = null!;
        public Status StatusNavigation { get; set; } = null!;
        public ICollection<DishesInOrder> DishesInOrders { get; set; }
    }
}