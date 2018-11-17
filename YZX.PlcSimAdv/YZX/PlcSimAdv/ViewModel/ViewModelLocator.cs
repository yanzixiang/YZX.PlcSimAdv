using System;
using System.Windows;

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace YZX.PlcSimAdv.ViewModel
{
  public class ViewModelLocator
  {
    public ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      SimpleIoc.Default.Register<CPUControlViewModel>();
      SimpleIoc.Default.Register<MainViewModel>();

    }

    public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>
        Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

    #region Vm

    public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

    public CPUControlViewModel CPU => ServiceLocator.Current.GetInstance<CPUControlViewModel>();

    #endregion
  }
}