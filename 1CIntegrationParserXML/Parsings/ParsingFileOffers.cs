using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
                offersDataTable.Columns.Add("size", typeof(string));
                offersDataTable.Columns.Add("price", typeof(string));
                offersDataTable.Columns.Add("currency", typeof(string));
                offersDataTable.Columns.Add("amount", typeof(string));

                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    Thread.Sleep(5000);
                    xmlDocument.Load(fullPath);
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                    xmlDocument.Load(fullPath);
                }

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
                        if (offer_id.Split('#').Length == 2)
                        {
                            goodKey = offer_id.Split('#')[0];
                            offerKey = offer_id.Split('#')[1];
                            newElementRow["offer_key"] = offerKey;
                            newElementRow["good_key"] = goodKey;
                        }
                        else
                        {
                            newElementRow["good_key"] = offer_id;
                        }
                    }
                    //===============================================

                    //Предложение и размер
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Наименование");
                    if (firstOrDefault != null)
                    {
                        var feature = firstOrDefault.LastChild.InnerText;
                        newElementRow["feature"] = feature;

                        var size = Regex.Match(feature, @"\(([^()]*)\)").Groups[1].Value;
                        newElementRow["size"] = size;
                    }
                    //===============================================

                    //Цена и Валюта
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Цены");
                    if (firstOrDefault != null)
                    {
                        var custom = firstOrDefault.LastChild.ChildNodes
                            .Cast<XmlElement>();

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
                        newElementRow["amount"] = (amount.Trim().Length == 0 ? "0" : amount.Trim());
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
                new FileLogger("Log.txt").LogMessage("Парсинг: " + error);
            }
        }

        public void LoadingInBD()
        {
            try
            {
                string sql = "";

                new FileLogger("Log.txt").LogMessage("Количество предложений: " + dataSource.Tables["OffersTable"].Rows.Count);

                //SQLiteProvider.ExecSql("delete from offers");
                var listSql = new List<string>();
                //OffersTable 

                var inBaseTable = SQLiteProvider.OpenSql("select offer_key, feature from offers")
                    .AsEnumerable()
                    .Select(x => new
                    {
                        offer_key = x["offer_key"],
                        feature = x["feature"]
                    })
                    .ToList();

                foreach (DataRow rowGroup in dataSource.Tables["OffersTable"].Rows)
                {
                    var cnt = inBaseTable.Count(x => (string) x.offer_key == rowGroup["offer_key"].ToString() && (string) x.feature == rowGroup["feature"].ToString());
                    if (cnt == 0 || rowGroup["offer_key"].ToString().Trim() == "")
                    {
                        var amount = rowGroup["amount"].ToString().Trim().Length == 0
                            ? "0"
                            : rowGroup["amount"].ToString().Trim();
                        sql = "insert into offers (good_key, offer_key, feature, price, currency, amount, size) values " +
                              "(N'" + rowGroup["good_key"] + "',N'" + rowGroup["offer_key"] + "',N'" + rowGroup["feature"] +
                              "'" +
                              "," + rowGroup["price"] + ",N'" + rowGroup["currency"] + "'," + amount + ",N'" + rowGroup["size"] + "')";
                        //SQLiteProvider.ExecSql(sql);
                    }
                    else
                    {
                        var amount = rowGroup["amount"].ToString().Trim().Length == 0
                            ? "0"
                            : rowGroup["amount"].ToString().Trim();
                        sql = "update offers set (price, currency, amount) = " +
                              "(" + rowGroup["price"] + ",N'" + rowGroup["currency"] + "'," + amount + ") " +
                              " where offer_key = N'" + rowGroup["offer_key"] + "' " +
                              " and good_key = N'" + rowGroup["good_key"] + "' " +
                              " and feature = N'" + rowGroup["feature"] + "' ";
                        //SQLiteProvider.ExecSql(sql);
                    }

                    listSql.Add(sql);
                }

                new FileLogger("Log.txt").LogMessage("Количество скриптов: " + listSql.Count);
                SQLiteProvider.ExecSql(listSql);
            }
            catch (Exception e)
            {
                var error = e.Message;
                new FileLogger("Log.txt").LogMessage(error);
            }
        }
    }
}
