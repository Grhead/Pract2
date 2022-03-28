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
            FinishDish = new ObservableCollection<Dish>(Service.db.Dishes.Where(x => x.Title == SelectedDish.Title));
            Needtopay = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));

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
        private ObservableCollection<Dish> _finishDish = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> FinishDish
        {
            get
            {
                return _finishDish;
            }
            set
            {
                _finishDish = value;
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

        private Order _selectedOrder = new Order();
        public Order SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }
        private string _getOrPay = "Создать заказ";
        public string GetOrPay
        {
            get 
            { 
                return _getOrPay; 
            }
            set
            {
                _getOrPay = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _getorpayCommand;
        public RelayCommand GetorpayCommand
        {
            get
            {
                return _getorpayCommand ??
                    (_getorpayCommand = new RelayCommand(x =>
                    {
                        if (FinishDish.Count != 0)
                        {
                            Order order = new Order();
                            order.Time = DateTime.Now;
                            order.IdClient = Service.ClientSession.Id;
                            order.Status = 1;
                            order.Sum = Convert.ToInt32(SumOfDishes);

                            Service.db.Orders.Add(order);
                            Service.db.SaveChanges();
                            foreach (var item in FinishDish)
                            {
                                DishesInOrder dishesInOrder = new DishesInOrder();
                                dishesInOrder.DishId = item.Id;
                                dishesInOrder.OrderId = Service.db.Orders.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                                Service.db.DishesInOrders.Add(dishesInOrder);
                                Service.db.SaveChanges();
                            }
                        }
                        Needtopay = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));
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
                        //FinishDish = new ObservableCollection<Dish>(Service.db.Dishes.Where(x => x.Title == SelectedDish.Title));
                        if (SelectedDish != null)
                        {
                            FinishDish.Add(Service.db.Dishes.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (var item in FinishDish)
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
                            FinishDish.Remove(FinishDish.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (var item in FinishDish)
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
        private ObservableCollection<Order> _needToPay = new ObservableCollection<Order>();
        public ObservableCollection<Order> Needtopay
        {
            get
            {
                return _needToPay;
            }
            set
            {
                _needToPay = value;
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
                        if (SelectedOrder != null)
                        {
                            status = Service.db.Orders.FirstOrDefault(x => x.Id == SelectedOrder.Id);

                            if (status != null)
                            {
                                status.Status = 2;
                                Service.db.SaveChanges();
                                OnPropertyChanged();
                            }
                        }
                        Needtopay = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));
                        OnPropertyChanged();
                    }));
            }
        }
    }
}