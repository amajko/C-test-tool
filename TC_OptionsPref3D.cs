using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;
using System.Diagnostics;
using System.Windows.Automation;
using System.Threading;

namespace TestCase
{
    class TC_OptionsPref3D
    {
        public static String tc = "uses Options menu, Preferences Sub-menu, twistie, 3D Map";
        public static String machinestatereq = "Started, Added Layer 3D Map";
        public static String parameters_csv_use = "parameters[0=Down presses within Preferences panel,1=x of 3D twistie,2=y of 3D twistie]";
        public static String classname = "TC_OptionsPref3D:";
        public static String compareimage = Launcher.logdirectory + "\\" + "test" + "\\" + "compareimage" + "\\" + classname.Replace(":", "_") + "end" + ".jpg";

        public void Invoke(String parameters_csv, LogTest lt)//AM: comma separated parms to customize keyboard or x,y parms
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + TC_OptionsPref3D.tc + "\r\n";
            Launcher.logcontent += classname + "Machine State Required: " + TC_OptionsPref3D.machinestatereq + "\r\n";
            Launcher.logcontent += classname + "Uses parameters_csv ([args3]): " + TC_OptionsPref3D.parameters_csv_use + "\r\n";
            Launcher.logcontent += classname + "Tests against " + TC_OptionsPref3D.compareimage + "\r\n";

            String[] parameters = parameters_csv.Split(',');

            //AM: find parent AutomationElement
            GetProcess gp = new GetProcess();
            String s = gp.GetTargetTitle();
            TC_Stop tc_s = new TC_Stop();
            Process p = tc_s.Get(s);
            Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";


            //AM: Find 1st control
            String test = "_Options";
            ObjectFinder of = new ObjectFinder();
            AutomationElement aefound = /*of.FindByName(p, test)*//*of.TestObjectsAll(test)*/of.TestObjectsAllFromProcess(p,test);
            Thread.Sleep(5000);
            Launcher.logcontent += classname + "aefound..NameProperty=" + aefound.GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n";

            if (aefound != null)
            {
                Launcher.logcontent += classname + "aefound=" + aefound.ToString() + " found!" + "\r\n";

                Clicker c = new Clicker();
                
                //AM: MoveToandClick
                c.ClickByClickablePoint(aefound, "Left");

                //AM: KeyBoardTo
                c.PressKey("Down", 1);
                c.PressKey("Enter", 1);
                c.PressKey("TAB", 1);
                int value;
                int.TryParse(parameters[0], out value);
                c.PressKey("Down", value);//AM:non-standard location, pick from argument

                //AM: Find 2nd exposed control
                Thread.Sleep(5000);
                test = "3D Map";
                AutomationElement aefound2 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(p,test);

                //AM: MoveToandClick
                int.TryParse(parameters[1], out value);
                int x = value;
                int.TryParse(parameters[2], out value);
                int y = value;
                c.ClickByClickablePointRelative(aefound2, "Left", x,y);

                //AM: KeyBoardTo
                c.PressKey("Down", 1);

                //AM: Find 3rd exposed control
                Thread.Sleep(5000);
                test = "Server URL";
                AutomationElement aefound3 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(p,test);

                //AM: MoveToandClick
                c.ClickByClickablePoint(aefound3, "Left");

                //AM: KeyBoardTo
                c.PressKey("Tab", 2);
                c.PressKey("Delete", 35);

                //AM: find text-or-value enabled  editable textfield
                Thread.Sleep(5000);
                test = "edit";
                AutomationElement aefound4 = of.TestObjectsAllFromElement(aefound3,test);
 
                //AM: KeyBoardTo
                c.SendKeyStringByChar(Launcher.ecurl);
                //c.SetValuePattern(aefound4, Launcher.ecurl);

                //AM: BEGIN----------how to do a test versus expectation
                ScreenCapture sc = new ScreenCapture();
                String file1 = sc.GetDesktopImage(TC_OptionsPref3D.classname.Replace(":", "_") + "end");
                CompareImage ci = new CompareImage();
                //LogTest lt = new LogTest();
                lt.LogTestResult("StartTest", classname/* + "\r\n" +*/, tc + "\r\n" + "run timestamp : " + Launcher.now, null, null, parameters);
                bool result = ci.compare(file1, compareimage);
                if (result)
                {
                    lt.LogTestResult("LogPass", null, null, null);
                }
                else
                {
                    lt.LogTestResult("LogError", null, "Image failed to compare with baseline image", null);
                }
                lt.LogTestResult("EndTest", null, null, null);
                lt.LogTestResult("ReportResults", null, null, null);
                //AM: END----------how to do a test versus expectation

                //AM: KeyBoardTo
                c.PressKey("Enter", 1);

            }
            else
            {
                Launcher.logcontent += classname + "aefound=" + aefound.ToString() + " NOT found?" + "\r\n";
            }

        }
    }
}
