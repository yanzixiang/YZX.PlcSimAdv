using System;
using System.Windows;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;

using Siemens.Simatic.Simulation.Runtime;
using Siemens.Simatic.PlcSim.Advanced;

namespace YZX.PlcSimAdv
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      Loaded += MainWindow_Loaded;

    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      EnableVirtualSwitch();

      a();
    }

    public readonly string EventLogName = @"YZX.PLCSimAdv";
    EventLog eventLog = new EventLog();
    public void InitEventLog()
    {
      eventLog = new EventLog(EventLogName);
      if (!EventLog.SourceExists("YZX.PLCSimAdv"))
      {
        EventLog.CreateEventSource("YZX.PLCSimAdv", "YZX.PLCSimAdv");
      }
    }

    public void Info(string message,int eventID=0, short category = 0, byte[] rawData = null)
    {
      eventLog.WriteEntry(message, EventLogEntryType.Information, eventID);
    }

    public void Error(string message, int eventID = 0, short category = 0, byte[] rawData = null)
    {
      eventLog.WriteEntry(message, EventLogEntryType.Error, eventID);
    }

    public void Warning(string message, int eventID = 0, short category = 0, byte[] rawData = null)
    {
      eventLog.WriteEntry(message, EventLogEntryType.Warning, eventID);
    }

    public void FailureAudit(string message, int eventID = 0, short category = 0, byte[] rawData = null)
    {
      eventLog.WriteEntry(message, EventLogEntryType.FailureAudit, eventID);
    }

    public void SuccessAudit(string message, int eventID = 0, short category = 0, byte[] rawData = null)
    {
      eventLog.WriteEntry(message, EventLogEntryType.SuccessAudit, eventID);
    }

    private static IConfigureAdaptersNET ConfigureCommunicationAdapters;
    IInstance instance;

    public static string AdapterName = "USB";
    public static string Name = "plcsim";
    public void EnableVirtualSwitch()
    {
      ConfigureCommunicationAdapters = new IConfigureAdaptersNETImpl();
      ConfigureCommunicationAdapters.ConfigureVirtualSwitch(AdapterName, true);
    }

    public bool HasName(string name)
    {
      SInstanceInfo[] SInstanceInfos = SimulationRuntimeManager.RegisteredInstanceInfo;
      foreach(SInstanceInfo instanceInfo in SInstanceInfos)
      {
        if(instanceInfo.Name == name)
        {
          return true;
        }
      }
      return false;
    }

    public void a()
    {

      try
      {
        if (HasName(Name))
        {
          instance = SimulationRuntimeManager.CreateInterface(Name);
        }
        else
        {
          instance = SimulationRuntimeManager.RegisterInstance(ECPUType.CPU1513, Name);
        }

        if (instance.OperatingState != EOperatingState.Run)
        {

          instance.CommunicationInterface = ECommunicationInterface.TCPIP;

          instance.OnConfigurationChanged += Instance_OnConfigurationChanged;
          instance.OnConfigurationChanging += Instance_OnConfigurationChanging;
          instance.OnEndOfCycle += Instance_OnEndOfCycle;
          instance.OnLedChanged += Instance_OnLedChanged;
          instance.OnOperatingStateChanged += Instance_OnOperatingStateChanged;

          instance.PowerOn(1000);

          Console.WriteLine(instance.ControllerShortDesignation);
          Console.WriteLine(instance.StoragePath);

          SIPSuite4 SIPSuite4 = new SIPSuite4("192.168.2.12", "255.255.255.0", "192.168.2.126");

          instance.SetIPSuite(0, SIPSuite4, true);


          instance.Run();

        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {

      }
    }

    private void Instance_OnOperatingStateChanged(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime, 
      EOperatingState in_PrevState, 
      EOperatingState in_OperatingState)
    {
    }

    private void Instance_OnLedChanged(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime, 
      ELEDType in_LEDType, 
      ELEDMode in_LEDMode)
    {
    }

    private void Instance_OnEndOfCycle(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime, 
      long in_CycleTime_ns, 
      uint in_CycleCount)
    {
    }

    private void Instance_OnConfigurationChanging(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime)
    {
    }

    private void Instance_OnConfigurationChanged(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime, 
      EInstanceConfigChanged in_InstanceConfigChanged, 
      uint in_Param1, 
      uint in_Param2, 
      uint in_Param3, 
      uint in_Param4)
    {
    }
  }
}
