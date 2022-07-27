using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntegrationTest
{
    class Sleeper
    {
        public void Sleep()
        {
            int sleepy = 30000;
            Console.WriteLine("...wait " + (sleepy / 1000) + " seconds...");
            Thread.Sleep(sleepy);

        }
        public void Sleep(int ms)
        {
            Console.WriteLine("...at " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " wait " + (ms / 1000) + " seconds or " + (ms/60000) + " minutes or " + (ms/3600000) + " hours...");
            Thread.Sleep(ms);
        }
    }
}
