using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels
{
    public class StuwardViewModel : StaticViewModel
    {
        public StuwardViewModel()
        {
            CheckDish = new ObservableCollection<Dish>(Service.db.Dishes);
            FinishDish = new ObservableCollection<Dish>(Service.db.Dishes.Where(x => x.Title == SelectedDish.Title));
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
                    }));
            }
        }
    }
}