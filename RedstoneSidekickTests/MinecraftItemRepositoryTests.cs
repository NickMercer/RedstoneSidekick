using NUnit.Framework;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using System.Collections.Generic;
using System.Linq;

namespace RedstoneSidekickTests
{
    public class MinecraftItemRepositoryTests
    {
        private MinecraftItemRepository _minecraftItemRepository;

        [SetUp]
        public void Initialize()
        {
            _minecraftItemRepository = new MinecraftItemRepository();
        }


        [Test]
        public void GetMinecraftItems_NoParams_ReturnsMinecraftItems()
        {
            var items = _minecraftItemRepository.GetMinecraftItems();

            Assert.IsTrue(items.Any());
        }

    }
}