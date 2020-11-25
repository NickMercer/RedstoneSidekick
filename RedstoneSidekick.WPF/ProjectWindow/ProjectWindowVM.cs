using NatickCommon.WPF;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Logic.StartupDataProcess;
using System.Collections.Generic;

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
                RequiredAmount = 5,
            };

            var diamondBlockIngredients = new CraftingTreeSimpleItem
            {
                Name = "Diamond",
                ImagePath = "/Images/Miscellaneous/Diamond.png",
                RequiredAmount = 9 * 1024,
                CurrentAmount = 1000
            };

            var item2 = new CraftingTreeCompoundItem
            {
                Name = "Block of Diamond",
                ImagePath = "/Images/Blocks/DiamondBlock.png",
                RequiredAmount = 1024,
                CurrentAmount = 6,
                Ingredients = new List<ICraftingTreeItem>() { diamondBlockIngredients }
            };

            items.Add(item1);
            items.Add(item2);

            return items;
        }
    }
}