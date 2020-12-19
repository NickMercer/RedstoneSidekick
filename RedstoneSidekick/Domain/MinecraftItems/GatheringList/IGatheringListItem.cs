using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public interface IGatheringListItem : INotifyPropertyChanged
    {
        public IMinecraftItem Item { get; set; }

        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public double GatheredPercent { get; }

        public bool IsChecked { get; set; }
    }
}
