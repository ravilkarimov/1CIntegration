using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Xml;
using _1CIntegrationDB;

namespace _1CIntegrationParserXML
{
    public class ParserXmlNameMessages : IBaseParserXml
    {
        private DataSet dataSource = new DataSet();

        public void ParsingFileXml(string fullPath)
        {
            try
            {
                DataTable messagesDataTable = new DataTable { TableName = "MessagesTable" };
                messagesDataTable.Columns.Add("good_key", typeof(string));
                messagesDataTable.Columns.Add("receipt_date", typeof(DateTime));

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

                #region Поступления
                var elements = xmlDocument.GetElementsByTagName("Документ.ПоступлениеТоваровУслуг");//Товары Строка
                var listNodes = elements.Cast<XmlElement>().Cast<XmlNode>().ToList(); 

                var data = new DateTime();

                foreach (var item in listNodes.Select(x => x.ChildNodes))
                {
                    //Дата
                    var orDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "КлючевыеСвойства");
                    if (orDefault != null)
                    {
                        var xmlNode = orDefault.ChildNodes.Cast<XmlNode>().ToList().FirstOrDefault(x => x.Name == "Дата");
                        if (xmlNode != null)
                        {
                            var firstOrDefault = xmlNode.InnerText;
                            if (!firstOrDefault .IsNullOrEmpty())
                            {
                               data  = Convert.ToDateTime(firstOrDefault);
                            }
                        }
                    }
                    //===============================================

                    var rows = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Товары").ChildNodes.Cast<XmlElement>().Cast<XmlNode>().ToList();
                    foreach (var row in rows)
                    {
                        DataRow newElementRow = messagesDataTable.NewRow();

                        var nodes = row.ChildNodes.Cast<XmlNode>().ToList();

                        newElementRow["receipt_date"] = data;
                        newElementRow["good_key"] = nodes.FirstOrDefault(x => x.Name == "ДанныеНоменклатуры").ChildNodes.Cast<XmlNode>().ToList()
                            .FirstOrDefault(x => x.Name == "Номенклатура").ChildNodes.Cast<XmlNode>().ToList().FirstOrDefault(x => x.Name == "Ссылка").InnerText;
                        //newElementRow["count"] = nodes.FirstOrDefault(x => x.Name == "Количество").InnerText;

                        messagesDataTable.Rows.Add(newElementRow);
                    }
                }
                #endregion

                dataSource.Tables.Add(messagesDataTable);
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

                new FileLogger("Log.txt").LogMessage("Количество поступлений: " + dataSource.Tables["MessagesTable"].Rows.Count);

                var listSql = new List<string>();
                //MessagesTable 

                var inBaseTable = SQLiteProvider.OpenSql("select good_key, receipt_date from receipts")
                    .AsEnumerable()
                    .Select(x => new
                    {
                        good_key = x["good_key"].ToString().Trim(),
                        receipt_date = Convert.ToDateTime(x["receipt_date"])
                    })
                    .ToList();

                var countAll = 0;
                var countElem = 0;
                foreach (var rowGroup in dataSource.Tables["MessagesTable"].AsEnumerable().Select(x => new
                {
                    good_key = x["good_key"].ToString().Trim(),
                    receipt_date = Convert.ToDateTime(x["receipt_date"])
                }).Distinct())
                {
                    var cnt = inBaseTable.Count(x => x.good_key == rowGroup.good_key && x.receipt_date == rowGroup.receipt_date);
                    if (cnt == 0)
                    {
                        sql = "insert into receipts (good_key, receipt_date) values " +
                              "(N'" + rowGroup.good_key + "','" + Convert.ToDateTime(rowGroup.receipt_date).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        //SQLiteProvider.ExecSql(sql);
                    }

                    listSql.Add(sql);
                    countElem++;
                    countAll++;

                    if (countElem == 100 || countAll == dataSource.Tables["MessagesTable"].Rows.Count)
                    {
                        SQLiteProvider.ExecSql(listSql);
                        listSql = new List<string>();
                        countElem = 0;
                    }
                }

                new FileLogger("Log.txt").LogMessage("Количество скриптов: " + countAll);
            }
            catch (Exception e)
            {
                var error = e.Message;
                new FileLogger("Log.txt").LogMessage(error);
            }
        }
    }
}
