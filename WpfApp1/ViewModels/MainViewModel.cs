using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : StaticViewModel
    {

        public MainViewModel()
        {
            //dishesInOrder = new ObservableCollection<DishesInOrder>(new CooskRDBContext().DishesInOrders.Include(q => q.Order).Include(x => x.Dish));
            order = new ObservableCollection<Order>(Service.db.Orders.Include(q=>q.StatusNavigation).Where(x => x.StatusNavigation.Id < 3));
        }
        
        //private ObservableCollection<DishesInOrder> dishesInOrder;
        private ObservableCollection<Order> order;

        //public ObservableCollection<DishesInOrder> DishInOrder
        //{
        //    get
        //    {
        //        return dishesInOrder;
        //    }
        //    set
        //    {
        //        dishesInOrder = value;
        //        OnPropertyChanged();
        //    }
        //}
        public ObservableCollection<Order> CheckOrder
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }
        private Order selectedItem = new Order();
        public Order SelectedOrder
        {
            get
            {
                return selectedItem;
            }
            set
            {
                
                selectedItem = value;
                DishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders.Where(x => x.OrderId == selectedItem.id).Include(x => x.Dish));
                int temp = 0;
                foreach (var item in dishesInOrders)
                {
                    temp += item.Dish.Time;
                }
                GenaralTime = temp;
                OrderDetailes = dishesInOrders.First(q=>q.OrderId == selectedItem.Id);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DishesInOrder> dishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders);
        public ObservableCollection<DishesInOrder> DishesInOrders
        {
            get
            {
                return dishesInOrders;
            }
            set
            {
                dishesInOrders = value;
                OnPropertyChanged();
            }
        }
        private DishesInOrder orderDetailes;
        public DishesInOrder OrderDetailes
        {
            get
            {
                return orderDetailes;
            }
            set
            {
                orderDetailes = value;
                OnPropertyChanged();
            }
        }
        private int genaralTime = 0;
        public int GenaralTime
        {
            get
            {
                return genaralTime;
            }
            set
            {
                genaralTime = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return changeStatus ??
                    (changeStatus = new RelayCommand(x =>
                    {
                        var status = Service.db.Orders.Where(x=>x.id == SelectedOrder.id).FirstOrDefault();
                        status.Status = 3;
                        Service.db.SaveChanges();
                    }));
            }
        }
    }
}