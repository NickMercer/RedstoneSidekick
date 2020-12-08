using NatickCommon.WPF;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        public RedstoneSidekickProject()
        {

        }

    }
}
