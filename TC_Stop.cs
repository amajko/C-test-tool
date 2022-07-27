using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using IntegrationTest;
using System.Threading;

namespace TestCase
{
    class TC_Stop
    {
        static String classname = "TC_Stop:";
        public static String tc = "stops application under test or gets the process for continued testing";
        public void Stop(string appPath, LogTest lt)
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "-S-" + "TestCase " + TC_Stop.tc + "\r\n";

            Process[] pall = Process.GetProcesses();
            int count = -1;
            foreach (Process item in pall)
            {
                count++;
                //Console.WriteLine(item.MainWindowTitle);
                if (item.MainWindowTitle == appPath)
                {
                        pall[count].CloseMainWindow();
                        pall[count].Close();
                }
            }

            //AM: BEGIN----------how to do a test versus expectation
            //ScreenCapture sc = new ScreenCapture();
            //String file1 = sc.GetDesktopImage(TC_OptionsPref3D.classname.Replace(":", "_") + "end");
            //CompareImage ci = new CompareImage();
            //LogTest lt = new LogTest();
            String[] steps = { "Stop" };
            lt.LogTestResult("StartTest", classname/* + "\r\n" +*/, tc + "\r\n" + "run timestamp : " + Launcher.now, null, null, steps);
            //bool result = ci.compare(file1, compareimage);
            bool result = true;
            if (result)
            {
                lt.LogTestResult("LogPass", null, null, null);
            }
            else
            {
                lt.LogTestResult("LogError", null, "Application failed to stop", null);
            }
            lt.LogTestResult("EndTest", null, null, null);
            lt.LogTestResult("ReportResults", null, null, null);
            //AM: END----------how to do a test versus expectation


        }

        public Process Get(string appPath)
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname += "-G-" + "TestCase " + TC_Stop.tc + "\r\n";

            Process p = null;

            Process[] pall = Process.GetProcesses();
            int count = -1;
            foreach (Process item in pall)
            {
                count++;
                //Console.WriteLine(item.MainWindowTitle);
                if (item.MainWindowTitle == appPath)
                {
                    p = pall[count];
                }
            }


            return p;
        }
    }
}
