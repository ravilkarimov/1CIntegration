using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using Ninject;
using Ninject.Modules;
using _1CIntegrationParserXML;

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
                    Path = "C:\\Users\\r.karimov\\Downloads\\Temp\\webdata",
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*.xml"
                };
                //C:\\Users\\r.karimov\\Downloads\\Temp\\webdata
                //C:\\Users\\Дмитрий\\Downloads\\webdata

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Thread.Sleep(2000);

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
    }
}
