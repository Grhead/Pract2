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
            dishesInOrder = new ObservableCollection<DishesInOrder>(new CooskRDBContext().DishesInOrders.Include(q => q.Order).Include(x => x.Dish));
        }

        private ObservableCollection<DishesInOrder> dishesInOrder;

        public ObservableCollection<DishesInOrder> DishInOrder
        {
            get => dishesInOrder;
            set
            {
                dishesInOrder = value;
                OnPropertyChanged();
            }
        }
    }
}
