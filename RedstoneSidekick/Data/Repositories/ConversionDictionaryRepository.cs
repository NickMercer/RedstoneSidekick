using Dapper;
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
    }
}
