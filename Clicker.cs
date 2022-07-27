using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;
using System.Windows.Input;
//using Microsoft.VisualBasic;
//using System.Windows.Forms;
using WindowsInput;




namespace IntegrationTest
{
    class Clicker
    {
        static String classname = "Clicker:";
        Key k;
        VirtualKeyCode vkc;

        static SynchronizedInputPattern syncInputPattern;
        static AutomationElement listElement, textblockElement, buttonElement;

        public void ClickByHook(AutomationElement aefound, String button)
        //by JZ
        {
            HookEventsToElement(aefound);
            if ((bool)aefound.GetCurrentPropertyValue(AutomationElement.IsSynchronizedInputPatternAvailableProperty))
            {
                syncInputPattern = (SynchronizedInputPattern)aefound.GetCurrentPattern(SynchronizedInputPattern.Pattern);
                GuaranteedMouseClick(aefound, "Left");
            }
        }

        static void HookEventsToElement(AutomationElement element)
        {
            AutomationEventHandler handler = new AutomationEventHandler(SynchronizedInputHandler);

            //JZ: subscribe to the InputReachedElement event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputReachedTargetEvent, element, TreeScope.Element, handler);

            //JZ: subscribe to the InputReachedOtherElement event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputReachedOtherElementEvent, element, TreeScope.Element, handler);

            //JZ; subscribe to the InputDiscarded event
            Automation.AddAutomationEventHandler(SynchronizedInputPattern.InputDiscardedEvent, element, TreeScope.Element, handler);
        }

        static void SynchronizedInputHandler(object src, AutomationEventArgs e)
        {
            switch (e.EventId.ProgrammaticName)
            {
                case "SynchronizedInputPatternIdentifiers.InputDiscardedEvent": //JZ: handle the case when the input is cancelled
                    Console.WriteLine(" ---- Input was Discarded - Try Again clicking On TextBox nested in Button");
                    //GuaranteedMouseClick(textblockElement);
                    break;

                case "SynchronizedInputPatternIdentifiers.InputReachedOtherElementEvent":
                    Console.WriteLine(" ---- Input Reached Element other than Target - Try Again clicking on Button");
                    //GuaranteedMouseClick(buttonElement);
                    break;

                case "SynchronizedInputPatternIdentifiers.InputReachedTargetEvent":
                    Console.WriteLine(" ---- SUCCESS: Input Reached Target");
                    break;
            }
        }

        static void GuaranteedMouseClick(AutomationElement Element, String button)
        {
            MouseButton mb = new MouseButton();

            //AM: sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
            //AM: bool reverse = true;//change this to false when running on right-handed mouse
            if (Launcher.reverse)
            {
                if (button.ToUpper() == "LEFT")
                    button = "RIGHT";
                else if (button.ToUpper() == "RIGHT")
                    button = "LEFT";
            }

            if (button.ToUpper() == "LEFT")
            {
                syncInputPattern.StartListening(SynchronizedInputType.MouseLeftButtonDown);
                mb = MouseButton.Left;
                Launcher.logcontent += classname + "-GMC-" + mb.ToString() + " is set for option " + button + "\r\n";
            }

            if (button.ToUpper() == "RIGHT")
            {
                syncInputPattern.StartListening(SynchronizedInputType.MouseRightButtonDown);
                mb = MouseButton.Right;
                Launcher.logcontent += classname + "-GMC-" + mb.ToString() + " is set for option " + button + "\r\n";
            }

            Thread.Sleep(5000); //JZ: Sleeping so that it is easier to see the actions happening :) 

            MoveToAndClick(Element, mb);

        }

        public static void MoveToAndClick(AutomationElement element, MouseButton mouseButton)
        {
            System.Windows.Point winPoint = element.GetClickablePoint();
            System.Drawing.Point drawingPoint = new System.Drawing.Point((int)winPoint.X, (int)winPoint.Y);
            Microsoft.Test.Input.Mouse.MoveTo(drawingPoint);
            Microsoft.Test.Input.Mouse.Click(mouseButton);
        }


