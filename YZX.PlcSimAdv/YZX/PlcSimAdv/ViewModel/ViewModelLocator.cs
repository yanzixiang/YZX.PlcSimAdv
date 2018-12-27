using System;
using System.Windows;

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

using YZX.PlcSimAdv.View;

namespace YZX.PlcSimAdv.ViewModel
{
  public class ViewModelLocator
  {
    public ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      SimpleIoc.Default.Register<CPUControlViewModel>();
      SimpleIoc.Default.Register<MainViewModel>();
      SimpleIoc.Default.Register<CPUControl>();

    }

    public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>
        Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

    #region Vm

    public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

    public CPUControlViewModel CPU => ServiceLocator.Current.GetInstance<CPUControlViewModel>();

    public CPUControl CPUControl => ServiceLocator.Current.GetInstance<CPUControl>();

    public long UsedMemory => GC.GetTotalMemory(true) / 1014;

    #endregion
  }
}