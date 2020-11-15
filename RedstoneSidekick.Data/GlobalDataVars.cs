using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Data
{
    public static class GlobalDataVars
    {
        public static string AppDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public const string SQLiteConnectionString = @"Data Source =.\RedstoneSidekickDB.db;Version=3;";
    }
}
