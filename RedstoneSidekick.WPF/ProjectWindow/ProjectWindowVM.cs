using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NatickCommon.WPF;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Logic.StartupDataProcess;

namespace RedstoneSidekick.WPF.ProjectWindow
{
    public class ProjectWindowVM : ViewModelBase
    {
        private string _projectName = "New Project";
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }


        private List<ICraftingTreeItem> _craftingTreeItems = new List<ICraftingTreeItem>();
        public List<ICraftingTreeItem> CraftingTreeItems
        {
            get { return _craftingTreeItems; }
            set { SetProperty(ref _craftingTreeItems, value); }
        }


        public ProjectWindowVM()
        {
            new ItemDataUpdateHandler().RefreshMinecraftItemData();

            CraftingTreeItems = CreateTestData();
        }

        private List<ICraftingTreeItem> CreateTestData()
        {
            var items = new List<ICraftingTreeItem>();

            var item1 = new CraftingTreeSimpleItem
            {
                Name = "Dirt Block",
                ImagePath = "/Images/Blocks/DirtBlock.png",
                Count = 5,
            };

            var item2 = new CraftingTreeSimpleItem
            {
                Name = "Block of Diamond",
                ImagePath = "/Images/Blocks/DiamondBlock.png",
                Count = 1024,
            };

            items.Add(item1);
            items.Add(item2);

            return items;
        }
    }
}
