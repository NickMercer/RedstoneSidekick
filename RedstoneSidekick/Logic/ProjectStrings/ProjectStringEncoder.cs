using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.ProjectStrings
{
    public static class ProjectStringEncoder
    {
        //Should be modular.
        //Encoding options: Project Name, CraftingTree Items/Counts, FullCraftingTree Data, Shopping List Info

        public static string Encode(RedstoneSidekickProject project, bool name = true, CraftingTreeOptions craftingTree = CraftingTreeOptions.Light, bool shoppingList = false)
        {
            StringBuilder projectStringBuilder = new StringBuilder();

            if (name)
            {
                projectStringBuilder.Append(AppendProjectName(project));
            }
            
            if (craftingTree != CraftingTreeOptions.None)
            {
                projectStringBuilder.Append(AppendCraftingTree(project, craftingTree));
            }

            if (shoppingList)
            {
                projectStringBuilder.Append(AppendShoppingList(project));
            }

            return projectStringBuilder.ToString();
        }


        private static bool AppendProjectName(RedstoneSidekickProject project)
        {
            throw new NotImplementedException();
        }
        private static bool AppendCraftingTree(RedstoneSidekickProject project, CraftingTreeOptions craftingTree)
        {
            throw new NotImplementedException();
        }
        private static bool AppendShoppingList(RedstoneSidekickProject project)
        {
            throw new NotImplementedException();
        }

        public enum CraftingTreeOptions
        {
            None,
            Light,
            Full
        }
    }
}
