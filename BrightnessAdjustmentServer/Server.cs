using System.Net;
using System.Net.Sockets;

namespace ImageAdjustmentSever
{
    class Server
    {
        const int port = 8080;

        private static void Main(string[] args)
        {
            ExecuteServer();
        }

        private static void ExecuteServer()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, port);

            server.Start();

            Console.WriteLine($"Server is up on adress {IPAddress.Loopback} and port {port}");

            while (true)
            {
                Console.WriteLine("Awaiting client connection ...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                NetworkStream stream = client.GetStream();

                try
                {
                    byte[] lengthBuffer = new byte[4];
                    stream.Read(lengthBuffer, 0, 4);
                    int imageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    Console.WriteLine($"Image length recieved: {imageLength} bytes");

                    byte[] imageData = new byte[imageLength];
                    int bytesRead = 0;

                    while(bytesRead < imageLength)
                    {
                        bytesRead += stream.Read(imageData, bytesRead, imageLength - bytesRead);
                    }

                    Console.WriteLine("Image recieved successfully.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                finally
                {
                    client.Close();
                }
            }
        }
    }
}
