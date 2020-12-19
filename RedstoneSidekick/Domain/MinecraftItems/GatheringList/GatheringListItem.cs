using Natick.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems.GatheringList
{
    public class GatheringListItem : IGatheringListItem, INotifyPropertyChanged
    {
        private IMinecraftItem _item;
        public IMinecraftItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        private int _requiredAmount;
        public int RequiredAmount
        {
            get { return _requiredAmount; }
            set
            {
                _requiredAmount = value.Clamp(1, int.MaxValue);
                OnPropertyChanged();
                UpdateGatheredPercent();
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
                UpdateGatheredPercent();
            }
        }

        private double _gatheredPercent;
        public double GatheredPercent
        {
            get { return _gatheredPercent; }
            set
            {
                _gatheredPercent = value;
                OnPropertyChanged();
            }
        }

        private bool _isChecked;
        public bool IsChecked 
        { 
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
                UpdateGatheredPercent();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        public GatheringListItem(IMinecraftItem item, int requiredAmount, int currentAmount, bool isChecked = false)
        {
            Item = item;
            RequiredAmount = requiredAmount;
            CurrentAmount = currentAmount;
            IsChecked = isChecked;
        }

        public void UpdateGatheredPercent()
        {
            var reqAmount = Math.Max(1, RequiredAmount);
            if(IsChecked)
            {
                GatheredPercent = 100;
            }
            else
            {
                GatheredPercent = (double)CurrentAmount / reqAmount * 100;
            }
        }
    }
}
