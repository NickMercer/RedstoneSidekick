﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.Recipes
{
    public class CraftingRecipe
    {
        public int Id { get; set; }

        public string ResultMinecraftId { get; set; }

        public int ResultCount { get; set; }
        
        public IEnumerable<CraftingIngredient> Ingredients { get; set; }
    }
}
