using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public ICollection<Order> Orders { get; set; }
    }
}