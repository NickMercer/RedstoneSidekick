using RedstoneSidekick.Domain.MinecraftItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain
{
    public class MinecraftItem : IMinecraftItem
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; } = "Error";

        public string ImagePath { get; set; } = "/Images/Blocks/Error.png";

        public string MinecraftId { get; set; } = "minecraft:error";

        public ItemCategory Category { get; set; } = ItemCategory.Error;

        public override string ToString()
        {
            return $"{Id} - {Name} - {MinecraftId}";
        }

    }
}
