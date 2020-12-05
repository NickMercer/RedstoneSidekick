using NatickCommon.WPF;
using RedstoneSidekick;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Logic.StartupDataProcess;
using System.Collections.Generic;

namespace RedstoneSidekickWPF.ProjectWindow
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
            new ItemDataUpdateHandler().RefreshAllData();

            CraftingTreeItems = CreateTestData();
        }


        private List<ICraftingTreeItem> CreateTestData()
        {
            var itemDictionary = new Dictionary<int, int>();
            itemDictionary.Add(10, 5);
            itemDictionary.Add(157, 1000);

            var craftingTree = new ProjectCraftingTree(itemDictionary);
            return craftingTree.Items;
        }
    }
}