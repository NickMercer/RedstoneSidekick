using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems
{
    public interface ICraftingTreeCompoundItem : ICraftingTreeItem
    {
        public List<ICraftingTreeItem> Ingredients { get; set; }
    }
}