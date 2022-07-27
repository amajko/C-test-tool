using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;
using KeyWord;
using System.Diagnostics;
using System.Windows.Automation;

namespace TestCase
{
    class TC_KeyWord
    {

        //comment

        public static String tc = "uses key words";
        //public static String machinestatereq = "Started, Added Layer 3D Map, Options-Preferences-3DMap set, Locator tab not clicked, Locations tab not clicked";
        public static String parameters_csv_use = "parameters[\r\n" +
            "0=keyword, --used in all\r\n" +
            "1=object name or automation id,--used in all, except LIST, LISTOBJECTS, SENDSTRING, CAPTURESCREEN\r\n" +
            "2=list item number or number key presses(0=holddown,-1=release,else exact number), --used in CLICKCHILD, KEY, CAPTURESCREEN, or 'right/left' click MOUSE, CLICKMOUSERELATIVETOLASTLOCATION\r\n" +
            "3=LocalizedControlType(none|button|edit|...), --used in CLICKITEMBYTYPE , --How many times to rotate the wheel #\r\n" +
            "4=ancestry of object(name or id) - semi-colon separated(Preferences;Preferences;PART_TreeView, e.g), --used in CLICKCHILD\r\n" +
            "5=x;y location relative to last mouse location - semi-colon separated(20;40, e.g), --used in CLICKMOUSERELATIVETOLASTLOCATION, MOUSE\r\n" +
            "6=text entry for textfield(lowercase only)] --used in SENDSTRING";
        public static String classname = "TC_KeyWord:";

        public void Do(String parameters_csv, LogTest lt)
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + tc + "\r\n";
            //Launcher.logcontent += classname + "Machine State Required: " + TC_LocatorLocation.machinestatereq + "\r\n";
            Launcher.logcontent += classname + "Uses parameters_csv ([args3]): " + parameters_csv_use + "\r\n";
            //Launcher.logcontent += classname + "Tests against " + TC_LocatorLocation.compareimage + "\r\n";

            String[] parameters = parameters_csv.Split(',');

            //AM: BEGIN----------how to do a test versus expectation
            //ScreenCapture sc = new ScreenCapture();
            //String file1 = sc.GetDesktopImage(TC_LocatorLocation.classname.Replace(":", "_") + "end");
            //CompareImage ci = new CompareImage();
            //LogTest lt = new LogTest();

            String[] steps = null;
            

            lt.LogTestResult("StartTest", classname/* + "\r\n" +*/, tc + "\r\n" + "run timestamp : " + Launcher.now, null, null, parameters);
            //bool result = ci.compare(file1, compareimage);
            bool result = true;
            //if (result)
            //{
            //    lt.LogTestResult("LogPass", null, null, null);
            //}
            //else
            //{
            //    lt.LogTestResult("LogError", null, "Image failed to compare with baseline image", null);
            //}
            //lt.LogTestResult("EndTest", null, null, null);
            //lt.LogTestResult("ReportResults", null, null, null);
            //AM: END----------how to do a test versus expectation

            String[] keywords = { "LIST", "LISTOBJECTS", "CLICKITEM", "CLICKITEMBYTYPE", "CLICKCHILD", "KEY", "CLICKMOUSERELATIVETOLASTLOCATION", "MOUSE", "SENDSTRING", "CAPTURESCREEN" };

            if (parameters[0].ToUpper() == "LIST")
            {
                for (int i = 0; i < keywords.Length; i++)
                {
                    Launcher.logcontent += classname + keywords[i] + "\r\n";
                }
            }

            if (parameters[0].ToUpper() == "LISTOBJECTS")
            {
                //AM: find parent AutomationElement
                GetProcess gp = new GetProcess();
                String s = gp.GetTargetTitle();
                TC_Stop tc_s = new TC_Stop();
                Process p = tc_s.Get(s);
                Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

                ObjectFinder of = new ObjectFinder();
                of.FindObjectsAllFromProcess(p);
            }

