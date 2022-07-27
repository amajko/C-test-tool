using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
//using System.Collections.IEnumerator;
using System.Collections;
using System.Threading;


namespace IntegrationTest
{
    class Walker
       
    {
        static String classname = "Walker:";
        public void WalkEnabledElements(AutomationElement rootElement)
        {


            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            AutomationElement elementNode = walker.GetFirstChild(rootElement);
            while (elementNode != null)
            {
                Launcher.logcontent += classname + "-WEE-" + elementNode.ToString() + "\r\n";
                WalkEnabledElements(elementNode);
                elementNode = walker.GetNextSibling(elementNode);
            }


        }

        public void WalkEnabledElementsFromCollection(System.Windows.Automation.AutomationElementCollection m2)
        {


            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-WEEfC-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";

            ien.Reset();
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                Launcher.logcontent += "-----------------------------------------------------------" + "\r\n";


                    Launcher.logcontent += classname + "-WEEfCFE-" + "Current=" + ien.Current.ToString() +
                    " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) +
                    " of AutomationID "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty) +
                    " of ControlType "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) +
                    " of LocalizedControlType "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.LocalizedControlTypeProperty) +
                    " of IsControlElement "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.IsControlElementProperty) +
                    " of IsEnabled "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.IsEnabledProperty)
                    + "\r\n";
                //Launcher.logcontent += "-----------------------------------------------------------" + "\r\n";
            }

        }

        //AM: deprecated
        public void WalkEnabledElementsFromCollectionFromElement(AutomationElement aebegin)
        {

            AutomationElementCollection m2 = aebegin.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-WEEfCFE-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";

            ien.Reset();
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                Launcher.logcontent += "-----------------------------------------------------------" + "\r\n";
                Launcher.logcontent += classname + "-WEEfCFE-" + "Current=" + ien.Current.ToString() + 
                    " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) +
                    " of AutomationID "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty) +
                    " of ControlType "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) +
                    " of IsControlElement "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.IsControlElementProperty) +
                    " of IsEnabled "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.IsEnabledProperty) 
                    + "\r\n";
                Launcher.logcontent += "-----------------------------------------------------------" + "\r\n";
            }

        }
        
        //AM: alwasy returns the last match if there's a list of matches
        public AutomationElement TestEnabledElementsFromCollection(System.Windows.Automation.AutomationElementCollection m2, String test)
        {

            AutomationElement ae = null;


            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-TEEfC-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";

            ien.Reset();
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                //AM: either by name or autoID
                 if(((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty)).ToString() == test
                     ||
                    ((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty)).ToString() == test)
                 {
                     ae = (AutomationElement)ien.Current;
                     Launcher.logcontent += classname + "-TEEfC-" + "Current=" + ien.Current.ToString() + " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n"; 
                     Launcher.logcontent += classname + "-TEEfC-" + "               /\\ " + test + " is found! /\\" + "\r\n";
                 }
            }

            return ae;
        }

        //AM: alwasy returns the last match if there's a list of matches
        public AutomationElement TestEnabledElementsFromCollectionByType(System.Windows.Automation.AutomationElementCollection m2, String test, String type)
        {

            AutomationElement ae = null;


            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-TEEfCbT-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";

            ien.Reset();
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                //AM: either by name or autoID
                if (((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty)).ToString() == test
                    ||
                   ((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty)).ToString() == test)
                {
                    if (((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.LocalizedControlTypeProperty)).ToString() == type)
                    {
                    ae = (AutomationElement)ien.Current;
                    Launcher.logcontent += classname + "-TEEfCbT-" + "Current=" + ien.Current.ToString() + " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n";
                    Launcher.logcontent += classname + "-TEEfCbT-" + "               /\\ " + test + " is found! /\\" + "\r\n";
                    }
                }
            }

            return ae;
        }

        //AM: alwasy returns the last match if there's a list of matches
        public AutomationElement TestEnabledElementsFromCollectionFromElement(AutomationElement aebegin, String test)
        {
            AutomationElementCollection m2 = null;
            AutomationElement ae = null;

            m2 = aebegin.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));
            
            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-TEEfCFE-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";
            Launcher.logcontent += classname + "-TEEfCFE-" + "Testing for " + test + "\r\n";
            ien.Reset();
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                //AM: either by name or autoID
                if (((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty)).ToString() == test
                                         ||
                    ((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty)).ToString() == test)
                 {
                     ae = (AutomationElement)ien.Current;
                     Launcher.logcontent += classname + "-TEEfCFE-" + "Current=" + ien.Current.ToString() + " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n"; 
                     Launcher.logcontent += classname + "-TEEfCFE-" + "               /\\ " + test + " is found! /\\" + "\r\n";
                 }
            }

            return ae;
        }

        public AutomationElement TestEnabledElementsFromCollectionFromElement(AutomationElement aebegin, String test, int index)
        {
            AutomationElementCollection m2 = null;
            AutomationElement ae = null;

            m2 = aebegin.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            IEnumerator ien = m2.GetEnumerator();
            Launcher.logcontent += classname + "-TEEfCFEover-" + "IEnumerator: " + ien.ToString()/* + ", Current element=" + ien.Current.ToString()*/ + "\r\n";
            Launcher.logcontent += classname + "-TEEfCFEover-" + "Testing for " + test + "\r\n";
            ien.Reset();
            int checkindex = -1;
            while (ien.MoveNext())
            {
                //Console.WriteLine(ien.Current.ToString());
                //AM: either by name or autoID
                if (((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty)).ToString() == test
                                         ||
                    ((String)((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.AutomationIdProperty)).ToString() == test)
                {
                    checkindex++;
                    Launcher.logcontent += classname + "-TEEfCFEover-" + "checkindex=" + checkindex + "\r\n";
                    if (checkindex == index)
                    {
                        ae = (AutomationElement)ien.Current;
                        Launcher.logcontent += classname + "-TEEfCFEover-" + "Current=" + ien.Current.ToString() + " of Name "/* + ien.GetType()  + "-"*/ + ((AutomationElement)ien.Current).GetCurrentPropertyValue(AutomationElement.NameProperty) + "\r\n";
                        Launcher.logcontent += classname + "-TEEfCFEover-" + "               /\\ " + test + " is found! /\\" + "\r\n";
                    }
                }
            }

            return ae;
        }
    }
}

