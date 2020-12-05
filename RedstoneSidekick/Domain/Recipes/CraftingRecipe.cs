using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.Recipes
{
    public class CraftingRecipe
    {
        public int Id { get; set; }

        public string ResultItemMinecraftId { get; set; }

        public int ResultCount { get; set; }
        
        public List<CraftingIngredient> Ingredients { get; set; }
    }
}
