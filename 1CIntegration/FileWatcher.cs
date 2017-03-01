using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using Ninject;
using _1CIntegrationParserXML;
using _1CIntegrationDB;

namespace _1CIntegration
{
    public class FileWatcher
    {
        private readonly IKernel Kernel;

        public FileWatcher(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            try
            {
                string[] args = System.Environment.GetCommandLineArgs();

                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = "h:\\root\\home\\djinaroshop-001\\www\\webdata\\",
                    NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.CreationTime |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName |
                                   NotifyFilters.LastAccess |
                                   NotifyFilters.LastWrite |
                                   NotifyFilters.Security |
                                   NotifyFilters.Size,
                    Filter = "*.xml"
                };
                //C:\\Users\\r.karimov\\Downloads\\Temp\\webdata
                //C:\\Users\\Дмитрий\\Downloads\\webdata
                //h:\\root\\home\\djinaroshop-001\\www\\webdata\\

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);
                watcher.Error += new ErrorEventHandler(OnError);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                new FileLogger("Log.txt").LogMessage("Watcher создан!");
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message);
            }
        }


        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Thread.Sleep(2000);

                while (new FileInfo(e.FullPath).Attributes == 0)
                {
                }

                new FileLogger("Log.txt").LogMessage("Файл '" + e.Name + "' изменился");

                if (Kernel != null)
                {
                    ParserXmlFactory factory = Kernel.Get<ParserXmlFactory>();
                    factory.FindTemplate(e.FullPath, e.Name);
                }
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public void OnError(object sender, ErrorEventArgs e)
        {
            throw e.GetException();
        }
    }
}