        public void ClickByClickablePoint(AutomationElement m, String button)
        {


            System.Windows.Point mpoint;
            if (m.TryGetClickablePoint(out mpoint))
            {
                Launcher.logcontent += classname + "-CCP-" + "I am clickable" + "\r\n";

                MouseDevice mouseDevice = Mouse.PrimaryDevice;
                MouseButtonEventArgs mouseButtonEventArgs = null;

                MouseButton mb = new MouseButton();


                //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
                //bool reverse = true;//change this to false when running on right-handed mouse
                if (Launcher.reverse)
                {
                    if (button.ToUpper() == "LEFT")
                        button = "RIGHT";
                    else if (button.ToUpper() == "RIGHT")
                        button = "LEFT";
                }


                if (button.ToUpper() == "LEFT")
                {
                    mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
                    Launcher.logcontent += classname + "-CCP-" + "I am left click" + "\r\n";
                    mb = MouseButton.Left;

                }
                else if (button.ToUpper() == "RIGHT")
                {
                    mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Right);
                    Launcher.logcontent += classname + "-CCP-" + "I am right click" + "\r\n";
                    mb = MouseButton.Right;
                }
                mouseButtonEventArgs.RoutedEvent = Mouse.MouseDownEvent;

                int x = (int)m.GetClickablePoint().X;
                int y = (int)m.GetClickablePoint().Y;

                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x, y));
                Console.WriteLine("...moved?...");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(mb.ToString() + "=" + mb.GetType());
                Microsoft.Test.Input.Mouse.Click(mb);
                Console.WriteLine("...clicked?...");
                System.Threading.Thread.Sleep(2000);

