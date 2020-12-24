using Natick.Utilities.ViewModels;
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

        public ObservableCollection<string> SortingTypes { get; set; }

        public string SortType { get; set; }

        private bool _canResetSort = true;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ToggleItemBrowserCommand ToggleItemBrowserCommand { get; set; }
        public AddItemCommand AddItemCommand { get; set; }
        public RemoveItemCommand RemoveItemCommand { get; set; }

        public ucCraftingTree()
        {
            SortingTypes = new ObservableCollection<string> { "----------", "Name", "Category", "Required (High to Low)", "Required (Low to High)" };
            SortType = "----------";

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
    }
}
