using Dapper;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.Repositories
{
    public class MinecraftItemRepository : IRepository
    {
        public IEnumerable<MinecraftItem> GetMinecraftItems()
        {
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT * FROM MinecraftItems;";
                return conn.Query<MinecraftItem>(commandString, new DynamicParameters());
            }
        }

        public MinecraftItem GetMinecraftItemById(int id)
        {
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT * FROM MinecraftItems WHERE Id = @Id;";
                return conn.Query<MinecraftItem>(commandString, new { Id = id }).FirstOrDefault();
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
