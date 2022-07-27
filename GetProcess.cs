using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    class GetProcess
    {
        static String classname = "GetProcess:";
        public String GetTargetTitle()
        {
            String s = "";

            FileIO fio = new FileIO();
            s = fio.Read(Launcher.logdirectory + "\\" + "info" + "\\" + Launcher.processtitle);

            return s;
        }
        public String GetSuiteReport()
        {
            String s = "";

            FileIO fio = new FileIO();
            s = fio.Read(Launcher.logdirectory + "\\" + "info" + "\\" + Launcher.reportfile);

            return s;
        }
        public String GetXY()
        {
            String s = "";

            FileIO fio = new FileIO();
            s = fio.Read(Launcher.logdirectory + "\\" + "info" + "\\" + Launcher.xyfile);

            return s;
        }
    }
}
