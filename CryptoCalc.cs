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
using IntegrationTest;

namespace Examples
{
    class CryptoCalc
    //JZ
    {
        static String s = String.Empty;
        public static void Start()
        {
            s = @"D:\Bin_CryptoCalc\Debug\CryptoCalc.exe";
            String t = "CryptoCalc";
            if (t == "CryptoCalc")
            {
                int sleepy = 45000;
                Process p = null;
                string targetTitle = "";

                /*Process*/
                p = Process.Start(s);
                int ct = 0;
                do
                {
                    Console.WriteLine(ct + ": Launching process under test. . . ");
                    ++ct;
                    Thread.Sleep(1000);//expanded to 1 second wait
                }
                while (p == null && ct < 50);

                if (p == null)
                    throw new Exception("Failed to launch process under test!");
                else
                    Console.WriteLine("Launched process under test!");

                p.WaitForInputIdle(); // Wait for target executable to be idle, i.e. wait until it has entered its UI message loop
                /*string */
                targetTitle = p.MainWindowTitle; // Get Title in run-time and pass it as param.  "NAVTEQ Earthscap 0.0.3" can be "0.0.4" tomorrow.

                Thread.Sleep(sleepy);  // Did p.WaitForInputIdle();//AM: may need to wait anyway, cause WaitforInputIdle releases before ES is fully available to user

                AutomationElement aeDesktop = AutomationElement.RootElement;
                if (aeDesktop == null)
                    throw new Exception("Unable to get Desktop");
                else
                    Console.WriteLine("Found Desktop\n");//never writes

                Console.WriteLine("RootElement: " + aeDesktop.GetType().ToString());
                Launcher.logcontent += "RootElement: " + aeDesktop.GetType().ToString() + "\r\n";

                // Section for each target under test
                // ==================================
                // CryptoCalc

                if (t == "CryptoCalc")
                {
                    AutomationElement aeForm = null; //aeForm is the application which has >= one Form
                    int numWaits = 0;
                    do
                    {
                        Console.WriteLine("Looking for CryptoCalc Application. . . ");
                        aeForm = aeDesktop.FindFirst(TreeScope.Children,
                          new PropertyCondition(AutomationElement.NameProperty, "CryptoCalc"));
                        ++numWaits;
                        Thread.Sleep(100);
                    } while (aeForm == null && numWaits < 50);

                    if (aeForm == null)
                        throw new Exception("Failed to find CryptoCalc Application main window!");
                    else
                        Console.WriteLine("Found CryptoCalc Application main window!");

                    Console.WriteLine("\nFinding all user controls");

                    // ---------------------------------------------------------------------------
                    // Get hold of UI controls - Button, TextBox, Radio button, Dropdownlist, Menu
                    // ---------------------------------------------------------------------------
                    Console.WriteLine("\r\n");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Get hold of all UI controls");
                    Console.WriteLine("---------------------------");
                    // Button
                    AutomationElement aeButton = aeForm.FindFirst(TreeScope.Children,
                      new PropertyCondition(AutomationElement.NameProperty, "Compute"));
                    if (aeButton == null)
                        throw new Exception("No Compute button");
                    else
                        Console.WriteLine("Got Compute button");

                    // TextBox collection to individual TextBoxes...
                    AutomationElementCollection aeAllTextBoxes = null;
                    aeAllTextBoxes = aeForm.FindAll(TreeScope.Children,
                          new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                    if (aeAllTextBoxes == null)
                        throw new Exception("No textboxes collection");
                    else
                        Console.WriteLine("Got a textbox collection. . .");
                    AutomationElement aeTextBox1 = aeAllTextBoxes[1]; // Caution: implied index order = load order
                    AutomationElement aeTextBox0 = aeAllTextBoxes[0]; // TextBox under "Compute" button
                    if (aeTextBox1 == null || aeTextBox0 == null)
                        throw new Exception("TextBox1 or TextBox0 not found");
                    else
                        Console.WriteLine("Got TextBox1 and TextBox0");

                    // Radio button
                    AutomationElement aeRadioButton1 = aeForm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "MD5 Hash"));
                    AutomationElement aeRadioButton2 = aeForm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "SHA1 Hash"));
                    AutomationElement aeRadioButton3 = aeForm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "DES Encrypt"));
                    if (aeRadioButton1 == null || aeRadioButton2 == null || aeRadioButton3 == null)
                        throw new Exception("RadioButton problem!");
                    else
                        Console.WriteLine("Got 3 RadioButtons");

                    /* ----------------------------------------------------------------------------------------------------
                    // ****************************
                    // Wrong way of find menu items
                    // ****************************
                    AutomationElement aeMenu1 = aeForm.FindFirst(TreeScope.Children,
                      new PropertyCondition(AutomationElement.NameProperty, "&File"));  // aeMenu1=null
                    AutomationElement aeMenu2 = aeForm.FindFirst(TreeScope.Children,
                      new PropertyCondition(AutomationElement.NameProperty, "File"));   // aeMenu2=null

                    // **************************************************************************************************************
                    // The following is to find menu "File" in ANY DESKTOP APPLICATION, e.g. Microsoft Word, Visual Studio 2010, etc.
                    // **************************************************************************************************************
                    Condition conditions = new AndCondition(
                      new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                      new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                      new PropertyCondition(AutomationElement.NameProperty, "File")
                      );
                    AutomationElement ae = aeDesktop.FindFirst(TreeScope.Descendants, conditions);    // what is in "ae"? 

                    // The following is to find menu "Opeions" in ANY DESKTOP APPLICATION, e.g. Work, Visual Studio 2010, etc.
                    // *** NOTE ***  You will get error if no desktop applications has menu "Options" in display.
                    //               If Earthscape menu item "Options" does NOT have [name="Options"] defined, then you get the error too.
                    Condition conditions2 = new AndCondition(
                      new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                      new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                      new PropertyCondition(AutomationElement.NameProperty, "Options")
                      );
                    // AutomationElement ae2 = aeDesktop.FindFirst(TreeScope.Descendants, conditions2);  // Generates ERROR - causing problem for this TestaGUI
                    ---------------------------------------------------------------------------------------------------- */

                    // ----------------------------
                    // Operate on found UI controls
                    // ----------------------------
                    Console.WriteLine("\r\n");
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Act on found UI controls");
                    Console.WriteLine("------------------------");

                    // TEXTBOX - WRITE to TextBox for calculation
                    Console.WriteLine("\nSetting input to 'Hello!'");
                    ValuePattern vpTextBox0 = (ValuePattern)aeTextBox0.GetCurrentPattern(ValuePattern.Pattern);
                    vpTextBox0.SetValue("Hello!");

                    // RADIO BUTTON - SELECT Radio buttons
                    Console.WriteLine("Selecting 'DES Encrypt' ");
                    SelectionItemPattern ipSelectRadioButton3 =
                      (SelectionItemPattern)aeRadioButton3.GetCurrentPattern(SelectionItemPattern.Pattern);
                    ipSelectRadioButton3.Select();

                    // BUTTON - CLICK Button
                    Console.WriteLine("Clicking on 'Compute' button");
                    InvokePattern ipClickButton1 =
                      (InvokePattern)aeButton.GetCurrentPattern(InvokePattern.Pattern);
                    ipClickButton1.Invoke();
                    Thread.Sleep(1500);

                    // CHECK - Check ACTUAL behavior(value) against EXPECTED behavior
                    Console.WriteLine("\r\nChecking TextBox0 for '91-1E-84-41-67-4B-FF-8F'");
                    string result =
                      (string)aeTextBox1.GetCurrentPropertyValue(ValuePattern.ValueProperty);

                    if (result == "91-1E-84-41-67-4B-FF-8F")
                    {
                        Console.WriteLine("Found actual test result value.");
                        Console.WriteLine("Test scenario: Pass.\n");
                    }
                    else
                    {
                        Console.WriteLine("Did not find actual test result value, or checking failed.");
                        Console.WriteLine("\nTest scenario: *FAIL*");
                    }

                    // MENU - Top-level can Expand but NOT Invoke in any circumstances
                    //        Submenu   can Invoke but only when parent menu is in Expand mode

                    // Console.WriteLine("\nClicking on File-Exit item in 5 seconds");

                    // --------------------------------
                    // *** Do NOT delete next Sleep ***
                    // --------------------------------
                    Thread.Sleep(5000); // *** NEED THIS LINE! ***

                    AutomationElement aeFileMenu = null;
                    aeFileMenu = aeForm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "File"));
                    if (aeFileMenu == null)
                        throw new Exception("Could not find File menu");
                    else
                        Console.WriteLine("Got File menu");
                    Thread.Sleep(1000);

                    Console.WriteLine("\nClicking on 'File' menu to expose submenu items.");
                    Thread.Sleep(2000);
                    ExpandCollapsePattern expClickFileMenu =
                      (ExpandCollapsePattern)aeFileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                    expClickFileMenu.Expand();
                    Thread.Sleep(10000);  // Visual help - let user see the "File" menu is fully expanded

                    // Can remove the above [Thread.Sleep(10000);] and use the following quick loop to get to submenu "File-Exit"
                    // However, user will not be able to see parent menu "File" is being expanded, a helpful cue
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
                        throw new Exception("Could not find submenu [File-Exit]");
                    else
                        Console.WriteLine("Got submenu 'File-Exit'");
                    Thread.Sleep(1000);

                    InvokePattern ipFileExitMenu =
                      (InvokePattern)aeFileExitMenu.GetCurrentPattern(InvokePattern.Pattern);
                    ipFileExitMenu.Invoke();

                    // Close myselfe
                    Console.WriteLine("\r\n");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("CLOSING MYSELF (this automation tester) IN 10 SECONDS");
                    Console.WriteLine("-----------------------------------------------------");
                    Thread.Sleep(10000);
                    p.CloseMainWindow();
                    p.Close();

                    // Console.WriteLine("\nEnd automation test run\n");
                    // Console.ReadLine();
                }//if args[0] == <program>
                //--end this application (CryptoCalc)

            }
        }
    }
}
