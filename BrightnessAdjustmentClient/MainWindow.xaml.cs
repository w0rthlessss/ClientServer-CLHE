using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;

namespace BrightnessAdjustmentClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;

        const string ipAdress = "127.0.0.1";
        const int port = 8080;

        private async void ConnectToServerAsync()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ipAdress, port);
                stream = client.GetStream();
            }
            catch 
            {
                MessageBox.Show("Не удалось подключиться к серверу! Убедитесь, что сервер запущен, и только тогда запустите приложение");
                Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServerAsync();
        }
    }
}