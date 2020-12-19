using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Logic.GatheringList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Natick.Utilities;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public class ProjectGatheringList
    {
        public ObservableCollection<IGatheringListItem> Items { get; set; } = new ObservableCollection<IGatheringListItem>();

        public ProjectGatheringList()
        {
            GatheringListItem item1 = new GatheringListItem(new MinecraftItem(), 10, 1);
            GatheringListItem item2 = new GatheringListItem(new MinecraftItem(), 12, 0);

            Items.Add(item1);
            Items.Add(item2);
        }

        public ProjectGatheringList(List<ICraftingTreeItem> craftingTree)
        {
            Items = GatheringListBuilder.GenerateGatheringList(craftingTree).ToObservableCollection();
        }
    }
}
