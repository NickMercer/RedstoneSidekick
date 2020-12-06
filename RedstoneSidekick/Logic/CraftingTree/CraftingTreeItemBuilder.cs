using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Logic.CraftingTree
{
    public static class CraftingTreeItemBuilder
    {
        private static MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();
        private static CraftingRecipeRepository _craftingRecipeRepository = new CraftingRecipeRepository();
        private static Dictionary<string, string> _conversionDictionary = new ConversionDictionaryRepository().GetConversionDictionary();

        public static ICraftingTreeItem CreateCraftingTreeItem(int itemId, int requiredAmount, int currentAmount = 0)
        {
            var mcItem = _minecraftItemRepository.GetMinecraftItemById(itemId);
            var recipe = _craftingRecipeRepository.GetRecipesByMinecraftId(mcItem.MinecraftId).FirstOrDefault();

            ICraftingTreeItem treeItem;

            if (recipe == null)
            {
                treeItem = new CraftingTreeSimpleItem(item: mcItem, requiredAmount: requiredAmount, currentAmount: currentAmount);
            }
            else
            {
                treeItem = CreateRootCompoundItem(mcItem, recipe, requiredAmount, currentAmount);
            }

            return treeItem;
        }

        private static CraftingTreeCompoundItem CreateRootCompoundItem(MinecraftItem mcItem, CraftingRecipe rootRecipe, int requiredAmount, int currentAmount)
        {
            rootRecipe.Ingredients = _craftingRecipeRepository.GetIngredientsByRecipeId(rootRecipe.Id).ToList();
            rootRecipe.Ingredients.ForEach(x => x = MinecraftIdConversion(x));

            List<ICraftingTreeItem> ingredients = new List<ICraftingTreeItem>();

            foreach(var ingredient in rootRecipe.Ingredients)
            {
                var childMCItem = _minecraftItemRepository.GetMinecraftItemById(ingredient.Id);
                var childItem = CreateChildItem(childMCItem, rootRecipe, ingredient.Count, 0);
                ingredients.Add(childItem);
            }

            return new CraftingTreeCompoundItem(item: mcItem, 
                                                requiredAmount: requiredAmount, 
                                                ingredients: ingredients, 
                                                recipeResultCount: rootRecipe.ResultCount, 
                                                currentAmount: currentAmount);
        }

        private static ICraftingTreeItem CreateChildItem(MinecraftItem mcItem, CraftingRecipe parentRecipe, int requiredAmount, int currentAmount)
        {
            ICraftingTreeItem childItem;
            var createSimpleItem = true;

            var recipe = _craftingRecipeRepository.GetRecipesByMinecraftId(mcItem.MinecraftId).FirstOrDefault();
            if (recipe != null)
            {
                recipe.Ingredients = _craftingRecipeRepository.GetIngredientsByRecipeId(recipe.Id).ToList();
                recipe.Ingredients.ForEach(x => x = MinecraftIdConversion(x));

                createSimpleItem = recipe.Ingredients.Select(x => x.IngredientMinecraftId).Contains(parentRecipe.ResultItemMinecraftId);
            }

            if (createSimpleItem)
            {
                var recipeAmount = parentRecipe.Ingredients.Where(x => x.Id == mcItem.Id).First().Count;
                childItem = new CraftingTreeSimpleItem(item: mcItem, recipeAmount: recipeAmount, currentAmount: currentAmount);
            }
            else
            {
                List<ICraftingTreeItem> ingredients = new List<ICraftingTreeItem>();

                foreach (var ingredient in recipe.Ingredients)
                {
                    var childMCItem = _minecraftItemRepository.GetMinecraftItemById(ingredient.Id);
                    var ingredientItem = CreateChildItem(childMCItem, recipe, ingredient.Count, 0);
                    ingredients.Add(ingredientItem);
                }

                var recipeAmount = parentRecipe.Ingredients.Where(x => x.Id == mcItem.Id).First().Count;

                childItem = new CraftingTreeCompoundItem(item: mcItem, 
                                                         ingredients: ingredients, 
                                                         recipeResultCount: recipe.ResultCount, 
                                                         recipeAmount: recipeAmount, 
                                                         currentAmount: currentAmount);
            }

            return childItem;
        }


        private static CraftingIngredient MinecraftIdConversion(CraftingIngredient ingredient)
        {
            if (_conversionDictionary.ContainsKey(ingredient.IngredientMinecraftId))
            {
                ingredient.IngredientMinecraftId = _conversionDictionary[ingredient.IngredientMinecraftId];
                ingredient.Id = _minecraftItemRepository.GetIdByMinecraftId(ingredient.IngredientMinecraftId);
            }
            return ingredient;
        }
    }
}