            if (parameters[0].ToUpper() == "CLICKITEM")
            {
                //AM: find parent AutomationElement
                GetProcess gp = new GetProcess();
                String s = gp.GetTargetTitle();
                TC_Stop tc_s = new TC_Stop();
                Process p = tc_s.Get(s);
                Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

                ObjectFinder of = new ObjectFinder();

                bool control = true;
                bool enabled = true;
                AutomationElement aefound = of.TestObjectsAllFromProcess(p, parameters[1], control, !enabled);
                //AM: make one bool false so that we use Walker istead of finding first object

                Clicker c = new Clicker();
                c.ClickByClickablePoint(aefound, "Left");
            }

            if (parameters[0].ToUpper() == "CLICKITEMBYTYPE")
            {
                //AM: find parent AutomationElement
                GetProcess gp = new GetProcess();
                String s = gp.GetTargetTitle();
                TC_Stop tc_s = new TC_Stop();
                Process p = tc_s.Get(s);
                Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

                ObjectFinder of = new ObjectFinder();

                bool control = true;
                bool enabled = true;
                AutomationElement aefound = null;
                try
                {
                    aefound = of.TestObjectsAllFromProcessByType(p, parameters[1], parameters[3], control, !enabled);
                }
                catch (Exception)
                {
                    result = false;
                }
                //AM: make one bool false so that we use Walker istead of finding first object
                if (result)
                {
                    Clicker c = new Clicker();
                    c.ClickByClickablePoint(aefound, "Left");
                }
            }

            if (parameters[0].ToUpper() == "CLICKCHILD")
            {
                //AM: find parent AutomationElement
                GetProcess gp = new GetProcess();
                String s = gp.GetTargetTitle();
                TC_Stop tc_s = new TC_Stop();
                Process p = tc_s.Get(s);
                Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

                ObjectFinder of = new ObjectFinder();

                String[] parameters_4 = parameters[4].Split(';');

                bool control = true;
                bool enabled = true;

                Console.WriteLine("AutomationElement aefound0 = of.TestObjectsAllFromProcess(p, parameters_4[0], control, !enabled);//AM: find first descendant");
                AutomationElement aefound0 = of.TestObjectsAllFromProcess(p, parameters_4[0], control, !enabled);//AM: find first descendant
                AutomationElement aefound = null;

                if (parameters_4.Length > 1)
                {
                    for (int i = 1; i < parameters_4.Length; i++)
                    {
                        Console.WriteLine("aefound = of.TestObjectsAllFromElement(aefound0, parameters_4[i], control, !enabled);//AM: iteratively find each next descendant from its parent");
                        aefound = of.TestObjectsAllFromElement(aefound0, parameters_4[i], control, !enabled);//AM: iteratively find each next descendant from its parent
                        aefound0 = aefound;
                    }
                }

                Console.WriteLine("aefound = of.TestObjectsAllFromElement(aefound0, parameters[1], control, !enabled);//AM: find object from last descendant ");
                int value;
                int.TryParse(parameters[2], out value);
                if (value >= 0)
                {
                    aefound = of.TestObjectsAllFromElement(aefound0, parameters[1], control, !enabled, value);//AM: find object from last descendant 
                }
                else
                {
                    aefound = of.TestObjectsAllFromElement(aefound0, parameters[1], control, !enabled);//AM: find object from last descendant 
                }

                Clicker c = new Clicker();
                c.ClickByClickablePoint(aefound, "Left");//AM: must get to ith item of that name 
            }

            if (parameters[0].ToUpper() == "KEY")
            {
                Clicker c = new Clicker();
                int value;
                int.TryParse(parameters[2], out value);
                c.PressKey(parameters[1], value);
            }

