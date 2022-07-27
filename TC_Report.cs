using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;

namespace TestCase
{
    class TC_Report
    {
        static String classname = "TC_Report:";
        public static String tc = "sends the latest ./result/*.rpt to web dashboard archive";

        public void Report()
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + TC_Report.tc + "\r\n";

            FileIO fio = new FileIO();
            Launcher.logcontent += classname + "finding files in " + Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + "\r\n";
            System.IO.FileInfo[] fi = fio.ListFiles(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\");
            for (int i = 1; i < fi.GetLength(0); i++)
            {
                //Console.WriteLine(fi.ElementAt(i).ToString());
                //Launcher.logcontent += classname + fi.ElementAt(i).ToString() + "\r\n";
            }
            String filename = fio.GetLatestFile(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\");
            Launcher.logcontent += classname + "latest file is " + filename + "\r\n";

            //convert to html
            String contents = fio.Read(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + filename);
            fio.Write(filename, "<pre>" + contents + "</pre>", LogTest.resultfolder);

            Launcher.logcontent += classname + fio.CopyFile(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + filename, Launcher.reportarchive + "\\" + "latest.rpt") + "\r\n";
            Launcher.logcontent += classname + fio.CopyFile(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + filename, Launcher.reportarchive + "\\" + "latest.htm") + "\r\n";
 
        }
    }
}
