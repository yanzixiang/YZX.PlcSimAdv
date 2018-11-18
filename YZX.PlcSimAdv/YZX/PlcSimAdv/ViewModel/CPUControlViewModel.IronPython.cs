using System;
using System.Collections.Generic;


using System.Reactive.Linq;

using YZX.PlcSimAdv.Task;

namespace YZX.PlcSimAdv.ViewModel
{
  partial class CPUControlViewModel
  {
    #region 方法
    public void InitIronPythonTasks()
    {
      IronPythonTaskInited = false;
      IronPythonTask task;
        task = new IronPythonTask("Task1",@"IronPythonTask\Task1.ipy");
      IronPythonTasks["Task1"] = task;

      task = new IronPythonTask("Task1", @"IronPythonTask\Task2.ipy");
      IronPythonTasks["Task2"] = task;

      SyncPointCount.Subscribe(i => {
        RunPythonTasks();
      });

      foreach (var itask in IronPythonTasks)
      {
        itask.Value.Init();
      }

      IronPythonTaskInited = true;
    }

    public void RunPythonTasks()
    {
      if (IronPythonTaskInited)
      {
        foreach (var ironPythonTask in IronPythonTasks.Values)
        {
          ironPythonTask.RunOneTime();
        }
      }
    }

    #endregion

    #region 属性
    public bool IronPythonTaskInited = false;
    public Dictionary<string,IronPythonTask> IronPythonTasks = 
      new Dictionary<string, IronPythonTask>();
    #endregion
  }
}
