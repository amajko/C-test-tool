using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace IntegrationTest
{
    class SumSuite
    {
        static String classname = "SumSuite:";

        public String Calc()
        {
            String s = "";

            GetProcess gp = new GetProcess();
            String filename = gp.GetSuiteReport();
            Launcher.logcontent += classname + "-C-" + "results filename is " + filename + "\r\n";
            Launcher.logcontent += classname + "-C-" + "LogTetst.resultfolder is " + LogTest.resultfolder + "\r\n";
            String fullfile = "";
            if (filename != "" && File.Exists(Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + filename))
            {
                fullfile = Launcher.logdirectory + "\\" + LogTest.resultfolder + "\\" + filename;
            }

            Launcher.logcontent += classname + "-C-" + "calculating from " + fullfile + "\r\n";

            FileIO fio = new FileIO();
            String filecontents = fio.Read(fullfile);

            //read each line
            //if begin with:
            //Total test run time : 00:00:02.1250136
            //TOTAL TESTS: 1
            //TOTAL PASSES: 1
            //TOTAL VERIFICATION FAILURES: 0
            //TOTAL UNEXPECTED EXCEPTIONS FAILURES: 0
            //split by : (for time handle special) and convert to int and add to summary vairables:
            //run time (long or date), tests(int), passes(int), vfails(int), uefails(int)
            //OR
            //for time, just record 1st: Test started at : 3:17:30 PM AND last: Test ended at : 3:22:57 PM

            //simpler (estimate)
            //count type of lines
            //all lines, lines with separator bar '================='
            //...

            String[] totallines = filecontents.Split('\r');
            int separatorlines = 0;
            int tests = 0;
            int steps = 0;
            int blanksteps = 0;
            int passes = 0;
            int vfails = 0;
            int uefails = 0;
            String timestamps = "";
            for (int i = 0; i < totallines.Length; i++)
            {
                if (totallines[i].Contains("run timestamp"))
                {
                    String[] splitted = totallines[i].Split(':');
                    timestamps += splitted[2] + ",";
                }
                if (totallines[i].StartsWith("\nTOTAL TESTS"))
                {
                    String[] splitted = totallines[i].Split(':');
                    String numberastext = splitted[1].Trim();
                    int value;
                    int.TryParse(numberastext, out value);
                    Launcher.logcontent += classname + "-C-" + "TOTAL TESTS: numberastext is " + numberastext + ", value=" + value + "\r\n";
                    tests += value;
                }
                if (totallines[i].StartsWith("\nTOTAL PASSES"))
                {
                    String[] splitted = totallines[i].Split(':');
                    String numberastext = splitted[1].Trim();
                    int value;
                    int.TryParse(numberastext, out value);
                    Launcher.logcontent += classname + "-C-" + "TOTAL PASSES: numberastext is " + numberastext + ", value=" + value + "\r\n";
                    passes += value;
                }
                if (totallines[i].StartsWith("\nTOTAL VERIFICATION FAILURES"))
                {
                    String[] splitted = totallines[i].Split(':');
                    String numberastext = splitted[1].Trim();
                    int value;
                    int.TryParse(numberastext, out value);
                    Launcher.logcontent += classname + "-C-" + "TOTAL VERIFICATION FAILURES: numberastext is " + numberastext + ", value=" + value + "\r\n";
                    vfails += value;
                }
                if (totallines[i].StartsWith("\nTOTAL UNEXPECTED EXCEPTIONS FAILURES"))
                {
                    String[] splitted = totallines[i].Split(':');
                    String numberastext = splitted[1].Trim();
                    int value;
                    int.TryParse(numberastext, out value);
                    Launcher.logcontent += classname + "-C-" + "TOTAL UNEXPECTED EXCEPTIONS FAILURES: numberastext is " + numberastext + ", value=" + value + "\r\n";
                    uefails += value;
                }
                if (totallines[i].Contains(") "))
                {
                    if (totallines[i].Length > 31)//a blank step will only have 30 chararcs like from '\n' up to includg '1) '
                    {
                        steps += 1;
                    }
                }
            }
            String[] timestampsarray = timestamps.Split(',');
            String beginruntimestamp = timestampsarray[0];
            String endruntimestamp = timestampsarray[timestampsarray.Length - 2];

            s += "<font color = 'blue'>";
            s += "          ---------- SUMMARY ----------" + "\r\n";
            s += "Suite Began at " + beginruntimestamp + "\r\n";
            s += "Suite Ended at " + endruntimestamp + "\r\n";
            s += "Total Tests = " + tests + "\r\n";
            s += "Total Steps = " + steps + "\r\n";
            s += "</font>";
            s += "<font color = 'green'>";
            s += "Total Tests Passed = " + passes + "\r\n";
            s += "</font>";
            s += "<font color = 'red'>";
            s += "Total verifications failed = " + vfails + "\r\n";
            s += "Total unexpected exceptions = " + uefails + "\r\n";
            s += "</font>";

            return s;
        }
    }
}
