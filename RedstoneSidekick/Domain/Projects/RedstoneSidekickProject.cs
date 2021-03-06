﻿using Natick.Utilities.ViewModels;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Domain.Projects
{
    public class RedstoneSidekickProject : ViewModelBase
    {

        private string _projectName = "Untitled Project";
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }


        private ProjectCraftingTree _craftingTree = new ProjectCraftingTree();
        public ProjectCraftingTree CraftingTree
        {
            get { return _craftingTree; }
            set
            {
                SetProperty(ref _craftingTree, value);
            }
        }

        private ProjectGatheringList _gatheringList;
        public ProjectGatheringList GatheringList
        {
            get { return _gatheringList; }
            set
            {
                SetProperty(ref _gatheringList, value);
            }
        }

        public RedstoneSidekickProject()
        {

        }

        ~RedstoneSidekickProject() 
        {
            Clear();
        }

        public void Clear()
        {
            if (CraftingTree != null)
            {
                foreach(var item in CraftingTree.Items.Where(x => x is ICraftingTreeCompoundItem))
                {
                    (item as ICraftingTreeCompoundItem).RemoveIngredientParents();
                }
                CraftingTree.Clear();
            }
            if(GatheringList != null) GatheringList.Clear();
        }

    }
}
