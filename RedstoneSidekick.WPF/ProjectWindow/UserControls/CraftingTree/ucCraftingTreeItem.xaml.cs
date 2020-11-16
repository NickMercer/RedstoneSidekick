using RedstoneSidekick.Domain.MinecraftItems;
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

namespace RedstoneSidekick.WPF.ProjectWindow.UserControls.CraftingTree
{
    /// <summary>
    /// Interaction logic for ucCraftingTreeItem.xaml
    /// </summary>
    public partial class ucCraftingTreeItem : UserControl
    {
        public ICraftingTreeItem Item
        {
            get { return (ICraftingTreeItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(ICraftingTreeItem), typeof(ucCraftingTreeItem), new PropertyMetadata(null));



        public ucCraftingTreeItem()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
