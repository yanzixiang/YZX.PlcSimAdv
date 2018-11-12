using System.Drawing;
using System.Windows;

namespace YZX.PlcSimAdv.View
{
  public partial class LabeledLed
  {
    public LabeledLed()
    {
      InitializeComponent();
    }

    public string Label { get; set; }

    public Color Color { get; set; }

    public readonly static DependencyProperty LabelDependency =
      DependencyProperty.Register("Label",typeof(string),typeof(LabeledLed)
    );

    public readonly static DependencyProperty ColorDependency =
      DependencyProperty.Register("Color", typeof(string), typeof(LabeledLed)
    );
  }
}
