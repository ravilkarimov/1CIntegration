using System;
using System.Data;
using System.Xml;
using Ninject;
using Ninject.Modules;

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
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RunParser()
        {
            
        }
    }

    public interface IBaseParserXml
    {
        string Name { get; set; }
        DataTable ParsingFileXml(string fullPath);
    }

    public class ParserXmlNameImport : IBaseParserXml
    {

        public DataTable ParsingFileXml(string fullPath)
        {
            try
            {
                DataTable resultDataTable = new DataTable();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fullPath);

                var elements = xmlDocument.GetElementsByTagName("Товар");

                foreach (XmlElement element in elements)
                {



                }

                return resultDataTable;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return null;
            }
        }

        public string Name { get; set; }
    }
}
