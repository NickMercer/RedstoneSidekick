
using System.Collections.Generic;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public interface ICraftingTreeItem
    {
        public MinecraftItem Item { get; set; }

        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public int RecipeAmount { get; set; }

        public bool IsChecked { get; set; }

        //public Dictionary<int, int> CalculateTotalMaterials(Dictionary<int, int> materialsDictionary);

    }
}