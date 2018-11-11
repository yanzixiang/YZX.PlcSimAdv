using System;
using System.Diagnostics;

namespace Siemens.Simatic.PlcSim.Advanced.AdapterConfigurator
{
  internal class Program
  {
    private static IConfigureAdaptersNET ConfigureCommunicationAdapters;

    private static int Main(string[] args)
    {
      Debugger.Launch();
      ConfigureCommunicationAdapters = new IConfigureAdaptersNETImpl();
      int num = -2;
      if (args.Length == 2)
      {
        try
        {
          string str = args[0];
          string AdapterName = args[1];
          if (str.Length > 50 || AdapterName.Length > 200)
            return num;
          if (!(str == "EnableVirtualSwitch"))
          {
            num = str == "DisableVirtualSwitch" ? (ConfigureCommunicationAdapters.ConfigureVirtualSwitch(AdapterName, false) ? 0 : -5) : (str == "ResetVirtualSwitchAndIPv6" ? (ConfigureCommunicationAdapters.ResetNetworkSettings() ? 0 : -5) : -4);
          }
          else
          {
            ConfigureCommunicationAdapters.ResetNetworkSettings();
            num = ConfigureCommunicationAdapters.ConfigureVirtualSwitch(AdapterName, true) ? 0 : -5;
          }
        }
        catch (Exception ex)
        {
          num = -3;
        }
      }
      return num;
    }
  }
}
