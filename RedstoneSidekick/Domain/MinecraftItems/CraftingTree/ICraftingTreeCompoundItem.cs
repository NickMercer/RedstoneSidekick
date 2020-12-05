using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public interface ICraftingTreeCompoundItem : ICraftingTreeItem
    {
        public List<ICraftingTreeItem> Ingredients { get; set; }
    }
}