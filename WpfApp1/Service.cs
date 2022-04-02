namespace WpfApp1
{
    public class Service
    {
        public static CooskRDBContext db = new CooskRDBContext();
        private static Client clientSession = new Client();
        public static Client ClientSession
        {
            get => clientSession;
            set => clientSession = value;
        }
    }
}
