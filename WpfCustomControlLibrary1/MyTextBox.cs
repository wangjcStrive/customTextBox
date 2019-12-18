using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCustomControlLibrary1
{
    public class NumericTextBox : TextBox
    {
        #region Properties
 
        public string DecimalSeparator { get; set; }
        public Boolean IsDecimalAllowed
        {
            get { return (Boolean)GetValue(IsDecimalAllowedProperty); }
            set { SetValue(IsDecimalAllowedProperty, value); }
        }
        public static readonly DependencyProperty IsDecimalAllowedProperty =
            DependencyProperty.Register("IsDecimalAllowed", typeof(Boolean), typeof(NumericTextBox), new PropertyMetadata(false));


        // Gets or sets the mask to apply to the textbox
        public int Scale
        {
            get { return (int)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(int), typeof(NumericTextBox), new PropertyMetadata(0));

        #endregion

        //TODO. wangjc. why static constructer?
        static NumericTextBox()
        {
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
            if (!e.Handled)
            {
                e.Handled = !MaxLengthReached(e);
            }
            base.OnPreviewTextInput(e);
        }

        bool AreAllValidNumericChars(string str)
        {
            if (string.IsNullOrEmpty(DecimalSeparator))
                DecimalSeparator = ".";

            bool ret = true;
            if (str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
                return ret;
            if (IsDecimalAllowed && str == DecimalSeparator)
                return ret;

            int l = str.Length;
            for (int i = 0; i < l; i++)
            {
                char ch = str[i];
                ret &= Char.IsDigit(ch);
            }

            return ret;
        }

        bool MaxLengthReached(TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            int precision = textBox.MaxLength - Scale - 2;

            string textToValidate = textBox.Text.Insert(textBox.CaretIndex, e.Text).Replace("-", "");
            string[] numericValues = textToValidate.Split(Convert.ToChar(DecimalSeparator));

            if ((numericValues.Length <= 2) && (numericValues[0].Length <= precision) && ((numericValues.Length == 1) || (numericValues[1].Length <= Scale)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
