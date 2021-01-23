using NbtLib;
using Newtonsoft.Json;
using RedstoneSidekick.Domain.MinecraftSchematics;
using RedstoneSidekick.Domain.MinecraftStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Palette = RedstoneSidekick.Domain.MinecraftSchematics.Palette;

namespace RedstoneSidekick.Domain
{
    public static class NBTConverter
    {
        public static Structure ConvertToStructure(string filePath)
        {
            var fileAsNBT = ReadFileAsNBT(filePath);

            var json = fileAsNBT.ToJsonString();

            var jsonWithoutInvalidNBT = json.RemoveInvalidNBT();

            return JsonConvert.DeserializeObject<Structure>(jsonWithoutInvalidNBT);
        } 

        public static Schematic ConvertToSchematic(string filePath)
        {
            var fileAsNBT = ReadFileAsNBT(filePath);

            var schematic = ConvertNBTToSchematic(fileAsNBT);

            return schematic;
        }

        private static NbtCompoundTag ReadFileAsNBT(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                return NbtConvert.ParseNbtStream(inputStream);
            }
        }

        private static string RemoveInvalidNBT(this string json)
        {
            int safeguard = 0;
            while (json.Contains("nbt") && safeguard < 10000)
            {
                var startIndex = json.IndexOf("nbt") - 1;
                var endIndex = json.IndexOf("pos", startIndex) - 1;
                json = json.Remove(startIndex, endIndex - startIndex);
                safeguard++;
            }

            return json;
        }

        private static Schematic ConvertNBTToSchematic(NbtCompoundTag nbt)
        {
            var palettes = ParsePalettes(nbt);
            var blockData = ParseBlockData(nbt);

            return new Schematic { Palette = palettes, BlockData = blockData };
        }

        private static List<Palette> ParsePalettes(NbtCompoundTag nbt)
        {
            nbt.TryGetValue("Palette", out var paletteValue);
            var palette = (NbtCompoundTag)paletteValue;

            List<Palette> palettes = new List<Palette>();

            foreach (var item in palette)
            {
                var name = item.Key;
                Dictionary<string, string> properties = new Dictionary<string, string>();
                var propertyPosition = item.Key.IndexOf("[");
                if (propertyPosition != -1)
                {
                    name = item.Key.Substring(0, propertyPosition);
                    var propertyString = item.Key.Substring(propertyPosition + 1);
                    propertyString = propertyString.Remove(propertyString.Length - 1, 1);
                    var propertiesArray = propertyString.Split(',');

                    foreach (var property in propertiesArray)
                    {
                        var pair = property.Split('=');

                        properties.Add(pair[0], pair[1]);
                    }

                }

                var intValue = int.Parse(item.Value.ToString());
                palettes.Add(new Palette { MinecraftId = name, Properties = properties, SchemBlockId = intValue });
            }

            return palettes;
        }

        private static List<int> ParseBlockData(NbtCompoundTag nbt)
        {
            nbt.TryGetValue("BlockData", out var blockDataValue);
            var blockDataNBT = (NbtByteArrayTag)blockDataValue;
            var blockDataBytes = blockDataNBT.Payload;

            var blockData = ParseBlockDataVarInts(blockDataBytes);

            return blockData.ToList();
        }

        //This was adapted from the sponge schematic reader itself (the specification for schematic files. 
        //Translated from Java to C#
        private static List<int> ParseBlockDataVarInts(byte[] bytes)
        {
            var blockList = new List<int>();

            var value = 0;
            var varintLength = 0;
            var i = 0;
            while(i < bytes.Length)
            {
                value = 0;
                varintLength = 0;

                while(true)
                {
                    value |= (bytes[i] & 127) << (varintLength++ * 7);

                    if(varintLength > 5)
                    {
                        //We errored out here somehow.
                        return blockList;
                    }

                    if((bytes[i] & 128) != 128)
                    {
                        i++;
                        break;
                    }

                    i++;
                }
                
                blockList.Add(value);
            }

            return blockList;
        }
    }
}
