using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
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
    /// Interaction logic for ucCraftingTreeItem.xaml
    /// </summary>
    ///
    public partial class ucCraftingTreeItem : UserControl
    {
        public ICraftingTreeItem Item
        {
            get { return (ICraftingTreeItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(ICraftingTreeItem), typeof(ucCraftingTreeItem), new PropertyMetadata(null, SetItem));

        private static void SetItem(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucCraftingTreeItem;
            var item = e.NewValue as ICraftingTreeItem;

            uc.CraftingVisible = item.IsSmeltingIngredient == false && item.IsRootItem == false;
            uc.SmeltingVisible = item.IsSmeltingIngredient == true && item.IsRootItem == false;
        }

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(ucCraftingTreeItem), new PropertyMetadata(false, SetSelected));

        private static void SetSelected(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as ucCraftingTreeItem;
            var selected = (bool)e.NewValue;
            if (selected)
            {
                uc.LayoutRoot.BorderBrush = (SolidColorBrush) Application.Current.Resources["RSDarkRed"];
            }
            else
            {
                uc.LayoutRoot.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        public bool CraftingVisible { get; set; }
        public bool SmeltingVisible { get; set; }

        public ucCraftingTreeItem()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
