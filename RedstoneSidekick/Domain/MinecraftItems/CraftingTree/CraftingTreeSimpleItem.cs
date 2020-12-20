using Natick.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class CraftingTreeSimpleItem : ICraftingTreeItem, INotifyPropertyChanged
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
                if(_currentAmount != prevAmount) UpdateParent();
                OnPropertyChanged();
            }
        }

        public int RecipeAmount { get; set; }
        
        public bool IsSmeltingIngredient { get; set; }

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


        public CraftingTreeSimpleItem(MinecraftItem item, int requiredAmount = 0, int recipeAmount = 0, int currentAmount = 0, ICraftingTreeCompoundItem parent = null, bool isSmeltingIngredient = false)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            RecipeAmount = recipeAmount;
            CurrentAmount = currentAmount;
            Parent = parent;
            IsRootItem = (Parent == null);
            IsSmeltingIngredient = isSmeltingIngredient;
        }

        public override string ToString()
        {
            return $"Simple Item: {Item.Id} - {Item.Name} - {Item.MinecraftId}";
        }

        public void UpdateParent()
        {
            if (Parent != null)
            {
                Parent.UpdateGatheredStatuses();
            }
        }
    }
}
