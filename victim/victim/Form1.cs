using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace victim
{
    public partial class Form1 : Form
    {

        TcpServer tcpServer;
        controller controller;
        public Form1()
        {
            InitializeComponent();
            tcpServer = new TcpServer();
        }

        internal void UpdateNewClient(EndPoint remoteEndPoint)
        {
            string ip = remoteEndPoint.ToString().Split(':')[0];
            string port = remoteEndPoint.ToString().Split(':')[1];
            //messegebox.Text += "New Client! from IP: "+ ip + " Port: "+ port +"\r\n";
        }

        internal void UpdateMessageBox(string remoteEndPoint)
        {
            messegebox.Text += remoteEndPoint+"\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcpServer.StartListeningForIncomingConnection(null,Int32.Parse(PortText.Text.Trim()), passtext.Text.Trim());
            messegebox.Text += string.Format("Server listening on port {0}, password is {1} \r\n", PortText.Text.Trim(), passtext.Text.Trim());
            passtext.Text = "";
            PortText.Text = "";
        }

        internal void setController(controller mController)
        {
            controller = mController;
            tcpServer.setController(mController);
        }

       
    }
}
