using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Natick.Utilities;
using System;

namespace RedstoneSidekickWPF.ProjectWindow.UserControls
{
    /// <summary>
    /// Interaction logic for ucGatheringListItem.xaml
    /// </summary>
    public partial class ucGatheringListItem : UserControl, INotifyPropertyChanged
    {
        public IGatheringListItem Item
        {
            get { return (IGatheringListItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(IGatheringListItem), typeof(ucGatheringListItem), new PropertyMetadata(null, SetItem));

        private static void SetItem(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucGatheringListItem;
            var item = e.NewValue as IGatheringListItem;

            if(item != null)
            {
                if(item.IsChecked == false)
                {
                    uc.DisplayCurrentAmount = item.CurrentAmount;
                }
                else
                {
                    uc.DisplayCurrentAmount = item.RequiredAmount;
                }
            }
        }

        public ListView ParentListView
        {
            get { return (ListView)GetValue(ParentListViewProperty); }
            set { SetValue(ParentListViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentListView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentListViewProperty =
            DependencyProperty.Register("ParentListView", typeof(ListView), typeof(ucGatheringListItem), new PropertyMetadata(null));


        private int _displayCurrentAmount;
        public int DisplayCurrentAmount
        {
            get { return _displayCurrentAmount; }
            set
            {
                if (value < Item.RequiredAmount)
                {
                    Item.IsChecked = false;
                    Item.CurrentAmount = value;
                }
                else
                {
                    Item.IsChecked = true;
                }

                _displayCurrentAmount = value.Clamp(0, Item.RequiredAmount);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ucGatheringListItem()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void ChildItem_Click(object sender, RoutedEventArgs e)
        {
            ParentListView.SelectedIndex = ParentListView.Items.IndexOf(this.Item);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if(Item != null)
            {
                DisplayCurrentAmount = Item.CurrentAmount;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if(Item != null)
            {
                DisplayCurrentAmount = Item.RequiredAmount;
            }
        }
    }
}
