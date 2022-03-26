using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Service
    {
        public static CooskRDBContext db = new CooskRDBContext();
        private static Client clientSession = new Client();
        public static Client ClientSession
        {
            get 
            { 
                return clientSession; 
            }
            set 
            { 
                clientSession = value; 
            }
        }

    }
    

}
