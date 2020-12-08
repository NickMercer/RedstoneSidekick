using RedstoneSidekick.Logic.CraftingTree;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class ProjectCraftingTree
    {
        public ObservableCollection<ICraftingTreeItem> Items { get; set; } = new ObservableCollection<ICraftingTreeItem>();

        
        public ProjectCraftingTree()
        {

        }

        public ProjectCraftingTree(Dictionary<int, int> itemDictionary)
        {
            Items = GenerateItemTree(itemDictionary);
        }

        private ObservableCollection<ICraftingTreeItem> GenerateItemTree(Dictionary<int, int> itemDictionary)
        {
            var itemTree = new ObservableCollection<ICraftingTreeItem>();

            foreach (var itemPair in itemDictionary)
            {
                var itemId = itemPair.Key;
                var itemCount = itemPair.Value;

                var treeItem = CraftingTreeItemBuilder.CreateCraftingTreeItem(itemId, itemCount);
                itemTree.Add(treeItem);
            }

            return itemTree;
        }
    }
}
