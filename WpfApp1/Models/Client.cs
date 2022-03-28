using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string SecondName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Experience { get; set; }
        public int Role { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual Role RoleNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
