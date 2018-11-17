using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using HandyControl.Controls;

using Siemens.Simatic.PlcSim.Advanced;
using Siemens.Simatic.Simulation.Runtime;

namespace YZX.PlcSimAdv.ViewModel
{
  public partial class CPUControlViewModel : ViewModelBase
  {
    public CPUControlViewModel()
    {
      this.PowerOnImage = UtilityFunctions.CreateBitBitmapSourceFromPath("/YZX/images/standby_power-on.png");
      this.PowerOffImage = UtilityFunctions.CreateBitBitmapSourceFromPath("/YZX/images/standby_power-on.png");
    }

    #region PowerImage

    private readonly ImageSource PowerOnImage;
    private readonly ImageSource PowerOffImage;

    private ImageSource m_PowerButtonImage;
    private void SetPowerButtonImage()
    {
      this.PowerButtonImageSource = true ? this.PowerOnImage : this.PowerOffImage;

      this.PowerPlcCommand = new RelayCommand(() => this.TogglePower());
    }
    public ImageSource PowerButtonImageSource
    {
      get
      {
        if (this.m_PowerButtonImage == null)
          this.SetPowerButtonImage();
        return this.m_PowerButtonImage;
      }
      set
      {
        if (this.m_PowerButtonImage == value)
          return;
        this.m_PowerButtonImage = value;
        this.RaisePropertyChanged("PowerButtonImageSource");
      }
    }
    #endregion

    #region 属性
    public string VPLCStoragePath
    {
      get
      {
        if(Instance == null)
        {
          return "c:\\plcsim";
        }
        switch (Instance.OperatingState)
        {
          case EOperatingState.Off:
            return "c:\\plcsim";
          case EOperatingState.Booting:
          case EOperatingState.Freeze:
          case EOperatingState.Hold:
          case EOperatingState.Run:
          case EOperatingState.ShuttingDown:
          case EOperatingState.Startup:
          case EOperatingState.Stop:
          default:
            return Instance.StoragePath;
        }
      }
    }

    public string VPLCIP {
      get
      {
        return Instance.ControllerIPSuite4.ToString();
      }
      set
      {

      }
    }

    public static readonly SolidColorBrush NeutralLedColorBrush = Brushes.LightGray;
    public static readonly SolidColorBrush PoweredOffColor = new SolidColorBrush(Color.FromRgb(85, 85, 85));

    private SolidColorBrush runLedColor;
    public SolidColorBrush RunLedColor {
      get
      {
        return runLedColor;
      }
      set
      {
        if (this.runLedColor == value)
          return;
        this.runLedColor = value;
        this.RaisePropertyChanged("RunLedColor");
      }
    }

    private SolidColorBrush errorLedColor;
    public SolidColorBrush ErrorLedColor {
      get
      {
        return errorLedColor;
      }
      set
      {
        if (this.errorLedColor == value)
          return;
        this.errorLedColor = value;
        this.RaisePropertyChanged("ErrorLedColor");
      }
    }

    private SolidColorBrush maintLedColor;
    public SolidColorBrush MaintLedColor {
      get
      {
        return maintLedColor;
      }
      set
      {
        if (this.maintLedColor == value)
          return;
        this.maintLedColor = value;
        this.RaisePropertyChanged("MaintLedColor");
      }
    }
    #endregion

