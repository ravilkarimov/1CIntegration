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
                               "inserted_on date, " +
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
                                "amount INT, " +
                                "inserted_on date, " +
                                "changed_on date ) ";

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

            SQLiteProvider.DoSql("create index ix_offers_price On  offers(price)");
            SQLiteProvider.DoSql("create index ix_offers_amount On  offers(amount)"); 
            SQLiteProvider.DoSql("create index ix_offers_size On  offers(size)");
            SQLiteProvider.DoSql("create index ix_goods_group_id On  goods(group_id)");
            SQLiteProvider.DoSql("create index ix_goods_brand_id On  goods(brand_id)");
            SQLiteProvider.DoSql("create index ix_goods_good On  goods(good)");

            const string sqlTriggerGoodsInsert = "create trigger trigger_goods_insert " +
                                                    "on goods " +
                                                    "for insert " +
                                                    "AS " +
                                                    "if @@ROWCOUNT > 0 " +
                                                    "begin " +
                                                    "   update goods " +
                                                    "   set inserted_on = GETDATE() " +
                                                    "   where good_id in (select good_id from INSERTED) " +
                                                    "end";

            const string sqlTriggerGoodsUpdate = "create trigger trigger_goods_update " +
                                                    "on goods " +
                                                    "for update " +
                                                    "AS " +
                                                    "if @@ROWCOUNT > 0 " +
                                                    "begin " +
                                                    "   update goods " +
                                                    "   set changed_on = GETDATE() " +
                                                    "   where good_id in (select good_id from INSERTED) " +
                                                    "end";

            const string sqlTriggerOffersInsert = "create trigger trigger_offers_insert " +
                                                       "on offers " +
                                                       "for insert " +
                                                       "AS " +
                                                       "if @@ROWCOUNT > 0 " +
                                                       "begin " +
                                                       "    update offers " +
                                                       "    set inserted_on = GETDATE() " +
                                                       "    where offer_id in (select offer_id from INSERTED) " +
                                                       "end";

            const string sqlTriggerOffersInserted = "create trigger trigger_offers_update " +
                                                       "on offers " +
                                                       "for update " +
                                                       "AS " +
                                                       "if @@ROWCOUNT > 0 " +
                                                       "begin " +
                                                       "    update offers " +
                                                       "    set changed_on = GETDATE() " +
                                                       "    where offer_id in (select offer_id from INSERTED) " +
                                                       "end";

            SQLiteProvider.DoSql(sqlTriggerGoodsInsert);
            SQLiteProvider.DoSql(sqlTriggerGoodsUpdate);
            SQLiteProvider.DoSql(sqlTriggerOffersInsert);
            SQLiteProvider.DoSql(sqlTriggerOffersInserted);
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
