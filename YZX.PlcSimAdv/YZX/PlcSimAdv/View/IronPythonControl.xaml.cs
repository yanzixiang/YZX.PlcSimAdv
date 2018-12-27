using System.Windows;

using CommonServiceLocator;
using HandyControl.Controls;
using PythonConsoleControl;

using YZX.PlcSimAdv.Task;
using YZX.PlcSimAdv.ViewModel;

using Extensions;

namespace YZX.PlcSimAdv.View
{
  public partial class IronPythonControl
  {
    CPUControlViewModel CPU;
    public IronPythonControl()
    {
      InitializeComponent();
      CPU = ServiceLocator.Current.GetInstance<CPUControlViewModel>();
      Loaded += IronPythonControl_Loaded;
    }

    private void IronPythonControl_Loaded(object sender, RoutedEventArgs e)
    {
      Console.WithHost(AfterConsoleLoaded);
    }

    public void Clear()
    {
      Dispatcher.Invoke(() =>
      {
        Console.Pad.Control.Clear();
      });
    }

    public void AfterConsoleLoaded(PythonConsoleHost host)
    {
      
      Console.SetVariable("CPU", CPU);

      Console.SetVariable("VIEW", this);

      var LoadedDlls =   AssemblyExtension.GetLoadedAssemblyWithoutDynamic();
      Console.SetVariable("LoadedDlls", LoadedDlls);

      Console.UpdateVariables();

      Console.Console.ExecuteFile(@"IronPythonTask\Init.ipy");
    }

    public void AddIronPythonDebugger(string name)
    {
      Dispatcher.Invoke(() =>
      {
        IronPythonTask task;
        bool getResult = CPU.IronPythonTasks.TryGetValue(name, out task);
        if (getResult)
        {
          TabItem item = new TabItem();
          IronPythonDebuger debugger = new IronPythonDebuger();
          debugger.ConnectToTask(task);
          item.Content = debugger;
          MainTab.Items.Add(item);
        }
      });
    }

    public void UpdateIronPythonDebuggers()
    {
    }

  }
}