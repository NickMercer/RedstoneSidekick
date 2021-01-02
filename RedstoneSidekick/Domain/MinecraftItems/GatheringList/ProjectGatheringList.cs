using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Logic.GatheringList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Natick.Utilities;
using System.Text;
using Natick.Utilities.ViewModels;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public class ProjectGatheringList : INotifyPropertyChanged
    {
        public RecursiveObservableCollection<IGatheringListItem> Items { get; set; } = new RecursiveObservableCollection<IGatheringListItem>();

        public ProjectGatheringList(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            var items = GatheringListBuilder.GenerateGatheringList(craftingTree);
            Items.AddRange(items);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateList(IEnumerable<ICraftingTreeItem> craftingTree)
        {
            Items.Clear();
            var newItems = GatheringListBuilder.GenerateGatheringList(craftingTree);
            Items.AddRange(newItems);
        }

        internal void Clear()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Items.Clear()));
        }
    }
}