            if (parameters[0].ToUpper() == "CLICKMOUSERELATIVETOLASTLOCATION")//ClickMouseRelativeToLastLocation
            {
                //AM: check for xyfile, and get last mouse x.y
                GetProcess gp = new GetProcess();
                String xy_csv = gp.GetXY();
                String[] xy = xy_csv.Split(',');

                int lastvaluex;
                int.TryParse(xy[0], out lastvaluex);
                int lastvaluey;
                int.TryParse(xy[1], out lastvaluey);

                String[] parameters_5 = parameters[5].Split(';');

                int relvaluex;
                int.TryParse(parameters_5[0], out relvaluex);
                int relvaluey;
                int.TryParse(parameters_5[1], out relvaluey);



                Clicker c = new Clicker();
                //c.ClickMouseLoc("Left", lastvaluex + relvaluex, lastvaluey + relvaluey);
                c.ClickMouseLoc(parameters[2], lastvaluex + relvaluex, lastvaluey + relvaluey);
            }

            if (parameters[0].ToUpper() == "MOUSE")//ClickMouseRelativeToLastLocation
            {
                //AM: check for xyfile, and get last mouse x.y
                GetProcess gp = new GetProcess();
                String xy_csv = gp.GetXY();
                String[] xy = xy_csv.Split(',');

                int lastvaluex;
                int.TryParse(xy[0], out lastvaluex);
                int lastvaluey;
                int.TryParse(xy[1], out lastvaluey);

                String[] parameters_5 = parameters[5].Split(';');

                int relvaluex;
                int.TryParse(parameters_5[0], out relvaluex);
                int relvaluey;
                int.TryParse(parameters_5[1], out relvaluey);



                Clicker c = new Clicker();
                if (parameters[1].ToUpper() == "CLICK")
                {
                    c.ClickMouseLoc(parameters[2], lastvaluex + relvaluex, lastvaluey + relvaluey);
                }
                if (parameters[1].ToUpper() == "DRAG")
                {
                    c.DragMouse(parameters[2], lastvaluex, lastvaluey, lastvaluex + relvaluex, lastvaluey + relvaluey);
                }

                if (parameters[1].ToUpper() == "DOUBLECLICK")
                {
                    c.DoubleClickMouseLoc(parameters[2], lastvaluex + relvaluex, lastvaluey + relvaluey);
                }

                //
                //SMV:WHEEL
                if (parameters[1].ToUpper() == "WHEEL")
                {
                    double wheelNum = Convert.ToDouble(parameters[3]);
                    c.WheelMouse(parameters[2], wheelNum);
                }
                //
            }

            if (parameters[0].ToUpper() == "SENDSTRING")
            {

                Clicker c = new Clicker();
                //AM: KeyBoardTo
                c.SendKeyStringByChar(parameters[6]);
            }

            if (parameters[0].ToUpper() == "CAPTURESCREEN")
            {
                //AM: BEGIN----------how to do a test versus expectation
                ScreenCapture sc = new ScreenCapture();
                String file1 = sc.GetDesktopImage(TC_KeyWord.classname.Replace(":", "_") + "end", parameters[2]);
                //CompareImage ci = new CompareImage();
                //LogTest lt = new LogTest();
                //lt.LogTestResult("StartTest", classname + "\r\n" + tc + "\r\n" + "run timestamp=" + Launcher.now, null, null);
                //bool result = ci.compare(file1, compareimage);
                //if (result)
                //{
                //    lt.LogTestResult("LogPass", null, null, null);
                //}
                //else
                //{
                //    lt.LogTestResult("LogError", null, "Image failed to compare with baseline image", null);
                //}
                //lt.LogTestResult("EndTest", null, null, null);
                //lt.LogTestResult("ReportResults", null, null, null);
                //AM: END----------how to do a test versus expectation
            }



            //AM: BEGIN----------how to do a test versus expectation
            //ScreenCapture sc = new ScreenCapture();
            //String file1 = sc.GetDesktopImage(TC_LocatorLocation.classname.Replace(":", "_") + "end");
            //CompareImage ci = new CompareImage();
            ////LogTest lt = new LogTest();
            //lt.LogTestResult("StartTest", classname + "\r\n" + tc + "\r\n" + "run timestamp=" + Launcher.now, null, null);
            //bool result = ci.compare(file1, compareimage);
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

        }
    }

}
