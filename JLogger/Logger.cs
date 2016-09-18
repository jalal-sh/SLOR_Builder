using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logger
{
    public static class Logger
    {
        static StreamWriter logFile = new StreamWriter(File.Open("log.log", FileMode.Append, FileAccess.Write, FileShare.Read));
        public static void LogInformation(string informationType, string data)
        {
            string message = "[" + DateTime.Now.ToString("o") + "][" + informationType + "]:" + data;
            logFile.WriteLine(message);
            logFile.Flush();
        }
        public static void LogException(Exception e)
        {

            string message = e.Message;
            while (e.InnerException != null)
            {
                e = e.InnerException;
                message += "[inner]" + e.Message;
            }
            LogInformation("Exception", message);
        }


    }
}
