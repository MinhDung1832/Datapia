using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DataAccess
{
    public class LogBuild
    {
        public static void CreateLogger(string str, string namecontroller)
        {
            StreamWriter sw = null;
            try
            {

                string str_dir = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";

                if (!Directory.Exists(str_dir))
                {
                    Directory.CreateDirectory(str_dir);
                }


                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\" + DateTime.Now.ToString("yyyyMMdd") + "_Log" + namecontroller + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + str);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
    }
}