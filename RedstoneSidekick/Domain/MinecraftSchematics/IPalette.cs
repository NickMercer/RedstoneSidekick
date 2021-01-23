using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftSchematics
{
    public interface IPalette
    {
        Dictionary<string, string> Properties { get; set; }
    }
}
