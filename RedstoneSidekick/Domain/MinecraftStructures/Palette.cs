using RedstoneSidekick.Domain.MinecraftSchematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftStructures
{
    public class Palette : IPalette
    {
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
