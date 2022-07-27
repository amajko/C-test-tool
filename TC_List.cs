using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;

namespace TestCase
{
    class TC_List
    {
        static String classname = "TC_List:";
        public static String tc = "lists testcases";
               public void List(string item)
               {
                   //AM: each TestCase has this single line describing it
                   //Launcher.logcontent += "TC_List:" + "TestCase " + TC_List.tc + "\r\n";
                   Launcher.logcontent += classname + item + "\r\n";

               }
    }
}
