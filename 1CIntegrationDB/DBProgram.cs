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
                                      "brand_id INT IDENTITY(1,1) NOT NULL, " +
                                      "brand TEXT, " +
                                      "is_actual INTEGER) ";

            const string sql_groups = "CREATE TABLE groups (" +
                                "group_id INT IDENTITY(1,1) NOT NULL, " +
                                "group_key TEXT, " +
                                "group_name TEXT, " +
                                "is_actual INTEGER) ";

            const string sql_goods = "CREATE TABLE goods (" +
                               "good_id INT IDENTITY(1,1) NOT NULL, " +
                               "good_key TEXT, " +
                               "good TEXT, " +
                               "group_id INTEGER, " +
                               "brand_id INTEGER, " +
                               "created_on TEXT, " +
                               "changed_on TEXT, " +
                               "img_path TEXT, " +
                               "is_actual INTEGER) ";

            const string sql_offers = "CREATE TABLE offers (" +
                                "offer_id INT IDENTITY(1,1) NOT NULL, " +
                                "offer_key TEXT, " +
                                "good_key TEXT, " +
                                "feature TEXT, " +
                                "size TEXT, " +
                                "price INTEGER, " +
                                "currency TEXT, " +
                                "amount INTEGER) ";

            const string sql_d_features = "CREATE TABLE features (" +
                                    "feature_id INT IDENTITY(1,1) NOT NULL, " +
                                    "good_id INTEGER, " +
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
                    var fillBrands = "INSERT INTO d_brands VALUES('Adidas',1);" +
                                         "INSERT INTO d_brands VALUES('Nike',1);" +
                                         "INSERT INTO d_brands VALUES('NB',1); " +
                                         "INSERT INTO d_brands VALUES('Saucony',1);" +
                                         "INSERT INTO d_brands VALUES('Asics',1);" +
                                         "INSERT INTO d_brands VALUES('Reebok',1);" +
                                         "INSERT INTO d_brands VALUES('Puma',1);" +
                                         "INSERT INTO d_brands VALUES('Vans',1);" +
                                         "INSERT INTO d_brands VALUES('Convers',1);" +
                                         "INSERT INTO d_brands VALUES('Jordan',1);" +
                                         "INSERT INTO d_brands VALUES('Другое',1);";

                    SQLiteProvider.ExecSql(fillBrands);
                }
            }
        }
    }
}
