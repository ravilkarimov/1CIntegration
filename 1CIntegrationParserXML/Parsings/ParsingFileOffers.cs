using System;
using System.Data;
using System.Linq;
using System.Xml;
using _1CIntegrationDB;

namespace _1CIntegrationParserXML
{
    public class ParserXmlNameOffers : IBaseParserXml
    {
        private DataSet dataSource = new DataSet();

        public void ParsingFileXml(string fullPath)
        {
            try
            {
                DataTable offersDataTable = new DataTable { TableName = "OffersTable" };
                offersDataTable.Columns.Add("offer_key", typeof(string));
                offersDataTable.Columns.Add("good_key", typeof(string));
                offersDataTable.Columns.Add("feature", typeof(string));
                offersDataTable.Columns.Add("price", typeof(string));
                offersDataTable.Columns.Add("currency", typeof(string));
                offersDataTable.Columns.Add("amount", typeof(string));

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fullPath);
                
                #region Предложения
                var elements = xmlDocument.GetElementsByTagName("Предложение");
                var listNodes = elements.Cast<XmlElement>().Cast<XmlNode>().ToList();

                foreach (var item in listNodes.Select(x => x.ChildNodes))
                {
                    DataRow newElementRow = offersDataTable.NewRow();
                    var goodKey = "";
                    var offerKey = "";

                    //Ид
                    var firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Ид");
                    if (firstOrDefault != null)
                    {
                        var offer_id =  firstOrDefault.LastChild.InnerText;
                        offerKey = offer_id.Split('#')[0];
                        goodKey = offer_id.Split('#')[1];
                        newElementRow["offer_key"] = offerKey;
                        newElementRow["good_key"] = goodKey;
                    }
                    //===============================================

                    //Предложение
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Наименование");
                    if (firstOrDefault != null)
                    {
                        var feature = firstOrDefault.LastChild.InnerText;
                        newElementRow["feature"] = feature;
                    }
                    //===============================================

                    //Цена и Валюта
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Цены");
                    if (firstOrDefault != null)
                    {
                        var custom = firstOrDefault.LastChild.ChildNodes
                            .Cast<XmlElement>().Cast<XmlNode>();

                        var priceDefault = custom.FirstOrDefault(x => x.Name == "ЦенаЗаЕдиницу");
                        if (priceDefault != null)
                        {
                            var price = priceDefault
                                .LastChild.InnerText;
                            newElementRow["price"] = price;
                        }

                        var currencyDefault = custom.FirstOrDefault(x => x.Name == "Валюта");
                        if (currencyDefault != null)
                        {
                            var currency = currencyDefault
                                .LastChild.InnerText;
                            newElementRow["currency"] = currency;
                        }
                    }
                    //===============================================
                    
                    //Количество на складе
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Склад");
                    if (firstOrDefault != null)
                    {
                        var amount = firstOrDefault.Attributes["КоличествоНаСкладе"].InnerText;
                        newElementRow["amount"] = amount;
                    }
                    //===============================================

                    offersDataTable.Rows.Add(newElementRow);
                }
                #endregion

                dataSource.Tables.Add(offersDataTable);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }

        public void LoadingInBD()
        {
            try
            {
                string sql = "";

                //OffersTable 
                foreach (DataRow rowGroup in dataSource.Tables["OffersTable"].Rows)
                {
                    int cnt = Convert.ToInt32(SQLiteProvider.OpenSql("select count(*) cnt from offers where offer_key = '" + rowGroup["offer_key"] + "'").Rows[0]["cnt"]);
                    if (cnt == 0)
                    {
                        sql = "insert into offers (good_key, offer_key, feature, price, currency, amount) values " +
                              "('" + rowGroup["good_key"] + "','" + rowGroup["offer_key"] + "','" + rowGroup["feature"] + "'" +
                              "," + rowGroup["price"] + ",'" + rowGroup["currency"] + "'," + rowGroup["amount"] + ")";
                        SQLiteProvider.ExecSql(sql);
                    }
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }
    }
}
