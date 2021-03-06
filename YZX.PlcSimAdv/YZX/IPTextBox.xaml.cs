using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IPtextbox
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class IPTextBox : UserControl
  {
    private const string errorMessage = "Please specify a value between 0 and 255.";

    public static readonly DependencyProperty Ip_tabindexProperty = DependencyProperty.Register(
      "Ip_tabindex", typeof(int), typeof(IPTextBox));


    /// <summary>
    /// Gets or sets tab index for ip box
    /// </summary>
    public int Ip_tabindex
    {
      get { return (int)GetValue(Ip_tabindexProperty); }
      set { SetValue(Ip_tabindexProperty, value); InvalidateVisual(); }
    }



    public IPTextBox()
    {
      InitializeComponent();
    }

    public IPTextBox(byte[] bytesToFill)
    {
      InitializeComponent();
      text1.Text = Convert.ToString(bytesToFill[0]);
      text2.Text = Convert.ToString(bytesToFill[1]);
      text3.Text = Convert.ToString(bytesToFill[2]);
      text4.Text = Convert.ToString(Convert.ToString(bytesToFill[3]));
    }
    public void FirstbyteFocus()
    {
      text1.Focus();
    }
    public void SecondbyteFocus()
    {
      text2.Focus();
    }
    public void ThirdbyteFocus()
    {
      text3.Focus();
    }
    public void FourthbyteFocus()
    {
      text4.Focus();
    }
    public void FirstbyteClear()
    {
      text1.Clear();
    }
    public void SecondbyteClear()
    {
      text2.Clear();
    }
    public void ThirdbyteClear()
    {
      text3.Clear();
    }
    public void FourthbyteClear()
    {
      text4.Clear();
    }
    public string FirstByte { get { return text1.Text; } set { text1.Text = Convert.ToString(value); } }

    public string SecondByte { get { return text2.Text; } set { text2.Text = Convert.ToString(value); } }

    public string ThirdByte { get { return text3.Text; } set { text3.Text = Convert.ToString(value); } }

    public string FourthByte { get { return text4.Text; } set { text4.Text = Convert.ToString(value); } }

    public bool IsFirstByteFocus { get { return text1.IsFocused; } }
    public bool IsSecondByteFocus { get { return text2.IsKeyboardFocused; } }
    public bool IsThirdByteFocus { get { return text3.IsKeyboardFocused; } }
    public bool IsFourthByteFocus { get { return text4.IsKeyboardFocused; } }

    private void textbox_PreviewKeyDown(object sender, KeyEventArgs e)
    {

      //get detail of text box which triggered the event
      TextBox tt = (TextBox)sender;

      string Current_textbox_name, prev_textbox_name, next_textbox_name, number_extractor;
      int prev, next;

      Current_textbox_name = tt.Name;


      number_extractor = Regex.Replace(Current_textbox_name, "[^0-9]+", string.Empty);
      //text1,text2,text3,text4 are the name of innertext boxes 
      //text box were named in such a way that by extracting last char we can get to know 
      //the position of text box 

      //prev_textbox and next_textbox are neighbours of current textbox

      prev = int.Parse(number_extractor);

      next = int.Parse(number_extractor);
      prev--;
      next++;
      prev = prev > 1 ? prev : 1; //if current textbox is 1 it wont have previous textbox
      next = next < 5 ? next : 4; //if current textbox is 4 it wont have next textbox 

      prev_textbox_name = Current_textbox_name.Remove(4, 1).Insert(4, prev.ToString());

      next_textbox_name = Current_textbox_name.Remove(4, 1).Insert(4, next.ToString());


      var Current_textbox = (TextBox)this.FindName(Current_textbox_name);
      var prev_textbox = (TextBox)this.FindName(prev_textbox_name);
      var next_textbox = (TextBox)this.FindName(next_textbox_name);


      if (e.Key == Key.Back)
      {
        if (Current_textbox.CaretIndex == 0)
        {
          prev_textbox.Focus();
          prev_textbox.CaretIndex = prev_textbox.Text.Length;
        }
      }
      else if (e.Key == Key.OemPeriod || e.Key == Key.Space || e.Key == Key.Decimal)
      {
        if (next_textbox.Name == Current_textbox.Name || Current_textbox.Text == "")
        { e.Handled = true; }
        else if (next_textbox.Text == "")
        {
          next_textbox.Focus();
          e.Handled = true;
        }
        else if (next_textbox.Text != "" && Current_textbox.SelectionLength == 0 && Current_textbox.CaretIndex != 0)
        {
          next_textbox.Focus();
          next_textbox.SelectionStart = 0;
          next_textbox.SelectionLength = next_textbox.Text.Length;
          e.Handled = true;
        }
        else if (e.Key == Key.Space)
          e.Handled = true;

      }
      else if (e.Key == Key.Right || e.Key == Key.Down)
      {
        if (next_textbox.Name == Current_textbox.Name && Current_textbox.CaretIndex == Current_textbox.Text.Length)
        { e.Handled = true; }
        else if (Current_textbox.Text == "" || Current_textbox.CaretIndex == Current_textbox.Text.Length)
        {
          next_textbox.Focus();
          next_textbox.CaretIndex = 0;
          e.Handled = true;
        }
        else if (e.Key == Key.Down)
        {
          Current_textbox.CaretIndex++;
          e.Handled = true;
        }

      }
      else if (e.Key == Key.Left || e.Key == Key.Up)
      {
        if (prev_textbox.Name == Current_textbox.Name && Current_textbox.CaretIndex == 0)
        { e.Handled = true; }
        else if (e.Key == Key.Up && Current_textbox.CaretIndex != 0)
        {
          Current_textbox.CaretIndex--;
          e.Handled = true;
        }
        else if (Current_textbox.Text == "" || Current_textbox.CaretIndex == 0)
        {
          prev_textbox.Focus();
          prev_textbox.CaretIndex = prev_textbox.Text.Length;
          e.Handled = true;
        }
      }
    }

    //checks whether input is digit
    private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsDigit(Convert.ToChar(e.Text)))
      {
        e.Handled = true;
        SystemSounds.Beep.Play();
      }
    }
    //checks whether textbox content > 255 when 3 characters have been entered.
    //clears if > 255, switches to next textbox otherwise 
    private void textbox_TextChanged(object sender, TextChangedEventArgs e)
    {
      TextBox text1 = (TextBox)sender;

      string Current_textbox, next_textbox, number_extractor;
      Current_textbox = text1.Name;
      number_extractor = Regex.Replace(Current_textbox, "[^0-9]+", string.Empty);
      int next;
      next = int.Parse(number_extractor);
      next++;
      next = next < 5 ? next : 4;
      next_textbox = Current_textbox.Remove(4, 1).Insert(4, next.ToString());
      var nexx = (TextBox)this.FindName(next_textbox);
      if (text1.Text.Length == 3)
      {
        try
        {
          Convert.ToByte(text1.Text);

        }
        catch (FormatException exception)
        {
          text1.Clear();
          text1.Focus();
          SystemSounds.Beep.Play();
          MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }
        catch (OverflowException exception)
        {
          text1.Clear();
          text1.Focus();
          SystemSounds.Beep.Play();
          MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }
        if (text1.CaretIndex != 2)
        {

          try
          {
            nexx.Focus();
          }catch(Exception ex)
          {

          }
        }
      }

    }
    private void control_GotFocus(object sender, RoutedEventArgs e)
    {

      text1.SelectAll();
    }
  }
}