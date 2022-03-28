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
            //_order = new ObservableCollection<Order>(Service.db.Orders.Include(q=>q.StatusNavigation).Where(x => x.StatusNavigation.Id < 3));
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
        private ObservableCollection<Order> _order = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckOrder
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(CheckOrder));
            }
        }
        private ObservableCollection<Order> _completeOrder = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckCompleteOrder
        {
            get
            {
                return _completeOrder;
            }
            set
            {
                _completeOrder = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(CheckCompleteOrder));
            }
        }
        private Order _selectedItem = new Order();
        public Order SelectedOrder
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    DishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders.Where(x => x.OrderId == _selectedItem.id).Include(x => x.Dish));
                    int temp = 0;
                    foreach (var item in _dishesInOrders)
                    {
                        temp += item.Dish.Time;
                    }
                    GenaralTime = temp;
                    OrderDetailes = _dishesInOrders.FirstOrDefault(q => q.OrderId == _selectedItem.Id);
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DishesInOrder> _dishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders);
        public ObservableCollection<DishesInOrder> DishesInOrders
        {
            get
            {
                return _dishesInOrders;
            }
            set
            {
                _dishesInOrders = value;
                OnPropertyChanged();
            }
        }
        private DishesInOrder _orderDetailes;
        public DishesInOrder OrderDetailes
        {
            get
            {
                return _orderDetailes;
            }
            set
            {
                _orderDetailes = value;
                OnPropertyChanged();
            }
        }
        private int _genaralTime = 0;
        public int GenaralTime
        {
            get
            {
                return _genaralTime;
            }
            set
            {
                _genaralTime = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return _changeStatus ??
                    (_changeStatus = new RelayCommand(x =>
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
        private RelayCommand _finishButton;
        
        public RelayCommand FinishButton
        {
            get
            {
                return _finishButton ??
                    (_finishButton = new RelayCommand(x =>
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