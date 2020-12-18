using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using RedstoneSidekickWPF.UserControlLibrary;
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

namespace RedstoneSidekickWPF.ProjectWindow.UserControls
{
    /// <summary>
    /// Interaction logic for ucGatheringListItem.xaml
    /// </summary>
    public partial class ucGatheringListItem : UserControl
    {
        public IGatheringListItem Item
        {
            get { return (IGatheringListItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(IGatheringListItem), typeof(ucGatheringListItem), new PropertyMetadata(null));



        public ListView ParentListView
        {
            get { return (ListView)GetValue(ParentListViewProperty); }
            set { SetValue(ParentListViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentListView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentListViewProperty =
            DependencyProperty.Register("ParentListView", typeof(ListView), typeof(ucGatheringListItem), new PropertyMetadata(null));



        public ucGatheringListItem()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void ChildItem_Click(object sender, RoutedEventArgs e)
        {
            ParentListView.SelectedIndex = ParentListView.Items.IndexOf(this.Item);
        }
    }
}
