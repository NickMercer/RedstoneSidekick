using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Logic.GatheringList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Natick.Utilities;
using System.Text;
using Natick.Utilities.ViewModels;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public class ProjectGatheringList
    {
        public RecursiveObservableCollection<IGatheringListItem> Items { get; set; } = new RecursiveObservableCollection<IGatheringListItem>();

        public ProjectGatheringList()
        {
            GatheringListItem item1 = new GatheringListItem(new MinecraftItem { Name = "Brewing Stand", Category = ItemCategory.Brewing }, 10, 1);
            GatheringListItem item2 = new GatheringListItem(new MinecraftItem { Name = "Stone", Category = ItemCategory.BuildingBlocks }, 12, 0);
            GatheringListItem item3 = new GatheringListItem(new MinecraftItem { Name = "Grass", Category = ItemCategory.BuildingBlocks}, 2, 0);
            GatheringListItem item4 = new GatheringListItem(new MinecraftItem { Name = "Redstone Dust", Category = ItemCategory.Redstone }, 5, 0);

            Items.Add(item1);
            Items.Add(item2);
            Items.Add(item3);
            Items.Add(item4);   
        }

        public ProjectGatheringList(List<ICraftingTreeItem> craftingTree)
        {
            Items = GatheringListBuilder.GenerateGatheringList(craftingTree).ToRecursiveObservableCollection();
        }
    }
}
