﻿using System;
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

        private MouseButtonEventHandler _clickOutsideControlEvent;


        public ucLabeledTextBox()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void AddMouseCaptureHandler()
        {
            _clickOutsideControlEvent = new MouseButtonEventHandler(HandleClickOutsideOfControl);
            AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, _clickOutsideControlEvent, true);
        }

        private void RemoveMouseCaptureHandler()
        {
            ReleaseMouseCapture();
            RemoveHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, _clickOutsideControlEvent);
        }

        private void HandleClickOutsideOfControl(object sender, MouseButtonEventArgs e)
        {
            ToggleToLabel();
        }

        private void ToggleToTextBox()
        {
            BTN_Label.Visibility = Visibility.Collapsed;
            SP_TextBox.Visibility = Visibility.Visible;
            TB_Input.Focus();
            TB_Input.Text = Value;
            if (Value == "0") TB_Input.Text = "";
            TB_Input.CaretIndex = TB_Input.Text.Length;
            Mouse.Capture(this, CaptureMode.SubTree);
            AddMouseCaptureHandler();
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
                if (String.IsNullOrWhiteSpace(editValue))
                {
                    Value = "0";
                }
            }
            RemoveMouseCaptureHandler();
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
            if((e.Key == Key.Enter || e.Key == Key.Tab) && SP_TextBox.Visibility == Visibility.Visible)
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

            return int.TryParse(input, out _) || String.IsNullOrWhiteSpace(input);
        }

        public enum Inputs
        {
            String,
            Int
        }
    }
}
