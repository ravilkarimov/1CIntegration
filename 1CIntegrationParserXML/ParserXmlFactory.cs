using System;
using Ninject;

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

}
