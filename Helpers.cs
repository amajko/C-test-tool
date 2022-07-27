using System;
using System.Windows.Automation;
using System.Windows.Input;
using IntegrationTest;

namespace Examples
{
    public class Helpers
    {
        public static void MoveToAndClick(AutomationElement element)
        //called from Launcher.GuranteedMouseClick
        {
            //sorry, im using left-handed mouse to develop; tho automation would occur on right-handed mouse :) 
            //bool reverse = true;//change this to false when running on right-handed mouse
            if (!Launcher.reverse)
            {
                MoveToAndClick(element, MouseButton.Left);
            }
            else
            {
                MoveToAndClick(element, MouseButton.Right);
            }
        }

        public static void MoveToAndClick(AutomationElement element, MouseButton mouseButton)
        {

            System.Windows.Point winPoint = new System.Windows.Point();
            try
            {
                /*System.Windows.Point*/
                winPoint = element.GetClickablePoint();//always null

            }
            catch (Exception e)
            {
                Launcher.logcontent += e.Message + "\r\n";
                Console.WriteLine(e.Message);
            }
            System.Drawing.Point drawingPoint = new System.Drawing.Point((int)winPoint.X, (int)winPoint.Y);
            Console.WriteLine("X,Y=" + (int)winPoint.X + ", " + (int)winPoint.Y);
            Launcher.logcontent += "X,Y=" + (int)winPoint.X + ", " + (int)winPoint.Y + "\r\n";
            Microsoft.Test.Input.Mouse.MoveTo(drawingPoint);//moves here!
            Console.WriteLine("...moved?...");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine(mouseButton.ToString() + "=" + mouseButton.GetType());
            Launcher.logcontent += mouseButton.ToString() + "=" + mouseButton.GetType() + "\r\n";
            Microsoft.Test.Input.Mouse.Click(mouseButton);//never clicks! and relocates mouse to 0,0 and clicks there!
            Console.WriteLine("...clicked?...");
            System.Threading.Thread.Sleep(5000);

        }
        public static void/*bool*/ MoveToAndClick2(AutomationElement element, MouseButton mouseButton)
        {
            //bool b = false;
            //System.Windows.Point winPoint = element.GetClickablePoint();
            //System.Drawing.Point drawingPoint = new System.Drawing.Point((int)winPoint.X, (int)winPoint.Y);
            //Microsoft.Test.Input.Mouse.MoveTo(drawingPoint);
            Microsoft.Test.Input.Mouse.Click(mouseButton);


            //return b;
        }

    }
}