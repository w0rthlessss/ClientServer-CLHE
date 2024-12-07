using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace CLHE_Algorithm
{
    public class CLHE
    {
        /// <summary>
        /// Class based on YCbCr color space
        /// </summary>
        private class YCbCr
        {
            public byte Y { get; set; }
            public byte Cb { get; set; }
            public byte Cr { get; set; }

            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

            /// <summary>
            /// Converting origin color channel values into YCbCr color space
            /// </summary>
            /// <param name="r"> Red color channel value </param>
            /// <param name="g"> Green color channel value </param>
            /// <param name="b"> Blue color channel value </param>
            public YCbCr(byte r, byte g, byte b)
            {
                Y = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
                Cb = (byte)(0.5 * b - 0.168736 * r - 0.331264 * g + 128);
                Cr = (byte)(0.5 * r - 0.418688 * g - 0.081312 * b + 128);
                R = r;
                G = g;
                B = b;
            }

            /// <summary>
            /// Converting color channel values back to RGB after setting new Y channel value
            /// </summary>
            public void TransformToRGB()
            {
                R = (byte)(Math.Min(255, Math.Abs(Y + 1.402 * (Cr - 128))));
                G = (byte)(Math.Min(255, Math.Abs(Y - 0.344136 * (Cb - 128) - 0.714136 * (Cr - 128))));
                B = (byte)(Math.Min(255, Math.Abs(Y + 1.772 * (Cb - 128))));
            }

        }

        static double alpha = 2.0f;
        const int histSize = 256;
        static int clipLimit = 1;


        /// <summary>
        /// Converting byte array into bitmap (image)
        /// </summary>
        /// <param name="imageData"> origin byte array </param>
        /// <returns></returns>
        private static Image ConvertByteArrayToBitmap(byte[] imageData)
        {
            MemoryStream ms = new MemoryStream(imageData);
            return Image.FromStream(ms);
        }


        /// <summary>
        /// Creation of image brightness histogram
        /// </summary>
        /// <param name="image"> Origin image</param>
        /// <param name="colorValues"> Matrix of saved origin color channel values</param>
        /// <returns> int array as histogram</returns>
        private static int[] MakeHistogram(Bitmap image, YCbCr[][] colorValues)
        {
            int x;

            int[] histogram = new int[histSize];
            for (int y = 0; y < image.Height; y++)
            {
                for (x = 0; x < image.Width; x++)
                {
                    YCbCr colorValue = colorValues[y][x];
                    histogram[colorValue.Y]++;
                }
            }

            return histogram;

        }


        /// <summary>
        /// Redistribute histogram values, using clip limt
        /// </summary>
        /// <param name="histogram"> Origin histogram </param>
        private static void RedistributeHistogram(ref int[] histogram)
        {
            int excesses = 0;

            for (int i = 0; i < histSize; i++)
            {
                int curExcess = histogram[i] - clipLimit;
                if (curExcess > 0) excesses += curExcess;
            }

            double binIncrement = Math.Round((double)(excesses / histSize));
            double upperLimit = clipLimit - binIncrement;

            for (int i = 0; i < histSize; i++)
            {
                if (histogram[i] > clipLimit) histogram[i] = clipLimit;
                else
                {
                    if (histogram[i] > upperLimit)
                    {
                        excesses -= histogram[i] - (int)upperLimit;
                        histogram[i] = (int)upperLimit;
                    }
                    else
                    {
                        excesses -= (int)binIncrement;
                        histogram[i] += (int)binIncrement;
                    }
                }
            }

            int oldExcesses = 0;

            do
            {
                int end = histSize - 1, start = 0;
                oldExcesses = excesses;

                while (excesses > 0 && start < end)
                {
                    int stepSize = histSize / excesses;
                    if (stepSize < 1) stepSize = 1;

                    for (int i = start; i < end && excesses > 0; i += stepSize)
                    {
                        if (histogram[i] < clipLimit)
                        {
                            histogram[i]++;
                            excesses--;
                        }
                    }
                    start++;
                }
            } while (excesses > 0 && excesses < oldExcesses);


        }


        /// <summary>
        /// Histogram equalization algprithm
        /// </summary>
        /// <param name="histogram"> Origin histogram </param>
        /// <param name="height"> Image height</param>
        /// <param name="width"> Image width </param>
        /// <returns></returns>
        private static byte[] Equalization(int[] histogram, int height, int width)
        {
            int n = width * height;

            int[] cdf = histogram;

            for (int i = 1; i < histSize; i++)
                cdf[i] += cdf[i - 1];

            double cdfMin = cdf.Where(q => q > 0).Min();

            byte[] lut = new byte[256];
            for (int i = 0; i < histSize; i++)
                lut[i] = (byte)(((cdf[i] - cdfMin) * 255 / (n - cdfMin)) + 0.5);

            return lut;
        }


        /// <summary>
        /// Saving origin color channel values into matrix
        /// </summary>
        /// <param name="image"> Origin image </param>
        /// <returns> </returns>
        private unsafe static YCbCr[][] SaveColorValues(Bitmap image)
        {
            YCbCr[][] colorValues = new YCbCr[(ulong)image.Height][];

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            byte* pixelPointer = (byte*)imageData.Scan0.ToPointer();

            int x;
            for (int y = 0; y < image.Height; y++)
            {
                colorValues[y] = new YCbCr[(ulong)image.Width];
                for (x = 0; x < image.Width; x++)
                {
                    int index = y * imageData.Stride + x * bytesPerPixel;

                    byte blue = pixelPointer[index];
                    byte green = pixelPointer[index + 1];
                    byte red = pixelPointer[index + 2];

                    colorValues[y][x] = new YCbCr(red, green, blue);
                }
            }

            image.UnlockBits(imageData);

            return colorValues;
        }

        /// <summary>
        /// Applying CLHE brightness adjustment algorithm
        /// </summary>
        /// <param name="image"></param>
        /// <returns> adjusted image </returns>
        private unsafe static Image ApplyCLHE(Bitmap image)
        {
            YCbCr[][] colorValues = SaveColorValues(image);
            clipLimit = (int)(Math.Ceiling(alpha * image.Width * image.Height / histSize));
            int[] hist = MakeHistogram(image, colorValues);
            RedistributeHistogram(ref hist);
            byte[] lut = Equalization(hist, image.Height, image.Width);

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, image.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            byte* pixelPointer = (byte*)imageData.Scan0.ToPointer();

            int x;
            for (int y = 0; y < image.Height; ++y)
            {
                for (x = 0; x < image.Width; ++x)
                {
                    colorValues[y][x].Y = lut[colorValues[y][x].Y];
                    colorValues[y][x].TransformToRGB();

                    int index = y * imageData.Stride + x * bytesPerPixel;

                    pixelPointer[index] = colorValues[y][x].B;
                    pixelPointer[index + 1] = colorValues[y][x].G;
                    pixelPointer[index + 2] = colorValues[y][x].R;
                }
            }

            image.UnlockBits(imageData);

            return image;
        }


        /// <summary>
        /// Entry function to algorithm
        /// </summary>
        /// <param name="originData"> image as byte array</param>
        /// <param name="a"> floating point value for calculating clip limit</param>
        /// <returns> byte array of adjusted image </returns>
        public static byte[] StartCLHE(byte[] originData, double a)
        {
            alpha = a;

            if (alpha < 1) return originData;

            Bitmap originImage = (Bitmap)ConvertByteArrayToBitmap(originData);
            Image adjustedImage = ApplyCLHE(originImage);

            using(var ms = new MemoryStream())
            {
                adjustedImage.Save(ms, adjustedImage.RawFormat);
                return ms.ToArray();
            }
        }
    }


}
