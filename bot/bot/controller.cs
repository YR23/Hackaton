using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    class controller
    {
        public Form1 form1 { get; set; }
        public Client tcpClient { get; set; }
        public controller()
        {
            form1 = new Form1();
        }

        public void UpdateMessageBox(string msg)
        {
            form1.UpdateMessageBox(msg);
        }
        
    }
}
