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
                _requiredAmount = value;
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



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public CraftingTreeSimpleItem(MinecraftItem item, int requiredAmount = 0, int recipeAmount = 0, int currentAmount = 0)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            RecipeAmount = recipeAmount;
            CurrentAmount = currentAmount;
        }

    }
}
