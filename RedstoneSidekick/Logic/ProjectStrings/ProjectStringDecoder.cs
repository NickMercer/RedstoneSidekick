using Newtonsoft.Json;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using static RedstoneSidekick.Logic.ProjectStrings.ProjectStringEncoder;

namespace RedstoneSidekick.Logic.ProjectStrings
{
    public static class ProjectStringDecoder
    {
        private const int _projectCodeSections = 3;

        //TODO: Add more thorough validation for project codes.
        public static bool IsValidProjectString(string projectCode)
        {
            var encodedProject = projectCode.Trim();

            var delimiterCount = encodedProject.Length - encodedProject.Replace("|", "").Length;

            if(delimiterCount != _projectCodeSections - 1)
            {
                return false;
            }

            return true;
        }

        public static RedstoneSidekickProject Decode(string projectString)
        {
            RedstoneSidekickProject project = null;

            projectString = projectString.Trim();
            string[] projectSections = projectString.Split('|');
            
            try
            {
                var projectName = DecodeProjectName(projectSections[0]);
                var projectCraftingTree = DecodeCraftingTreeItems(projectSections[1]);
                var projectShoppingList = DecodeShoppingList(projectSections[2]);

                project = new RedstoneSidekickProject
                {
                    ProjectName = projectName,
                    CraftingTree = projectCraftingTree
                };
            }
            catch
            {

            }


            return project;
        }

        private static string DecodeProjectName(string nameSection)
        {
            return nameSection;
        }

        private static ProjectCraftingTree DecodeCraftingTreeItems(string craftingSection)
        {
            var craftingTree = new ProjectCraftingTree();

            if (int.TryParse(craftingSection.Substring(0, 1), out int treeType))
            {
                var craftingTreeBytes = GetCraftingTreeBytes(craftingSection);

                if(craftingTreeBytes == null)
                {
                    return craftingTree;
                }    

                switch ((CraftingTreeOptions)treeType)
                {
                    case CraftingTreeOptions.Full:
                        craftingTree = DecodeFullCraftingTree(craftingTreeBytes);
                        break;

                    case CraftingTreeOptions.Light:
                        craftingTree = DecodeLightCraftingTree(craftingTreeBytes);
                        break;
                }
            }
            else
            {
                //TODO: Log string failue
            }

            return craftingTree;
        }


        //TODO: Update this to the bulletproof interface serialization here: https://skrift.io/issues/bulletproof-interface-deserialization-in-jsonnet/. 
        private static ProjectCraftingTree DecodeFullCraftingTree(byte[] craftingTreeBytes)
        {
            ProjectCraftingTree craftingTree = new ProjectCraftingTree();

            using(MemoryStream m = new MemoryStream(craftingTreeBytes))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    var treeJson = reader.ReadString();
                    var settings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    craftingTree.Items = JsonConvert.DeserializeObject<ObservableCollection<ICraftingTreeItem>>(treeJson, settings);
                    //craftingTree = JsonConvert.DeserializeObject<ProjectCraftingTree>(treeJson);
                }
            }

            return craftingTree;
        }
        private static ProjectCraftingTree DecodeLightCraftingTree(byte[] craftingTreeBytes)
        {
            var craftingTree = new ProjectCraftingTree();

            using (MemoryStream m = new MemoryStream(craftingTreeBytes))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    var itemDictionary = new Dictionary<int, int>();

                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var itemId = reader.ReadInt32();
                        var itemCount = reader.ReadInt32();
                        itemDictionary.Add(itemId, itemCount);
                    }

                    craftingTree = new ProjectCraftingTree(itemDictionary);
                }
            }

            return craftingTree;
        }


        private static byte[] GetCraftingTreeBytes(string craftingSection)
        {
            var craftingTreeString = craftingSection.Substring(1);
            byte[] craftingTreeBytes = null;

            try
            {
                craftingTreeBytes = Convert.FromBase64String(craftingTreeString);
            }
            catch
            {
                //TODO: Log incorrect crafting tree error.
            }

            return craftingTreeBytes;
        }

        private static object DecodeShoppingList(string shoppingSection)
        {
            return null;
        }
    }
}
