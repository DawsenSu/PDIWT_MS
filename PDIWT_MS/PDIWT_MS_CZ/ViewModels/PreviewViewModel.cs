using System;
using DevExpress.Mvvm;
using Bentley.Interop.MicroStationDGN;

using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PDIWT_MS_CZ.ViewModels
{
    public class PreviewViewModel : ViewModelBase
    {
        public SmartSolidElement CZ
        {
            get { return GetProperty(() => CZ); }
            set { SetProperty(() => CZ, value); }
        }

        public ImageSource Image1
        {
            get { return GetProperty(() => Image1); }
            set { SetProperty(() => Image1, value); }
        }
        //public ImageSource Image2
        //{
        //    get { return GetProperty(() => Image2); }
        //    set { SetProperty(() => Image2, value); }
        //}
        //public ImageSource Image3
        //{
        //    get { return GetProperty(() => Image3); }
        //    set { SetProperty(() => Image3, value); }
        //}
        //public ImageSource Image4
        //{
        //    get { return GetProperty(() => Image4); }
        //    set { SetProperty(() => Image4, value); }
        //}

        public void GetAllIamge()
        {
            System.Drawing.Size picsize = new System.Drawing.Size { Height = 300, Width = 300 };

            SmartSolidElement cz1, cz2, cz3, cz4;
            cz1 = CZ.Clone().AsSmartSolidElement; cz2 = CZ.Clone().AsSmartSolidElement; cz3 = CZ.Clone().AsSmartSolidElement; cz4 = CZ.Clone().AsSmartSolidElement;

            Point3d origin = CZ.Origin;

            Bitmap[] bitmaparray = new Bitmap[4];
            //CZ.RotateAboutZ(ref origin, Math.PI);
            IntPtr metafilehandle = new IntPtr(cz1.DrawToEnhancedMetafile(picsize.Width, picsize.Height, true));
            bitmaparray[0] = new Bitmap(new System.Drawing.Imaging.Metafile(metafilehandle, true));
            //Image3 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            cz2.Rotate(ref origin, Math.PI* 5/ 4, 0, Math.PI/4);
            metafilehandle = new IntPtr(cz2.DrawToEnhancedMetafile(picsize.Width, picsize.Height, true));
            bitmaparray[1] = new Bitmap(new System.Drawing.Imaging.Metafile(metafilehandle, true));
            //Image2 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            cz3.Rotate(ref origin, - Math.PI / 2, 0, 0);  
            metafilehandle = new IntPtr(cz3.DrawToEnhancedMetafile(picsize.Width, picsize.Height, true));
            bitmaparray[2] = new Bitmap(new System.Drawing.Imaging.Metafile(metafilehandle, true));
            //Image1  = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());


            cz4.Rotate(ref origin,- Math.PI / 2, 0, Math.PI/2);
            metafilehandle = new IntPtr(cz4.DrawToEnhancedMetafile(picsize.Width, picsize.Height, true));
            bitmaparray[3] = new Bitmap(new System.Drawing.Imaging.Metafile(metafilehandle, true));
            //Image4 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            Image1 = BitmapHelper.MergeBitmapToArray(bitmaparray, 2, 2, 5);
        }

        Bentley.Interop.MicroStationDGN.Application app = Program.COM_App;
    }
}