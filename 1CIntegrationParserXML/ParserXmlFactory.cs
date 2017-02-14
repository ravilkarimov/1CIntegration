using System;
using System.Data;
using System.Linq;
using System.Xml;
using Ninject;
using _1CIntegrationDB;

namespace _1CIntegrationParserXML
{
    public class ParserXmlFactory
    {
        public IKernel Kernel { get; set; }

        public ParserXmlFactory(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        public void FindTemplate(string fullPathFile, string nameFile)
        {
            try
            {
                if (Kernel != null)
                {
                    var parsingImpl = Kernel.Get<IBaseParserXml>(nameFile.Replace("0_1.xml", ""));

                    parsingImpl.ParsingFileXml(fullPathFile);
                    parsingImpl.LoadingInBD();

                }
            }
            catch (Exception)
            {
                
            }
        }

        private void RunParser()
        {
            
        }
    }

    public interface IBaseParserXml
    {
        void ParsingFileXml(string fullPath);
        void LoadingInBD();
    }

    public class ParserXmlNameImport : IBaseParserXml
    {
        private DataSet dataSource;

        public void ParsingFileXml(string fullPath)
        {
            try
            {
                DataTable groupsDataTable = new DataTable {TableName = "GroupsTable"};
                groupsDataTable.Columns.Add("group_key", typeof(string));
                groupsDataTable.Columns.Add("group_name", typeof(string));

                DataTable elementsDataTable = new DataTable {TableName = "ElementsTable"};
                elementsDataTable.Columns.Add("good_key", typeof(string));
                elementsDataTable.Columns.Add("good", typeof(string));
                elementsDataTable.Columns.Add("group_key", typeof(string));

                DataTable featuresDataTable = new DataTable {TableName = "FeaturesTable"};
                featuresDataTable.Columns.Add("good_key", typeof(string));
                featuresDataTable.Columns.Add("feature_key", typeof(string));
                featuresDataTable.Columns.Add("value", typeof(string));

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fullPath);

                #region Группы
                var groups = xmlDocument.GetElementsByTagName("Группа");
                var listNodes = groups.Cast<XmlElement>().Cast<XmlNode>().ToList();

                foreach (var item in listNodes.Select(x => x.ChildNodes))
                {
                    DataRow newGroupRow = groupsDataTable.NewRow();
                    var firstOrDefault = item.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Ид");
                    if (firstOrDefault != null)
                        newGroupRow["group_key"] = firstOrDefault.LastChild.Value;
                    firstOrDefault = item.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Наименование");
                    if (firstOrDefault != null)
                        newGroupRow["group_name"] = firstOrDefault.LastChild.Value;
                    groupsDataTable.Rows.Add(newGroupRow);
                }

                #endregion

                #region Товары
                var elements = xmlDocument.GetElementsByTagName("Товар");
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

                    elementsDataTable.Rows.Add(newElementRow);
                }
                #endregion

                dataSource.Tables.Add(groupsDataTable);
                dataSource.Tables.Add(elementsDataTable);
                dataSource.Tables.Add(featuresDataTable);
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
                DataTable goodTable = SQLiteProvider.OpenSql("select * from good");
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }
    }
}
