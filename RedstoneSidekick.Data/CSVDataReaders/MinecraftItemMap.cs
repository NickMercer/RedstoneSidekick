using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration;
using RedstoneSidekick.Domain;

namespace RedstoneSidekick.Data.CSVDataReaders
{
    public class MinecraftItemMap : ClassMap<MinecraftItem>
    {
        public MinecraftItemMap()
        {
            Map(m => m.Id).Name(new string[] { "Id", "Type" });
            Map(m => m.Name).Name("Name");
            Map(m => m.ImagePath).Name("ImagePath");
            Map(m => m.MinecraftId).Name("MinecraftId");
            Map(m => m.Category).Name("Category");
        }
    }
}
