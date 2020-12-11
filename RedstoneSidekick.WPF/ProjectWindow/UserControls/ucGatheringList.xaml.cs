using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NatickCommon.ExtensionMethods;
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
    /// Interaction logic for ucGatheringList.xaml
    /// </summary>
    public partial class ucGatheringList : UserControl
    { 
        public ObservableCollection<IGatheringListItem> Items
        {
            get { return (ObservableCollection<IGatheringListItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<IGatheringListItem>), typeof(ucGatheringList), new PropertyMetadata(null));

        public ObservableCollection<string> SortingTypes { get; set; }

        public string SortType { get; set; }

  
        public ucGatheringList()
        {
            SortingTypes = new ObservableCollection<string> { "Name", "Category", "Required (High to Low)", "Required (Low to High)", "Percent Gathered (High to Low)", "Percent Gathered (Low to High)" };
            SortType = "Name";

            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void SortingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sortedItems = Items;

            switch (SortType)
            {
                case "Name":
                    sortedItems = Items.OrderBy(x => x.Item.Name).ToObservableCollection();
                    break;

                case "Category":
                    sortedItems = Items.OrderBy(x => x.Item.Category).ToObservableCollection();
                    break;

                case "Required (High to Low)":
                    sortedItems = Items.OrderByDescending(x => x.RequiredAmount).ToObservableCollection();
                    break;

                case "Required (Low to High)":
                    sortedItems = Items.OrderBy(x => x.RequiredAmount).ToObservableCollection();
                    break;

                case "Pecent Gathered (High to Low)":
                    sortedItems = Items.OrderByDescending(x => x.GatheredPercent).ToObservableCollection();
                    break;

                case "Percent Gathered (Low to High)":
                    sortedItems = Items.OrderBy(x => x.GatheredPercent).ToObservableCollection();
                    break;
            }

            Items = sortedItems;
        }
    }
}
