using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Role
    {
        public Role()
        {
            Clients = new HashSet<Client>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
    }
}
