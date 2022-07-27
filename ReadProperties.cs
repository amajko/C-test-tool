using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest
{
    class ReadProperties
    {
        public void Read()
        {
            //****AM: read properties file and reset defualt static booleans
            FileIO fioread = new FileIO();
            String properties = fioread.Read("testbgui.properties");
            if (properties != "File Not Found")
            {
                String[] parse = properties.Split('\n');
                foreach (String line in parse)
                {
                    //Console.WriteLine(line);
                    if (line.StartsWith("reverse"))
                    {
                        String[] property = line.Split('=');
                        Console.WriteLine("reverse=" + property[1]);
                        if (property[1].Trim() == "true")
                        {
                            Launcher.reverse = true;
                        }
                        else
                        {
                            Launcher.reverse = false;
                        }

                    }
                    if (line.StartsWith("printme"))
                    {
                        String[] property = line.Split('=');
                        Console.WriteLine("printme=" + property[1]);
                        if (property[1].Trim() == "true")
                        {
                            Launcher.printme = true;
                        }
                        else
                        {
                            Launcher.printme = false;
                        }

                    }
                    if (line.StartsWith("ecurl"))
                    {
                        String[] property = line.Split('=');
                        Console.WriteLine("ecurl=" + property[1]);
                        Launcher.ecurl = property[1].Trim();
                    }

                    if (line.StartsWith("reportarchive"))
                    {
                        String[] property = line.Split('=');
                        Console.WriteLine("reportarchive=" + property[1]);
                        Launcher.reportarchive = property[1].Trim();
                    }

                }
                //****AM: end read properties file and reset defualt static booleans


            }//if properties != "File not found"
        }//function
    }//class
}//namespace
