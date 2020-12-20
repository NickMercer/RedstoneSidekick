using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Logic.CraftingTree
{
    public static class CraftingTreeSearchExtension
    {
        /// <summary>
        /// Gets all items within the Crafting tree. Roots, compound ingredients, and base ingredients in one list.
        /// </summary>
        /// <param name="rootNodes"></param>
        /// <returns></returns>
        public static IEnumerable<ICraftingTreeItem> Descendants(this IEnumerable<ICraftingTreeItem> rootNodes)
        {
            var nodes = new Stack<ICraftingTreeItem>(rootNodes);
            
            while(nodes.Any())
            {
                ICraftingTreeItem node = nodes.Pop();
                yield return node;
                if(node is ICraftingTreeCompoundItem)
                {
                    var compoundNode = node as ICraftingTreeCompoundItem;
                    foreach(var n in compoundNode.Ingredients)
                    {
                        nodes.Push(n);
                    }
                }
            }
        }
    }
}
