using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.ComponentModel;


namespace BrightnessAdjustmentClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient? client;
        private NetworkStream? stream;

        const string ipAdress = "127.0.0.1";
        const int port = 8080;

        Brush gridBrush = (Brush)(new BrushConverter().ConvertFromString("#ADADAD")!);
        Brush origCurveBrush = (Brush)(new BrushConverter().ConvertFromString("#F0FFFFFF")!);
        Brush adjCurveBrush = (Brush)(new BrushConverter().ConvertFromString("#F000FF00")!);

        int[] origHist = new int[256];
        int[] adjHist = new int[256];

        double alpha = 0;

        string filepath = "";

        System.Drawing.Image? originalImage;
        System.Drawing.Image? adjustedImage;

        byte[] originalData = new byte[1];
        byte[] adjustedData = new byte[1];

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
            HistogramCanvas.SizeChanged += CanvasSizeChanged;
            AlphaSlider.ValueChanged += SliderValueChanged;
            OpenImageBtn.MouseLeftButtonDown += OpenImage;
            this.Closing += DisconnectFromServer;
            Show();
            
            DrawGrid();

        }

        private void DisconnectFromServer(object? sender, CancelEventArgs e)
        {
            client!.Close();
        }

        private void Normalize(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool res = SendImageToServer();
                if (res == false)
                {
                    MessageBox.Show("Сервер разорвал соединение. Запустите приложение после того, как убедитесь, что сервер запущен!");
                    Close();
                }
                else
                {
                    RecieveResponseFromServer();

                    MemoryStream ms = new MemoryStream(adjustedData);
                    adjustedImage = System.Drawing.Image.FromStream(ms);

                    DisplayImage(adjustedImage);
                    DrawBarHistogram(adjHist, adjCurveBrush);
                    DrawBarHistogram(origHist, origCurveBrush);                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка!{ex.ToString()}");
                return;
            }

        }

        private void OpenImage(object sender, MouseButtonEventArgs e)
        {            
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Открыть изображение",
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog() == true)
            {
                AlphaSlider.Value = 1;
                origHist = new int[256];
                adjHist = new int[256];
                HistogramCanvas.Children.Clear();
                DrawGrid();

                SaveImageBtn.Cursor = Cursors.Arrow;
                SaveImageBtn.MouseLeftButtonDown -= SaveImage;

                filepath = dialog.FileName;
                byte[] fileData = File.ReadAllBytes(filepath);

                System.Drawing.Image tmpImage;
                try
                {
                    string extension = System.IO.Path.GetExtension(filepath).ToLower();
                    if (extension != ".jpeg" && extension != ".jpg" && extension != ".png")
                    {
                        MessageBox.Show("Выбранный файл не является изображением формата JPEG или PNG.",
                                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    tmpImage = System.Drawing.Image.FromFile(filepath);
                    
                }
                catch
                {
                    MessageBox.Show("Выбранный файл не является изображением!", "Ошибка!");
                    return;
                }
                imageName.Content = filepath.Substring(filepath.LastIndexOf('\\') + 1);
                imageSize.Content = $"{Math.Round((double)fileData.Length / 1024, 2)} КБ";
                imageRes.Content = $"{tmpImage.Width}x{tmpImage.Height} px.";
                imageColorSize.Content = tmpImage.PixelFormat.ToString();

                originalData = fileData;
                originalImage = tmpImage;
                DisplayImage(originalImage);

                Apply.Cursor = Cursors.Hand;
                Apply.MouseLeftButtonDown += Normalize;
            }
                        
        }

        private ImageSource ConvertToImageSource(System.Drawing.Image drawingImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Сохраняем System.Drawing.Image в поток
                drawingImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Загружаем поток в BitmapImage
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string value = AlphaSlider.Value.ToString();
            value = value.Length > 3 ? value.Substring(0, 3): value;
            SliderValueTextBox.Text = value;
            alpha = AlphaSlider.Value;
        }

        private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HistogramCanvas.Height = HistogramCanvas.Width;
            DrawGrid();
            if(adjHist != new int[256])
            {
                DrawBarHistogram(adjHist, adjCurveBrush);
                DrawBarHistogram(origHist, origCurveBrush);
            }
        }

        private void DrawBarHistogram(int[] histogram, Brush brushColor)
        {
            double canvasWidth = HistogramCanvas.ActualWidth;
            double canvasHeight = HistogramCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0) return;

            int max = histogram.Max();

            double barWidth = canvasWidth / histogram.Length;
            double yScale = canvasHeight / max;

            for (int i = 0; i < histogram.Length; i++)
            {
                double height = histogram[i] * yScale;
                double x = i * barWidth;
                double y = canvasHeight - height; 

                Rectangle rect = new Rectangle
                {
                    Fill = brushColor,
                    Width = barWidth, 
                    Height = height
                };

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                HistogramCanvas.Children.Add(rect);
            }
        }

        private void DrawGrid()
        {
            HistogramCanvas.Children.Clear();
            
            double canvasWidth = HistogramCanvas.ActualWidth;
            double canvasHeight = HistogramCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0) return;

            double cellWidth = canvasWidth / 8;
            double cellHeight = canvasHeight / 8;

            for (int i = 1; i < 8; i++)
            {
                double x = i * cellWidth;
                var line = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = canvasHeight,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                HistogramCanvas.Children.Add(line);
            }

            for (int i = 1; i < 8; i++)
            {
                double y = i * cellHeight;
                var line = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = canvasWidth,
                    Y2 = y,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                HistogramCanvas.Children.Add(line);
            }
        }

        private void DisplayImage(System.Drawing.Image image)
        {
            ImagePlaceholder.Source = ConvertToImageSource(image);
        }

        private bool SendImageToServer()
        {
            try
            {
                byte[] length = BitConverter.GetBytes(originalData.Length);
                stream!.Write(length, 0, 4);

                stream.Write(originalData, 0, originalData.Length);

                byte[] doubleBuff = BitConverter.GetBytes(alpha);
                stream.Write(doubleBuff, 0, 8);                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке изображения: {ex.Message}");
                return false;
            }
            return true;
        }

        private void RecieveResponseFromServer()
        {
            try
            {
                byte[] respLength = new byte[4];
                stream!.Read(respLength, 0, 4);
                int responseLength = BitConverter.ToInt32(respLength, 0);

                byte[] tmpAdjData = new byte[responseLength];
                int bytesRead = 0;
                while (bytesRead < responseLength)
                    bytesRead += stream.Read(tmpAdjData, bytesRead, responseLength - bytesRead);

                adjustedData = tmpAdjData;

                byte[] origHistData = new byte[256 * 4];
                stream.Read(origHistData, 0, origHistData.Length);
                origHist = new int[256];
                Buffer.BlockCopy(origHistData, 0, origHist, 0, origHistData.Length);

                byte[] adjHistData = new byte[256 * 4];
                stream.Read(adjHistData, 0, adjHistData.Length);
                adjHist = new int[256];
                Buffer.BlockCopy(adjHistData, 0, adjHist, 0, adjHistData.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при при получении ответа от сервера: {ex.Message}");
                return;
            }

            SaveImageBtn.Cursor = Cursors.Hand;
            SaveImageBtn.MouseLeftButtonDown += SaveImage;
        }

        private void SaveImage(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "Сохранить обработанное изображение",
                Filter = "PNG (*.png)|*.png|JPEG (*.jpeg;*.jpg;*.jfif)|*.jpg|Все файлы (*.*)|*.*",
                InitialDirectory = System.IO.Path.GetDirectoryName(filepath)
            };
            if(dialog.ShowDialog() == true)
            {
                if(dialog.FileName != string.Empty && adjustedImage != null)
                {
                    try
                    {
                        adjustedImage.Save(dialog.FileName);
                        MessageBox.Show("Измененное изображение было успешно сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Изображение не было сохранено.{Environment.NewLine}Ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
        }
    }
}
