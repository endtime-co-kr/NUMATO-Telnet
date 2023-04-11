using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;

namespace NUMATOTelnet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelnetComm telnet = new TelnetComm();
            telnet.Run("192.168.123.98", 23);
            
            telnet.OpenSession();

            //while (true)
            //{
            //    Thread.Sleep(1000);
            //}
            while (true)
            {
                telnet.SetCommand("gpio set 3");
                Thread.Sleep(500);
                telnet.SetCommand("gpio clear 3");
                Thread.Sleep(500);
            }


        }
    }
}
