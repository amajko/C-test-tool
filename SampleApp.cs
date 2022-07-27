using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using Microsoft.Test;
using Microsoft.Test.ApplicationControl;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;


namespace Examples
{
    class SampleApp
        //JZ
    {
        static SynchronizedInputPattern syncInputPattern;
        static AutomationElement listElement, textblockElement, buttonElement;
        static int loopCount = 0;
        static String s = String.Empty;

         public static void Start()
        {
            s = @"D:\Bin_SampleApp\Debug\SampleApp.exe";

            RunSampleApp();//also closes the app
            //return;//kicks outof this method
        }
                
        static public void RunSampleApp()
        //called from SampleApp
        {
            AutomationElement aeForm = StartApplication(s, null);

            // Using "ById" but actually on controls' "Name" attribute 
            listElement = AutomationUtilities.FindElementsById(aeForm, "MyList")[0];
            buttonElement = AutomationUtilities.FindElementsById(aeForm, "b")[0];
            textblockElement = AutomationUtilities.FindElementsById(buttonElement, "tbl")[0];
            HookEventsToElement(buttonElement);

            if ((bool)buttonElement.GetCurrentPropertyValue(AutomationElement.IsSynchronizedInputPatternAvailableProperty))
            {
                syncInputPattern = (SynchronizedInputPattern)buttonElement.GetCurrentPattern(SynchronizedInputPattern.Pattern);

                Console.WriteLine("****** Goal is to 'Click the Button' ******");
                GuaranteedMouseClick(buttonElement);
            }

            Thread.Sleep(12000);
            AutomationElement aeFileMenu = null;
            aeFileMenu = aeForm.FindFirst(TreeScope.Descendants,
              new PropertyCondition(AutomationElement.NameProperty, "File"));
            if (aeFileMenu == null)
                throw new Exception("Could not find 'File' menu");
            else
                Console.WriteLine("Got 'File' menu");
            Thread.Sleep(1000);

            Console.WriteLine("\nClicking on 'File' menu to expose submenu items.");
            Thread.Sleep(2000);

            ExpandCollapsePattern expClickFileMenu =
                     (ExpandCollapsePattern)aeFileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            expClickFileMenu.Expand();
            Thread.Sleep(10000);  // Visual help - let user see the "File" menu is fully expanded

            // Can remove the above [Thread.Sleep(10000);] and use the following quick loop to get to submenu "File-Exit"
            // However, user will not be able to see parent menu "File" is being expanded, which is a helpful cue
            int numWait = 0;
            AutomationElement aeFileExitMenu;
            do
            {
                Thread.Sleep(10);
                Console.WriteLine("\nLooking for submenu File-Exit. . . ");

                aeFileExitMenu = aeForm.FindFirst(TreeScope.Descendants,
                  new PropertyCondition(AutomationElement.NameProperty, "Exit"));

                ++numWait;
            }
            while (aeFileExitMenu == null && numWait < 100);

            if (aeFileExitMenu == null)
                throw new Exception("Could not find submenu 'File-Exit'");
            else
                Console.WriteLine("Got submenu 'File-Exit'");
            Thread.Sleep(1000);

            Console.WriteLine("\nClicking 'File-Exit' to close 'SampleApp.exe' the app-under-test");
            InvokePattern ipFileExitMenu =
              (InvokePattern)aeFileExitMenu.GetCurrentPattern(InvokePattern.Pattern);
            ipFileExitMenu.Invoke();

            // Close myselfe
            Console.WriteLine("\r\n");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("CLOSING MYSELF (this UI Automation tester) IN 10 SECONDS");
            Console.WriteLine("--------------------------------------------------------");
            Thread.Sleep(10000);
        }

        static public AutomationElement StartApplication(string appPath, string arguments)
        //caled from RunSampleApp
        {
            int MAXTIME = 5000; //   Total length in milliseconds to wait for the application to start
            int TIMEWAIT = 100; //   Timespan to wait till trying to find the window

            Process process = null;
            // Library.ValidateArgumentNonNull(appPath, "appPath");
            ProcessStartInfo psi = new ProcessStartInfo();

            process = new Process();
            psi.FileName = appPath;

            if (arguments != null)
            {
                psi.Arguments = arguments;
            }

            // UIVerifyLogger.LogComment("Starting({0})", appPath);
            process.StartInfo = psi;
            process.Start();

            // UIVerifyLogger.MonitorProcess(process);

            int runningTime = 0;
            while (process.MainWindowHandle.Equals(IntPtr.Zero))
            {
                if (runningTime > MAXTIME)
                    throw new Exception("Could not find " + appPath);

                Thread.Sleep(TIMEWAIT);
                runningTime += TIMEWAIT;

                process.Refresh();
            }

            // UIVerifyLogger.LogComment("{0} started", appPath);
            // UIVerifyLogger.LogComment("Obtained an AutomationElement for {0}", appPath);
            return AutomationElement.FromHandle(process.MainWindowHandle);
        }

 
        static void HookEventsToElement(AutomationElement element)
        //called from RunSampleApp
        {
            AutomationEventHandler handler = new AutomationEventHandler(SynchronizedInputHandler);

            // subscribe to the InputReachedElement event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputReachedTargetEvent, element, TreeScope.Element, handler);

            // subscribe to the InputReachedOtherElement event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputReachedOtherElementEvent, element, TreeScope.Element, handler);

            // subscribe to the InputDiscarded event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputDiscardedEvent, element, TreeScope.Element, handler);
        }

        static void SynchronizedInputHandler(object src, AutomationEventArgs e)
        //instantiated from HookToElement
        {
            switch (e.EventId.ProgrammaticName)
            {
                case "SynchronizedInputPatternIdentifiers.InputDiscardedEvent": // handle the case when the input is cancelled
                    Console.WriteLine(" ---- Input was Discarded - Try Again clicking On TextBox nested in Button");
                    GuaranteedMouseClick(textblockElement);
                    break;

                case "SynchronizedInputPatternIdentifiers.InputReachedOtherElementEvent":
                    Console.WriteLine(" ---- Input Reached Element other than Target - Try Again clicking on Button");
                    GuaranteedMouseClick(buttonElement);
                    break;

                case "SynchronizedInputPatternIdentifiers.InputReachedTargetEvent":
                    Console.WriteLine(" ---- SUCCESS: Input Reached Target");
                    break;
            }
        }

        static void GuaranteedMouseClick(AutomationElement buttonElement)
        //called from RunSampleApp and Main."EarthScape"
        {
            syncInputPattern.StartListening(SynchronizedInputType.MouseLeftButtonDown);
            Thread.Sleep(5000); //Sleeping so that it is easier to see the actions happening :) 

            switch (loopCount++)
            {
                case 0: //Click on someother element
                    Console.WriteLine(" -- Click on the List");
                    Helpers.MoveToAndClick(listElement);
                    break;

                case 1:
                    Console.WriteLine(" -- Click on the Textbox inside Button");
                    Helpers.MoveToAndClick(textblockElement);
                    break;

                case 2:
                    Console.WriteLine(" -- Click on the Button");
                    Helpers.MoveToAndClick(buttonElement);
                    break;
            }
        }

    }
}
