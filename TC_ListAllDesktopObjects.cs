using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;
using System.Diagnostics;
using System.Windows.Automation;

namespace TestCase
{
    class TC_ListAllDesktopObjects
    {
        public static String tc = "lists all Desktop objects";
        public static String machinestatereq = "Started";
        static String classname = "TC_ListAllDesktopObjects:";
        public void ListAll()
        {
            //AM: find parent AutomationElement
            GetProcess gp = new GetProcess();
            String s = gp.GetTargetTitle();
            TC_Stop tc_s = new TC_Stop();
            Process p = tc_s.Get(s);
            Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

            ObjectFinder of = new ObjectFinder();
            //of.FindObjectsAll();
            of.FindObjectsAllFromProcess(p);

        }
    }
}
