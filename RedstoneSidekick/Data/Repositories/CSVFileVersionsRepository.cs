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
    public class CSVFileVersionsRepository : IRepository
    {
        public CSVFileVersion GetCSVFileVersion(string fileName)
        {

            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))

            {
                var commandString = "SELECT * FROM FileVersions WHERE FileName = @FileName";
                var item = conn.Query<CSVFileVersion>(commandString, new { FileName = fileName });
                return item.FirstOrDefault();
            }
        }

        public int UpdateCSVFileVersion(CSVFileVersion versionInfo)
        {
            var updatedRows = 0;
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = $"UPDATE FileVersions SET Version = @Version WHERE FileName = @FileName";
                updatedRows += conn.Execute(commandString, new { Version = versionInfo.Version, FileName = versionInfo.FileName });
            }

            return updatedRows;
        }
    }
}