    #region 方法
    public void TogglePower()
    {

      try
      {
        //EnableVirtualSwitch();

        SimulationRuntimeManager.OnRunTimemanagerLost += SimulationRuntimeManager_OnRunTimemanagerLost;
        SimulationRuntimeManager.OnConfigurationChanged += SimulationRuntimeManager_OnConfigurationChanged;


        if (HasName(Name))
        {
          Instance = SimulationRuntimeManager.CreateInterface(Name);
        }
        else
        {
          Instance = SimulationRuntimeManager.RegisterInstance(ECPUType.CPU1513, Name);
        }

        if (Instance.OperatingState != EOperatingState.Run)
        {

          Instance.CommunicationInterface = ECommunicationInterface.TCPIP;
          Instance.StoragePath = VPLCStoragePath;

          Instance.IsSendSyncEventInDefaultModeEnabled = true;

          Instance.OnConfigurationChanged += Instance_OnConfigurationChanged;
          Instance.OnConfigurationChanging += Instance_OnConfigurationChanging;
          Instance.OnLedChanged += Instance_OnLedChanged;
          Instance.OnOperatingStateChanged += Instance_OnOperatingStateChanged;
          Instance.OnSyncPointReached += Instance_OnSyncPointReached;
          Instance.OnProcessEventDone += Instance_OnProcessEventDone;
          Instance.OnUpdateEventDone += Instance_OnUpdateEventDone;

          var result = Instance.PowerOn(5000);
          switch (result)
          {
            case ERuntimeErrorCode.OK:
              AfterPowerOnOK();
              break;
            case ERuntimeErrorCode.Timeout:
              break;
          }



        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Growl.Error(ex.ToString());
      }
      finally
      {

      }
    }

    private void Instance_OnUpdateEventDone(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_SystemTime, 
      uint in_HardwareIdentifier)
    {
    }

    private void AfterPowerOnOK()
    {
      Console.WriteLine(Instance.ControllerShortDesignation);
      Console.WriteLine(Instance.StoragePath);

      Instance.Run();


    }

    public void AfterStartupOK()
    {
      InitIronPythonTasks();
    }


    public void AfterRunOK()
    {

    }

    public void SetIP(string ip,string mask,string gateway)
    {
      SIPSuite4 SIPSuite4 = new SIPSuite4(ip, mask, gateway);

      Instance.SetIPSuite(0, SIPSuite4, true);
    }

    #region LED
    public void UpdateRunLed(ELEDMode mode)
    {
      switch (mode)
      {
        case ELEDMode.On:
          RunLedColor = Brushes.LawnGreen;
          break;
        case ELEDMode.Off:
          RunLedColor = PoweredOffColor;
          break;
        case ELEDMode.FlashSlow:
          break;
        case ELEDMode.FlashFast:
          break;
      }
    }

    public void UpdateErrorLed(ELEDMode mode)
    {
      switch (mode)
      {
        case ELEDMode.On:
          ErrorLedColor = Brushes.Red;
          break;
        case ELEDMode.Off:
          ErrorLedColor = Brushes.Yellow;
          break;
        case ELEDMode.FlashSlow:
          break;
        case ELEDMode.FlashFast:
          break;
      }
    }

    public void UpdateMaintLed(ELEDMode mode)
    {
      switch (mode)
      {
        case ELEDMode.On:
          MaintLedColor = Brushes.Red;
          break;
        case ELEDMode.Off:
          MaintLedColor = Brushes.Orange;
          break;
        case ELEDMode.FlashSlow:
          break;
        case ELEDMode.FlashFast:
          break;
      }
    }
    #endregion

    #endregion

    #region 事件回调

    private void SimulationRuntimeManager_OnConfigurationChanged(ERuntimeConfigChanged in_RuntimeConfigChanged, uint in_Param1, uint in_Param2, int in_Param3)
    {
    }

    private void SimulationRuntimeManager_OnRunTimemanagerLost()
    {
    }

    private void Instance_OnOperatingStateChanged(IInstance in_Sender,
      ERuntimeErrorCode in_ErrorCode,
      DateTime in_DateTime,
      EOperatingState in_PrevState,
      EOperatingState in_OperatingState)
    {
      Console.WriteLine(string.Format("Instance_OnOperatingStateChanged From {0} To {1}", in_PrevState, in_OperatingState));
      switch (in_OperatingState)
      {
        case EOperatingState.InvalidOperatingState:
          break;
        case EOperatingState.Off:
          break;
        case EOperatingState.Booting:
          break;
        case EOperatingState.Stop:
          break;
        case EOperatingState.Startup:
          AfterStartupOK();
          break;
        case EOperatingState.Run:
          AfterRunOK();
          break;
        case EOperatingState.Freeze:
          break;
        case EOperatingState.ShuttingDown:
          break;
        case EOperatingState.Hold:
          break;
      }
    }

    private void Instance_OnLedChanged(IInstance in_Sender,
      ERuntimeErrorCode in_ErrorCode,
      DateTime in_DateTime,
      ELEDType in_LEDType,
      ELEDMode in_LEDMode)
    {
      //Growl.Info(string.Format("Instance_OnOperatingStateChanged From {0} To {1}", in_LEDType,in_LEDMode));
      switch (in_LEDType)
      {
        case ELEDType.Run:
          UpdateRunLed(in_LEDMode);
          break;
        case ELEDType.Stop:
          UpdateErrorLed(in_LEDMode);
          break;
        case ELEDType.Maint:
          UpdateMaintLed(in_LEDMode);
          break;
      }
    }

    private void Instance_OnSyncPointReached(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_DateTime, 
      uint in_PipId, 
      long in_TimeSinceSameSyncPoint_ns, 
      long in_TimeSinceAnySyncPoint_ns, 
      uint in_SyncPointCount)
    {
      if (IronPythonTaskInited)
      {
        foreach (var ironPythonTask in IronPythonTasks)
        {
          ironPythonTask.RunOneTime();
        }
      }
    }

    private void Instance_OnProcessEventDone(IInstance in_Sender, 
      ERuntimeErrorCode in_ErrorCode, 
      DateTime in_SystemTime, 
      uint in_HardwareIdentifier, 
      uint in_Channel, 
      EProcessEventType in_ProcessEventType, 
      uint in_SequenceNumber)
    {
    }



    private void Instance_OnConfigurationChanging(IInstance in_Sender,
      ERuntimeErrorCode in_ErrorCode,
      DateTime in_DateTime)
    {
      //Growl.Info(string.Format("Instance_OnConfigurationChanging "));
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
      //Growl.Info(string.Format("Instance_OnConfigurationChanged "));
    }
    #endregion

    #region 命令
    private RelayCommand runCommand;

    public RelayCommand RunCommand
    {
      get
      {
        return this.runCommand;
      }
      set
      {
        if (this.runCommand == value)
          return;
        this.runCommand = value;
        this.RaisePropertyChanged("RunCommand");
      }
    }

    private RelayCommand toggleTopCommand;
    public RelayCommand ToggleTopCommand
    {
      get
      {
        return this.toggleTopCommand;
      }
      set
      {
        if (this.toggleTopCommand == value)
          return;
        this.toggleTopCommand = value;
        this.RaisePropertyChanged("ToggleTopCommand");
      }
    }

    private RelayCommand stopCommand;

    public RelayCommand StopCommand
    {
      get
      {
        return this.stopCommand;
      }
      set
      {
        if (this.stopCommand == value)
          return;
        this.stopCommand = value;
        this.RaisePropertyChanged("StopCommand");
      }
    }

    private RelayCommand mresCommand;
    public RelayCommand MresCommand
    {
      get
      {
        return this.mresCommand;
      }
      set
      {
        if (this.mresCommand == value)
          return;
        this.mresCommand = value;
        this.RaisePropertyChanged("MresCommand");
      }
    }

    private bool powerButtonIsDepressed;
    public bool PowerButtonIsDepressed
    {
      get
      {
        return this.powerButtonIsDepressed;
      }
      set
      {
        if (this.powerButtonIsDepressed == value)
          return;
        this.powerButtonIsDepressed = value;
        this.RaisePropertyChanged("PowerButtonIsDepressed");
      }
    }

    private bool runButtonIsDepressed;
    public bool RunButtonIsDepressed
    {
      get
      {
        return this.runButtonIsDepressed;
      }
      set
      {
        if (this.runButtonIsDepressed == value)
          return;
        this.runButtonIsDepressed = value;
        this.RaisePropertyChanged("RunButtonIsDepressed");
      }
    }

    private bool stopButtonIsDepressed;
    public bool StopButtonIsDepressed
    {
      get
      {
        return this.stopButtonIsDepressed;
      }
      set
      {
        if (this.stopButtonIsDepressed == value)
          return;
        this.stopButtonIsDepressed = value;
        this.RaisePropertyChanged("StopButtonIsDepressed");
      }
    }

    private bool mresButtonIsDepressed;
    public bool MresButtonIsDepressed
    {
      get
      {
        return this.mresButtonIsDepressed;
      }
      set
      {
        if (this.mresButtonIsDepressed == value)
          return;
        this.mresButtonIsDepressed = value;
        this.RaisePropertyChanged("MresButtonIsDepressed");
      }
    }

    private RelayCommand powerPlcCommand;
    public RelayCommand PowerPlcCommand
    {
      get
      {
        return this.powerPlcCommand;
      }
      private set
      {
        if (this.powerPlcCommand == value)
          return;
        this.powerPlcCommand = value;
        this.RaisePropertyChanged("PowerPlcCommand");
      }
    }

    private RelayCommand powerOffPlcCommand;
    private RelayCommand PowerOffPlcCommand
    {
      get
      {
        return this.powerOffPlcCommand;
      }
      set
      {
        if (this.powerOffPlcCommand == value)
          return;
        this.powerOffPlcCommand = value;
        this.RaisePropertyChanged("PowerOffPlcCommand");
      }
    }
    #endregion

    private static IConfigureAdaptersNET ConfigureCommunicationAdapters;
    public IInstance Instance;

    public static string AdapterName = "USB";
    public static string Name = "plcsim";
    public void EnableVirtualSwitch()
    {
      ConfigureCommunicationAdapters = new IConfigureAdaptersNETImpl();
      ConfigureCommunicationAdapters.ResetNetworkSettings();
      var result = ConfigureCommunicationAdapters.ConfigureVirtualSwitch(AdapterName, true);
    }

    public bool HasName(string name)
    {
      SInstanceInfo[] SInstanceInfos = SimulationRuntimeManager.RegisteredInstanceInfo;
      foreach (SInstanceInfo instanceInfo in SInstanceInfos)
      {
        if (instanceInfo.Name == name)
        {
          return true;
        }
      }
      return false;
    }




  }
}
