using RedstoneSidekick.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class ProjectCraftingTree
    {

        private MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();
        private CraftingRecipeRepository _craftingRecipeRepository = new CraftingRecipeRepository();

        public List<ICraftingTreeItem> Items { get; set; } = new List<ICraftingTreeItem>();

        
        public ProjectCraftingTree(Dictionary<int, int> itemDictionary)
        {
            Items = GenerateItemTree(itemDictionary);
        }

        private List<ICraftingTreeItem> GenerateItemTree(Dictionary<int, int> itemDictionary)
        {
            var itemTree = new List<ICraftingTreeItem>();

            //Foreach pair in the dictionary. Determine whether there is a recipe for that item. 
            //If there is, create a compound item and 
            //recursively do the same with it, adding those to the items ingredients list.
            //If there is no recipe for the item, add it as a simple item.

            foreach (var itemPair in itemDictionary)
            {
                var itemId = itemPair.Key;
                var itemCount = itemPair.Value;
                
                var mcItem = _minecraftItemRepository.GetMinecraftItemById(itemId);
                var recipe = _craftingRecipeRepository.GetRecipeByMinecraftId(mcItem.MinecraftId);

                ICraftingTreeItem treeItem = null;

                if(recipe == null)
                {
                    treeItem = new CraftingTreeSimpleItem(mcItem, itemCount);
                    itemTree.Add(treeItem);
                }

                
            }

            return itemTree;
        }
    }
}
