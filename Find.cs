using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntegrationTest
{
    class Find
    {
        static String classname = "Find:";
        public String FindIt(String sut,String company,String s)
        {
            String found = "";

            String drive = "c:";
            String sep = /*"\\"*/Path.DirectorySeparatorChar.ToString();
            String programfiles = "Program Files";
            //String company = "NAVTEQ";

            Launcher.logcontent += classname + "GetDirectories in " + drive + sep + programfiles + sep + company + sep + "\r\n";
            String[] dirs = Directory.GetDirectories(drive + sep + programfiles + sep + company + sep);
            foreach (String dir in dirs)
            {
                if (dir.IndexOf(company) >= 0)
                {
                    Launcher.logcontent += classname + "GetFiles in " + dir + "\r\n";
                    String[] files = Directory.GetFiles(dir);
                    foreach (String file in files)
                    {
                        if (file.IndexOf(sut+".exe") > 0 && file.EndsWith(".exe"))
                        {
                            found = file;
                        }
                    }
                }
            }

            Console.WriteLine("Found executable at: " + found);
            Sleeper sleepy = new Sleeper();
            sleepy.Sleep(2000);
            return found;
        }
    }
}
