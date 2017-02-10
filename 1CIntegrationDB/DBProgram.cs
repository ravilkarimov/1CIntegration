using System;
using System.Data.SQLite;

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
            createTable();
            fillTable();
            printHighscores();
        }

        void createNewDB()
        {
            SQLiteConnection.CreateFile("db.sqlite");
        }

        void connectToDB()
        {
            m_dbConnection = new SQLiteConnection("Data Source = db.sqlite; Version = 3;");
            m_dbConnection.Open();
        }

        void createTable()
        {
            string sql = "create table highscores (name varchar(20), score int)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void fillTable()
        {
            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into highscores (name, score) values ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void printHighscores()
        {
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            Console.ReadLine();
        }
    }
}
