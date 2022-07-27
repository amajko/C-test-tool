using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTest;

namespace TestCase
{
    class TC_Sleep
    {
        public static String tc = "pauses a test run";
        public static String machinestatereq = "Any";
        public static String parameters_csv_use = "parameters[0=milliseconds to sleep]";
        static String classname = "TC_Sleep:";

        public void Sleep(String parameters_csv)
        {
            //AM: each TestCase has this single line describing it
            Launcher.logcontent += classname + "TestCase " + TC_Sleep.tc + "\r\n";
            Launcher.logcontent += classname + "Machine State Required: " + TC_Sleep.machinestatereq + "\r\n";
            Launcher.logcontent += classname + "Uses parameters_csv ([args3]): " + TC_Sleep.parameters_csv_use + "\r\n";

            String[] parameters = parameters_csv.Split(',');

            int value;
            int.TryParse(parameters[0], out value);

            Sleeper sl = new Sleeper();
            sl.Sleep(value);
        }
    }
}
