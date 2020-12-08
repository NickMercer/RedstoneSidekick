using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftStructures
{
    public class Structure
    {
        public IList<int> Size { get; set; }
        public IList<object> Entities { get; set; }
        public IList<Block> Blocks { get; set; }
        public IList<Palette> Palette { get; set; }
        public int DataVersion { get; set; }
    }
}
