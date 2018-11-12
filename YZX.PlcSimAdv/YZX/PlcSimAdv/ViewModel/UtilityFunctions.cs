using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YZX.PlcSimAdv.ViewModel
{
  public class UtilityFunctions
  {
    public static ImageSource CreateBitmapSource(Bitmap bitmap)
    {
      BitmapSource sourceFromHbitmap = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
      sourceFromHbitmap.Freeze();
      return sourceFromHbitmap;
    }

    public static ImageSource CreateBitBitmapSourceFromPath(string path)
    {
      var ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
      return ImageSource;
    }
  }
}
