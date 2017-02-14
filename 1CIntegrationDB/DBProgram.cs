﻿using System;
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
            string sql_groups = "CREATE TABLE IF NOT EXISTS groups (" +
                        "group_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "group_key TEXT, " +
                        "group_name TEXT, " +
                        "is_actual INTEGER) ";

            string sql_goods = "CREATE TABLE IF NOT EXISTS goods (" +
                        "good_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "good_key TEXT, " +
                        "good TEXT, " +
                        "group_id INTEGER, " +
                        "feature_id INTEGER, " +
                        "is_actual INTEGER) ";

            string sql_offers = "CREATE TABLE IF NOT EXISTS offers (" +
                        "offer_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "offer_key TEXT, " +
                        "good_id INTEGER, " +
                        "feature TEXT, " +
                        "price INTEGER, " +
                        "currency TEXT, " +
                        "amount INTEGER) ";

            string sql_d_features = "CREATE TABLE IF NOT EXISTS d_features (" +
                        "feature_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "good_id INTEGER " +
                        "feature TEXT " +
                        "value INTEGER) ";

            doSQL(sql_groups);
            doSQL(sql_goods);
            doSQL(sql_offers);
            doSQL(sql_d_features);
        }

        void doSQL(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void execSQL(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine(reader);
            Console.ReadLine();
        }
    }
}
