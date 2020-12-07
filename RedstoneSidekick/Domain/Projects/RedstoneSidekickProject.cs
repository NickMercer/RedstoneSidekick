using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.Projects
{
    public class RedstoneSidekickProject
    {
        public string ProjectName { get; set; } = "Untitled Project";

        public ProjectCraftingTree CraftingTree { get; set; } = new ProjectCraftingTree();

    }
}
