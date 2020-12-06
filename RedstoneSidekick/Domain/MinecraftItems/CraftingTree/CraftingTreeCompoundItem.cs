using RedstoneSidekick.Domain.Recipes;
using RedstoneSidekick.Logic.CraftingTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeCompoundItem : ICraftingTreeCompoundItem, INotifyPropertyChanged
    {
        public MinecraftItem Item { get; set; }

        private int _requiredAmount;
        public int RequiredAmount
        {
            get { return _requiredAmount; }
            set
            {
                _requiredAmount = value;
                UpdateIngredientCounts();
                OnPropertyChanged();
            }
        }

        private int _currentAmount;
        public int CurrentAmount
        {
            get { return _currentAmount; }
            set
            {
                _currentAmount = value;
                OnPropertyChanged();
            }
        }

        public int RecipeAmount { get; set; }

        public bool IsChecked { get; set; }
        
        public List<ICraftingTreeItem> Ingredients { get; set; }
        
        public int RecipeResultCount { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
        
        public CraftingTreeCompoundItem(MinecraftItem item, List<ICraftingTreeItem> ingredients, int requiredAmount = 0, int recipeResultCount = 0, int recipeAmount = 0, int currentAmount = 0)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            Ingredients = ingredients;
            RecipeResultCount = recipeResultCount;
            RecipeAmount = recipeAmount;
            CurrentAmount = currentAmount;

            UpdateIngredientCounts();
        }


        public void UpdateIngredientCounts()
        {
            
            if (Ingredients == null) return;

            var amountToMake = RequiredAmount - CurrentAmount;
            var recipeMultiplier = Math.Ceiling((double)amountToMake / RecipeResultCount);

            foreach (var ingredient in Ingredients)
            {
                ingredient.RequiredAmount = (int)recipeMultiplier * ingredient.RecipeAmount;
            }
        }
    }
}
