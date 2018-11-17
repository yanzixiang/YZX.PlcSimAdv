using System.Windows;

using CommonServiceLocator;
using PythonConsoleControl;

using YZX.PlcSimAdv.ViewModel;

namespace YZX.PlcSimAdv.View
{
  public partial class IronPythonControl
  {
    public IronPythonControl()
    {
      InitializeComponent();
      Loaded += IronPythonControl_Loaded;
    }

    private void IronPythonControl_Loaded(object sender, RoutedEventArgs e)
    {
      Console.WithHost(AfterConsoleLoaded);
    }

    public void AfterConsoleLoaded(PythonConsoleHost host)
    {
      var cpu = ServiceLocator.Current.GetInstance<CPUControlViewModel>();
      Console.SetVariable("CPU", cpu);

      Console.SetVariable("self", this);

      Console.UpdateVariables();
    }
  }
}