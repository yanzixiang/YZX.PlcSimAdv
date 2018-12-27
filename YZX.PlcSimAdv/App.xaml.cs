using System;
using System.Diagnostics;
using System.Windows;

using YZX.PlcSimAdv.View;

namespace YZX.PlcSimAdv
{
  public partial class App:Application
  {
    public App()
    {
      Exit += App_Exit;
    }

    private void App_Exit(object sender, ExitEventArgs e)
    {
      ControlPanelHost.KillControlPanel();
    }
  }
}
