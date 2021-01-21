using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftSchematics;
using RedstoneSidekick.Domain.MinecraftStructures;
using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Logic.StructureFiles
{
    public class SchematicProcessor : IStructureProcessor
    {
        public RedstoneSidekickProject CreateProjectFromFile(string filePath, string fileName)
        {
            RedstoneSidekickProject project = null;

            if (File.Exists(filePath))
            {
                project = new RedstoneSidekickProject();
                Schematic schematic = NBTConverter.ConvertToSchematic(filePath);

                string projectName = fileName;
                int extPos = projectName.LastIndexOf(".");

                project.ProjectName = projectName.Substring(0, extPos);
                var itemDictionary = CreateBlockList(schematic);
                project.CraftingTree = new ProjectCraftingTree(itemDictionary);
            }

            return project;
        }

        public RedstoneSidekickProject AddStructureToProject(RedstoneSidekickProject project, string filePath, string fileName)
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


        private Dictionary<int, int> CreateBlockList(Schematic schematic)
        {
            Dictionary<int, int> itemDictionary = new Dictionary<int, int>();

            if(schematic != null)
            {
                itemDictionary = AddPaletteBlocks(schematic);
                itemDictionary = SetBlockCounts(schematic, itemDictionary);
                itemDictionary = RemoveNonBlocks(itemDictionary);
            }
            else
            {
                throw new NullReferenceException("Schematic was Null. Make sure you're passing in a schematic object.");
            }

            return itemDictionary;
        }

        private Dictionary<int, int> AddPaletteBlocks(Schematic schematic)
        {
            var itemDictionary = new Dictionary<int, int>();

            foreach (KeyValuePair<int, string> blockType in schematic.Palette)
            {
                var blockId = blockType.Value.GetItemId();

                if (!itemDictionary.ContainsKey(blockId))
                {
                    itemDictionary.Add(blockId, 0);
                }
            }

            return itemDictionary;
        }

        //For now we're assuming (probably ignorantly) that schematics don't have the same block count issues as structures.
        //If this gives weird counts, then we need to refactor the nbt reading to take the things in those straight brackets as properties
        //and turn them into structure style blocks. Then we need to use the structure version of determine block count.
        private Dictionary<int, int> SetBlockCounts(Schematic schematic, Dictionary<int, int> itemDictionary)
        {
            foreach (int block in schematic.BlockData)
            {
                var blockMinecraftId = schematic.Palette.FirstOrDefault(x => x.Key == block).Value;
                if (blockMinecraftId == null) continue;

                var blockId = blockMinecraftId.GetItemId();
                var itemCount = itemDictionary[blockId];

                itemCount++;

                itemDictionary[blockId] = itemCount;
            }

            return itemDictionary;
        }

        private Dictionary<int, int> RemoveNonBlocks(Dictionary<int, int> itemDictionary)
        {
            var errorBlock = 0;
            var airBlock = 1;

            itemDictionary.Remove(errorBlock);
            itemDictionary.Remove(airBlock);

            return itemDictionary;
        }
    }
}
