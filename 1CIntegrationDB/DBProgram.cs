using System.Data;

namespace _1CIntegrationDB
{
    public class DBProgram
    {
        public DBProgram()
        {
            createTables();
            fillTables();
        }

        void createTables()
        {
            const string sql_brands = "CREATE TABLE d_brands (" +
                                      "brand_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                      "brand TEXT, " +
                                      "is_actual INTEGER) ";

            const string sql_groups = "CREATE TABLE groups (" +
                                "group_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                "group_key TEXT, " +
                                "group_name TEXT, " +
                                "is_actual INTEGER) ";

            const string sql_goods = "CREATE TABLE IF NOT EXISTS goods (" +
                               "good_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                               "good_key TEXT, " +
                               "good TEXT, " +
                               "group_id INTEGER, " +
                               "brand_id INTEGER, " +
                               "created_on TEXT, " +
                               "changed_on TEXT, " +
                               "img_path TEXT, " +
                               "is_actual INTEGER) ";

            const string sql_offers = "CREATE TABLE IF NOT EXISTS offers (" +
                                "offer_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                "offer_key TEXT, " +
                                "good_key TEXT, " +
                                "feature TEXT, " +
                                "size TEXT, " +
                                "price INTEGER, " +
                                "currency TEXT, " +
                                "amount INTEGER) ";

            const string sql_d_features = "CREATE TABLE IF NOT EXISTS features (" +
                                    "feature_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "good_id INTEGER " +
                                    "feature TEXT " +
                                    "value INTEGER " +
                                    "feature_key TEXT ) ";

            SQLiteProvider.DoSql(sql_brands);
            SQLiteProvider.DoSql(sql_groups);
            SQLiteProvider.DoSql(sql_goods);
            SQLiteProvider.DoSql("CREATE UNIQUE INDEX ix_goods_good_id ON goods (good_id)");
            SQLiteProvider.DoSql(sql_offers);
            SQLiteProvider.DoSql(sql_d_features);
        }

        void fillTables()
        {
            
            //Проверить на пустоту справочник брендов
            var dt = SQLiteProvider.OpenSql("SELECT * FROM d_brands");
            //Если справочник брендов пустой, заполняем его
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    var fillBrands = "INSERT INTO d_brands VALUES(1,'Adidas',1);      " +
                                         "INSERT INTO d_brands VALUES(2,'Nike',1);        " +
                                         "INSERT INTO d_brands VALUES(3,'NB',1); " +
                                         "INSERT INTO d_brands VALUES(4,'Saucony',1);     " +
                                         "INSERT INTO d_brands VALUES(5,'Asics',1);       " +
                                         "INSERT INTO d_brands VALUES(6,'Reebok',1);      " +
                                         "INSERT INTO d_brands VALUES(7,'Puma',1);        " +
                                         "INSERT INTO d_brands VALUES(8,'Vans',1);        " +
                                         "INSERT INTO d_brands VALUES(9,'Convers',1);     " +
                                         "INSERT INTO d_brands VALUES(10,'Jordan',1);     " +
                                         "INSERT INTO d_brands VALUES(11,'Другое',1);     ";

                    SQLiteProvider.DoSql(fillBrands);
                }
            }
        }
    }
}
