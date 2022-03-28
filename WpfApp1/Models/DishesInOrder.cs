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

        public Dish Dish { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}