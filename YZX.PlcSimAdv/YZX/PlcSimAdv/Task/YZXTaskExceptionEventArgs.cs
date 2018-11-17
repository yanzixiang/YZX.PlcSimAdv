using System;

using IronPython.Runtime.Exceptions;

namespace YZX.PlcSimAdv.Task
{
  public class YZXTaskExceptionEventArgs : EventArgs
  {
    public Exception exception;

    public YZXTaskExceptionEventArgs()
    {

    }
  }
}
