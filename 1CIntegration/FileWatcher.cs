using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Xml;

namespace _1CIntegration
{ 
    public class FileWatcher
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            try
            {
                string[] args = System.Environment.GetCommandLineArgs(); 

                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = "C:\\Users\\r.karimov\\Downloads\\Temp\\webdata";

                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                watcher.Filter = "*.xml";

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Thread.Sleep(1000);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(e.FullPath);

                if (e.Name == "offers0_1.xml")
                {
                    var elements = xmlDocument.GetElementsByTagName("Предложение");

                    List<Sklad> list = new List<Sklad>();

                    foreach (XmlElement element in elements)
                    {
                        var sklad = new Sklad();

                        var childNodes = element.ChildNodes.Cast<XmlNode>();

                        var xmlNodes = childNodes as XmlNode[] ?? childNodes.ToArray();

                        sklad.Id = xmlNodes.Where(x => x.Name == "Ид").Select(x => x.ChildNodes).First().Item(0).Value.AsInteger();
                        sklad.Name = xmlNodes.Where(x => x.Name == "Наименование").Select(x => x.ChildNodes).First().Item(0).Value;

                        var store = new Store
                        {
                            StoreId =
                                xmlNodes.Where(x => x.Name == "Склад")
                                    .Select(x => x.Attributes["ИдСклада"])
                                    .First()
                                    .Value,
                            CountInStore =
                                xmlNodes.Where(x => x.Name == "Склад")
                                    .Select(x => x.Attributes["КоличествоНаСкладе"])
                                    .First()
                                    .Value.AsInteger()
                        };

                        sklad.Store = store;
                    }

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
