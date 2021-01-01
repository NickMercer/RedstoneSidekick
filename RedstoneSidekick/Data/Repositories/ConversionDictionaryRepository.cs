using Dapper;
using RedstoneSidekick.Domain.MinecraftItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.Repositories
{
    public class ConversionDictionaryRepository : IRepository
    {
        public Dictionary<string, string> GetConversionDictionary()
        {
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT FoundMinecraftId AS [Key], ReplacedMinecraftId AS [Value] FROM ConversionDictionary;";
                return conn.Query<KeyValuePair<string, string>>(commandString).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        public int InsertDictionaryEntries(IEnumerable<ConversionItem> conversionItems)
        {
            var createdRows = 0;
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                foreach (var item in conversionItems)
                {
                    var commandString = "INSERT INTO ConversionDictionary (FoundMinecraftId, ReplacedMinecraftId) " +
                                        "VALUES (@Found, @Replaced) " +
                                        "ON CONFLICT(FoundMinecraftId) DO UPDATE SET ReplacedMinecraftId = @Replaced";
                    createdRows += conn.Execute(commandString, new { Found = item.FoundMinecraftId, Replaced = item.ReplacedMinecraftId });
                }
            }

            return createdRows;
        }
    }
}
