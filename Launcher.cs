using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;
using System.Threading;
using TestCase;
using Examples;


//al.majko@navteq.com (AM), xin.3.zheng@navteq.com(JZ), sue.vanderwiel@nokia.com  1/18/12 (based on TestaGUI)
namespace IntegrationTest
{
    class Launcher
    {
        static String classname = "Launcher:";
        static LogTest logtest = new LogTest();

        //AM: sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
        public static bool reverse = false;//AM: change this to false when running on right-handed mouse
        public static bool printme = false;//AM: set print of logs to working directory on/off (these are NOT testcase outcome logs)
        public static String ecurl = "";//AM: set to url for 3DMaps
        public static String reportarchive = "";//AM: set to location for web service Dashoard to hold latest results-report
        //-----/\-----AM: will allow setting from a properties file

        //AM: Logger
        public static String now = DateTime.Now.ToString("MMddyyyy_HHmmss");
        public static String logdirectory = "\\TestbGUI";
        public static String logname = "Launcher." + now + ".log";
        public static String logcontent = classname + now + "\r\n";
        public static FileIO fiolog = new FileIO();
        public static String processtitle = "testbgui.process.targetTitle.txt";
        public static String reportfile = "testbgui.suite.reportfile.txt";
        public static String xyfile = "testbgui.xy.lastmouselocation.txt";

        [STAThread] // JZ
        static void Main(string[] args)
        {
            //AM: Logger for debugging purposes
            Launcher.fiolog.Write(Launcher.logname, Launcher.logcontent, "LOG");

            //AM: report logger for pass/fail test results
            Launcher.logtest.SetLogger();

            // JZ: Keep one argument in csproj
            Console.WriteLine("Argument size: " + args.Length);
            logcontent = classname + "---begin---" + "\r\n";//AM: keep this as begin of logcontent, AFTER file has been initally creted and written to, above
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Argument[" + i + "] : " + args[i]);
                logcontent += classname + "Argument[" + i + "] : " + args[i] + "\r\n";
            }

            //AM: using Microsoft Logger
            LogTest lt = new LogTest();
            //lt.Log();

            //====================AM: Begin Robot Resource Creation=========================
            /*This will create a resource file 'CSHARP.txt' that will contain keyword definitions for running this .exe
             * Running this app without parms will cause this to be written, so that it can be included as a resource in the Robot test case
             * 
             * This should be a standard snippet in every C# implementation of a Robot keyword
             */
            int testlimit = 5;
            // JZ: Bypass next if-block as we may not have 5 arguments now (Earthscale title will NOT be one argument)
            if (args.Length == 0) //JZ: multiply by 100
            {
                String filename = "CSharpResource.txt";
                Console.WriteLine("Writing resource file: " + filename);
                FileIO fio = new FileIO();

                /* Create file with this string:
                *  *** Settings ***
                   Resource        resource.txt

                   *** Keywords ***
                   Test A GUI
                       [Arguments]  ${SUT}  ${executable}  ${mainWindow}  ${shortcut}  ${index}
                       Run  ${r_working_directory}\\TestbGUI \ ${SUT} ${executable} ${mainWindow} ${shortcut} ${index}
                */

                String content = "*** Settings ***";
                content += "\r\n";
                content += "Documentation   Some arguments are _csv which can be comma separated values for multiple actions to be run";
                content += "\r\n";
                content += "Resource        resource.txt";
                content += "\r\n";
                content += "\r\n";
                content += "***Keywords ***";
                content += "\r\n";
                content += "Test A GUI";
                content += "\r\n";
                content += "    [Arguments]  ${SUT}  ${executable}  ${testcase}  ${parameters_csv}  # executable can be the word 'Find' and then the latest version of the SUT will be found in 'Program Files'";
                content += "\r\n";
                content += "    Run  ${r_working_directory}" + "\\\\" + "TestbGUI" + " ${SUT} ${executable} ${testcase} ${parameters_csv}";
                content += "\r\n";

                fio.Write(filename, content,"info");
                Environment.Exit(0);
            }

