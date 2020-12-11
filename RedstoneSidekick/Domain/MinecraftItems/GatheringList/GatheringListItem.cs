using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public class GatheringListItem : IGatheringListItem
    {
        public IMinecraftItem Item { get; set; }
        public int RequiredAmount { get; set; }
        public int CurrentAmount { get; set; }
        public double GatheredPercent => (double)CurrentAmount / RequiredAmount;
        public bool IsChecked { get; set; }

        public GatheringListItem(IMinecraftItem item, int requiredAmount, int currentAmount, bool isChecked = false)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            CurrentAmount = currentAmount;
            IsChecked = isChecked;
        }
    }
}
