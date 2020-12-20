using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System;
using System.Collections.Generic;
using System.Linq;
using RedstoneSidekick.Logic.CraftingTree;
using System.Text;

namespace RedstoneSidekick.Logic.GatheringList
{
    public static class GatheringListBuilder
    {
        public static IEnumerable<IGatheringListItem> GenerateGatheringList(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            var simpleItems = GetBaseIngredients(craftingTree);
            var gatheringList = ConvertToGatheringItems(simpleItems).ToList();

            return gatheringList;
        }
        private static IEnumerable<ICraftingTreeItem> GetBaseIngredients(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            var simpleItems = craftingTree.Descendants().Where(x => (x is CraftingTreeSimpleItem || (x as ICraftingTreeCompoundItem).Ingredients.All(x => x.CurrentAmount == x.RequiredAmount)));

            simpleItems = simpleItems.Where(x => x.CurrentAmount < x.RequiredAmount);

            return simpleItems;
        }

        private static IEnumerable<IGatheringListItem> ConvertToGatheringItems(IEnumerable<ICraftingTreeItem> simpleItems)
        {
            return simpleItems.GroupBy(x => x.Item.Id).Select(g => new GatheringListItem(g.First().Item, g.Sum(x => x.RequiredAmount),g.Sum(x => x.CurrentAmount)));
        }

    }
}
