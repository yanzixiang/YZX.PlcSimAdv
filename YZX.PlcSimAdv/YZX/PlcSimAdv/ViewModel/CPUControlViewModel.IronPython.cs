using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Xml.Serialization;

using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using IronPython.Runtime.Operations;

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
      IronPythonTasks.Add(task);

      task = new IronPythonTask("Task1", @"IronPythonTask\Task2.ipy");
      IronPythonTasks.Add(task);


      foreach(var itask in IronPythonTasks)
      {
        itask.Init();
      }

      IronPythonTaskInited = true;
    }

    #endregion

    #region 属性
    public bool IronPythonTaskInited = false;
    public List<IronPythonTask> IronPythonTasks = new List<IronPythonTask>();
    #endregion
  }
}
