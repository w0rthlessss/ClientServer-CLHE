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

        private static void Main(string[] args)
        {
            /*byte[] imageData = File.ReadAllBytes("try.jpg");
            byte[] adj = CLHE.StartCLHE(imageData, 2);
            MemoryStream ms = new MemoryStream(adj);
            Image a = Image.FromStream(ms);
            a.Save("output.jpg");
            a.Dispose();*/
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
                    int imageLength = 0;
                    try
                    {
                        stream.Read(lengthBuffer, 0, 4);
                        imageLength = BitConverter.ToInt32(lengthBuffer, 0);
                        Console.WriteLine($"Image length recieved: {imageLength} bytes");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }                    

                    byte[] imageData = new byte[imageLength];
                    int bytesRead = 0;
                    try
                    {
                        while (bytesRead < imageLength)
                        {
                            bytesRead += stream.Read(imageData, bytesRead, imageLength - bytesRead);
                        }
                        Console.WriteLine("Image recieved successfully.");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }

                    byte[] doubleBuffer = new byte[8];
                    double alpha = 0;
                    try
                    {
                        stream.Read(doubleBuffer, 0, 8);
                        alpha = BitConverter.ToDouble(doubleBuffer, 0);
                        Console.WriteLine($"Alpha parameter recieved successfully: {alpha}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }

                    byte[] adjustedImageData = CLHE.StartCLHE(imageData, alpha);
                    Console.WriteLine("Image was processed successfully.");

                    byte[] response = BitConverter.GetBytes(adjustedImageData.Length);
                    try
                    {
                        stream.Write(response, 0, 4);
                        stream.Write(adjustedImageData, 0, adjustedImageData.Length);
                        Console.WriteLine("Image was successfully sent to client.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }

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
