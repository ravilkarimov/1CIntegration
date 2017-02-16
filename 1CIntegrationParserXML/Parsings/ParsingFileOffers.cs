using System;
using System.Data;
using System.Linq;
using System.Xml;

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
                offersDataTable.Columns.Add("good_key", typeof(string));
                offersDataTable.Columns.Add("good", typeof(string));
                offersDataTable.Columns.Add("group_key", typeof(string));

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fullPath);
                
                #region Предложения
                var elements = xmlDocument.GetElementsByTagName("Предложение");
                listNodes = elements.Cast<XmlElement>().Cast<XmlNode>().ToList();

                foreach (var item in listNodes.Select(x => x.ChildNodes))
                {
                    DataRow newElementRow = elementsDataTable.NewRow();
                    var goodKey = "";

                    //Ид
                    var firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Ид");
                    if (firstOrDefault != null)
                    {
                        goodKey = firstOrDefault.LastChild.InnerText;
                        newElementRow["good_key"] = goodKey;
                    }
                    //===============================================

                    //Наименование
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Наименование");
                    if (firstOrDefault != null)
                    {
                        var good = firstOrDefault.LastChild.InnerText;
                        newElementRow["good"] = good;
                    }
                    //===============================================

                    //Группа
                    firstOrDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "Группы");
                    if (firstOrDefault != null)
                    {
                        var groupKey = firstOrDefault.LastChild.InnerText;
                        newElementRow["group_key"] = groupKey;
                    }
                    //===============================================

                    //Фичи
                    var orDefault = item.Cast<XmlElement>()
                        .Cast<XmlNode>()
                        .FirstOrDefault(x => x.Name == "ХарактеристикиТовара");
                    if (orDefault != null)
                    {
                        var features = orDefault
                            .ChildNodes.Cast<XmlElement>().Cast<XmlNode>().ToList();

                        foreach (var feature in features.Select(x => x.ChildNodes))
                        {
                            DataRow newFeatureRow = featuresDataTable.NewRow();
                            newFeatureRow["good_key"] = goodKey;

                            var featureKey = feature.Cast<XmlElement>()
                                .Cast<XmlNode>()
                                .FirstOrDefault(x => x.Name == "Ид");
                            if (featureKey != null) newFeatureRow["feature_key"] = featureKey.LastChild.InnerText;

                            var featureValue = feature.Cast<XmlElement>()
                                .Cast<XmlNode>()
                                .FirstOrDefault(x => x.Name == "Значение");
                            if (featureValue != null) newFeatureRow["value"] = featureValue.LastChild.InnerText;


                            featuresDataTable.Rows.Add(newFeatureRow);
                        }
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

                //GroupsTable 
                foreach (DataRow rowGroup in dataSource.Tables["GroupsTable"].Rows)
                {
                    int cnt = Convert.ToInt32(SQLiteProvider.OpenSql("select count(*) cnt from groups where group_key = '" + rowGroup["group_key"] + "'").Rows[0]["cnt"]);
                    if (cnt == 0)
                    {
                        sql = "insert into groups (group_key, group_name) values ('" + rowGroup["group_key"] + "','" + rowGroup["group_name"] + "')";
                        SQLiteProvider.ExecSql(sql);
                    }
                }

                //ElementsTable 
                foreach (DataRow rowGroup in dataSource.Tables["ElementsTable"].Rows)
                {
                    int cnt = Convert.ToInt32(SQLiteProvider.OpenSql("select count(*) cnt from goods where good_key = '" + rowGroup["good_key"] + "'").Rows[0]["cnt"]);
                    if (cnt == 0)
                    {
                        string groupId = SQLiteProvider.OpenSql("select group_id from groups where group_key = '" + rowGroup["group_key"] + "'").Rows[0]["group_id"].ToString();

                        sql = "insert into goods (good_key, good, group_id) " +
                              "values " +
                              "('" + rowGroup["good_key"] + "','" + rowGroup["good"] + "', " + groupId + ")";
                        SQLiteProvider.ExecSql(sql);
                    }
                }

                //FeaturesTable
                //Пока не делаем
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }
    }
}