            ReadProperties rp = new ReadProperties();
            rp.Read();

            try
            {//AM: block to capture ANY exception, so that logcontent can be flushed and saved
                //JZ: Examples
                if (args[0] == "SampleApp")
                {
                    SampleApp.Start();
                }
                if (args[0] == "CryptoCalc")
                {
                    CryptoCalc.Start();
                }


                //AM: EarthScape
                if (args[0] == "EarthScape" || args[0] == "firefox")
                //AM: pssobile arguments: 0:${SUT}  1:${executable}  2:${testcase};
                {
                    //AM: cycle thru various testcases

                    if (args[2] == "Start")
                    {
                        String s = args[1];
                        if(s.ToUpper() == "FIND")   //Can say NOT FIND to avoid everything....actual executable will bipass it.
                        {
                            Find f = new Find();
                            s = f.FindIt(args[0],args[3],s);
                        }

                        TC_Start tc_s = new TC_Start();
                        String targetTitle = tc_s.Start(s, args[3], Launcher.logtest);
                        //AM: writing this so can find name to stop it later
                        FileIO fio = new FileIO();
                        fio.Write("testbgui.process.targetTitle.txt", targetTitle, "info");
                    }

                    if (args[2] == "Stop")
                    {
                        
                        Sleeper sleep = new Sleeper();
                        sleep.Sleep();

                        //AM: reading mainwindowtitle of process that is open
                        GetProcess gp = new GetProcess();
                        String s = gp.GetTargetTitle();
                        TC_Stop tc_s = new TC_Stop();
                        tc_s.Stop(s, Launcher.logtest);

                        //AM: removing the file entry
                        FileIO fio = new FileIO();
                        fio.Write("testbgui.process.targetTitle.txt", "", "info");

                    }

                    if (args[2] == "List")//AM: a (clumsy) method to list all the testcases
                    {
                        String[] validtests = new String[] { "Start", "Stop", "List", "Sleep", "Suite", "Report", "OptionsPref3D", "LocatorLocation", "ListAllDesktopObjects", "KeyWord" };

                        //AM: instacne all tc's so can get their static descriptions?
                        TC_Start tc1 = new TC_Start();
                        TC_Stop tc2 = new TC_Stop();
                        TC_List tc3 = new TC_List();
                        TC_Sleep tc3b = new TC_Sleep();
                        TC_Suite tc3c = new TC_Suite();
                        TC_Report tc3d = new TC_Report();
                        TC_OptionsPref3D tc4 = new TC_OptionsPref3D();
                        TC_LocatorLocation tc4b = new TC_LocatorLocation();
                        TC_ListAllDesktopObjects tc5 = new TC_ListAllDesktopObjects();
                        TC_KeyWord tckw1 = new TC_KeyWord();

                        Launcher.logcontent += classname + "TestCases:" + "\r\n";
                        int count = -1;
                        foreach (String item in validtests)
                        {
                            count++;
                            Launcher.logcontent += classname + validtests[count] + "\r\n";

                        }

                        TC_List tc_l = new TC_List();

                        tc_l.List("Start=" + TC_Start.tc);
                        tc_l.List("Start uses parameters_csv ([args3]): " + TC_Start.parameters_csv_use);
                        tc_l.List("Stop=" + TC_Stop.tc);
                        tc_l.List("List=" + TC_List.tc);
                        tc_l.List("Sleep=" + TC_Sleep.tc);
                        tc_l.List("Suite=" + TC_Suite.tc);
                        tc_l.List("Suite uses parameters_csv ([args3]): " + TC_Suite.parameters_csv_use);
                        tc_l.List("Report=" + TC_Report.tc);
                        tc_l.List("OptionsPref3D=" + TC_OptionsPref3D.tc);
                        tc_l.List("OptionsPref3D requires that " + args[0] + " is " + TC_OptionsPref3D.machinestatereq);
                        tc_l.List("OptionsPref3D uses parameters_csv ([args3]): " + TC_OptionsPref3D.parameters_csv_use);
                        tc_l.List("OptionsPref3D tests against " + TC_OptionsPref3D.compareimage);
                        tc_l.List("LocatorLocation=" + TC_LocatorLocation.tc);
                        tc_l.List("LocatorLocation requires that " + args[0] + " is " + TC_LocatorLocation.machinestatereq);
                        tc_l.List("LocatorLocation uses parameters_csv ([args3]): " + TC_LocatorLocation.parameters_csv_use);
                        tc_l.List("LocatorLocation tests against " + TC_LocatorLocation.compareimage);
                        tc_l.List("ListAllDesktopObjects=" + TC_ListAllDesktopObjects.tc);
                        tc_l.List("ListAllDesktopObjects requires that " + args[0] + " is " + TC_ListAllDesktopObjects.machinestatereq);
                        tc_l.List("KeyWord=" + TC_KeyWord.tc);
                        tc_l.List("KeyWord uses parameters_csv ([args3]): " + TC_KeyWord.parameters_csv_use);

                        //AM: test the logger for test pass/fail staements to console and 'results.pt' file
                        //LogTest logtest = new LogTest();
                        logtest.LogUI();
                        logtest.ShowInfo();
                    }

                    if (args[2] == "Sleep")
                    {
                        TC_Sleep tc_sl = new TC_Sleep();
                        tc_sl.Sleep(args[3]);
                      }
                    if (args[2] == "Suite")
                    {
                        TC_Suite tc_s = new TC_Suite();
                        tc_s.Manage(args[3], Launcher.logtest);
                    }
                    if (args[2] == "Report")
                    {
                        TC_Report tc_r = new TC_Report();
                        tc_r.Report();
                    }
                    if (args[2] == "ListAllDesktopObjects")
                    {
                        TC_ListAllDesktopObjects tclado = new TC_ListAllDesktopObjects();
                        tclado.ListAll();
                    }

                    if (args[2] == "OptionsPref3D")
                    {
                        TC_OptionsPref3D tcop3 = new TC_OptionsPref3D();
                        //AM: parse 3rd indexed args[3] which should be csv's for custom parms
                        tcop3.Invoke(args[3], Launcher.logtest);
                        //ScreenCapture sc = new ScreenCapture();
                        //sc.GetDesktopImage(TC_OptionsPref3D.classname.Replace(":","-") + "_end");
                    }

                    if (args[2] == "LocatorLocation")
                    {
                        TC_LocatorLocation tcll = new TC_LocatorLocation();
                        //AM: parse 3rd indexed args[3] which should be csv's for custom parms
                        //AM: how to start a logger for test results...
                        tcll.Invoke(args[3], Launcher.logtest);
                        //ScreenCapture sc = new ScreenCapture();
                        //sc.GetDesktopImage(TC_LocatorLocation.classname.Replace(":", "-") + "_end");
                    }

                    if (args[2] == "KeyWord")
                    {
                        TC_KeyWord tckw = new TC_KeyWord();
                        tckw.Do(args[3], Launcher.logtest);
                    }

                    //AM: end cycle thru various testcases

                }//end EarthScape




                logcontent += classname + "---end---" + "\r\n";
                Launcher.fiolog.Append(Launcher.logname, Launcher.logcontent, "log");
                //lt.LogTestResult("ReportResults", null, null, null);

            }
            catch(Exception e)
            {
                Console.WriteLine("Fatal Error: " + e);
                Launcher.logcontent += classname + "Fatal Error: " + e + "\r\n";
                Launcher.fiolog.Append(Launcher.logname, Launcher.logcontent, "log");
                //lt.LogTestResult("ReportResults", null, null, null);
                Environment.Exit(0);
            }

        }
    }
}
