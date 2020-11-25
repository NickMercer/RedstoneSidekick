using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems
{
    public class CraftingTreeSimpleItem : ICraftingTreeItem
    {
        public int Id { get; set; }
     
        public string Name { get; set; }
        
        public string ImagePath { get; set; }
        
        public string MinecraftId { get; set; }
        
        public ItemCategory Category { get; set; }
        
        public int RequiredAmount { get; set; }
        
        public int CurrentAmount { get; set; }
        
        public bool IsChecked { get; set; }
    }
}
