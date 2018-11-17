using System;
using System.Threading;
using System.Windows.Media;

using GalaSoft.MvvmLight.Threading;

namespace YZX.PlcSimAdv.ViewModel
{
  public class LedFlasher
  {
    private readonly object _FlashLock = new object();
    private readonly Timer Flasher;
    private readonly Action<SolidColorBrush> _setColor;
    private bool _continueFlashing;
    private SolidColorBrush _flashColor1;
    private SolidColorBrush _flashColor2;
    private bool _flashCycle;
    private int _flashInterval;

    public LedFlasher(Action<SolidColorBrush> setColor)
    {
      this._setColor = setColor;
      this.Flasher = new Timer(new TimerCallback(this.FlashCallback));
    }

    public void StopFlash()
    {
      object obj = this._FlashLock;
      bool lockTaken = false;
      try
      {
        Monitor.Enter(obj, ref lockTaken);
        this._continueFlashing = false;
        this.Flasher.Change(-1, -1);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(obj);
      }
    }

    public void Flash(SolidColorBrush color1, SolidColorBrush color2, int interval)
    {
      object obj = this._FlashLock;
      bool lockTaken = false;
      try
      {
        Monitor.Enter(obj, ref lockTaken);
        this._flashColor1 = color1;
        this._flashColor2 = color2;
        this._flashCycle = false;
        this._continueFlashing = true;
        this._flashInterval = interval;
        this.ScheduleNextFlash();
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(obj);
      }
    }

    private void FlashCallback(object state)
    {
      object obj = this._FlashLock;
      bool lockTaken = false;
      try
      {
        Monitor.Enter(obj, ref lockTaken);
        if (!this._continueFlashing)
          return;
        this._flashCycle = !this._flashCycle;
        SolidColorBrush currentColor = this._flashCycle ? this._flashColor1 : this._flashColor2;
        DispatcherHelper.CheckBeginInvokeOnUI(() =>
       {
         this._setColor(currentColor);
         this.ScheduleNextFlash();
       });
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(obj);
      }
    }

    private void ScheduleNextFlash()
    {
      this.Flasher.Change(this._flashInterval, -1);
    }
  }
}
