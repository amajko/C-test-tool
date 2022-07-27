using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Test.UIAutomation.Logging;
using Microsoft.Test.UIAutomation.Logging.InfoObjects;
using System.Diagnostics;
using TestCase;
using System.IO;

namespace IntegrationTest
{
    class LogTest
    {
        public static String resultfolder = "result";
        static String classname = "LogTest:";
        static String fontgreen = "<font color='green'>";
        static String fontred = "<font color='red'>";
        static String fontend = "</font>";
        static String bold = "<b>";
        static String boldend = "</b>";



        public LogTest()
        {
        if (!Directory.Exists(@"\TestbGUI\result"))
                Directory.CreateDirectory(@"\TestbGUI\result");
        }

        public void LogUI()
        {

            //AM: belo writes to console; must write to file
            System.IO.StreamWriter sw1 = new System.IO.StreamWriter("results.rpt");
            sw1.AutoFlush = true;
            Console.SetOut(sw1);

            //Set logger
            Logger.SetLogger("amNokUIALogging.dll");

            //Report a pass
            Logger.StartTest("...Test1...");
            Logger.LogComment("--comment here---");
            Logger.LogPass();
            Logger.EndTest();

             //Report a fail
            Logger.StartTest("...Test2...");
            Logger.LogComment("--comment there---");
            Logger.LogError("...error...");
            Logger.EndTest();

            //Report all results
            Logger.ReportResults();
            //Logger.CloseLog(); //AM: not doable, need unnec classes fr uiaverify

            //UIVerifyLogger.GenerateXMLLog("test.xml"); //AM: not doable, need unnec classes fr uiaverify

            Sleeper s = new Sleeper();
            s.Sleep(10000);

            //AM: abv writes to file; must redirect writes to console
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(Console.OpenStandardOutput());
            sw2.AutoFlush = true;
            Console.SetOut(sw2);

        }

        public void ShowInfo()
        {
            BaseInfo bi = new BaseInfo();
            String s = /*BaseInfo.toString()*/bi.ToString();
            Console.WriteLine(s);

            CommentInfo ci = new CommentInfo("this is a comment");
            Console.WriteLine(ci.ToString());

            ExceptionInfo ei = new ExceptionInfo(new System.Exception());
            Console.WriteLine(ei.ToString());

            //AM: find parent AutomationElement
            GetProcess gp = new GetProcess();
            String st = gp.GetTargetTitle();
            TC_Stop tc_s = new TC_Stop();
            Process p = tc_s.Get(st);
            MonitorProcessInfo mi = new MonitorProcessInfo(p);
            Console.WriteLine(mi.ToString());

            StartTestInfo sti = new StartTestInfo(null,new Microsoft.Test.UIAutomation.TestManager.TestCaseAttribute(),null);
            Console.WriteLine(sti.ToString());

            TestEndInfo tei = new TestEndInfo();
            Console.WriteLine(tei.ToString());

            TestResultInfo.TestResults tritrp = new TestResultInfo.TestResults();
            String passed = TestResultInfo.TestResults.Passed.ToString();
            Console.WriteLine(tritrp.ToString() + "=" + passed);
            TestResultInfo.TestResults tritrf = new TestResultInfo.TestResults();
            String failed = TestResultInfo.TestResults.Failed.ToString();
            Console.WriteLine(tritrf.ToString() + "=" + failed);

            TestResultInfo trip = new TestResultInfo(tritrp);
            Console.WriteLine(trip.ToString());
            TestResultInfo trif = new TestResultInfo(tritrf);
            Console.WriteLine(trif.ToString());

            ReportResultsInfo rri = new ReportResultsInfo();
            Console.WriteLine(rri.ToString());



            Sleeper sl = new Sleeper();
            sl.Sleep(20000);

 
        }

