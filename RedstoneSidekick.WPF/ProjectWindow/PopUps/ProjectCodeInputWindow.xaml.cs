using RedstoneSidekick.Logic.ProjectStrings;
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
    /// Interaction logic for ProjectCodeInputWindow.xaml
    /// </summary>
    public partial class ProjectCodeInputWindow : Window
    {
        public string ProjectCode { get; set; }

        public ProjectCodeInputWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            var isValid = ProjectStringDecoder.IsValidProjectString(TB_ProjectCode.Text);

            if (isValid)
            {
                ProjectCode = TB_ProjectCode.Text;
                Close();
            }
            else
            {
                L_Error.Visibility = Visibility.Visible;
                L_Error.Content = "This is not a valid Redstone Sidekick Project Code";
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectCode = null;
            Close();
        }
    }
}
