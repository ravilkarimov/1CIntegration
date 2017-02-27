using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace _1CIntegrationDB
{
    public class SQLiteProvider
    {
        private static readonly string DatabaseName = "h:\\root\\home\\djinaroshop-001\\www\\db\\db.sqlite";
        //C:\\Users\\r.karimov\\Downloads\\db.sqlite
        //C:\\Users\\Дмитрий\\db.sqlite

        private static readonly string ConnectionString = string.Format("Data Source={0}; Version=3; UseUTF16Encoding=True;", DatabaseName);

        static SQLiteProvider()
        {
            try
            {
                if (!File.Exists(DatabaseName))
                {
                    try
                    {
                        SQLiteConnection.CreateFile(DatabaseName);
                    }
                    catch (SQLiteException eLite)
                    {
                        throw eLite;
                    }
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public static void DoSql(string sql)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException)
            {
            }
        }

        public static DataTable OpenSql(string sqlString)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                { 
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlString, c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            var dt = new DataTable();
                            dt.Load(rdr);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
                throw new Exception("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
            }
        }

        public static int ExecSql(string sqlString)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlString, c))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception("Ошибка при ExecSQL ("+sqlString+"): " + e.Message);
            }
        }

        public static int ExecSql(List<string> sqlStringBuilder)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.Open();
                    foreach (var sql in sqlStringBuilder)
                    {
                        using (var cmd = new SQLiteCommand(sql, c))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                new FileLogger("Log.txt").LogMessage(sql);
                            }
                            catch (SQLiteException eCmd)
                            {
                                throw new Exception("Ошибка при ExecSQL(" + sql + "): " + eCmd.Message);
                            }
                        }
                    }
                }

                return 1;
            }
            catch (SQLiteException e)
            {
                throw new Exception("Ошибка при ExecSQL(List): " + e.Message);
            }
        }
    }
}
