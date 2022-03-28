using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp1.ViewModels
{
    public class StuwardViewModel : StaticViewModel
    {
        public StuwardViewModel()
        {
            CheckDish = new ObservableCollection<Dish>(Service.db.Dishes);
            FinishDishes = new ObservableCollection<Dish>(Service.db.Dishes.Where(x => x.Title == SelectedDish.Title));
            NeedPaymentList = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));

        }
        private ObservableCollection<Dish> _checkDish = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> CheckDish
        {
            get
            {
                return _checkDish;
            }
            set
            {
                _checkDish = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Dish> _finishDishes = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> FinishDishes
        {
            get
            {
                return _finishDishes;
            }
            set
            {
                _finishDishes = value;
                OnPropertyChanged();
            }
        }
        private Dish _selectedDish = new Dish();
        public Dish SelectedDish
        {
            get
            {
                return _selectedDish;
            }
            set
            {
                _selectedDish = value;
                OnPropertyChanged();
            }
        }

        private Order _selectedItem = new Order();
        public Order SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        private string _createOrderButtonContent = "Создать заказ";
        public string CreateOrderButtonContent
        {
            get 
            { 
                return _createOrderButtonContent; 
            }
            set
            {
                _createOrderButtonContent = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _createOrder;
        public RelayCommand CreateOrder
        {
            get
            {
                return _createOrder ??
                    (_createOrder = new RelayCommand(x =>
                    {
                        if (FinishDishes.Count != 0)
                        {
                            Order order = new Order();
                            order.Time = DateTime.Now;
                            order.IdClient = Service.ClientSession.Id;
                            order.Status = 1;
                            order.Sum = Convert.ToInt32(SumOfDishes);

                            Service.db.Orders.Add(order);
                            Service.db.SaveChanges();
                            foreach (var item in FinishDishes)
                            {
                                DishesInOrder dishesInOrder = new DishesInOrder();
                                dishesInOrder.DishId = item.Id;
                                dishesInOrder.OrderId = Service.db.Orders.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                                Service.db.DishesInOrders.Add(dishesInOrder);
                                Service.db.SaveChanges();
                            }
                        }
                        NeedPaymentList = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));
                        OnPropertyChanged();
                    }));

            }
        }
        private RelayCommand _addButtonCommand;
        public RelayCommand AddButtonCommand
        {
            get
            {
                return _addButtonCommand ??
                    (_addButtonCommand = new RelayCommand(x =>
                    {
                        //FinishDishes = new ObservableCollection<Dish>(Service.db.Dishes.Where(x => x.Title == SelectedDish.Title));
                        if (SelectedDish != null)
                        {
                            FinishDishes.Add(Service.db.Dishes.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (var item in FinishDishes)
                        {
                            temp += Convert.ToDouble(item.Price);
                        }
                        SumOfDishes = temp;
                        OnPropertyChanged();
                    }));
            }
        }
        private RelayCommand _deleteButtonCommand;
        public RelayCommand DeleteButtonCommand
        {
            get
            {
                return _deleteButtonCommand ??
                    (_deleteButtonCommand = new RelayCommand(x =>
                    {
                        if (SelectedDish != null)
                        {
                            FinishDishes.Remove(FinishDishes.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (var item in FinishDishes)
                        {
                            temp += Convert.ToDouble(item.Price);
                        }
                        SumOfDishes = temp;
                        OnPropertyChanged();
                    }));
            }
        }
        private double _sumOfDishes;
        public double SumOfDishes
        {
            get
            {
                return _sumOfDishes;
            }
            set
            {
                _sumOfDishes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Order> _needPaymentList = new ObservableCollection<Order>();
        public ObservableCollection<Order> NeedPaymentList
        {
            get
            {
                return _needPaymentList;
            }
            set
            {
                _needPaymentList = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _payCommand;
        public RelayCommand PayCommand
        {
            get
            {
                return _payCommand ??
                    (_payCommand = new RelayCommand(x =>
                    {
                        Order status = new Order();
                        if (SelectedItem != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedItem.Id);

                            if (status != null)
                            {
                                status.Status = 2;
                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        NeedPaymentList = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));
                        OnPropertyChanged();
                    }));
            }
        }
    }
}