using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftItems
{
    public interface IMinecraftItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string MinecraftId { get; set; }

        public ItemCategory Category { get; set; }
    }
}
