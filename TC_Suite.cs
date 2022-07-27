using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;

namespace TestCase
{
    class TC_Suite
    {
        static String classname = "TC_Suite:";
        public static String tc = "sets up suite reporting for a multi-test run";
        public static String parameters_csv_use = "parameters[0=Start|Stop,1=description of test]";

        public void Manage(String parameters_csv, LogTest lt)
        {
            Launcher.logcontent += classname + "TestCase " + tc + "\r\n";
            Launcher.logcontent += classname + "Uses parameters_csv ([args3]): " + parameters_csv_use + "\r\n";

            String[] parameters = parameters_csv.Split(',');

            lt.LogTestResult("StartTest", classname/* + "\r\n" +*/, tc/* + "\r\n" + parameters[1]*/, null, null, parameters);
            if (parameters[0].ToUpper() == "START")
            {
                //lt.LogTestResult("LogComment", null, "******************************Suite starts******************************", null);
                FileIO fio = new FileIO();
                fio.Write(Launcher.reportfile, Launcher.logname + ".rpt", "info");
            }
            //lt.LogTestResult("EndTest", null, null, null);
            //lt.LogTestResult("ReportResults", null, null, null);
            if (parameters[0].ToUpper() == "STOP")
            {
                SumSuite ss = new SumSuite();
                String summary = ss.Calc();
                lt.LogTestResult("LogComment", null, summary, null);

                //lt.LogTestResult("LogComment", null, "******************************Suite stops******************************", null);
                FileIO fio = new FileIO();
                fio.Write(Launcher.reportfile, "", "info");
            }
            //AM:sleep a lil so new file is created with second timestamp diff for next test that wont overwrite this test's log
            Sleeper sleepy = new Sleeper();
            sleepy.Sleep(1500);
        }

 
    }
}
