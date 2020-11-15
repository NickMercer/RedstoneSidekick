using Dapper;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace RedstoneSidekick.Data.Repositories
{
    public class MinecraftItemRepository : IRepository
    {
        public List<MinecraftItem> GetMinecraftItems()
        {
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var items = conn.Query<MinecraftItem>("SELECT * FROM MinecraftItems;", new DynamicParameters());
                return items.AsList();
            }
        }

        public int InsertMinecraftItems(IEnumerable<MinecraftItem> minecraftItems)
        {
            var createdRows = 0;
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                foreach (var item in minecraftItems)
                {
                    var commandString = "INSERT INTO MinecraftItems (Name, ImagePath, MinecraftId, Category) " +
                                        "VALUES (@Name, @ImagePath, @MinecraftId, @Category) " +
                                        "ON CONFLICT(Name) DO UPDATE SET ImagePath = @ImagePath, MinecraftId = @MinecraftId, Category = @Category;";
                    createdRows += conn.Execute(commandString, new { Name = item.Name, ImagePath = item.ImagePath, MinecraftId = item.MinecraftId, Category = item.Category });
                }
            }

            return createdRows;
        }

        public int InsertMinecraftItem(MinecraftItem minecraftItem)
        {
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "INSERT INTO MinecraftItems (Name, ImagePath, MinecraftId, Category) VALUES (@Name, @ImagePath, @MinecraftId, @Category);";
                return conn.Execute(commandString, minecraftItem);
            }
        }
    }
}
