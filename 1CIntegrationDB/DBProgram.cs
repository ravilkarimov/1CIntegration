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
            const string sql_brands = "IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='d_brands' AND xtype='U') " +
                                      "CREATE TABLE d_brands (" +
                                      "brand_id INT IDENTITY(1,1) NOT NULL, " +
                                      "brand nchar(20), " +
                                      "is_actual INT) ";

            const string sql_groups = "IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='groups' AND xtype='U') " +
                                      "CREATE TABLE groups (" +
                                      "group_id INT IDENTITY(1,1) NOT NULL, " +
                                      "group_key nvarchar(50), " +
                                      "group_name nvarchar(50), " +
                                      "is_actual INT) ";

            const string sql_goods = "IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='goods' AND xtype='U') " +
                                     "CREATE TABLE goods (" +
                                     "good_id INT IDENTITY(1,1) NOT NULL, " +
                                     "good_key nvarchar(50), " +
                                     "good nvarchar(100), " +
                                     "group_id INT, " +
                                     "brand_id INT, " +
                                     "inserted_on DATE, " +
                                     "changed_on DATE, " +
                                     "img_path nvarchar(150), " +
                                     "is_actual INT) ";

            const string sql_offers = "IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='offers' AND xtype='U') " +
                                      "CREATE TABLE offers (" +
                                      "offer_id INT IDENTITY(1,1) NOT NULL, " +
                                      "offer_key nvarchar(50), " +
                                      "good_key nvarchar(50), " +
                                      "feature nvarchar(150), " +
                                      "size nvarchar(20), " +
                                      "price INT, " +
                                      "currency nchar(10), " +
                                      "amount INT, " +
                                      "inserted_on DATE, " +
                                      "changed_on DATE) ";

            const string sql_d_features = "IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='features' AND xtype='U') " +
                                          "CREATE TABLE features (" +
                                          "feature_id INT IDENTITY(1,1) NOT NULL, " +
                                          "good_id INT, " +
                                          "feature nvarchar(255), " +
                                          "value INT, " +
                                          "feature_key nvarchar(255) ) ";

            SQLiteProvider.DoSql(sql_brands);
            SQLiteProvider.DoSql(sql_groups);
            SQLiteProvider.DoSql(sql_goods);
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_good_id') CREATE UNIQUE INDEX ix_goods_good_id ON goods (good_id)");
            SQLiteProvider.DoSql(sql_offers);
            SQLiteProvider.DoSql(sql_d_features);

            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_offers_price') create index ix_offers_price On  offers(price)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_offers_amount') create index ix_offers_amount On  offers(amount)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_offers_size') create index ix_offers_size On  offers(size)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_offers_inserted_on') create index ix_offers_inserted_on on offers(inserted_on)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_offers_changed_on') create index ix_offers_changed_on on offers(changed_on)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_group_id') create index ix_goods_group_id On  goods(group_id)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_brand_id') create index ix_goods_brand_id On  goods(brand_id)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_good') create index ix_goods_good On  goods(good)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_inserted_on') create index ix_goods_inserted_on on goods(inserted_on)");
            SQLiteProvider.DoSql("IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='ix_goods_changed_on') create index ix_goods_changed_on on goods(changed_on)");

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
