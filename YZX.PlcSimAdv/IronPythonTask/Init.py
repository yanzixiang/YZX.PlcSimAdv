# -*- coding: utf-8 -*-
try:
  import clr
  
  for f in LoadedDlls:
    flocation = f.Location
    clr.AddReferenceToFileAndPath(flocation)
  
  from System.Threading import Thread
  from System.Diagnostics import *
  from System.Windows.Interop import *
  from Pinvoke import *

except Exception:
  print Exception
  print "import Exception in Init"

def GetControlPanelHwndWraper():
  ControlPanelProcess =  Process.GetProcessesByName("Siemens.Simatic.PlcSim.Advanced.UserInterface")[0]
  print ControlPanelProcess
  ControlPanelHwndWraper = User32.FindWindow("HwndWraper","S7-PLCSIM Advanced V2.0 SP1")
  print ControlPanelHwndWraper

def RunOneTime():
  print CPU
  print VIEW

RunOneTime()
