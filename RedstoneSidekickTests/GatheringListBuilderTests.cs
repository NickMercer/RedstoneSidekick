using NUnit.Framework;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using RedstoneSidekick.Logic.GatheringList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedstoneSidekickTests
{
    public class GatheringListBuilderTests
    {
        private List<ICraftingTreeItem> _testCraftingTree;
        private List<IGatheringListItem> _testGatheringList;

        [SetUp]
        public void InitializeTests()
        {
            var testItems = new Dictionary<int, int>
            {
                { 10, 5 },
                { 2, 1 },
                { 157, 7 },
                { 183, 4 },
                { 13, 5 }
            };
            _testCraftingTree = new ProjectCraftingTree(testItems).Items.ToList();

            _testGatheringList = new List<IGatheringListItem>
            {
                new GatheringListItem(new MinecraftItem { Id = 10,
                                                        Name = "Dirt",
                                                        ImagePath = "/Images/Blocks/DirtBlock.png",
                                                        MinecraftId = "minecraft:dirt",
                                                        Category = ItemCategory.BuildingBlocks },
                                        5, 0, false),
                new GatheringListItem(new MinecraftItem { Id = 13,
                                                        Name = "Cobblestone",
                                                        ImagePath = "/Images/Blocks/CobblestoneBlock.png",
                                                        MinecraftId = "minecraft:cobblestone",
                                                        Category = ItemCategory.BuildingBlocks },
                                        6, 0, false),
                new GatheringListItem(new MinecraftItem { Id = 513,
                                                        Name = "Diamond",
                                                        ImagePath = "/Images/Miscellaneous/Diamond.png",
                                                        MinecraftId = "minecraft:diamond",
                                                        Category = ItemCategory.Miscellaneous },
                                        63, 0, false),
                new GatheringListItem(new MinecraftItem { Id = 33,
                                                        Name = "Oak Log",
                                                        ImagePath = "/Images/Blocks/OakLog.png",
                                                        MinecraftId = "minecraft:oak_log",
                                                        Category = ItemCategory.BuildingBlocks },
                                        4, 0, false),
                new GatheringListItem(new MinecraftItem { Id = 35,
                                                        Name = "Birch Log",
                                                        ImagePath = "/Images/Blocks/BirchLog.png",
                                                        MinecraftId = "minecraft:birch_log",
                                                        Category = ItemCategory.BuildingBlocks },
                                        14, 0, false)
            };
           
        }

        [Test]
        public void GetItemIds_CraftingTree_ReturnsValidItemIds()
        {
            var gatheringList = GatheringListBuilder.GenerateGatheringList(_testCraftingTree);

        }
    }
}
