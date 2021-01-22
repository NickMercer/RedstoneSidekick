using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftSchematics
{
    public class Palette
    {
        public string MinecraftId { get; set; }
        public int SchemBlockId { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
