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
using System.Windows.Shapes;

namespace RedstoneSidekickWPF.ProjectWindow.PopUps
{
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public string WindowTitle
        {
            get { return (string)GetValue(WindowTitleProperty); }
            set { SetValue(WindowTitleProperty, value); }
        }


        public static readonly DependencyProperty WindowTitleProperty =
            DependencyProperty.Register("WindowTitle", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata("Title", SetTitle));

        private static void SetTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as MessageBoxWindow;
            var title = (string)e.NewValue;
            
            window.Title = title;
        }

        public string MessageText
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata("", SetMessage));

        private static void SetMessage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as MessageBoxWindow;
            var message = (string)e.NewValue;

            window.Text_Message.Text = message;
        }

        public MessageBoxWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
