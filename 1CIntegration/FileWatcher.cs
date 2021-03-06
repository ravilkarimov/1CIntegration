﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using Ninject;
using _1CIntegrationParserXML;
using _1CIntegrationDB;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

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
                    Path = ConfigurationManager.AppSettings["FileWatcher"],
                    NotifyFilter = NotifyFilters.CreationTime |
                                   NotifyFilters.LastWrite |
                                   NotifyFilters.Size,
                    Filter = "*.*",
                    IncludeSubdirectories = true,
                    InternalBufferSize = 64
                };
                //C:\\Users\\r.karimov\\Downloads\\Temp\\webdata
                //C:\\Users\\Дмитрий\\Downloads\\webdata
                //h:\\root\\home\\djinaroshop-001\\www\\webdata\\

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                //watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //watcher.Renamed += new RenamedEventHandler(OnRenamed);
                //watcher.Error += new ErrorEventHandler(OnError);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                new FileLogger("Log.txt").LogMessage("Watcher создан!");
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message);
            }
        }

        private List<string> listFleName = new List<string>(); 

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType == WatcherChangeTypes.Changed &&
                    Regex.IsMatch(e.FullPath, @"\.xml", RegexOptions.IgnoreCase) &&
                    !listFleName.Contains(e.Name))
                {
                    Task.Factory.StartNew(() =>
                    {
                        listFleName.Add(e.Name);

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

                        listFleName.Remove(e.Name);

                    });
                }
                else if (e.ChangeType == WatcherChangeTypes.Changed &&
                         Regex.IsMatch(e.FullPath, @"\.jpg", RegexOptions.IgnoreCase) &&
                         e.FullPath.IndexOf("_min.jpg", StringComparison.Ordinal) < 0 &&
                    !listFleName.Contains(e.Name))
                {
                    listFleName.Add(e.Name);

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(1000);

                        while (new FileInfo(e.FullPath).Attributes == 0)
                        {
                        }

                        using (var fs = System.IO.File.OpenRead(e.FullPath))
                        {
                            using (var image = Image.FromStream(fs, true, true))
                            {
                                image.ResizeBitmapUpto(350, 350, InterpolationMode.HighQualityBicubic)
                                    .ToFileSave(e.FullPath, e.Name);
                            }
                        }

                        listFleName.Remove(e.Name);

                    });
                }
            }
            catch (Exception e22)
            {
                new FileLogger("Log.txt").LogMessage(e22.Message);
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public void OnError(object sender, ErrorEventArgs e)
        {
            throw e.GetException();
        }
    }
}
