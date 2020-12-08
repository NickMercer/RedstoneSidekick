using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftStructures
{
    public class Block
    {
        public IDictionary<string, object> Nbt { get; set; }
        public IList<int> Pos { get; set; }
        public int State { get; set; }
    }
}
