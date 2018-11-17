namespace YZX.PlcSimAdv.Task
{
  public interface IronPythonTaskMonitor
  {
    void ConnectToTask(IronPythonTask task);
    void DisconnectFromTask();
  }
}
