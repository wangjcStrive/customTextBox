using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCustomControlLibrary1
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControlLibrary1"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControlLibrary1;assembly=WpfCustomControlLibrary1"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl2/>
    ///
    /// </summary>
    public class NumTextBox : TextBox
    {
        public static readonly DependencyProperty NumTypeProperty = DependencyProperty.Register("NumType", typeof(Type), typeof(NumTextBox));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(decimal), typeof(NumTextBox), new FrameworkPropertyMetadata(
                new PropertyChangedCallback(CheckProperty)));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(decimal), typeof(NumTextBox), new FrameworkPropertyMetadata(
                new PropertyChangedCallback(CheckProperty)));
        public static readonly DependencyProperty PointLenthProperty = DependencyProperty.Register("PointLenth", typeof(int), typeof(NumTextBox));
        public static readonly DependencyProperty NumberLengthProperty = DependencyProperty.Register("NumberLength", typeof(int), typeof(NumTextBox));
        //  private System.Globalization.CultureInfo cI;
        public NumTextBox()
        {
            MinValue = decimal.MinValue;
            MaxValue = decimal.MaxValue;
            PointLenth = 2;
            NumberLength = 13;

        }
        public enum Type { Decimal, UDecimal, Int, UInt }
        /// <summary>
        /// 设置或获取textbox的输入数据类型
        /// </summary>
        public Type NumType
        {
            get { return (Type)GetValue(NumTypeProperty); }
            set { SetValue(NumTypeProperty, value); }
        }
        /// <summary>
        /// 设定或获取最大值
        /// </summary>
        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// 设定或获取最小值
        /// </summary>
        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        /// <summary>
        /// 设置或获取小数点前的位数
        /// </summary>
        public int NumberLength
        {
            get { return (int)GetValue(NumberLengthProperty); }
            set { SetValue(NumberLengthProperty, value); }
        }
        /// <summary>
        /// 设置或获取小数点后位数长度
        /// </summary>
        public int PointLenth
        {
            get { return (int)GetValue(PointLenthProperty); }
            set { SetValue(PointLenthProperty, value); }
        }

        private string val = "";
        /// <summary>
        /// 设定最大值最小值依赖属性回调函数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CheckProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumTextBox ntb = d as NumTextBox;
            if (ntb.MaxValue < ntb.MinValue)
                ntb.MaxValue = ntb.MinValue;
        }
        /// <summary>
        /// 重写KeyDown事件，提供与事件有关的数据，过滤输入数据格式
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            string txt = this.Text;
            int ind = this.CaretIndex;
            if (txt.Contains("."))
            {
                // Console.WriteLine(txt.Split('.')[0] + "he " + txt.Split('.')[1]);
                if (txt.Split('.')[1].Length >= PointLenth && ind > txt.Split('.')[0].Length && this.SelectionLength == 0)//控制小数点后输入位数
                {
                    e.Handled = true;
                    return;
                }
                else if (txt.Split('.')[0].Length >= NumberLength && ind <= txt.Split('.')[0].Length)//控制小数点前输入位数（有小数点）
                {
                    e.Handled = true;
                    return;
                }
            }
            else if (txt.Length == NumberLength && e.Key != Key.Decimal && e.Key != Key.OemPeriod)//控制小数点前输入位数（无小数点）
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                val = ".";
            }
            else
            {

                val = "";
            }
            switch (NumType)
            {

                case Type.UInt:
                    //屏蔽非法按键
                    if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key.ToString() == "Tab")
                    {
                        e.Handled = false;
                    }
                    else if (((e.Key >= Key.D0 && e.Key <= Key.D9)) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                        if (e.Key.ToString() != "RightCtrl")
                        { }
                    }
                    break;
                case Type.Int:
                    //屏蔽非法按键
                    if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract || e.Key.ToString() == "Tab")
                    {
                        if ((txt.Contains("-") || this.CaretIndex != 0) && e.Key == Key.Subtract)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemMinus) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                    {
                        if ((txt.Contains("-") || this.CaretIndex != 0) && e.Key == Key.OemMinus)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                        if (e.Key.ToString() != "RightCtrl")
                        { }
                    }
                    break;
                case Type.Decimal:
                    //屏蔽非法按键
                    if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal || e.Key == Key.Subtract || e.Key.ToString() == "Tab")
                    {
                        if (txt.Contains(".") && e.Key == Key.Decimal)
                        {
                            e.Handled = true;
                            return;
                        }
                        else if ((txt.Contains("-") || this.CaretIndex != 0) && e.Key == Key.Subtract)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod || e.Key == Key.OemMinus) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                    {
                        if (txt.Contains(".") && e.Key == Key.OemPeriod)
                        {
                            e.Handled = true;
                            return;
                        }
                        else if ((txt.Contains("-") || this.CaretIndex != 0) && e.Key == Key.OemMinus)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                        if (e.Key.ToString() != "RightCtrl")
                        { }
                    }
                    break;
                case Type.UDecimal:
                default:
                    //屏蔽非法按键
                    if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal || e.Key.ToString() == "Tab")
                    {
                        if (txt.Contains(".") && e.Key == Key.Decimal)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                    {
                        if (txt.Contains(".") && e.Key == Key.OemPeriod)
                        {
                            e.Handled = true;
                            return;
                        }
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                        if (e.Key.ToString() != "RightCtrl")
                        { }
                    }

                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            string ss = this.Text;
            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        ///粘贴内容过滤，设定最大值、最小值，限制小数点输入长度
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            int t1 = this.Text.Length;
            if (this.Text.ToString() == "1.")
                return;
            if (t1 != 0)//用于是否可以将文本空置空
            {
                decimal d = 0;
                if (this.Text != "-" && this.Text != "." && this.Text != "-0" && this.Text != "-." && this.Text != "-0." && val != ".")
                {
                    if (decimal.TryParse(this.Text, out d))
                    {
                        if (NumType == Type.Decimal || NumType == Type.UDecimal)
                        {
                            if (d.ToString().Split('.')[0].Length > NumberLength)//
                            {
                                d = 0;
                            }
                            else
                            {
                                d = Math.Round(d, PointLenth, MidpointRounding.AwayFromZero);
                            }
                        }
                        else
                            if (d.ToString().Split('.')[0].Length > NumberLength)//
                        {
                            d = 0;
                        }
                        else
                        {
                            d = Math.Round(d, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    int t2 = d.ToString().Length;
                    if (Math.Abs(t1 - d.ToString().Length) > 0)
                    {
                        this.Text = d.ToString();
                        this.CaretIndex = d.ToString().Length;
                    }
                    else
                    {
                        this.Text = d.ToString();
                    }

                }
                if ((NumType == Type.UDecimal || NumType == Type.UInt) && this.Text.Contains("-"))
                {
                    this.Text = Math.Abs(d).ToString();
                }
                if ((NumType == Type.UInt || NumType == Type.Int) && this.Text.Contains("."))
                {
                    this.Text = int.Parse(d.ToString()).ToString();
                }

                //this.Text = checkMinMax(d.ToString());
            }
            base.OnTextChanged(e);
        }

        /// <summary>
        ///如果数据是0，得到光标，清空数据
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            //InputMethod.SetPreferredImeState(this, InputMethodState.Off);
            //cI = InputLanguageManager.GetInputLanguage(this);
            //decimal d = 0;
            //if (decimal.TryParse(this.Text, out d))
            //{
            if (this.Text == "0")
            {
                this.Text = "";
            }
            //}
            //else
            //{
            //    this.Text = "";
            //}
            base.OnGotFocus(e);
        }

        public string checkMinMax(string input)
        {
            decimal d = 0;
            string result = null;
            if (decimal.TryParse(input, out d))
            {
                if (d < MinValue)
                {
                    d = MinValue;
                }

                else if (d > MaxValue)
                {
                    d = MaxValue;
                }
                result = d.ToString();
            }
            else if (string.IsNullOrEmpty(this.Text))
            {
                result = "";
            }
            else
            {
                result = d.ToString();
            }
            return result;
        }

        /// <summary>
        ///失去光标，确定最大值最小值
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            decimal d = 0;
            if (decimal.TryParse(this.Text, out d))
            {
                if (d < MinValue)
                {
                    d = MinValue;
                    this.Text = d.ToString();
                }

                else if (d > MaxValue)
                {
                    d = MaxValue;
                    this.Text = d.ToString();
                }
            }
            else if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = "";
            }
            else
            {
                this.Text = d.ToString();
            }
            base.OnLostFocus(e);
            //System.Globalization.CultureInfo cI = InputLanguageManager.GetInputLanguage(this);
            //InputMethod.SetPreferredImeState(this, InputMethodState.DoNotCare);
            //InputLanguageManager.SetInputLanguage(this, cI);
            //cI = null;
        }
    }
}
