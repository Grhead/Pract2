using System.Linq;
using WpfApp1.View;

namespace WpfApp1.ViewModels
{
    internal class AuthViewModel : StaticViewModel
    {
        private RelayCommand _getLoginCommand;
        private string _ifAcceptButtonContent = "NOPE";
        private string _login;
        private string _password;
        public string Login
        {
            get => _login;
            set => _login = value;
        }
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        public string AcceptButtonContent
        {
            get => _ifAcceptButtonContent;
            set
            {
                _ifAcceptButtonContent = value;
                OnPropertyChanged();
            }
        }
        public AuthViewModel()
        {
        }
        public RelayCommand GetLoginCommand => _getLoginCommand ??
                    (_getLoginCommand = new RelayCommand(x =>
                    {
                        Client client = Service.db.Clients.FirstOrDefault(q => q.Login == _login && q.Password == _password);
                        if (client == null)
                        {
                            AcceptButtonContent = "NOPE";
                        }
                        else if (client.Role == 1)
                        {
                            AcceptButtonContent = "YEAH";
                            new MainWindow().Close();
                            new StuwardWindow().Show();
                            Service.ClientSession = client;

                        }
                        else if (client.Role == 2)
                        {
                            AcceptButtonContent = "YEAH";
                            new MainWindow().Close();
                            new CookWindow().Show();

                        }
                        else if (client.Role == 3)
                        {
                            AcceptButtonContent = "YEAH";
                            new MainWindow().Close();
                            new AdminWindow().Show();
                        }
                    }));
    }
}
