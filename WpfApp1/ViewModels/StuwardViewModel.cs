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
        private ObservableCollection<Dish> checkDish = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> CheckDish
        {
            get
            {
                return checkDish;
            }
            set
            {
                checkDish = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Dish> finishDish = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> FinishDish
        {
            get
            {
                return finishDish;
            }
            set
            {
                finishDish = value;
                OnPropertyChanged();
            }
        }
        private Dish selectedDish = new Dish();
        public Dish SelectedDish
        {
            get
            {
                return selectedDish;
            }
            set
            {
                selectedDish = value;
                OnPropertyChanged();
            }
        }

        private Order selectedOrder = new Order();
        public Order SelectedOrder
        {
            get
            {
                return selectedOrder;
            }
            set
            {
                selectedOrder = value;
                OnPropertyChanged();
            }
        }
        private string getorpay = "Создать заказ";
        public string GetOrPay
        {
            get 
            { 
                return getorpay; 
            }
            set
            {
                getorpay = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand getorpayCommand;
        public RelayCommand GetorpayCommand
        {
            get
            {
                return getorpayCommand ??
                    (getorpayCommand = new RelayCommand(x =>
                    {
                        if (FinishDish.Count != 0)
                        {
                            Order order = new Order();
                            order.Time = DateTime.Now;
                            order.IdClient = Service.ClientSession.Id;
                            order.Status = 1;
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
        private RelayCommand addButtonCommand;
        public RelayCommand AddButtonCommand
        {
            get
            {
                return addButtonCommand ??
                    (addButtonCommand = new RelayCommand(x =>
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
        private RelayCommand deleteButtonCommand;
        public RelayCommand DeleteButtonCommand
        {
            get
            {
                return deleteButtonCommand ??
                    (deleteButtonCommand = new RelayCommand(x =>
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
        private double sumOfDishes;
        public double SumOfDishes
        {
            get
            {
                return sumOfDishes;
            }
            set
            {
                sumOfDishes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Order> needtopay = new ObservableCollection<Order>();
        public ObservableCollection<Order> Needtopay
        {
            get
            {
                return needtopay;
            }
            set
            {
                needtopay = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand payCommand;
        public RelayCommand PayCommand
        {
            get
            {
                return payCommand ??
                    (payCommand = new RelayCommand(x =>
                    {
                        Order status = new Order();
                        if (SelectedDish != null)
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