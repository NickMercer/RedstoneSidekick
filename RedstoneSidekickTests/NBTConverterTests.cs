using Natick.Utilities.ExtensionMethods;
using NUnit.Framework;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.MinecraftStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekickTests
{
    public class NBTConverterTests
    {
        [Test]
        public void ConvertToSchematic_GivenSchemFile_CreatesValidSchematic()
        {
            var schemFilePath = "carousel.schem";

            var schematic = NBTConverter.ConvertToSchematic(schemFilePath);

            Assert.IsNotNull(schematic);
        }

    }
}
