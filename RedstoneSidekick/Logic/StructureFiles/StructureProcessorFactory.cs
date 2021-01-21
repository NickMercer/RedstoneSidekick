using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.StructureFiles
{
    public static class StructureProcessorFactory
    {
        public static IStructureProcessor Build(string fileName)
        {
            var extension = fileName.Substring(fileName.LastIndexOf("."));

            IStructureProcessor processor;

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
                    break;
            }

            return processor;
        }
    }
}
