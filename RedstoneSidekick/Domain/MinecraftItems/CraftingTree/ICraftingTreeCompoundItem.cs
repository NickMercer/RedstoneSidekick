using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public interface ICraftingTreeCompoundItem : ICraftingTreeItem
    {
        public List<ICraftingTreeItem> Ingredients { get; set; }

        public int RecipeResultCount { get; set; }

        public void UpdateIngredientCounts();

        public void UpdateGatheredStatuses();
        public void UpdateIngredientParents();
    }
}