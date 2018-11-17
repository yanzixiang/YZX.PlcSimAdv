using System;
using System.Collections.Generic;
using System.Net;
using IsoOnTcp;

namespace YZX.PlcSimAdv.ViewModel
{
  partial class CPUControlViewModel
  {
    public static IsoToS7online S7VPLCISOServer;
    public static int S7VPLCISOServerPort = 8102;
    public static string S7VPLCISOServerName = "S7VPLC";

    public static List<byte[]> S7VPLCTSAPS = new List<byte[]>()
    {
    new byte[] { 0x01, 0x01 },
    new byte[] { 0x02, 0x01 },
    new byte[] { 0x03, 0x01 }
    };

    public static void StartS7VPLCISOServer(string VPLCIP)
    {
      try
      {
        S7VPLCISOServer = new IsoToS7online(false);
        IPAddress localhost = "127.0.0.1".ToIPAddress();
        IPAddress address = VPLCIP.ToIPAddress();
        S7VPLCISOServer.start(S7VPLCISOServerName, localhost, S7VPLCTSAPS, address, 0, 1);
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}