                Launcher.logcontent += classname + "-CCP-" + m.ToString() + " Clickable X,Y:" + x + ", " + y + "\r\n";
                //AM: write last x,y to info directory, thus allowing for NEXT click to be rel to it
                FileIO fio = new FileIO();
                fio.Write(Launcher.xyfile, x + "," + y, "info");
                Sleeper sleep = new Sleeper();
                sleep.Sleep(2000);

            }

            else
            {

                //Assert.Fail();

            }

        }//end functiom

        public void ClickByClickablePointRelative(AutomationElement m, String button, int xrel, int yrel)
        {


            System.Windows.Point mpoint;
            if (m.TryGetClickablePoint(out mpoint))
            {
                Launcher.logcontent += classname + "-CCPR-" + "I am clickable" + "\r\n";

                MouseDevice mouseDevice = Mouse.PrimaryDevice;
                MouseButtonEventArgs mouseButtonEventArgs = null;

                MouseButton mb = new MouseButton();


                //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
                //bool reverse = true;//change this to false when running on right-handed mouse
                if (Launcher.reverse)
                {
                    if (button.ToUpper() == "LEFT")
                        button = "RIGHT";
                    else if (button.ToUpper() == "RIGHT")
                        button = "LEFT";
                }


                if (button.ToUpper() == "LEFT")
                {
                    mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
                    Launcher.logcontent += classname + "-CCPR-" + "I am left click" + "\r\n";
                    mb = MouseButton.Left;

                }
                else if (button.ToUpper() == "RIGHT")
                {
                    mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Right);
                    Launcher.logcontent += classname + "-CCPR-" + "I am right click" + "\r\n";
                    mb = MouseButton.Right;
                }
                mouseButtonEventArgs.RoutedEvent = Mouse.MouseDownEvent;

                int x = (int)m.GetClickablePoint().X;
                int y = (int)m.GetClickablePoint().Y;

                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x + xrel, y + yrel));
                Console.WriteLine("...moved?...");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(mb.ToString() + "=" + mb.GetType());
                Microsoft.Test.Input.Mouse.Click(mb);
                Console.WriteLine("...clicked?...");
                System.Threading.Thread.Sleep(2000);

                Launcher.logcontent += classname + "-CCPR-" + m.ToString() + " Clickable Xrel,Yrel:" + (x + xrel) + ", " + (y + yrel) + "\r\n";

            }

            else
            {

                //Assert.Fail();

            }

        }//end functiom

        public void PressKey(String key, int count)
        {


            switch (key.ToUpper())
            {
                case "DOWN":
                    k = Key.Down;
                    break;

                case "UP":
                    k = Key.Up;
                    break;

                case "ENTER":
                    k = Key.Enter;
                    break;

                case "TAB":
                    k = Key.Tab;
                    break;

                case "DELETE":
                    k = Key.Delete;
                    break;

                case "CTRL":
                    k = Key.LeftCtrl;
                    vkc = VirtualKeyCode.LCONTROL;
                    break;

                //AM: these keys may be held down in junction iwth other keys as well as single-pressed alone,
                case "ALT":
                    k = Key.RightAlt;
                    vkc = VirtualKeyCode.RMENU;//right-alt
                    break;

                case "S":
                    k = Key.S;  //presses the 'S' key
                    break;

                case "A":
                    k = Key.A;  //presses the 'S' key
                    break;

            }

            if (count != 0 && count != -1)
            {
                for (int i = 1; i <= count; i++)
                {
                    Microsoft.Test.Input.Keyboard.Press(k);
                    Launcher.logcontent += classname + "-PK-" + i + ":" + key + " ";
                    if (key != "delete" && key != "DELETE" && key != "Delete")
                    {
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                if (count == 0)//AM: hold down key and dont release
                {
                    InputSimulator.SimulateKeyDown(vkc);
                }
                if (count == -1)//AM: releasekey
                {
                    InputSimulator.SimulateKeyUp(vkc);
                }
            }

            Launcher.logcontent += /*classname + "-PK-" +*/ "\r\n";
        }//function

        public void SendKeyString(String keys)
        {
            Microsoft.VisualBasic.Devices.Keyboard kb = new Microsoft.VisualBasic.Devices.Keyboard();
            //kb.SendKeys(keys);//AM: fails, object doesnt handle input: must use SendWait?
            /*
             * Fatal Error: System.InvalidOperationException: SendKeys cannot run inside this a
pplication because the application is not handling Windows messages.  Either cha
nge the application to handle messages, or use the SendKeys.SendWait method.
at System.Windows.Forms.SendKeys.Send(String keys, Control control, Boolean w
ait)
at System.Windows.Forms.SendKeys.Send(String keys)*/
            System.Windows.Forms.SendKeys.SendWait(keys);//AM: fails (sends some of the string but window closes too fast)
            Launcher.logcontent += classname + "-SKS-" + keys + "\r\n";
            Thread.Sleep(5000);
        }

        public void SendKeyStringByChar(String keys)
        {
            Microsoft.VisualBasic.Devices.Keyboard kb = new Microsoft.VisualBasic.Devices.Keyboard();
            //kb.SendKeys(keys);//AM: fails, object doesnt handle input: must use SendWait?
            /*
             * Fatal Error: System.InvalidOperationException: SendKeys cannot run inside this a
pplication because the application is not handling Windows messages.  Either cha
nge the application to handle messages, or use the SendKeys.SendWait method.
at System.Windows.Forms.SendKeys.Send(String keys, Control control, Boolean w
ait)
at System.Windows.Forms.SendKeys.Send(String keys)*/
            //System.Windows.Forms.SendKeys.SendWait(keys);//AM: fails (sends some of the string but window closes too fast); 
            //probly must esp special chars like ":" and send as "{:}"
            if (true)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Char c = keys.ElementAt(i);
                    Char[] d = { c };
                    String s = null;

                    s = new String(d);
                    Char c2 = s.ElementAt(0);
                    if (/*d[0] == ':' || */d[0] == '+' || d[0] == '^' | d[0] == '%' || d[0] == '~' || d[0] == '(' || d[0] == ')' || d[0] == '[' || d[0] == ']')
                    //AM: The plus sign (+), caret (^), percent sign (%), tilde (~), and parentheses () have special meanings to SendKeys. 
                    //To specify one of these characters, enclose it within braces ({}). For example, to specify the plus sign, use "{+}". 
                    //AM: To specify brace characters, use "{{}" and "{}}". Brackets ([ ]) 
                    {
                        //d[1] = d[0];
                        //d[0] = '{';
                        //d[2] = '}';
                        Char[] d2 = { '{', c2, '}' };
                        s = new String(d2);
                    }

                    else if (d[0] == ':')
                    {
                        switch (s)
                        {
                            case ":":

                                Char[] d2 = { /*'+', '{', c2, '}'*/' ' };

                                s = /*d2*/ "";

                                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                                InputSimulator.SimulateKeyPress(VirtualKeyCode.OEM_1);
                                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                                break;
                        }
                    }

                    //AM: handling upper case fails no matter...
                    //if (s.ToUpper() == s && s != " ")
                    //{
                    //    Char[] d2 = { '{', c2, '}' };
                    //    s = new String(d2);
                    //    Console.WriteLine("UpperCase: " + s);
                    //}
                    //AM: handling upper case ...

                    //if (s.ToUpper() == s && s[0].
                    if (Char.IsLetter(c2))
                    {
                        if (s.ToUpper() == s && s != " ")
                        {
                            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                            //try either of lines a or b:
                            InputSimulator.SimulateTextEntry(s);//line a
                            //InputSimulator.SimulateTextEntry(s.ToLower());//line b
                            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                            Console.WriteLine("UpperCase: " + s + " " + s.ToUpper());
                            s = "";
                            //break;
                        }
                    }


                    System.Windows.Forms.SendKeys.SendWait(s/*new String(d)*/);
                    //InputSimulator.

                    System.Windows.Forms.SendKeys.Flush();
                    Launcher.logcontent += classname + "-SKSBC-" + s/*new String(d)*//* + "\r\n"*/ + " ";
                    Thread.Sleep(100);
                }
            }
            Launcher.logcontent += /*classname + "-SKSBC-" + d + */"\r\n";
            Launcher.logcontent += classname + "-SKSBC-" + keys + "\r\n";
            Thread.Sleep(5000);
        }

        public void SetValuePattern(AutomationElement ae, String s)
        {
            ValuePattern vpTextBox1 =
  (ValuePattern)ae.GetCurrentPattern(ValuePattern.Pattern);
            vpTextBox1.SetValue(s);

        }

        public void ClickMouseLoc(String button, int x, int y)
        {
            MouseButton mb = new MouseButton();

            //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
            //bool reverse = true;//change this to false when running on right-handed mouse
            if (Launcher.reverse)
            {
                if (button.ToUpper() == "LEFT")
                    button = "RIGHT";
                else if (button.ToUpper() == "RIGHT")
                    button = "LEFT";
            }


            if (button.ToUpper() == "LEFT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
                Launcher.logcontent += classname + "-CML-" + "I am left click" + "\r\n";
                mb = MouseButton.Left;

            }
            else if (button.ToUpper() == "RIGHT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Right);
                Launcher.logcontent += classname + "-CML-" + "I am right click" + "\r\n";
                mb = MouseButton.Right;
            }

            if (button.ToUpper() == "MIDDLE")
            {
                Launcher.logcontent += classname + "-CML-" + "I am middle click" + "\r\n";
                mb = MouseButton.Middle;
            }

            Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x, y));
            Console.WriteLine("...moved?...");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(mb.ToString() + "=" + mb.GetType());
            Microsoft.Test.Input.Mouse.Click(mb);
            Console.WriteLine("...clicked?...");
            System.Threading.Thread.Sleep(2000);



            Launcher.logcontent += classname + "-CML-" + " Clicked X,Y:" + x + ", " + y + "\r\n";
            //AM: write last x,y to info directory, thus allowing for NEXT click to be rel to it
            FileIO fio = new FileIO();
            fio.Write(Launcher.xyfile, x + "," + y, "info");
            Sleeper sleep = new Sleeper();
            sleep.Sleep(2000);
        }

        public void DoubleClickMouseLoc(String button, int x, int y)
        {
            MouseButton mb = new MouseButton();

            //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
            //bool reverse = true;//change this to false when running on right-handed mouse
            if (Launcher.reverse)
            {
                if (button.ToUpper() == "LEFT")
                    button = "RIGHT";
                else if (button.ToUpper() == "RIGHT")
                    button = "LEFT";
            }


            if (button.ToUpper() == "LEFT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
                Launcher.logcontent += classname + "-CML-" + "I am left click" + "\r\n";
                mb = MouseButton.Left;

            }
            else if (button.ToUpper() == "RIGHT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Right);
                Launcher.logcontent += classname + "-CML-" + "I am right click" + "\r\n";
                mb = MouseButton.Right;
            }

            if (button.ToUpper() == "MIDDLE")
            {
                Launcher.logcontent += classname + "-CML-" + "I am middle click" + "\r\n";
                mb = MouseButton.Middle;
            }

            Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x, y));
            Console.WriteLine("...moved?...");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(mb.ToString() + "=" + mb.GetType());
            Microsoft.Test.Input.Mouse.DoubleClick(mb);
            Console.WriteLine("...clicked?...");
            System.Threading.Thread.Sleep(2000);



            Launcher.logcontent += classname + "-CML-" + " Clicked X,Y:" + x + ", " + y + "\r\n";
            //AM: write last x,y to info directory, thus allowing for NEXT click to be rel to it
            FileIO fio = new FileIO();
            fio.Write(Launcher.xyfile, x + "," + y, "info");
            Sleeper sleep = new Sleeper();
            sleep.Sleep(2000);
        }

        //
        //
        //
        //SMV: Trying to implement a mouse wheel function/method
        public void WheelMouse(String button, double w1)
        {
            //MouseAction mb = new MouseAction();
            //mb = MouseAction.WheelClick;
            Launcher.logcontent += classname + "Wheelie!!\r\n";
            Microsoft.Test.Input.Mouse.Scroll(w1);

        }
        //
        //
        //

        public void DragMouse(String button, int x1, int y1, int x2, int y2)
        {

            MouseButton mb = new MouseButton();
            MouseButton mbl = new MouseButton();
            MouseButton mbr = new MouseButton();
            mbl = MouseButton.Left;
            mbr = MouseButton.Right;

            int MOUSEEVENTF_LEFTDOWN = 0x02;
            int MOUSEEVENTF_LEFTUP = 0x04;
            int MOUSEEVENTF_RIGHTDOWN = 0x08;
            int MOUSEEVENTF_RIGHTUP = 0x10;
            int MOUSEEVENTF_MOVE = 0x01;

            //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
            //bool reverse = true;//change this to false when running on right-handed mouse
            if (Launcher.reverse)
            {
                if (button.ToUpper() == "LEFT")
                    button = "RIGHT";
                else if (button.ToUpper() == "RIGHT")
                    button = "LEFT";
            }


            if (button.ToUpper() == "LEFT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
                Launcher.logcontent += classname + "-DM-" + "I am left click" + "\r\n";
                mb = MouseButton.Left;


                //use this to drag from (x1,y1) to (x2,y2)

                //mouse_event(MOUSEEVENTF_LEFTDOWN, x1, y1, 0, 0);
                //mouse_event(MOUSEEVENTF_MOVE, x2, y2, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, x2, y2, 0, 0);

            }
            else if (button.ToUpper() == "RIGHT")
            {
                //mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Right);
                Launcher.logcontent += classname + "-DM-" + "I am right click" + "\r\n";
                mb = MouseButton.Right;

                //use this to drag from (x1,y1) to (x2,y2)

                //mouse_event(MOUSEEVENTF_RIGHTDOWN, x1, y1, 0, 0);
                //mouse_event(MOUSEEVENTF_MOVE, x2, y2, 0, 0);
                //mouse_event(MOUSEEVENTF_RIGHTUP, x2, y2, 0, 0);

            }

            if (button.ToUpper() == "MIDDLE")
            {
                Launcher.logcontent += classname + "-DM-" + "I am both click" + "\r\n";
                mb = MouseButton.Middle;

                //use this to drag from (x1,y1) to (x2,y2)

                //mouse_event(MOUSEEVENTF_LEFTDOWN, x1, y1, 0, 0);
                //mouse_event(MOUSEEVENTF_RIGHTDOWN, x1, y1, 0, 0);
                //mouse_event(MOUSEEVENTF_MOVE, x2, y2, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, x2, y2, 0, 0);
                //mouse_event(MOUSEEVENTF_RIGHTUP, x2, y2, 0, 0);

            }


            if (button.ToUpper() != "BOTH")
            {
                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x1, y1));
                Console.WriteLine("...moved?...");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(mb.ToString() + "=" + mb.GetType());
                Microsoft.Test.Input.Mouse.Down(mb);
                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x2, y2));
                Microsoft.Test.Input.Mouse.Up(mb);
                Console.WriteLine("...dragged?...");
                System.Threading.Thread.Sleep(2000);
            }
            if (button.ToUpper() == "BOTH")
            {
                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x1, y1));
                Console.WriteLine("...moved?...");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine(mbl.ToString() + "=" + mbl.GetType());
                Console.WriteLine(mbr.ToString() + "=" + mbr.GetType());
                Microsoft.Test.Input.Mouse.Down(mbl);
                Microsoft.Test.Input.Mouse.Down(mbr);
                Microsoft.Test.Input.Mouse.MoveTo(new System.Drawing.Point(x2, y2));
                Microsoft.Test.Input.Mouse.Up(mbl);
                Microsoft.Test.Input.Mouse.Up(mbr);
                Console.WriteLine("...dragged?...");
                System.Threading.Thread.Sleep(2000);
            }
            Launcher.logcontent += classname + "-DM-" + " Dragged to X,Y:" + x2 + ", " + y2 + "\r\n";
            //AM: write last x,y to info directory, thus allowing for NEXT click to be rel to it
            FileIO fio = new FileIO();
            fio.Write(Launcher.xyfile, x2 + "," + y2, "info");
            Sleeper sleep = new Sleeper();
            sleep.Sleep(2000);
        }

    }//namespace

}//class
