using Natick.Utilities.ViewModels;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekickWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ucCraftingTree.xaml
    /// </summary>
    public partial class ucCraftingTree : UserControl, INotifyPropertyChanged
    {
        public RecursiveObservableCollection<ICraftingTreeItem> Items
        {
            get { return (RecursiveObservableCollection<ICraftingTreeItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(RecursiveObservableCollection<ICraftingTreeItem>), typeof(ucCraftingTree), new PropertyMetadata(null, SetItems));

        private static void SetItems(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucCraftingTree;
            var newItems = e.NewValue as RecursiveObservableCollection<ICraftingTreeItem>;
            var oldItems = e.OldValue as RecursiveObservableCollection<ICraftingTreeItem>;

            if (oldItems != null && (newItems == null || !oldItems.SequenceEqual(newItems)))
            {
                oldItems.ChildElementPropertyChanged -= uc.ResetSortType;
            }

            if (newItems != null && (oldItems == null || !newItems.SequenceEqual(oldItems)))
            {
                newItems.ChildElementPropertyChanged += uc.ResetSortType;
            }
        }

        public ObservableCollection<IMinecraftItem> MinecraftItems
        {
            get { return (ObservableCollection<IMinecraftItem>)GetValue(MinecraftItemsProperty); }
            set { SetValue(MinecraftItemsProperty, value); }
        }

        public static readonly DependencyProperty MinecraftItemsProperty =
            DependencyProperty.Register("MinecraftItems", typeof(ObservableCollection<IMinecraftItem>), typeof(ucCraftingTree), new PropertyMetadata(null, SetMinecraftItems));

        private static void SetMinecraftItems(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucCraftingTree;
            var list = e.NewValue as ObservableCollection<IMinecraftItem>;

            if(uc.MinecraftItems.FirstOrDefault(x => x.Name == "Air") != null)
            {
                uc.MinecraftItems.Remove(uc.MinecraftItems.First(x => x.Name == "Air"));
            }

            uc._minecraftItemView = (CollectionView)CollectionViewSource.GetDefaultView(uc.MinecraftItems);
            uc._minecraftItemView.Filter = uc.MinecraftItemFilter;
            uc._filters.Add("Category", uc.ItemCategoryFilter);
            uc._filters.Add("Search Text", uc.SearchTextFilter);
        }

        public ObservableCollection<string> SortingTypes { get; set; }

        public string SortType { get; set; }

        private bool _canResetSort = true;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public List<string> Categories { get; set; }

        private int _categoryIndex;
        public int CategoryIndex
        {
            get { return _categoryIndex; }
            set 
            { 
                _categoryIndex = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, Predicate<IMinecraftItem>> _filters = new Dictionary<string, Predicate<IMinecraftItem>>();

        private ICollectionView _minecraftItemView;

        public ToggleItemBrowserCommand ToggleItemBrowserCommand { get; set; }
        public AddItemCommand AddItemCommand { get; set; }
        public RemoveItemCommand RemoveItemCommand { get; set; }

        public ucCraftingTree()
        {
            SortingTypes = new ObservableCollection<string> { "----------", "Name", "Category", "Required (High to Low)", "Required (Low to High)" };
            SortType = "----------";

            Categories = new List<string>
            {
                "All",
                "Building Blocks",
                "Decorations",
                "Redstone",
                "Transportation",
                "Miscellaneous",
                "Foodstuffs",
                "Brewing",
                "Combat"
            };
            CategoryIndex = 0;

            ToggleItemBrowserCommand = new ToggleItemBrowserCommand(this);
            AddItemCommand = new AddItemCommand(this);
            RemoveItemCommand = new RemoveItemCommand(this);

            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void ResetSortType(RecursiveObservableCollection<ICraftingTreeItem>.ChildElementPropertyChangedEventArgs e)
        {
            if (_canResetSort)
            {
                SortType = "----------";
                CB_SortingTypes.SelectedIndex = SortingTypes.IndexOf(SortType);
            }
        }


        private void SortingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CraftingTreeView.ItemsSource == null) return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CraftingTreeView.ItemsSource);

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
            }
        }

        private void CB_SortingTypes_DropDownClosed(object sender, EventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem.ToString() == "----------")
            {
                _canResetSort = false;
            }
            else
            {
                _canResetSort = true;
            }
        }

        #region Add Item Menu Sorting

        private void ItemCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMinecraftItems();
        }

        private void TB_ItemSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMinecraftItems();
        }

        private void FilterMinecraftItems()
        {
            _minecraftItemView.Refresh();
        }

        private bool MinecraftItemFilter(object obj)
        {
            var item = obj as IMinecraftItem;
            return _filters.Values.Aggregate(true, (prevValue, predicate) => prevValue && predicate(item));
        }

        private bool SearchTextFilter(object obj)
        {
            var item = obj as IMinecraftItem;
            if(String.IsNullOrWhiteSpace(TB_ItemSearch.Text))
            {
                return true;
            }
            else
            {
                return item.Name.ToLower().Contains(TB_ItemSearch.Text.ToLower());
            }
        }

        private bool ItemCategoryFilter(object obj)
        {
            IMinecraftItem item = obj as IMinecraftItem;

            switch (Categories[CategoryIndex])
            {
                case "All":
                    return true;

                case "Building Blocks":
                    return item.Category == ItemCategory.BuildingBlocks ? true : false;

                case "Decorations":
                    return item.Category == ItemCategory.Decorations ? true : false;

                case "Redstone":
                    return item.Category == ItemCategory.Redstone ? true : false;

                case "Transportation":
                    return item.Category == ItemCategory.Transportation ? true : false;

                case "Miscellaneous":
                    return item.Category == ItemCategory.Miscellaneous ? true : false;

                case "Foodstuffs":
                    return item.Category == ItemCategory.Foodstuffs ? true : false;

                case "Brewing":
                    return item.Category == ItemCategory.Brewing ? true : false;

                case "Combat":
                    return item.Category == ItemCategory.Combat ? true : false;
            }

            return false;
        }



        #endregion

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as Button).DataContext;
            LV_MinecraftItems.SelectedItem = item;
            AddItemCommand.Execute(item);
        }
    }
}
