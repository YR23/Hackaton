using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace victim
{
    class controller
    {
        public Form1 form1 { get; set; }
        public TcpServer tcpServer { get; set; }
        public controller()
        {
            form1 = new Form1();
        }

        internal void NewClient(EndPoint remoteEndPoint)
        {
            form1.UpdateNewClient(remoteEndPoint);
        }

        internal void message(string remoteEndPoint)
        {
            form1.UpdateMessageBox(remoteEndPoint);
        }
    }
}
