using RedstoneSidekick.WPF.ProjectWindow.UserControls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RedstoneSidekick.WPF.ProjectWindow
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        private readonly ProjectWindowVM _projectWindowVM;

        public ProjectWindow()
        {
            _projectWindowVM = new ProjectWindowVM();
            this.DataContext = _projectWindowVM;

            InitializeComponent();
        }

        private int _focusedTabIndex = 1;

        #region Project Name

        private void TextBox_ProjectName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox box = sender as TextBox;

            box.BorderThickness = new Thickness(2);

            if (box.Focusable == false)
            {
                var margin = box.Margin;
                margin.Left = box.Margin.Left - 2;
                box.Margin = margin;

                box.CaretBrush = new SolidColorBrush(Colors.Black);
                box.Focusable = true;
                box.IsReadOnly = false;
                box.Focus();
            }
        }
        private void TextBox_ProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                var margin = box.Margin;
                margin.Left = box.Margin.Left + 2;
                box.Margin = margin;

                box.CaretBrush = new SolidColorBrush(Colors.White);
                box.Focusable = false;
                box.IsReadOnly = true;
                box.IsReadOnlyCaretVisible = false;
                box.BorderThickness = new Thickness(0);
            }
        }

        #endregion

        #region Project Tabs

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            Rectangle_Cursor.SetValue(Grid.ColumnProperty, index);

            if (_focusedTabIndex != index)
            {
                switch (index)
                {
                    case 1:
                        FocusButton(BTN_CraftingTreeTab);
                        DeFocusButton(BTN_GatheringListTab);
                        Grid_ProjectView.Background = new SolidColorBrush(Colors.Beige);
                        Grid_ProjectView.Children.Add(new ucCraftingTree());
                        Grid_ProjectView.Children.Remove(new ucGatheringList());
                        break;
                    case 2:
                        FocusButton(BTN_GatheringListTab);
                        DeFocusButton(BTN_CraftingTreeTab);
                        Grid_ProjectView.Background = new SolidColorBrush(Colors.DarkRed);
                        Grid_ProjectView.Children.Add(new ucGatheringList());
                        Grid_ProjectView.Children.Remove(new ucCraftingTree());
                        break;
                    case 3:
                        Grid_ProjectView.Background = new SolidColorBrush(Colors.Red);
                        break;
                }

                _focusedTabIndex = index;
            }
        }
        private void FocusButton(Button button)
        {
            button.Opacity = 1;

            ThicknessAnimation animation = new ThicknessAnimation(new Thickness(2, 10, 2, 0), new TimeSpan(0, 0, 0, 0, 50));

            var border = button.Parent as Border;
            border.BeginAnimation(MarginProperty, animation);
        }
        private void DeFocusButton(Button button)
        {
            button.Opacity = 0.75;

            ThicknessAnimation animation = new ThicknessAnimation(new Thickness(2, 13, 2, 0), new TimeSpan(0, 0, 0, 0, 50));

            var border = button.Parent as Border;
            border.BeginAnimation(MarginProperty, animation);
        }

        #endregion
    }
}
