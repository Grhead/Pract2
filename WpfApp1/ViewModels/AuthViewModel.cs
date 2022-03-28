using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.View;

namespace WpfApp1.ViewModels
{
    class AuthViewModel : StaticViewModel
    {
        private RelayCommand _getLoginCommand;
        private string _ifAccept = "NOPE";
        private string _login;
        private string _password;
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public string Accept
        {
            get 
            {
                return _ifAccept;  
            }
            set
            {
                _ifAccept = value;
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
                return _getLoginCommand ??
                    (_getLoginCommand = new RelayCommand(x =>
                    {
                        Client client = Service.db.Clients.FirstOrDefault(q=>q.Login == _login && q.Password == _password);
                        if (client == null)
                        {
                            Accept = "NOPE";
                        } else if (client.Role == 1)
                        {
                            Accept = "YEAH";
                            new MainWindow().Close();
                            new StuwardWindow().Show();
                            Service.ClientSession = client;

                        }
                        else if (client.Role == 2)
                        {
                            Accept = "YEAH";
                            new MainWindow().Close();
                            new CookWindow().Show();
                            
                        } else if (client.Role == 3)
                        {
                            Accept = "YEAH";
                            new MainWindow().Close();
                            new AdminWindow().Show();
                        }
                    }));
            }
        }
    }
}
