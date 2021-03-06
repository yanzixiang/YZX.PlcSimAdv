﻿using System;
using System.Windows;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;

using Siemens.Simatic.Simulation.Runtime;
using Siemens.Simatic.PlcSim.Advanced;

using HandyControl.Controls;
using YZX.PlcSimAdv.View;

namespace YZX.PlcSimAdv
{
  public partial class MainWindow:Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Loaded += MainWindow_Loaded;

    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      Title.TitleFontSize = 100;
      Title.DisplayText("YZX.PlcSimAdv");
      ShowIronPythonControl();
    }

    private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
    }

    IronPythonControl IronPythonControl = new IronPythonControl();
    public void ShowIronPythonControl()
    {
      PageTransitionControl.ShowPage(IronPythonControl);
    }


    ControlPanelHost ControlPanelHost = new ControlPanelHost();
    public void ShowControlPanelHost()
    {
      PageTransitionControl.ShowPage(ControlPanelHost);
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
  }
}
