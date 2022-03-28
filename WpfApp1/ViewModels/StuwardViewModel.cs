using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
            get => _checkDish;
            set
            {
                _checkDish = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Dish> _finishDishes = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> FinishDishes
        {
            get => _finishDishes;
            set
            {
                _finishDishes = value;
                OnPropertyChanged();
            }
        }
        private Dish _selectedDish = new Dish();
        public Dish SelectedDish
        {
            get => _selectedDish;
            set
            {
                _selectedDish = value;
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
                OnPropertyChanged();
            }
        }
        private RelayCommand _createOrder;
        public RelayCommand CreateOrder => _createOrder ??
                    (_createOrder = new RelayCommand(x =>
                    {
                        if (FinishDishes.Count != 0)
                        {
                            Order order = new Order
                            {
                                Time = DateTime.Now,
                                IdClient = Service.ClientSession.Id,
                                Status = 1,
                                Sum = Convert.ToInt32(SumOfDishes)
                            };

                            Service.db.Orders.Add(order);
                            Service.db.SaveChanges();
                            foreach (Dish? item in FinishDishes)
                            {
                                DishesInOrder dishesInOrder = new DishesInOrder
                                {
                                    DishId = item.Id,
                                    OrderId = Service.db.Orders.OrderByDescending(x => x.Id).FirstOrDefault().Id
                                };
                                Service.db.DishesInOrders.Add(dishesInOrder);
                                Service.db.SaveChanges();
                            }
                        }
                        NeedPaymentList = new ObservableCollection<Order>(Service.db.Orders.Include(x => x.StatusNavigation).Where(x => x.Status == 1));
                        OnPropertyChanged();
                    }));
        private RelayCommand _addButtonCommand;
        public RelayCommand AddButtonCommand => _addButtonCommand ??
                    (_addButtonCommand = new RelayCommand(x =>
                    {
                        if (SelectedDish.Title != null)
                        {
                            FinishDishes.Add(Service.db.Dishes.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (Dish item in FinishDishes)
                        {
                            if (item != null)
                            {
                                temp += Convert.ToDouble(item.Price);
                            }
                        }
                        SumOfDishes = temp;
                        OnPropertyChanged();
                    }));
        private RelayCommand _deleteButtonCommand;
        public RelayCommand DeleteButtonCommand => _deleteButtonCommand ??
                    (_deleteButtonCommand = new RelayCommand(x =>
                    {
                        if (SelectedDish != null && FinishDishes.Count != 0)
                        {
                            FinishDishes.Remove(FinishDishes.FirstOrDefault(x => x.Title == SelectedDish.Title));
                        }
                        double temp = 0;
                        foreach (Dish item in FinishDishes)
                        {
                            if (item != null)
                            {
                                temp += Convert.ToDouble(item.Price);
                            }
                        }
                        SumOfDishes = temp;
                        OnPropertyChanged();
                    }));
        private double _sumOfDishes;
        public double SumOfDishes
        {
            get => _sumOfDishes;
            set
            {
                _sumOfDishes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Order> _needPaymentList = new ObservableCollection<Order>();
        public ObservableCollection<Order> NeedPaymentList
        {
            get => _needPaymentList;
            set
            {
                _needPaymentList = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _payCommand;
        public RelayCommand PayCommand => _payCommand ??
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