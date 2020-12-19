using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Natick.Utilities;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Natick.Utilities.ViewModels;

namespace RedstoneSidekickWPF.ProjectWindow.UserControls
{
    /// <summary>
    /// Interaction logic for ucGatheringList.xaml
    /// </summary>
    public partial class ucGatheringList : UserControl
    { 
        public RecursiveObservableCollection<IGatheringListItem> Items
        {
            get { return (RecursiveObservableCollection<IGatheringListItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(RecursiveObservableCollection<IGatheringListItem>), typeof(ucGatheringList), new PropertyMetadata(null, SetItems));

        private static void SetItems(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucGatheringList;
            var newItems = e.NewValue as RecursiveObservableCollection<IGatheringListItem>;
            var oldItems = e.OldValue as RecursiveObservableCollection<IGatheringListItem>;

            if(oldItems != null && newItems != oldItems)
            {
                oldItems.ChildElementPropertyChanged -= uc.ResetSortType;
            }

            if(newItems != null && newItems != oldItems)
            {
                newItems.ChildElementPropertyChanged += uc.ResetSortType;
            }
        }

        public ObservableCollection<string> SortingTypes { get; set; }

        public string SortType { get; set; }

        private bool _canResetSort = false;
  
        public ucGatheringList()
        {
            SortingTypes = new ObservableCollection<string> { "----------", "Name", "Category", "Required (High to Low)", "Required (Low to High)", "Percent Gathered (High to Low)", "Percent Gathered (Low to High)" };
            SortType = "----------";

            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void ResetSortType(RecursiveObservableCollection<IGatheringListItem>.ChildElementPropertyChangedEventArgs e)
        {
            if (_canResetSort)
            {
                SortType = "----------";
                CB_SortingTypes.SelectedIndex = SortingTypes.IndexOf(SortType);
            }
        }


        private void SortingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GatheringListView.ItemsSource == null) return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(GatheringListView.ItemsSource);

            switch (SortType)
            {
                case "Name":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;

                case "Category":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("Item.Category", ListSortDirection.Ascending));
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;

                case "Required (High to Low)":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("RequiredAmount", ListSortDirection.Descending));
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;

                case "Required (Low to High)":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("RequiredAmount", ListSortDirection.Ascending));
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;

                case "Percent Gathered (High to Low)":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("GatheredPercent", ListSortDirection.Descending));
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;

                case "Percent Gathered (Low to High)":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription("GatheredPercent", ListSortDirection.Ascending));
                    view.SortDescriptions.Add(new SortDescription("Item.Name", ListSortDirection.Ascending));
                    break;
            }

        }

        private void CB_SortingTypes_DropDownClosed(object sender, EventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if(comboBox.SelectedItem.ToString() == "----------")
            {
                _canResetSort = false;
            }
            else
            {
                _canResetSort = true;
            }
        }
    }
}
