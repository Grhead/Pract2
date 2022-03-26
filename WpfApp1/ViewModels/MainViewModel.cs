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
            //order = new ObservableCollection<Order>(Service.db.Orders.Include(q=>q.StatusNavigation).Where(x => x.StatusNavigation.Id < 3));
            CheckOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 2));
            CheckCompleteOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));

        }

        //private ObservableCollection<DishesInOrder> dishesInOrder;


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
        private ObservableCollection<Order> order = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckOrder
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(CheckOrder));
            }
        }
        private ObservableCollection<Order> Completeorder = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckCompleteOrder
        {
            get
            {
                return Completeorder;
            }
            set
            {
                Completeorder = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(CheckCompleteOrder));
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
                if (selectedItem != null)
                {
                    DishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders.Where(x => x.OrderId == selectedItem.id).Include(x => x.Dish));
                    int temp = 0;
                    foreach (var item in dishesInOrders)
                    {
                        temp += item.Dish.Time;
                    }
                    GenaralTime = temp;
                    OrderDetailes = dishesInOrders.FirstOrDefault(q => q.OrderId == selectedItem.Id);
                }
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
                        Order status = new Order();
                        if (SelectedOrder != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedOrder.Id);
                            
                            if (status != null)
                            {
                                status.Status = 3;
                                
                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        CheckOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 2));
                        CheckCompleteOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));
                        OnPropertyChanged();
                    }));
            }
        }
        private RelayCommand finishButton;
        
        public RelayCommand FinishButton
        {
            get
            {
                return finishButton ??
                    (finishButton = new RelayCommand(x =>
                    {
                        Order status = new Order();
                        if (SelectedOrder != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedOrder.Id && x.Status == 3);
                            if (status != null)
                            {
                                status.Status = 4;
                                    
                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        CheckCompleteOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));
                        OnPropertyChanged();
                    }));
            }
        }

    }
}