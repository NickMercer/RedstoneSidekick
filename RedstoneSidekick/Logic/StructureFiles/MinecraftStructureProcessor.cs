using Microsoft.Win32;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftStructures;
using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;


namespace RedstoneSidekick.Logic.StructureFiles
{

    public static class MinecraftStructureProcessor
    {
        private static MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();
        private static Dictionary<string, string> _conversionDictionary = new ConversionDictionaryRepository().GetConversionDictionary();
        private static Dictionary<string, int> _minecraftIdToIdDictionary = new MinecraftItemRepository().GetMinecraftIdToIdDictionary();

        public static RedstoneSidekickProject CreateProjectFromStructureFile(string filePath, string fileName)
        {
            RedstoneSidekickProject project = null;

            if (File.Exists(filePath))
            {
                project = new RedstoneSidekickProject();
                Structure structure = NBTConverter.ConvertToStructure(filePath);

                string projectName = fileName;
                int extPos = projectName.LastIndexOf(".");

                project.ProjectName = projectName.Substring(0, extPos);
                var itemDictionary = CreateBlockList(structure);
                project.CraftingTree = new ProjectCraftingTree(itemDictionary);
            }
            else
            {
                MessageBox.Show("File does not exist. Please try again", "File Does Not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return project;
        }

        public static RedstoneSidekickProject AddStructureToProject(RedstoneSidekickProject project, string filePath, string fileName)
        {
            var addedStructureTree = CreateProjectFromStructureFile(filePath, fileName).CraftingTree;

            foreach (var item in addedStructureTree.Items)
            {
                var itemInProject = project.CraftingTree.Items.FirstOrDefault(x => x.Item.Id == item.Item.Id);

                if (itemInProject != null)
                {
                    itemInProject.RequiredAmount += item.RequiredAmount;
                }
                else
                {
                    project.CraftingTree.Items.Add(item);
                }
            }

            return project;
            
        }

        private static Dictionary<int, int> CreateBlockList(Structure structure)
        {
            Dictionary<int, int> itemDictionary = new Dictionary<int, int>();

            if (structure != null)
            {
                itemDictionary = AddPaletteBlocks(structure);
                itemDictionary = SetBlockCounts(structure, itemDictionary);
                itemDictionary = RemoveNonBlocks(itemDictionary);
            }
            else
            {
                throw new NullReferenceException("Structure was Null. Make sure you're passing in a structure object.");
            }

            return itemDictionary;
        }

        private static Dictionary<int, int> AddPaletteBlocks(Structure structure)
        {
            var itemDictionary = new Dictionary<int, int>();

            foreach(Palette blockType in structure.Palette)
            {
                var blockId = blockType.Name.GetItemId();

                if (!itemDictionary.ContainsKey(blockId))
                {
                    itemDictionary.Add(blockId, 0);
                }
            }

            return itemDictionary;
        }

        private static Dictionary<int, int> SetBlockCounts(Structure structure, Dictionary<int, int> itemDictionary)
        {
            foreach(Block block in structure.Blocks)
            {
                itemDictionary = DetermineBlockCount(structure, block, itemDictionary);
            }

            return itemDictionary;
        }

        private static Dictionary<int, int> DetermineBlockCount(Structure structure, Block block, Dictionary<int, int> itemDictionary)
        {
            Palette palette = structure.Palette[block.State];
            var itemId = palette.Name.GetItemId();
            int itemCount = itemDictionary[itemId];

            if (palette.Properties != null)
            {
                string value;
                if (palette.Properties.TryGetValue("half", out value))          //Doors, 2 High Plants
                {
                    if (value != "upper")
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("part", out value))     //Beds
                {
                    if (value == "foot")
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("layers", out value))     //Snow Layers
                {
                    itemCount += int.Parse(value);
                }
                else if (palette.Properties.TryGetValue("type", out value))     //Slabs
                {
                    if (value == "bottom" || value == "top")
                    {
                        itemCount++;
                    }
                    else if (value == "double")
                    {
                        itemCount += 2;
                    }
                    else
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("pickles", out value))     //Sea Pickles
                {
                    itemCount += int.Parse(value);
                }
                else                                                            //Everything Else
                {
                    itemCount++;
                }
            }
            else
            {
                itemCount++;
            }

            itemDictionary[itemId] = itemCount;

            return itemDictionary;
        }

        private static Dictionary<int, int> RemoveNonBlocks(Dictionary<int, int> itemDictionary)
        {
            var errorBlock = 0;
            var airBlock = 1;

            itemDictionary.Remove(errorBlock);
            itemDictionary.Remove(airBlock);

            return itemDictionary;
        }

        private static int GetItemId(this string minecraftId)
        {
            var id = 0;

            if (_conversionDictionary.ContainsKey(minecraftId))
            {
                minecraftId = _conversionDictionary[minecraftId];
            }

            id = _minecraftIdToIdDictionary[minecraftId];

            return id;
        }

    }
}
