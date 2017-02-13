using System;
using Ninject.Modules;

namespace _1CIntegrationParserXML
{
    public class ParserXmlFactory : NinjectModule
    {
        private void FindTemplate(string fullPathFile, string nameFile)
        {
            try
            {
                var parsingImpl = Bind<IBaseParserXml>().To<IBaseParserXml>().WithPropertyValue("NameFile", nameFile.Replace(".xml", ""));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RunParser()
        {
            
        }

        public override void Load()
        {
            
        }
    }

    public interface IBaseParserXml
    {
        string NameFile { get; }
    }

    public class ParserXmlNameImport : IBaseParserXml
    {
        public string NameFile
        {
            get { return "import"; }
        }
    }
}
