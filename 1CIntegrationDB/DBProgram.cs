using System;
using System.Data.SQLite;
using System.IO;

namespace _1CIntegrationDB
{
    public class DBProgram
    {
        SQLiteConnection m_dbConnection;

        static void Main(string[] args)
        {
            DBProgram p = new DBProgram();
        }

        public DBProgram()
        {
            createNewDB();
            connectToDB();
            createTables();
        }

        void createNewDB()
        {
            if (!File.Exists("db.sqlite"))
            {
                SQLiteConnection.CreateFile("db.sqlite");
            }
            return;
        }

        void connectToDB()
        {
            m_dbConnection = new SQLiteConnection("Data Source = db.sqlite; Version = 3;");
            m_dbConnection.Open();
        }
        void createTables()
        {
            string sql = "CREATE TABLE IF NOT EXISTS  highscores (name VARCHAR(20), score INT)";
            DoSql(sql);

            string sql_groups = "CREATE TABLE groups (" +
                        "group_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "group_key TEXT, " +
                        "group TEXT, " +
                        "is_actual INTEGER) ";
            
            string sql_goods = "CREATE TABLE goods (" +
                        "good_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "good_key TEXT, " +
                        "good TEXT, " +
                        "group_id INTEGER, " +
                        "feature_id INTEGER, " +
                        "is_actual INTEGER) ";

            string sql_offers = "CREATE TABLE offers (" +
                        "offer_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "offer_key TEXT, " +
                        "good_id INTEGER, " +
                        "feature TEXT, " +
                        "price INTEGER, " +
                        "currency TEXT, " +
                        "amount INTEGER) ";

            string sql_d_features = "CREATE TABLE d_features (" +
                        "feature_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "good_id INTEGER " +
                        "feature TEXT) ";

            DoSql(sql_groups);
            DoSql(sql_goods);
            DoSql(sql_offers);
            DoSql(sql_d_features);
        }

        void DoSql(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void ExecSql(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine(reader);
            Console.ReadLine();
        }
    }
}
