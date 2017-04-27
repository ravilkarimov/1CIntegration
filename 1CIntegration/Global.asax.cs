using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using _1CIntegrationDB;
using _1CIntegrationParserXML;

namespace _1CIntegration
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IKernel kernel = new StandardKernel(
                new HelperModule()
                //new ServiceModule()
                );

            new DBProgram();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(60000);

                    var fileWatcher = kernel.Get<FileWatcher>();
                    fileWatcher.Run();
                }
            }, TaskCreationOptions.LongRunning);
            

        }
    }

    public class HelperModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel != null)
            {
                Bind<FileWatcher>().To<FileWatcher>().WithConstructorArgument("Kernel", Kernel);
                Bind<ParserXmlFactory>().To<ParserXmlFactory>().InSingletonScope().WithConstructorArgument("Kernel", Kernel);
            }
            Bind<IBaseParserXml>().To<ParserXmlNameImport>().InTransientScope().Named("import0_1");
            Bind<IBaseParserXml>().To<ParserXmlNameOffers>().InTransientScope().Named("offers0_1");
            Bind<IBaseParserXml>().To<ParserXmlNameMessages>().InTransientScope().Named("Message_РТ_СТ");
            Bind<IBaseParserXml>().To<ParserXmlNameMessages>().InTransientScope().Named("Message_РТ_01");
        }
    }

    /*public class ServiceModule : NinjectModule
       {
           public override void Load()
           {
               var helper = Kernel.Get<IConfigHelper>();
               Bind<IMyService>()
                   .To<MyServiceImpl>()
                   .WithConstructorArgument("myArg", helper.MyProperty);
           }
       }*/

}