using Newtonsoft.Json;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedstoneSidekick.Logic.ProjectStrings
{
    public static class ProjectStringEncoder
    {

        public static string Encode(RedstoneSidekickProject project, CraftingTreeOptions craftingTree = CraftingTreeOptions.Light)
        {
            StringBuilder projectStringBuilder = new StringBuilder();

            projectStringBuilder.Append(AppendProjectName(project));
            
            projectStringBuilder.Append(AppendCraftingTree(project, craftingTree));

            projectStringBuilder.Append(AppendShoppingList(project));

            return projectStringBuilder.ToString();
        }


        private static string AppendProjectName(RedstoneSidekickProject project)
        {
            return $"{project.ProjectName}";
        }
        private static string AppendCraftingTree(RedstoneSidekickProject project, CraftingTreeOptions craftingTree)
        {
            var craftingTreeString = "|";

            switch (craftingTree)
            {
                case CraftingTreeOptions.Full:
                    craftingTreeString += "2";
                    craftingTreeString += ConvertFullCraftingTreeToBase64(project.CraftingTree);
                    break;

                case CraftingTreeOptions.Light:
                    craftingTreeString += "1";
                    craftingTreeString += ConvertLightCraftingTreeToBase64(project.CraftingTree);
                    break;

                default:
                    craftingTreeString += "0";
                    break;
            }

            return craftingTreeString;
        }

        private static string ConvertFullCraftingTreeToBase64(ProjectCraftingTree craftingTree)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    var settings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    var treeJson = JsonConvert.SerializeObject(craftingTree.Items, settings);
                    writer.Write(treeJson);
                }

                byte[] treeArray = m.ToArray();

                return Convert.ToBase64String(treeArray);
            }
        }
        private static string ConvertLightCraftingTreeToBase64(ProjectCraftingTree craftingTree)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    foreach (var item in craftingTree.Items)
                    {
                        writer.Write(item.Item.Id);
                        writer.Write(item.RequiredAmount);
                    }
                }

                byte[] treeArray = m.ToArray();

                return Convert.ToBase64String(treeArray);
            }
        }

        private static string AppendShoppingList(RedstoneSidekickProject project)
        {
            return "|";
        }

        public enum CraftingTreeOptions
        {
            None,
            Light,
            Full
        }
    }
}
