using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels
{
    class AuthViewModel : StaticViewModel
    {
        private RelayCommand getLoginCommand;
        private string IfAccept = "NOPE";
        private string login;
        private string password;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public string Accept
        {
            get 
            {
                return IfAccept;  
            }
            set
            {
                IfAccept = value;
                OnPropertyChanged();
            }
        }
        public AuthViewModel()
        {
        }
        public RelayCommand GetLoginCommand
        {
            get 
            {
                return getLoginCommand ??
                    (getLoginCommand = new RelayCommand(x =>
                    {
                        Client client = Service.db.Clients.FirstOrDefault(q=>q.Login == login && q.Password == password);
                        if (client == null)
                        {
                            Accept = "NOPE";
                        } else if (client.Role == 1)
                        {
                            Accept = "YEAH";
                            new StuwardWindow().Show();
                            new MainWindow().Close();
                        }
                        else if (client.Role == 2)
                        {
                            Accept = "YEAH";
                            new CookWindow().Show();
                            new MainWindow().Close();
                        }
                    }));
            }
        }
    }
}
