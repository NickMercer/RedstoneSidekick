using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.GatheringList
{
    public static class GatheringListBuilder
    {
        public static List<IGatheringListItem> GenerateGatheringList(List<ICraftingTreeItem> craftingTree)
        {
            return new List<IGatheringListItem>();
        }
    }
}
