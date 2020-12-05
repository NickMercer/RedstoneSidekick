using RedstoneSidekick.Domain.MinecraftItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain
{
    public class MinecraftItem : IMinecraftItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string MinecraftId { get; set; }

        public ItemCategory Category { get; set; }
    }
}
