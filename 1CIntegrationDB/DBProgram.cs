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
                                      "brand nvarchar(255), " +
                                      "is_actual INT) ";

            const string sql_groups = "CREATE TABLE groups (" +
                                "group_id INT IDENTITY(1,1) NOT NULL, " +
                                "group_key nvarchar(255), " +
                                "group_name nvarchar(255), " +
                                "is_actual INT) ";

            const string sql_goods = "CREATE TABLE goods (" +
                               "good_id INT IDENTITY(1,1) NOT NULL, " +
                               "good_key nvarchar(255), " +
                               "good nvarchar(255), " +
                               "group_id INT, " +
                               "brand_id INT, " +
                               "created_on date, " +
                               "changed_on date, " +
                               "img_path nvarchar(500), " +
                               "is_actual INT) ";

            const string sql_offers = "CREATE TABLE offers (" +
                                "offer_id INT IDENTITY(1,1) NOT NULL, " +
                                "offer_key nvarchar(255), " +
                                "good_key nvarchar(255), " +
                                "feature nvarchar(255), " +
                                "size nvarchar(20), " +
                                "price INT, " +
                                "currency nvarchar(255), " +
                                "amount INT) ";

            const string sql_d_features = "CREATE TABLE features (" +
                                    "feature_id INT IDENTITY(1,1) NOT NULL, " +
                                    "good_id INT, " +
                                    "feature nvarchar(255) " +
                                    "value INT " +
                                    "feature_key nvarchar(255) ) ";

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
                    var fillBrands = "INSERT INTO d_brands VALUES(N'Adidas',1);" +
                                         "INSERT INTO d_brands VALUES(N'Nike',2);" +
                                         "INSERT INTO d_brands VALUES(N'NB',3); " +
                                         "INSERT INTO d_brands VALUES(N'Saucony',4);" +
                                         "INSERT INTO d_brands VALUES(N'Asics',5);" +
                                         "INSERT INTO d_brands VALUES(N'Reebok',6);" +
                                         "INSERT INTO d_brands VALUES(N'Puma',7);" +
                                         "INSERT INTO d_brands VALUES(N'Vans',8);" +
                                         "INSERT INTO d_brands VALUES(N'Convers',9);" +
                                         "INSERT INTO d_brands VALUES(N'Jordan',10);" +
                                         "INSERT INTO d_brands VALUES(N'Другое',255);";

                    SQLiteProvider.ExecSql(fillBrands);
                }
            }
        }
    }
}
