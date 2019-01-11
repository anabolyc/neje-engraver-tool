using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NejeEngraverApp
{
    internal class ImageProcess
    {
        public static Bitmap getOriginPicture(Bitmap image, int threshold, int max_width, int max_height)
        {
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("ERROR! Image format is not supported.Image formate:" + image.PixelFormat.ToString());
                return new Bitmap(100, 100);
            }
            threshold = 255 - threshold;
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            byte[] array = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, array, 0, array.Length);
            image.UnlockBits(bitmapData);
            Rectangle srcRect = new Rectangle(0, 0, 0, 0);
            int num = 0;
            int num2 = 0;
            while ((int)array[num2 * bitmapData.Stride + num * 3] > threshold)
            {
                num++;
                if (num > image.Width - 1)
                {
                    num = 0;
                    num2++;
                    if (num2 == image.Height)
                    {
                        break;
                    }
                }
            }
            srcRect.Y = num2;
            num = image.Width - 1;
            num2 = image.Height - 1;
            while ((int)array[num2 * bitmapData.Stride + num * 3] > threshold)
            {
                num--;
                if (num < 0)
                {
                    num = image.Width - 1;
                    num2--;
                    if (num2 < 0)
                    {
                        num2 = 0;
                        break;
                    }
                }
            }
            srcRect.Height = num2 - srcRect.Y + 1;
            num = 0;
            num2 = 0;
            while ((int)array[num2 * bitmapData.Stride + num * 3] > threshold)
            {
                num2++;
                if (num2 > image.Height - 1)
                {
                    num2 = 0;
                    num++;
                    if (num == image.Width)
                    {
                        break;
                    }
                }
            }
            srcRect.X = num;
            num = image.Width - 1;
            num2 = image.Height - 1;
            while ((int)array[num2 * bitmapData.Stride + num * 3] > threshold)
            {
                num2--;
                if (num2 < 0)
                {
                    num2 = image.Height - 1;
                    num--;
                    if (num < 0)
                    {
                        num = 0;
                        break;
                    }
                }
            }
            srcRect.Width = num - srcRect.X + 1;
            Bitmap bitmap;
            if (srcRect.Width <= max_width && srcRect.Height <= max_height)
            {
                bitmap = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format24bppRgb);
            }
            else if ((float)srcRect.Width / (float)max_width > (float)srcRect.Height / (float)max_height)
            {
                bitmap = new Bitmap(max_width, srcRect.Height * max_width / srcRect.Width, PixelFormat.Format24bppRgb);
            }
            else
            {
                bitmap = new Bitmap(srcRect.Width * max_height / srcRect.Height, max_height, PixelFormat.Format24bppRgb);
            }
            Graphics expr_263 = Graphics.FromImage(bitmap);
            expr_263.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), srcRect, GraphicsUnit.Pixel);
            expr_263.Dispose();
            return bitmap;
        }

        public static Bitmap toBlack(Bitmap image, int filter)
        {
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("ERROR! Image format is not supported.");
                return new Bitmap(100, 100);
            }
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            byte[] array = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, array, 0, array.Length);
            image.UnlockBits(bitmapData);
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            filter *= 3;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if ((int)(array[j * bitmapData.Stride + i * 3] + array[j * bitmapData.Stride + i * 3 + 1] + array[j * bitmapData.Stride + i * 3 + 2]) < filter)
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }
            graphics.Dispose();
            return bitmap;
        }

        public static Bitmap toShake(Bitmap image)
        {
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("ERROR! Image format is not supported.");
                return new Bitmap(100, 100);
            }
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            byte[] array = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, array, 0, array.Length);
            image.UnlockBits(bitmapData);
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            short[,] array2 = new short[image.Width + 1, image.Height + 1];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    array2[i, j] = (short)((array[j * bitmapData.Stride + i * 3 + 2] * 3 + array[j * bitmapData.Stride + i * 3 + 1] * 6 + array[j * bitmapData.Stride + i * 3]) / 10);
                }
            }
            for (int k = 0; k < image.Width; k++)
            {
                for (int l = 0; l < image.Height; l++)
                {
                    short num;
                    if (array2[k, l] > 128)
                    {
                        num = (short)(array2[k, l] - 255);
                    }
                    else
                    {
                        bitmap.SetPixel(k, l, Color.Black);
                        num = array2[k, l];
                    }
                    array2[k + 1, l] = (short)(num * 3 / 8 + array2[k + 1, l]);
                    array2[k, l + 1] = (short)(num * 3 / 8 + array2[k, l + 1]);
                    array2[k + 1, l + 1] = (short)(num / 4 + array2[k + 1, l + 1]);
                }
            }
            graphics.Dispose();
            return bitmap;
        }
    }
}
