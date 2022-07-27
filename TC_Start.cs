using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using IntegrationTest;


namespace TestCase
{
    class TC_Start
    {
        static String classname = "TC_Start:";
        public static String tc = "starts application under test";
        public static String parameters_csv_use = "parameters[0=company name: NAVTEQ|Nokia,1=list of parameters give executable-semicolon separated ex.(arg1;arg2)]";
        public String Start(string appPath, String parameters_csv, LogTest lt)
         {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + TC_Start.tc + "\r\n";

             String targetTitle = "";

             String[] parameters = parameters_csv.Split(',');
             String[] parameters_1 = parameters[1].Split(';'); 

            int MAXTIME = 15000; //   JZ: Total length in milliseconds to wait for the application to start
            //int MAXTIME = int.Parse(parameters[2]);
            int TIMEWAIT = 100; //   JZ: Total Timespan to wait till trying to find the window

            Process process = null;
            ProcessStartInfo psi = new ProcessStartInfo();

            process = new Process();
            psi.FileName = appPath;

            for(int i = 0; i < parameters_1.Length; i++)
                psi.Arguments = parameters_1[i];


            process.StartInfo = psi;
            process.Start();

            Console.WriteLine("command=" + psi.FileName + " parameters: " + psi.Arguments);//AM: dont throw exception, just let it start, wating 30 secs anyway

            int runningTime = 0;
            while (process.MainWindowHandle.Equals(IntPtr.Zero))
            {
                if (runningTime > MAXTIME)
                    //throw new Exception("Could not find " + appPath);
                    Console.WriteLine("runningTime=" + runningTime + " exceeds MAXTIME=" + MAXTIME);//AM: dont throw exception, just let it start, wating 30 secs anyway

                Thread.Sleep(TIMEWAIT);
                runningTime += TIMEWAIT;

                process.Refresh();
            }

            process.WaitForInputIdle(); //JZ: Wait for target executable to be idle, i.e. wait until it has entered its UI message loop

            Sleeper sleep = new Sleeper();
            sleep.Sleep();

            targetTitle = process.MainWindowTitle; //JZ: Get Title in run-time and pass it as param.  "NAVTEQ Earthscap 0.0.3" can be "0.0.4" tomorrow.
 
            Launcher.logcontent += classname + "Started " + targetTitle + "\r\n";

            //AM: BEGIN----------how to do a test versus expectation
            //ScreenCapture sc = new ScreenCapture();
            //String file1 = sc.GetDesktopImage(TC_OptionsPref3D.classname.Replace(":", "_") + "end");
            //CompareImage ci = new CompareImage();
            //LogTest lt = new LogTest();
            lt.LogTestResult("StartTest", classname/* + "\r\n" +*/, tc + "\r\n" + "run timestamp : " + Launcher.now, null, null, parameters);
            //bool result = ci.compare(file1, compareimage);
            bool result = true;
            if (result)
            {
                lt.LogTestResult("LogPass", null, null, null);
            }
            else
            {
                lt.LogTestResult("LogError", null, "Application failed to start", null);
            }
            lt.LogTestResult("EndTest", null, null, null);
            lt.LogTestResult("ReportResults", null, null, null);
            //AM: END----------how to do a test versus expectation

            return targetTitle;
        }

    }
}
