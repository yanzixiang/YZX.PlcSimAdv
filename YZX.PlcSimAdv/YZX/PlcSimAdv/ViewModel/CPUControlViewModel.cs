using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using YZX.PlcSimAdv.Properties;

namespace YZX.PlcSimAdv.ViewModel
{
  public class CPUControlViewModel : ViewModelBase
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

  }
}
