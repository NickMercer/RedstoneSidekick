using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.StructureFiles
{
    public static class StructureProcessorFactory
    {
        public static StructureProcessor Build(string fileName)
        {
            var extension = fileName.Substring(fileName.LastIndexOf("."));

            StructureProcessor processor;

            switch (extension)
            {
                case ".nbt":
                    processor = new MinecraftStructureProcessor();
                    break;

                case ".schematic":
                case ".schem":
                    processor = new SchematicProcessor();
                    break;

                default:
                    throw new ArgumentException("The given file was of an invalid file type. Please provide a schematic or structure file.");
            }

            return processor;
        }
    }
}
