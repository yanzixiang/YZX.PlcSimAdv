using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net;

namespace YZX.PlcSimAdv.ViewModel
{
  public static class UtilityFunctions
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

    public static long ConvertIp(this string octet)
    {
      return Convert.ToInt64(Convert.ToByte(octet));
    }

    public static long ConvertIpAddressStringToLong(this string ipAddress)
    {
      long num = 0L;
      string str = ipAddress;
      char[] chArray = new char[1]
      {
        '.'
      };
      string[] ss = str.Split(chArray);
      IEnumerable<string> ns = ss.Reverse();
      foreach (string octet in ns)
      {
        num = num << 8 | ConvertIp(octet);
      }
      return num;
    }

    public static IPAddress ToIPAddress(this string ipAddress)
    {
      IPAddress address = new IPAddress(ipAddress.ConvertIpAddressStringToLong());
      return address;
    }
  }
}
