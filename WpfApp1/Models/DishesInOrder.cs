﻿using System;
using System.Collections.Generic;
using WpfApp1.ViewModels;
namespace WpfApp1
{
    public partial class DishesInOrder : ViewModels.StaticViewModel
    {
        public int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public int OrderId { get; set; }
        public int DishId { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}