        public void LogTestResult(String logtype, String testinfo, String testcommentORerror, Exception exception)
        {

            //AM: belo writes to console; must write to file--appends

            //AM: check for suite file, and append to THAT if exists so that results can be collected for numerous testcases per suite run
            GetProcess gp = new GetProcess();
            String filename = gp.GetSuiteReport();
            String fullfile = "";
            if (filename != "" && File.Exists(Launcher.logdirectory + "\\" + resultfolder + "\\" + filename))
            {
                fullfile = Launcher.logdirectory + "\\" + resultfolder + "\\" + filename;
            }
            else
            {
                fullfile = Launcher.logdirectory + "\\" + resultfolder + "\\" + Launcher.logname + ".rpt";
            }

            System.IO.StreamWriter sw1 = new System.IO.StreamWriter(fullfile, true, Encoding.UTF8);
            sw1.AutoFlush = true;
            Console.SetOut(sw1);

            if (logtype == "StartTest")
            {
                Logger.LogComment(bold);
                Logger.StartTest(testinfo);
                Logger.LogComment(boldend);
            }

            if (logtype == "EndTest")
            {
                Logger.EndTest();
            }

            if (logtype == "LogPass")
            {
                Logger.LogComment(fontgreen);
                Logger.LogPass();
                Logger.LogComment("<center>Passed.</center>");
                Logger.LogComment(fontend);
            }

            if (logtype == "LogError")
            {
                Logger.LogComment(fontred);
                Logger.LogError(testcommentORerror);
                Logger.LogComment(fontend);
            }

            if (logtype == "LogComment")
            {
                Logger.LogComment(testcommentORerror);
            }

            if (logtype == "ReportResults")
            {
                //Report all results
                //AM: but only if there is no suite running (TC_Suite 'stop' will empty this string)
                //AM: but ReportResults is not cumulative, anyway, so make this test always true, for now
                if (filename == "" || filename != "")
                {
                    Logger.ReportResults();
                }
            }


            //AM: abv writes to file; must redirect writes to console
            sw1.Close();
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(Console.OpenStandardOutput());
            sw2.AutoFlush = true;
            Console.SetOut(sw2);
        }

        public void LogTestResult(String logtype, String test, String testinfo, String testcommentORerror, Exception exception, String[] Steps)
        {

            //AM: belo writes to console; must write to file--appends

            //AM: check for suite file, and append to THAT if exists so that results can be collected for numerous testcases per suite run
            GetProcess gp = new GetProcess();
            String filename = gp.GetSuiteReport();
            String fullfile = "";
            if (filename != "" && File.Exists(Launcher.logdirectory + "\\" + resultfolder + "\\" + filename))
            {
                fullfile = Launcher.logdirectory + "\\" + resultfolder + "\\" + filename;
            }
            else
            {
                fullfile = Launcher.logdirectory + "\\" + resultfolder + "\\" + Launcher.logname + ".rpt";
            }

            System.IO.StreamWriter sw1 = new System.IO.StreamWriter(fullfile, true, Encoding.UTF8);
            sw1.AutoFlush = true;
            Console.SetOut(sw1);

            if (logtype == "StartTest")
            {
                Logger.LogComment(bold);
                Logger.StartTest(test, testinfo, Steps);
                Logger.LogComment(boldend);
            }

            if (logtype == "EndTest")
            {
                Logger.EndTest();
            }

            if (logtype == "LogPass")
            {
                Logger.LogComment(fontgreen);
                Logger.LogPass();
                Logger.LogComment("<center>Passed.</center>");
                Logger.LogComment(fontend);
            }

            if (logtype == "LogError")
            {
                Logger.LogComment(fontred);
                Logger.LogError(testcommentORerror);
                Logger.LogComment(fontend);
            }

            if (logtype == "LogComment")
            {
                Logger.LogComment(testcommentORerror);
            }

            if (logtype == "ReportResults")
            {
                //Report all results
                //AM: but only if there is no suite running (TC_Suite 'stop' will empty this string)
                //AM: but ReportResults is not cumulative, anyway, so make this test always true, for now
                if (filename == "" || filename != "")
                {
                Logger.ReportResults();
                }
            }


            //AM: abv writes to file; must redirect writes to console
            sw1.Close();
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(Console.OpenStandardOutput());
            sw2.AutoFlush = true;
            Console.SetOut(sw2);
        }

        public void SetLogger()
        {
            //Set logger
            Logger.SetLogger("amNokUIALogging.dll");

        }

    }
}
