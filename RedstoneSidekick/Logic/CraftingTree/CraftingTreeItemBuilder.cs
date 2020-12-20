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
        private static readonly MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();
        private static readonly CraftingRecipeRepository _craftingRecipeRepository = new CraftingRecipeRepository();
        private static readonly Dictionary<string, string> _conversionDictionary = new ConversionDictionaryRepository().GetConversionDictionary();
        private static readonly IEnumerable<SmeltingRecipe> _smeltingRecipes = new SmeltingRecipeRepository().GetSmeltingRecipes();


        public static ICraftingTreeItem CreateCraftingTreeItem(int itemId, int requiredAmount, int currentAmount = 0)
        {
            var mcItem = _minecraftItemRepository.GetMinecraftItemById(itemId);
            var recipe = _craftingRecipeRepository.GetRecipesByMinecraftId(mcItem.MinecraftId).FirstOrDefault();

            ICraftingTreeItem treeItem;

            if (recipe == null)
            {
                var smeltingRecipe = _smeltingRecipes.FirstOrDefault(x => x.ResultId == mcItem.Id);

                if(smeltingRecipe != null)
                {
                    treeItem = CreateSmeltingItem(mcItem, smeltingRecipe, requiredAmount, currentAmount, parent: null);
                }
                else
                {
                    treeItem = new CraftingTreeSimpleItem(item: mcItem, requiredAmount: requiredAmount, currentAmount: currentAmount, parent: null);
                }
            }
            else
            {
                treeItem = CreateRootCompoundItem(mcItem, recipe, requiredAmount, currentAmount);
            }

            return treeItem;
        }

        private static CraftingTreeCompoundItem CreateSmeltingItem(MinecraftItem mcItem, SmeltingRecipe smeltingRecipe, int requiredAmount, int currentAmount, ICraftingTreeCompoundItem parent = null, int recipeAmount = 1)
        {
            List<ICraftingTreeItem> ingredients = new List<ICraftingTreeItem>();

            var smeltingIngredientMCItem = _minecraftItemRepository.GetMinecraftItemById(smeltingRecipe.IngredientId);
            var smeltingIngredientItem = CreateSmeltingIngredient(smeltingIngredientMCItem, currentAmount);
            ingredients.Add(smeltingIngredientItem);


            var smeltingItem = new CraftingTreeCompoundItem(item: mcItem,
                                                requiredAmount: requiredAmount,
                                                ingredients: ingredients,
                                                recipeResultCount: 1,
                                                recipeAmount: recipeAmount,
                                                currentAmount: currentAmount,
                                                parent: parent);

            smeltingIngredientItem.Parent = smeltingItem;

            return smeltingItem;
        }

        private static ICraftingTreeItem CreateSmeltingIngredient(MinecraftItem mcItem, int currentAmount)
        {
            ICraftingTreeItem childItem;
            var createSimpleItem = true;

            var recipe = _craftingRecipeRepository.GetRecipesByMinecraftId(mcItem.MinecraftId).FirstOrDefault();
            if (recipe != null)
            {

                createSimpleItem = false;
            }

            if (createSimpleItem)
            {
                var recipeAmount = 1;
                childItem = new CraftingTreeSimpleItem(item: mcItem, recipeAmount: recipeAmount, currentAmount: currentAmount, isSmeltingIngredient: true);
            }
            else
            {
                recipe.Ingredients = _craftingRecipeRepository.GetIngredientsByRecipeId(recipe.Id);
                recipe.Ingredients.ToList().ForEach(x => x = MinecraftIdConversion(x));

                List<ICraftingTreeItem> ingredients = new List<ICraftingTreeItem>();

                foreach (var ingredient in recipe.Ingredients)
                {
                    var childMCItem = _minecraftItemRepository.GetMinecraftItemById(ingredient.Id);
                    var ingredientItem = CreateChildItem(childMCItem, recipe, ingredient.Count, 0);
                    ingredients.Add(ingredientItem);
                }

                var recipeAmount = 1;

                childItem = new CraftingTreeCompoundItem(item: mcItem,
                                                         ingredients: ingredients,
                                                         recipeResultCount: recipe.ResultCount,
                                                         recipeAmount: recipeAmount,
                                                         currentAmount: currentAmount,
                                                         isSmeltingIngredient: true);

                ingredients.ForEach(x => x.Parent = childItem as ICraftingTreeCompoundItem);
            }

            return childItem;
        }

        private static CraftingTreeCompoundItem CreateRootCompoundItem(MinecraftItem mcItem, CraftingRecipe rootRecipe, int requiredAmount, int currentAmount)
        {
            rootRecipe.Ingredients = _craftingRecipeRepository.GetIngredientsByRecipeId(rootRecipe.Id).ToList();
            rootRecipe.Ingredients.ToList().ForEach(x => x = MinecraftIdConversion(x));

            List<ICraftingTreeItem> ingredients = new List<ICraftingTreeItem>();

            foreach(var ingredient in rootRecipe.Ingredients)
            {
                var childMCItem = _minecraftItemRepository.GetMinecraftItemById(ingredient.Id);
                var childItem = CreateChildItem(childMCItem, rootRecipe, ingredient.Count, 0);
                ingredients.Add(childItem);
            }

            var rootItem = new CraftingTreeCompoundItem(item: mcItem, 
                                                requiredAmount: requiredAmount, 
                                                ingredients: ingredients, 
                                                recipeResultCount: rootRecipe.ResultCount, 
                                                currentAmount: currentAmount,
                                                parent: null);

            ingredients.ForEach(x => x.Parent = rootItem);

            return rootItem;
        }

        private static ICraftingTreeItem CreateChildItem(MinecraftItem mcItem, CraftingRecipe parentRecipe, int requiredAmount, int currentAmount)
        {
            ICraftingTreeItem childItem;
            var createSimpleItem = true;

            var recipe = _craftingRecipeRepository.GetRecipesByMinecraftId(mcItem.MinecraftId).FirstOrDefault();
            var smeltingRecipe = _smeltingRecipes.FirstOrDefault(x => x.ResultId == mcItem.Id);

            if(smeltingRecipe != null)
            {
                var recipeAmount = parentRecipe.Ingredients.Where(x => x.Id == mcItem.Id).First().Count;
                childItem = CreateSmeltingItem(mcItem, smeltingRecipe, requiredAmount, currentAmount, recipeAmount: recipeAmount);
                return childItem;
            }

            if (recipe != null)
            {
                recipe.Ingredients = _craftingRecipeRepository.GetIngredientsByRecipeId(recipe.Id).ToList();
                recipe.Ingredients.ToList().ForEach(x => x = MinecraftIdConversion(x));

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

                ingredients.ForEach(x => x.Parent = childItem as ICraftingTreeCompoundItem);
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
