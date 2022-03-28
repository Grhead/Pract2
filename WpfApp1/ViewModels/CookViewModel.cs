using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp1.ViewModels
{
    public class CookViewModel : StaticViewModel
    {

        public CookViewModel()
        {
            GetOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 2));
            FinishOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));
        }
        private ObservableCollection<Order> _getOrder = new ObservableCollection<Order>();

        public ObservableCollection<Order> GetOrder
        {
            get => _getOrder;
            set
            {
                _getOrder = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Order> _finishOrder = new ObservableCollection<Order>();

        public ObservableCollection<Order> FinishOrder
        {
            get => _finishOrder;
            set
            {
                _finishOrder = value;
                OnPropertyChanged();
            }
        }
        private Order _selectedItem = new Order();
        public Order SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    DishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders.Where(x => x.OrderId == _selectedItem.id).Include(x => x.Dish));
                    int temp = 0;
                    foreach (DishesInOrder? item in _dishesInOrders)
                    {
                        temp += item.Dish.Time;
                    }
                    TotalTime = temp;
                    OrderDetailes = _dishesInOrders.FirstOrDefault(q => q.OrderId == _selectedItem.Id);
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DishesInOrder> _dishesInOrders = new ObservableCollection<DishesInOrder>(Service.db.DishesInOrders);
        public ObservableCollection<DishesInOrder> DishesInOrders
        {
            get => _dishesInOrders;
            set
            {
                _dishesInOrders = value;
                OnPropertyChanged();
            }
        }
        private DishesInOrder _orderDetailes;
        public DishesInOrder OrderDetailes
        {
            get => _orderDetailes;
            set
            {
                _orderDetailes = value;
                OnPropertyChanged();
            }
        }
        private int _totalTime = 0;
        public int TotalTime
        {
            get => _totalTime;
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _startCook;
        public RelayCommand StartCook => _startCook ??
                    (_startCook = new RelayCommand(x =>
                    {
                        Order status = new Order();
                        if (SelectedItem != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedItem.Id);

                            if (status != null)
                            {
                                status.Status = 3;

                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        GetOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 2));
                        FinishOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));
                        OnPropertyChanged();
                    }));
        private RelayCommand _orderReady;

        public RelayCommand OrderReady => _orderReady ??
                    (_orderReady = new RelayCommand(x =>
                    {
                        Order status = new Order();
                        if (SelectedItem != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedItem.Id && x.Status == 3);
                            if (status != null)
                            {
                                status.Status = 4;

                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        FinishOrder = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 3));
                        OnPropertyChanged();
                    }));

    }
}