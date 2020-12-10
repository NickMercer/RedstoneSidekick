using NatickCommon.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeSimpleItem : ICraftingTreeItem, INotifyPropertyChanged
    {
        public MinecraftItem Item { get; set; }

        private int _requiredAmount;
        public int RequiredAmount
        {
            get { return _requiredAmount; }
            set
            {
                _requiredAmount = value.Clamp(0, int.MaxValue);
                OnPropertyChanged();
            }
        }

        private int _currentAmount;
        public int CurrentAmount
        {
            get { return _currentAmount; }
            set
            {
                _currentAmount = value.Clamp(0, RequiredAmount);
                OnPropertyChanged();
            }
        }

        public int RecipeAmount { get; set; }
        
        public bool IsSmeltingIngredient { get; set; }

        public bool IsRootItem { get; set; }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public CraftingTreeSimpleItem(MinecraftItem item, int requiredAmount = 0, int recipeAmount = 0, int currentAmount = 0, bool isRootItem = false, bool isSmeltingIngredient = false)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            RecipeAmount = recipeAmount;
            CurrentAmount = currentAmount;
            IsRootItem = isRootItem;
            IsSmeltingIngredient = isSmeltingIngredient;
        }

    }
}
