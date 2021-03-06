﻿using Natick.Utilities.ViewModels;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using RedstoneSidekick.Logic.CraftingTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public class ProjectCraftingTree : INotifyPropertyChanged
    {
        public RecursiveObservableCollection<ICraftingTreeItem> Items { get; set; } = new RecursiveObservableCollection<ICraftingTreeItem>();

        
        public ProjectCraftingTree()
        {

        }

        public ProjectCraftingTree(Dictionary<int, int> itemDictionary)
        {
            Items = GenerateItemTree(itemDictionary);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private RecursiveObservableCollection<ICraftingTreeItem> GenerateItemTree(Dictionary<int, int> itemDictionary)
        {
            var itemTree = new RecursiveObservableCollection<ICraftingTreeItem>();

            foreach (var itemPair in itemDictionary)
            {
                var itemId = itemPair.Key;
                var itemCount = itemPair.Value;

                var treeItem = CraftingTreeItemBuilder.CreateCraftingTreeItem(itemId, itemCount);
                itemTree.Add(treeItem);
            }

            return itemTree;
        }

        public void Clear()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Items.Clear()));
        }

        public void UpdateItemTree(IEnumerable<IGatheringListItem> gatheringList)
        {
            var allItems = Items.Descendants();

            foreach (var gatheringItem in gatheringList)
            {
                var craftingItems = allItems.Where(x => x.Item.Id == gatheringItem.Item.Id);

                var amountToAdd = gatheringItem.CurrentAmount;
                if (gatheringItem.IsChecked) amountToAdd = gatheringItem.RequiredAmount;

                foreach (var item in craftingItems)
                {
                    if (amountToAdd <= 0)
                    {
                        item.CurrentAmount = 0;
                        break;
                    }

                    if (amountToAdd > item.RequiredAmount)
                    {
                        item.CurrentAmount = item.RequiredAmount;
                        amountToAdd -= item.RequiredAmount;
                    }
                    else
                    {
                        item.CurrentAmount = amountToAdd;
                        amountToAdd = 0;
                    }
                    
                }
            }

        }
    }
}
