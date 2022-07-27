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
    class TC_LocatorLocation
    {
        public static String tc = "uses Locators layer, gets a location";
        public static String machinestatereq = "Started, Added Layer 3D Map, Options-Preferences-3DMap set, Locator tab not clicked, Locations tab not clicked";
        public static String parameters_csv_use = "parameters[0=x of location bounding box,1=y of location bounding box,2=radius in km,3=x location of Locator tab,4=y location of Locator tab,5=tabs to reach latitude field]";
        public static String classname = "TC_LocatorLocation:";
        public static String compareimage = Launcher.logdirectory + "\\" + "test" + "\\" + "compareimage" + "\\" + classname.Replace(":", "_") + "end" + ".jpg";

        public void Invoke(String parameters_csv, LogTest lt)//AM: comma separated parms to customize keyboard or x,y parms
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + TC_LocatorLocation.tc + "\r\n";
            Launcher.logcontent += classname + "Machine State Required: " + TC_LocatorLocation.machinestatereq + "\r\n";
            Launcher.logcontent += classname + "Uses parameters_csv ([args3]): " + TC_LocatorLocation.parameters_csv_use + "\r\n";
            Launcher.logcontent += classname + "Tests against " + TC_LocatorLocation.compareimage + "\r\n";

            String[] parameters = parameters_csv.Split(',');

            //AM: find parent AutomationElement
            GetProcess gp = new GetProcess();
            String s = gp.GetTargetTitle();
            TC_Stop tc_s = new TC_Stop();
            Process p = tc_s.Get(s);
            Launcher.logcontent += classname + "p.MainWindowTitle=" + p.MainWindowTitle + "\r\n";

            bool control = true;
            bool enabled = true;

            //AM: Find 1st control
            String test = "_Help";//AM: Locator_PanelLocatorView not visible at top of UIAVerify
            ObjectFinder of = new ObjectFinder();
            //AM: belo for test purposes
            //AutomationElementCollection aec = of.FindObjectsAllFromProcess(p);
            AutomationElement aefound = /*of.FindByName(p, test)*//*of.TestObjectsAll(test)*/of.TestObjectsAllFromProcess(p,test,!control,enabled);
                            //AM: must find by control type: 2 'Location' s: 'tab' or 'view' item
                            // make one bool false so that we use Walker istead of finding first object
            Thread.Sleep(5000);
            //Launcher.logcontent += classname + "aefound...NameProperty=" + aefound.GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n";

            if (aefound != null)
            {
                Launcher.logcontent += classname + "aefound=" + aefound.GetCurrentPropertyValue(AutomationElement.NameProperty) + "-" + aefound.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty) + " found!" + "\r\n";

                Clicker c = new Clicker();
                
                //AM: MoveToandClick
                int value;
                int.TryParse(parameters[3], out value);
                int x = value;
                int.TryParse(parameters[4], out value);
                int y = value;
                //c.ClickByClickablePoint(aefound, "Left");
                c.ClickByClickablePointRelative(aefound, "Left", x, y);

                //AM: find intervening parent, FindAll(descendants) finds only immediate children? (in this implementation?)..
                //AM: also, test...ByProcess when objects have been exposed thru mouse cliks works better than Test...FromElement
                Thread.Sleep(5000);
                test = "tabCtrl";
                //AM: not nec (doesnt find on amajkostrvr1)
                //AutomationElement aefound1b = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound*/p, test, control, !enabled);//AM: visible at top thru UIAVerify

                //AM: Find 2nd exposed control
                Thread.Sleep(5000);
                test = "Location";
                AutomationElement aefound2 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound1b*/p, test, control, !enabled);//AM: visible at top thru UIAVerify

                //AM: MoveToandCLick
                c.ClickByClickablePoint(aefound2, "Left");

                //AM: Find 3rd exposed control
                //AM: not needed: well, needed for amajko4strvr1
                Thread.Sleep(5000);
                test = "Latitude";
                AutomationElement aefound3 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound2*/p, test, control, !enabled);

                //AM: MoveToandCLick
                c.ClickByClickablePoint(aefound3, "Left");

                //AM: find edit control and edit
                //Thread.Sleep(5000);
                //test = "edit";
                //AutomationElement aefound3b = of.TestObjectsAllFromProcess(/*aefound3*/p, test, control, !enabled);
                // c.SetValuePattern(aefound3b, parameters[0]);

                //AM: KeyBoardTo
                int.TryParse(parameters[5], out value);
                c.PressKey("TAB", value);
                c.SendKeyStringByChar(parameters[0]);

                //AM: Find 4th exposed control
                //AM: not needed
                //Thread.Sleep(5000);
                //test = "Longitude";
                //AutomationElement aefound4 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound2*/p, test, control, !enabled);

                //AM: find edit control and edit
                //Thread.Sleep(5000);
                //test = "edit";
                //AutomationElement aefound4b = of.TestObjectsAllFromProcess(/*aefound4*/p, test, control, !enabled);
                //c.SetValuePattern(aefound4b, parameters[1]);

                //AM: KeyBoardTo
                c.PressKey("TAB", 1);
                c.SendKeyStringByChar(parameters[1]);

                //AM: Find 5th exposed control
                //AM: not needed
                //Thread.Sleep(5000);
                //test = "Radius (km)";
                //AutomationElement aefound5 = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound2*/p, test, control, !enabled);

                //AM: find edit control and edit
                //Thread.Sleep(5000);
                //test = "edit";
                //AutomationElement aefound5b = of.TestObjectsAllFromProcess(/*aefound5*/p, test, control, !enabled);
                //c.SetValuePattern(aefound5b, parameters[2]);

                //AM: KeyBoardTo
                c.PressKey("TAB", 1);
                c.SendKeyStringByChar(parameters[2]);

                //AM: Find "Go" button
                //Thread.Sleep(5000);
                //test = "Go";
                //AutomationElement aefoundgo = of.TestObjectsAllFromProcess(/*aefound2*/p, test, control, !enabled);

                //AM: MoveToandClick
                //c.ClickByClickablePoint(aefound, "Left");

                //AM: KeyBoardTo
                c.PressKey("TAB", 1);
                c.PressKey("Enter", 1);

                //AM:wait for response of bb to client
                Thread.Sleep(30000);

                //AM: BEGIN----------how to do a test versus expectation
                ScreenCapture sc = new ScreenCapture();
                String file1 = sc.GetDesktopImage(TC_LocatorLocation.classname.Replace(":", "_") + "end");
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

                //AM:***begin reverse exposures of panels so machine state is returned to beginning
                Thread.Sleep(5000);
                test = "Region";
                AutomationElement aefoundret = /*of.FindByName(p, test)*/of.TestObjectsAllFromProcess(/*aefound1b*/p, test, control, !enabled);//AM: visible at top thru UIAVerify

                //AM: MoveToandCLick
                c.ClickByClickablePoint(aefoundret, "Left");

                //AM:MoveToandClick
                //AM: prev tab from "Locator"
                c.ClickByClickablePointRelative(aefound, "Left", x-75, y);

                //AM:***end reverse exposures ...

            }
            else
            {
                Launcher.logcontent += classname + "aefound=" + aefound.ToString() + " NOT found?" + "\r\n";
            }

        }
    }
}
