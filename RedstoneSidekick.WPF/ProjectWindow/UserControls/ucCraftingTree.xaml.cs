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
    /// Interaction logic for ucCraftingTree.xaml
    /// </summary>
    public partial class ucCraftingTree : UserControl
    {
        public List<ICraftingTreeItem> Items
        {
            get { return (List<ICraftingTreeItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<ICraftingTreeItem>), typeof(ucCraftingTree), new PropertyMetadata(null));


        public ucCraftingTree()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
