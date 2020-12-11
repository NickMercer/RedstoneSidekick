
using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public interface ICraftingTreeItem
    {
        public IMinecraftItem Item { get; set; }

        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public int RecipeAmount { get; set; }

        public bool IsRootItem { get; set; }

        public bool IsSmeltingIngredient { get; set; }

    }
}