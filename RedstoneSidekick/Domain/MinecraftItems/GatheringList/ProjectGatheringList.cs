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

        public ProjectGatheringList(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            var items = GatheringListBuilder.GenerateGatheringList(craftingTree);
            Items.AddRange(items);
        }

        public void UpdateList(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            Items.Clear();
            var newItems = GatheringListBuilder.GenerateGatheringList(craftingTree);
            Items.AddRange(newItems);
        }
    }
}
