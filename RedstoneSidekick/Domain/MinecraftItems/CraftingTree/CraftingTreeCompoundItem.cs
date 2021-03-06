﻿using Natick.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeCompoundItem : ICraftingTreeCompoundItem, INotifyPropertyChanged
    {
        public IMinecraftItem Item { get; set; }

        private int _requiredAmount;
        public int RequiredAmount
        {
            get { return _requiredAmount; }
            set
            {
                var prevAmount = _requiredAmount;
                _requiredAmount = value.Clamp(0, int.MaxValue);
                UpdateIngredientCounts();
                if(_requiredAmount != prevAmount) UpdateParent();
                OnPropertyChanged();
            }
        }

        private int _currentAmount;
        public int CurrentAmount
        {
            get { return _currentAmount; }
            set
            {
                var prevAmount = _currentAmount;
                _currentAmount = value.Clamp(0, RequiredAmount);
                UpdateIngredientCounts();
                if(_currentAmount != prevAmount) UpdateParent();
                OnPropertyChanged();
            }
        }

        public int RecipeAmount { get; set; }

        public bool IsSmeltingIngredient { get; set; }
        
        public List<ICraftingTreeItem> Ingredients { get; set; }
        
        public int RecipeResultCount { get; set; }
        public bool IsRootItem { get; set; }
        
        private ICraftingTreeCompoundItem _parent;
        public ICraftingTreeCompoundItem Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                IsRootItem = (_parent == null);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public CraftingTreeCompoundItem()
        {
            Item = new MinecraftItem();
            RequiredAmount = 0;
            Ingredients = new List<ICraftingTreeItem>();
            RecipeResultCount = 0;
            RecipeAmount = 0;
            CurrentAmount = 0;
            Parent = null;
            IsRootItem = true;
            IsSmeltingIngredient = false;
        }
        
        public CraftingTreeCompoundItem(MinecraftItem item, List<ICraftingTreeItem> ingredients, int requiredAmount = 0, int recipeResultCount = 0, int recipeAmount = 0, int currentAmount = 0, ICraftingTreeCompoundItem parent = null, bool isSmeltingIngredient = false)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            Ingredients = ingredients;
            RecipeResultCount = recipeResultCount;
            RecipeAmount = recipeAmount;
            CurrentAmount = currentAmount;
            Parent = parent;
            IsRootItem = (Parent == null);
            IsSmeltingIngredient = isSmeltingIngredient;

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
                ingredient.CurrentAmount = Math.Min(ingredient.CurrentAmount, ingredient.RequiredAmount);
            }
        }

        public void UpdateIngredientParents()
        {
            if (Ingredients == null) return;

            foreach(var ingredient in Ingredients)
            {
                ingredient.Parent = this;
                if(ingredient is ICraftingTreeCompoundItem)
                {
                    (ingredient as ICraftingTreeCompoundItem).UpdateIngredientParents();
                }
            }
        }

        public void RemoveIngredientParents()
        {
            if (Ingredients == null) return;

            foreach (var ingredient in Ingredients)
            {
                ingredient.Parent = null;
                if (ingredient is ICraftingTreeCompoundItem)
                {
                    (ingredient as ICraftingTreeCompoundItem).RemoveIngredientParents();
                }
            }
        }

        public override string ToString()
        {
            return $"Compound Item: {Item.Id} - {Item.Name} - {Item.MinecraftId}";
        }

        public void UpdateParent()
        {
            if(Parent != null)
            {
                Parent.UpdateGatheredStatuses();
            }
        }

        public void UpdateGatheredStatuses()
        {
            //Figure out how many recipes are completed now by this change.
            //add that amount to this objects have count.
            //Then set ingredients counts to the remainder and lower their required amounts.
            var satisfiedIngredients = new List<int>();
            foreach (var ingredient in Ingredients)
            {
                var recipeCount = ingredient.CurrentAmount / ingredient.RecipeAmount;
                satisfiedIngredients.Add(recipeCount);
            }

            var recipesToFulfill = satisfiedIngredients.Min();

            foreach (var ingredient in Ingredients)
            {
                ingredient.CurrentAmount -= recipesToFulfill * ingredient.RecipeAmount;
            }

            CurrentAmount += recipesToFulfill * RecipeResultCount;
        }
    }
}
