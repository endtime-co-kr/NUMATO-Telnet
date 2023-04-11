using De.Mud.Telnet;
using Net.Graphite.Telnet;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NUMATOTelnet
{
    public class TelnetComm
    {
        private TelnetWrapper t;
        private bool done = false;
        private StreamReader sr;

        public void Run(string host, int port)
        {
            t = new TelnetWrapper();

            t.Disconnected += new DisconnectedEventHandler(this.OnDisconnect);
            t.DataAvailable += new DataAvailableEventHandler(this.OnDataAvailable);

            t.TerminalType = "NETWORK-VIRTUAL-TERMINAL";
            t.Hostname = host;
            t.Port = port;
            Console.WriteLine("Connecting ...");
            t.Connect();
        }

        public void OpenSession()
        {
            int i;
            char ch;

            try
            {
                t.Receive();

                //while (!done)
                //    //t.Send(Console.ReadLine() + t.CRLF);
                //    t.Send(Console.ReadLine() + t.CRLF);
            }
            catch
            {
                t.Disconnect();
                throw;
            }
        }

        private void OnDisconnect(object sender, EventArgs e)
        {
            done = true;
            Console.WriteLine("\nDisconnected.");
        }

        private void OnDataAvailable(object sender, DataAvailableEventArgs e)
        {
            Console.Write(e.Data);
            CheckKeyword(e.Data);
        }

        List<string> _keywords = new List<string>() { "User Name:", "Password:", "successfully", ">" };
        bool _isReady = false;
        ManualResetEvent isReadyEvt = new ManualResetEvent(false);
        private bool CheckKeyword(string recvBuf)
        {
            foreach (var key in _keywords)
            {
                if (recvBuf.Contains(key) == true)
                {
                    switch (key)
                    {
                        case "User Name:":
                            t.Send("admin" + Environment.NewLine);
                            break;
                        case "Password:":
                            t.Send("admin" + Environment.NewLine);
                            break;
                        case "successfully":
                            t.Send(Environment.NewLine);
                            break;
                        case ">":
                            _isReady = true;
                            isReadyEvt.Set();
                            break;
                    }
                }
            }
            return true;
        }

        public void SetCommand(string cmd)
        {
            if (_isReady == false)
            {
                isReadyEvt.WaitOne();
            }
            t.Send(cmd + Environment.NewLine);
            _isReady = false;
        }
    }
}
