using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using Microsoft.Test;
using System.Threading;

namespace IntegrationTest
{
    class ObjectFinder
    {
        static String classname = "ObjectFinder:";
        public AutomationElement FindByID(Process p, String id)
        {
            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);
            System.Windows.Automation.AutomationElement m2b = AutomationUtilities.FindElementsById(aefound, id)[0];
 
            return aefound;
        }

        public AutomationElement FindByName(Process p, String name)
        {
            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

             PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
             PropertyCondition p2 = new PropertyCondition(AutomationElement.NameProperty, name);
             PropertyCondition p3 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
             Condition conditions = new AndCondition(p1, p2,p3);

            System.Windows.Automation.AutomationElement aereturn = aefound.FindFirst(TreeScope.Descendants, conditions);

            return aereturn;
        }

        public System.Windows.Automation.AutomationElementCollection FindObjectsAll()
        {
            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty,true);
            Condition conditions = new AndCondition(p1 ,p2);

            // JZ end

             Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aeDesktop2.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants");
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-FOA-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                w.WalkEnabledElementsFromCollection(aec);
            }

            return aec;

        }

        public System.Windows.Automation.AutomationElementCollection FindObjectsAllFromProcess(Process p)
        {
            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            //AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition conditions = new AndCondition(p1, p2);

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aefound.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants");
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-FOAFP-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                w.WalkEnabledElementsFromCollection(aec);
            }

            return aec;

        }

        public System.Windows.Automation.AutomationElementCollection FindObjectsAllFromElement(AutomationElement ae)
        {
            AutomationElement aefound = /*AutomationElement.FromHandle(p.MainWindowHandle)*/ae;

            //JZ begin
            //AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition conditions = new AndCondition(p1, p2);

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aefound.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants");
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-FOAFE-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                w.WalkEnabledElementsFromCollection(aec);
            }

            return aec;

        }

        public System.Windows.Automation.AutomationElement TestObjectsAll(String test)
        {
            AutomationElement ae = null;

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition conditions = new AndCondition(p1, p2);

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aeDesktop2.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants");
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-TOA-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                ae = w.TestEnabledElementsFromCollection(aec, test);
            }

            return ae;

        }

        //AM: alwasy returns the last match if there's a list of matches (see Walker)
        public System.Windows.Automation.AutomationElement TestObjectsAllFromProcess(Process p, String test)
        {
            AutomationElement ae = null;

            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition conditions = new AndCondition(p1, p2);

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aefound.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants");
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-TOAFP-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                ae = w.TestEnabledElementsFromCollection(aec, test);
            }

            return ae;

        }

        //AM: alwasy returns the last match if there's a list of matches (see Walker)
        public System.Windows.Automation.AutomationElement TestObjectsAllFromProcess(Process p, String test, bool control, bool enabled)
        {
            AutomationElement ae = null;

            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;


            Condition conditions = null;
            PropertyCondition p1 = null;
            PropertyCondition p2 = null;
            if (control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
                p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
                conditions = new AndCondition(p1, p2);
            }
            else if (!control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            }
            else if (control && !enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            }

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = null;
            if (control && enabled)
            {
                Console.WriteLine("   ...in aefound.FindFirst...   ");
                ae = aefound.FindFirst(TreeScope.Descendants, conditions);
            }
            else
            {
                Console.WriteLine("   ...in aefound.FindAll...   ");
                aec = aefound.FindAll(TreeScope.Descendants, p1);
            }
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (!control || !enabled)
            {
                if (aec == null)
                {
                    throw new Exception("Unable to get All Descendants from " + test);
                }
                else
                {
                    Console.WriteLine("Found Descendants");
                    Launcher.logcontent += classname + "-TOAFPover-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                    Walker w = new Walker();
                    ae = w.TestEnabledElementsFromCollection(aec, test);
                }
            }
            if (ae == null)
            {
                throw new Exception("Unable to get Element from " + test);
            }
            else
            {
                Launcher.logcontent += classname + "-TOAFPover-" + "Element is " + ae.GetType().ToString() + "\r\n";
            }
            return ae;

        }

        //AM: alwasy returns the last match if there's a list of matches (see Walker)
        public System.Windows.Automation.AutomationElement TestObjectsAllFromProcessByType(Process p, String test, String type, bool control, bool enabled)
        {
            AutomationElement ae = null;

            AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;


            Condition conditions = null;
            PropertyCondition p1 = null;
            PropertyCondition p2 = null;
            if (control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
                p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
                conditions = new AndCondition(p1, p2);
            }
            else if (!control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            }
            else if (control && !enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            }

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = null;
            if (control && enabled)
            {
                Console.WriteLine("   ...in aefound.FindFirst...   ");
                ae = aefound.FindFirst(TreeScope.Descendants, conditions);
            }
            else
            {
                Console.WriteLine("   ...in aefound.FindAll...   ");
                aec = aefound.FindAll(TreeScope.Descendants, p1);
            }
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (!control || !enabled)
            {
                if (aec == null)
                {
                    throw new Exception("Unable to get All Descendants from " + test);
                }
                else
                {
                    Console.WriteLine("Found Descendants");
                    Launcher.logcontent += classname + "-TOAFPover-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                    Walker w = new Walker();
                    ae = w.TestEnabledElementsFromCollectionByType(aec, test, type);
                }
            }
            if (ae == null)
            {
                throw new Exception("Unable to get Element from " + test);
            }
            else
            {
                Launcher.logcontent += classname + "-TOAFPover-" + "Element is " + ae.GetType().ToString() + "\r\n";
            }
            return ae;

        }

        //AM: alwasy returns the last match if there's a list of matches (see Walker)
        public System.Windows.Automation.AutomationElement TestObjectsAllFromElement(AutomationElement aefound, String test)
        {
            AutomationElement ae = null;

            //AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            PropertyCondition p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            PropertyCondition p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition conditions = new AndCondition(p1, p2);

            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = aefound.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (aec == null)
            {
                throw new Exception("Unable to get All Descendants from " + test);
            }
            else
            {
                Console.WriteLine("Found Descendants");
                Launcher.logcontent += classname + "-TOAFE-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                Walker w = new Walker();
                ae = w.TestEnabledElementsFromCollectionFromElement(aefound, test);
            }

            return ae;

        }

        //AM: alwasy returns the last match if there's a list of matches (see Walker)
        public System.Windows.Automation.AutomationElement TestObjectsAllFromElement(AutomationElement aefound, String test, bool control, bool enabled)
        {
            AutomationElement ae = null;

            //AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            Condition conditions = null;
            PropertyCondition p1 = null;
            PropertyCondition p2 = null;
            bool both = false;
            if (control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
                p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
                conditions = new AndCondition(p1, p2);
                both = true;

            }
            else if (!control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            }
            else if (control && !enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            }
            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = null;
            if (both)
            {
                ae = aefound.FindFirst(TreeScope.Descendants, conditions);
            }
            else
            {
                aec = aefound.FindAll(TreeScope.Descendants, p1);
            }
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (!control || !enabled)
            {
                if (aec == null)
                {
                    throw new Exception("Unable to get All Descendants from " + test);
                }
                else
                {
                    Console.WriteLine("Found Descendants");
                    Launcher.logcontent += classname + "-TOAFEover-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                    Walker w = new Walker();
                    ae = w.TestEnabledElementsFromCollectionFromElement(aefound, test);
                }
            }

            if (ae == null)
            {
                throw new Exception("Unable to get Element from " + test);
            }
            return ae;

        }

        public System.Windows.Automation.AutomationElement TestObjectsAllFromElement(AutomationElement aefound, String test, bool control, bool enabled, int index)
        {
            AutomationElement ae = null;

            //AutomationElement aefound = AutomationElement.FromHandle(p.MainWindowHandle);

            //JZ begin
            AutomationElement aeDesktop2 = AutomationElement.RootElement;

            Condition conditions = null;
            PropertyCondition p1 = null;
            PropertyCondition p2 = null;
            bool both = false;
            if (control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
                p2 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
                conditions = new AndCondition(p1, p2);
                both = true;

            }
            else if (!control && enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            }
            else if (control && !enabled)
            {
                p1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            }
            // JZ end

            Console.WriteLine("...starting to find all" + "..." + DateTime.Now.ToString());
            AutomationElementCollection aec = null;
            if (both)
            {
                ae = aefound.FindFirst(TreeScope.Descendants, conditions);
            }
            else
            {
                aec = aefound.FindAll(TreeScope.Descendants, p1);
            }
            Console.WriteLine("...ending to find all" + "..." + DateTime.Now.ToString());
            Thread.Sleep(5000);

            if (!control || !enabled)
            {
                if (aec == null)
                {
                    throw new Exception("Unable to get All Descendants from " + test);
                }
                else
                {
                    Console.WriteLine("Found Descendants");
                    Launcher.logcontent += classname + "-TOAFEoverindex-" + "Descendants in " + aec.GetType().ToString() + "\r\n";
                    Walker w = new Walker();
                    ae = w.TestEnabledElementsFromCollectionFromElement(aefound, test, index);
                }
            }

            if (ae == null)
            {
                throw new Exception("Unable to get Element from " + test);
            }
            return ae;

        }
    }
}
