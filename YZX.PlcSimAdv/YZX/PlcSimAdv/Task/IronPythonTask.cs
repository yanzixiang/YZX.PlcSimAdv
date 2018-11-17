using System;
using System.Threading;
using System.Xml.Serialization;
using System.Collections.Generic;

using Microsoft.Scripting.Hosting;

using IronPython.Hosting;
using IronPython.Runtime.Exceptions;

using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using IronPython.Runtime.Operations;

namespace YZX.PlcSimAdv.Task
{
  /// <summary>
  /// IronPython任务
  /// </summary>
  public class IronPythonTask
  {
    [XmlIgnore]
    public ScriptEngine Engine;

    [XmlIgnore]
    public ScriptScope Scope;

    private ScriptSource scriptSource;
    private CompiledCode compiledCode;
    CodeContext codeContext = DefaultContext.Default;

    [XmlIgnore]
    public Timer ResumeTimer;
    public bool JMC = true;

    public string Name { get; set; }

    public string Path { get; set; }

    public bool CompileSucess { get; private set; }

    /// <summary>
    /// 上次开始执行时间
    /// </summary>
    [XmlIgnore]
    public DateTime LastStartTime { get; protected set; }

    /// <summary>
    /// 上次结束执行时间
    /// </summary>
    [XmlIgnore]
    public DateTime LastFinishTime { get; protected set; }

    /// <summary>
    /// 创建IronPython任务
    /// </summary>
    /// <param name="pyfile">任务文件</param>
    public IronPythonTask(string name, string path)
    {
      Name = name;
      Path = path;
    }

    [XmlIgnore]
    public double RunTimes { get; private set; }

    public void Resume(object state = null)
    {
      _dbgContinue.Set();
    }
    AutoResetEvent _dbgContinue = new AutoResetEvent(false);

    private TracebackDelegate OnTracebackReceived(TraceBackFrame frame, string result, object payload)
    {
      if (TracebackEvent != null)
      {
        bool NotMyCode = frame.f_code.co_filename != Path;

        if (JMC & NotMyCode)
        {
          Console.WriteLine("Skip");
          return OnTracebackReceived;
        }
        IPYTracebackEventArgs args = new IPYTracebackEventArgs();
        args.frame = frame;
        args.result = result;
        args.payload = payload;
        try
        {
          //Console.WriteLine(String.Format("{0},{1},{2}",frame,result,payload));
          TracebackEvent(this, args);
        }catch(Exception ex)
        {
          Console.WriteLine(ex);
        }
      }
      WaitInput();
      return OnTracebackReceived;
    }

    private void WaitInput()
    {
      //Console.WriteLine("StartWaitInput"); 
      if (monitors.Count > 0)
      {
        _dbgContinue.WaitOne();
      }
    }


    public event EventHandler<IPYTracebackEventArgs> TracebackEvent;
    public event EventHandler<YZXTaskExceptionEventArgs> ExceptionEvent;

    public void Reload()
    {
      try
      {
        Engine = Python.CreateEngine();
        Scope = Engine.CreateScope();

        scriptSource = Engine.CreateScriptSourceFromFile(Path);
        compiledCode = scriptSource.Compile();
        compiledCode.Execute(Scope);

        Engine.SetTrace(OnTracebackReceived);

        CompileSucess = true;
      }
      catch(Exception ex)
      {
        CompileSucess = false;

      }
    }

    public void Init()
    {
      try
      {
        Reload();
        
        PythonFunction InitAction = (PythonFunction)GetVariable("Init");


        PythonCalls.Call(codeContext, InitAction);
      }
      catch (ImportException e)
      {
      }
    }

    public bool RunOneTime()
    {

      if(CompileSucess != true)
      {
        return false;
      }

      try
      {
        LastStartTime = DateTime.Now;

        PythonFunction InitAction = (PythonFunction)GetVariable("RunOneTime");


        PythonCalls.Call(codeContext, InitAction);


        LastFinishTime = DateTime.Now;
        return true;
      }catch(Exception ex)
      {
        YZXTaskExceptionEventArgs args = new YZXTaskExceptionEventArgs();
        args.exception = ex;
        ExceptionEvent(this,args);
      }
      return false;
    }

    public void Reset()
    {
    }

    public object GetVariable(string name)
    {
      return Scope.GetVariable(name);
    }
    public T GetVariable<T>(string name)
    {
      return Scope.GetVariable<T>(name);
    }

    private List<IronPythonTaskMonitor> monitors = new List<IronPythonTaskMonitor>();
    public void AddMonitor(IronPythonTaskMonitor monitor)
    {
      monitors.Add(monitor);
    }

    public void RemoveMonitor(IronPythonTaskMonitor monitor)
    {
      monitors.Remove(monitor);
    }
  }
}