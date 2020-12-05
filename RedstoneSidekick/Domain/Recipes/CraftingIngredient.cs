using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.Recipes
{
    public class CraftingIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public string IngredientMinecraftId { get; set; }
        
        public int Count { get; set; }
    }
}
