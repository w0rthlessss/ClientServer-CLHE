using System.Net;
using System.Net.Sockets;
using CLHE_Algorithm;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageAdjustmentSever
{
    class Server
    {
        const int port = 8080;
        const int histSize = 256;
        const int histByteSize = 1024;

        private static void Main(string[] args)
        {
            ExecuteServer();
        }

        private static void HandleClientConnection(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    byte[] lengthBuffer = new byte[4];
                    int imageLength = 0;

                    stream.Read(lengthBuffer, 0, 4);
                    imageLength = BitConverter.ToInt32(lengthBuffer, 0);
                    if (imageLength == 0) break;
                    Console.WriteLine($"Image length recieved: {imageLength} bytes");

                    byte[] imageData = new byte[imageLength];
                    int bytesRead = 0;
                    while (bytesRead < imageLength)
                    {
                        bytesRead += stream.Read(imageData, bytesRead, imageLength - bytesRead);
                    }
                    Console.WriteLine("Image recieved successfully.");

                    byte[] doubleBuffer = new byte[8];
                    double alpha = 0;
                    stream.Read(doubleBuffer, 0, 8);
                    alpha = BitConverter.ToDouble(doubleBuffer, 0);
                    Console.WriteLine($"Alpha parameter recieved successfully: {alpha}");

                    int[] origHist = new int[histSize];
                    int[] adjHist = new int[histSize];

                    byte[] origHistBytes = new byte[histByteSize];
                    byte[] adjHistBytes = new byte[histByteSize];

                    byte[] adjustedImageData = CLHE.StartCLHE(imageData, alpha, ref origHist, ref adjHist);
                    Console.WriteLine("Image was processed successfully.");


                    Buffer.BlockCopy(origHist, 0, origHistBytes, 0, histByteSize);
                    Buffer.BlockCopy(adjHist, 0, adjHistBytes, 0, histByteSize);

                    byte[] imageSize = BitConverter.GetBytes(adjustedImageData.Length);
                    stream.Write(imageSize, 0, imageSize.Length);
                    stream.Write(adjustedImageData, 0, adjustedImageData.Length);
                    Console.WriteLine("Image was successfully sent to client.");

                    stream.Write(origHistBytes, 0, histByteSize);
                    stream.Write(adjHistBytes, 0, histByteSize);
                    Console.WriteLine("Histograms were successfully sent to client.");

                    Array.Clear(lengthBuffer, 0, lengthBuffer.Length);
                    Array.Clear(imageData, 0, imageData.Length);
                    Array.Clear(doubleBuffer, 0, doubleBuffer.Length);
                    Array.Clear(origHistBytes, 0, origHistBytes.Length);
                    Array.Clear(adjHistBytes, 0, adjHistBytes.Length);

                    Console.WriteLine(Environment.NewLine);
                }

            }
            catch (IOException ex)  
            {
                Console.WriteLine($"Соединение с клиентом было закрыто: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке данных: {ex.Message}");
                return;
            }

            finally
            {
                stream.Close();
                client.Close();
                Console.WriteLine($"Клиент отключился.");

            }
        }

        private static  Task ExecuteServer()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, port);

            server.Start();

            Console.WriteLine($"Server is up on adress {IPAddress.Loopback} and port {port}");

            while (true)
            {
                Console.WriteLine("Awaiting client connection ...");
                TcpClient client =  server.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                 HandleClientConnection(client);
                
            }
        }
    }
}
