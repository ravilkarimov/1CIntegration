using System;
using System.IO;
using System.Text;

namespace _1CIntegrationDB
{
    /// Интерфейс обьекта, показывающего логи
    /// 
    public abstract class Logger
    {
        public Logger()
        {
            Log.OnLogHandler += new Log.LogEventHandler(LogMessage);
        }

        public abstract void LogMessage(string Message);
    }

    /// 

    /// Показывать логи на консоль
    /// 

    class ConsoleLogger : Logger
    {
        public override void LogMessage(string Message)
        {
            Console.WriteLine(Message);
        }
    }

    class ConsoleLoggerWithTime : Logger
    {
        public override void LogMessage(string Message)
        {
            Console.WriteLine(DateTime.Now + ": " + Message);
        }
    }

    /// 

    /// Сохранять логи в файл
    /// 
    public class FileLogger : Logger
    {
        string path = "C:\\Users\\r.karimov\\Downloads\\";
        //h:\\root\\home\\djinaroshop-001\\www\\logs\\
        //C:\\Users\\Дмитрий\\
        //C:\\Users\\r.karimov\\Downloads\\

        public FileLogger(string fileName)
            : base()
        {
            path = path + fileName;
        }

        public override void LogMessage(string Message)
        {
            File.AppendAllText(path, DateTime.Now + ": " + Message + Environment.NewLine);
        }
    }

    /// 

    /// "Глобальная переменная" 
    /// 

    class Log
    {
        /// 

        /// Свойство и делегат
        /// 

        public delegate void LogEventHandler(string Message);
        static public event LogEventHandler OnLogHandler;

        /// 

        /// Пользовательские функции
        /// 

        static public void WriteLine(string Message)
        {
            if (OnLogHandler != null)
            {
                OnLogHandler(Message);
            }
        }
    }
}
