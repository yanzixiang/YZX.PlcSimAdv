using System;
using System.Windows;
using System.Diagnostics;

namespace YZX.PlcSimAdv.View
{
  public partial class ControlPanelHost
  {
    public string ControlPanelExePath =
      @"C:\Program Files (x86)\Siemens\Automation\PLCSIMADV\bin\Siemens.Simatic.PlcSim.Advanced.UserInterface.exe";
    public static string ControlPanelProcessName = "Siemens.Simatic.PlcSim.Advanced.UserInterface";
    public string ClassName = "ControlPanelFloatingWindow";
    
    public double PanelHeight = 650.51;
    public double PanelWidth = 462;
    public ControlPanelHost()
    {
      KillControlPanel();

      InitializeComponent();

      Host.ExeName = ControlPanelExePath;
      Host.ProcessName = ControlPanelProcessName;

      Loaded += ControlPanelHost_Loaded;
      Unloaded += ControlPanelHost_Unloaded;

    }

    public static void KillControlPanel()
    {
      try
      {
        var ps = Process.GetProcessesByName(ControlPanelProcessName);
        foreach (var p in ps)
        {
          p.Kill();
        }
      }catch(Exception ex)
      {

      }
    }

    private void ControlPanelHost_Unloaded(object sender, RoutedEventArgs e)
    {
      
    }

    private void ControlPanelHost_Loaded(object sender, RoutedEventArgs e)
    {
    }
  }
}