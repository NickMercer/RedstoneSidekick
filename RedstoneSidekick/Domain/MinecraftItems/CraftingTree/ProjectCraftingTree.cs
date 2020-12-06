using RedstoneSidekick.Logic.CraftingTree;
using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class ProjectCraftingTree
    {
        public List<ICraftingTreeItem> Items { get; set; } = new List<ICraftingTreeItem>();

        
        public ProjectCraftingTree(Dictionary<int, int> itemDictionary)
        {
            Items = GenerateItemTree(itemDictionary);
        }

        private List<ICraftingTreeItem> GenerateItemTree(Dictionary<int, int> itemDictionary)
        {
            var itemTree = new List<ICraftingTreeItem>();

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
