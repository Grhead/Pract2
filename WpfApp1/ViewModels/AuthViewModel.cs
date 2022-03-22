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
    class AuthViewModel : ViewModels.StaticViewModel
    {
        private RelayCommand getLoginCommand;

        private ObservableCollection<Client> clients;
        private string IfAccept = "NOPE";
        public RelayCommand GetLoginCommand
        {
            get 
            {
                return getLoginCommand ??
                    (getLoginCommand = new RelayCommand(x =>
                    {
                        Client client = new Client();
                        if (client.Login == login && client.Password == password)
                        {
                            IfAccept = "YEAH";
                        }
                    }));
            }
        }

        public AuthViewModel()
        {

        }
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

        
    }
}
