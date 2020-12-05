using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedstoneSidekick.Tests.Data.Tests
{
    [TestClass]
    public class MinecraftItemRepositoryTests
    {
        private MinecraftItemRepository _minecraftItemRepository;

        [TestInitialize]
        public void Initialize()
        {
            _minecraftItemRepository = new MinecraftItemRepository();
        }


        [TestMethod]
        public void GetMinecraftItems_NoParams_ReturnsMinecraftItems()
        {
            var items = _minecraftItemRepository.GetMinecraftItems();

            Assert.IsTrue(items.Any());
        }

        [TestMethod]
        public void InsertMinecraftItems_ListOfItems_CreatesMinecraftItems()
        {
            var minecraftItems = new List<MinecraftItem>();

            var i = 0;
            while (i < 10)
            {
                minecraftItems.Add(new MinecraftItem
                {
                    Id = i,
                    Name = "no",
                    ImagePath = "image path / path",
                    MinecraftId = "minecraft:no",
                    Category = ItemCategory.Brewing
                });
                i++;
            }

            var itemsAdded = _minecraftItemRepository.InsertMinecraftItems(minecraftItems);

            Assert.IsTrue(itemsAdded > 0);
        }

        [TestMethod]
        public void InsertMinecraftItem_Item_CreatesMinecraftItem()
        {
            var item = new MinecraftItem
            {
                Name = "no",
                ImagePath = "imagepath/no",
                MinecraftId = "minecraft:no",
                Category = ItemCategory.BuildingBlocks
            };

            var added = _minecraftItemRepository.InsertMinecraftItem(item);
            Assert.AreEqual(1, added);
        }
    }
}
