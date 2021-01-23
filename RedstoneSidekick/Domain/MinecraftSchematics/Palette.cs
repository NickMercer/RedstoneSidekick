using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftSchematics
{
    public class Palette : IPalette
    {
        public string MinecraftId { get; set; }
        public int SchemBlockId { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public override string ToString()
        {
            var properties = Properties.Select(kvp => kvp.Key + ": " + kvp.Value);
            var propertyString = string.Join('|', properties);
            return $"{SchemBlockId} - {MinecraftId} - {propertyString}";
        }
    }
}
