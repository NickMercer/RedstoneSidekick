using Microsoft.Win32;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
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

    public class MinecraftStructureProcessor : StructureProcessor
    {
        
        public override RedstoneSidekickProject CreateProjectFromFile(string filePath, string fileName)
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

            return project;
        }

        public override RedstoneSidekickProject AddStructureToProject(RedstoneSidekickProject project, string filePath, string fileName)
        {
            var addedStructureTree = CreateProjectFromFile(filePath, fileName).CraftingTree;

            if (addedStructureTree == null)
            {
                return project;
            }

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

            foreach (Palette blockType in structure.Palette)
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
            foreach (Block block in structure.Blocks)
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
            itemDictionary = ParsePropertyBasedCounts(itemDictionary, palette, itemId);

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
    }

    public static class ProcessorExtension
    {
        private static readonly Dictionary<string, string> _conversionDictionary = new ConversionDictionaryRepository().GetConversionDictionary();
        private static readonly Dictionary<string, int> _minecraftIdToIdDictionary = new MinecraftItemRepository().GetMinecraftIdToIdDictionary();

        public static int GetItemId(this string minecraftId)
        {
            var id = 0;

            if (_conversionDictionary.ContainsKey(minecraftId))
            {
                minecraftId = _conversionDictionary[minecraftId];
            }

            if (_minecraftIdToIdDictionary.ContainsKey(minecraftId))
            {
                id = _minecraftIdToIdDictionary[minecraftId];
            }

            return id;
        }
    }
}
