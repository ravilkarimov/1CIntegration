using System.Data.SQLite;

namespace _1CIntegrationDB
{
    public class DBProgram
    {
        public DBProgram()
        {
            createTables();
        }

        void createTables()
        {
            string sql_groups = "CREATE TABLE groups (" +
                                "group_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                "group_key TEXT, " +
                                "group_name TEXT, " +
                                "is_actual INTEGER) ";

            string sql_goods = "CREATE TABLE IF NOT EXISTS goods (" +
                               "good_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                               "good_key TEXT, " +
                               "good TEXT, " +
                               "group_id INTEGER, " +
                               "is_actual INTEGER, " +
                               "img_path TEXT) ";

            string sql_offers = "CREATE TABLE IF NOT EXISTS offers (" +
                                "offer_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                "offer_key TEXT, " +
                                "good_key TEXT, " +
                                "feature TEXT, " +
                                "price INTEGER, " +
                                "currency TEXT, " +
                                "amount INTEGER) ";

            string sql_d_features = "CREATE TABLE IF NOT EXISTS features (" +
                                    "feature_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "good_id INTEGER " +
                                    "feature TEXT " +
                                    "value INTEGER " +
                                    "feature_key TEXT ) ";

            SQLiteProvider.DoSql(sql_groups);
            SQLiteProvider.DoSql(sql_goods);
            SQLiteProvider.DoSql(sql_offers);
            SQLiteProvider.DoSql(sql_d_features);
        }
    }
}
