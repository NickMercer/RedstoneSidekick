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

namespace RedstoneSidekickWPF.UserControlLibrary
{
    /// <summary>
    /// Interaction logic for ucLabeledTextBox.xaml
    /// </summary>
    public partial class ucLabeledTextBox : UserControl
    {   
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ucLabeledTextBox), new PropertyMetadata(""));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(ucLabeledTextBox), new PropertyMetadata(""));


        public Inputs InputType
        {
            get { return (Inputs)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType", typeof(Inputs), typeof(ucLabeledTextBox), new PropertyMetadata(Inputs.String, SetInput));

        private static void SetInput(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucLabeledTextBox;
            var input = (Inputs)e.NewValue;

            uc.InputType = input;
        }


        public bool Editable
        {
            get { return (bool)GetValue(EditableProperty); }
            set { SetValue(EditableProperty, value); }
        }

        public static readonly DependencyProperty EditableProperty =
            DependencyProperty.Register("Editable", typeof(bool), typeof(ucLabeledTextBox), new PropertyMetadata(true));




        public ucLabeledTextBox()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void ToggleToTextBox()
        {
            BTN_Label.Visibility = Visibility.Collapsed;
            SP_TextBox.Visibility = Visibility.Visible;
            TB_Input.Focus();
            TB_Input.Text = Value;
        }

        private void ToggleToLabel()
        {
            BTN_Label.Visibility = Visibility.Visible;
            SP_TextBox.Visibility = Visibility.Collapsed;
            LayoutRoot.Focus();

            var editValue = TB_Input.Text;

            if (StringValidation(editValue))
            {
                Value = editValue;
            }
            else if(IntValidation(editValue))
            {
                Value = editValue;
            }
        }

        private void BTN_Label_Click(object sender, RoutedEventArgs e)
        {
            if (Editable)
            {
                ToggleToTextBox();
            }
        }

        private void BTN_TextBox_Click(object sender, RoutedEventArgs e)
        {
             ToggleToLabel();
        }

        private void TB_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && SP_TextBox.Visibility == Visibility.Visible)
            {
                ToggleToLabel();
            }
        }

        private bool StringValidation(string input)
        {
            if(InputType != Inputs.String)
            {
                return false;
            }


            return !String.IsNullOrWhiteSpace(input);
        }

        private bool IntValidation(string input)
        {
            if (InputType != Inputs.Int)
            {
                return false;
            }

            return int.TryParse(input, out _);
        }

        public enum Inputs
        {
            String,
            Int
        }
    }
}
