﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Domain.MinecraftSchematics
{
    public class Schematic
    {
        public List<Palette> Palette { get; set; }
        public IList<int> BlockData { get; set; }
    }
}
