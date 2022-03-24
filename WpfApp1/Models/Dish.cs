using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Dish
    {
        public Dish()
        {
            DishesInOrders = new HashSet<DishesInOrder>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int Time { get; set; }

        public virtual ICollection<DishesInOrder> DishesInOrders { get; set; }
    }
}
