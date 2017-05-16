using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PDIWT_MS_CZ.ViewModels
{
    public static class BitmapHelper
    {
        public static BitmapSource MergeBitmapToArray(Bitmap[] bitmaps, int rownum, int colnum, int spacepixel)
        {
            if (bitmaps!=null && bitmaps.Length == (rownum*colnum))
            {
                System.Drawing.Size maxsize = new System.Drawing.Size();
                foreach (var bitmap in bitmaps)
                {
                    if (bitmap.Size.Width > maxsize.Width || bitmap.Size.Height > maxsize.Height)
                    {
                        maxsize = bitmap.Size;
                    }
                }
                System.Drawing.Size wholebitmapsize = new System.Drawing.Size(maxsize.Width * rownum + spacepixel * (rownum + 1), maxsize.Height * colnum + spacepixel * (colnum + 1));
                Bitmap wholebitmap = new Bitmap(wholebitmapsize.Width, wholebitmapsize.Height);
                Graphics g = Graphics.FromImage(wholebitmap);
                for (int i = 0; i < rownum; i++)
                {
                    for (int j = 0; j < colnum; j++)
                    {
                        g.DrawImage(bitmaps[i + j * rownum], new Rectangle(new System.Drawing.Point((i + 1) * spacepixel + i * maxsize.Width, (j + 1) * spacepixel + j * maxsize.Height), maxsize));
                    }
                }
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(wholebitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                              
            }
            else
            {
                return new BitmapImage();
            }
            
        }
    }
}
