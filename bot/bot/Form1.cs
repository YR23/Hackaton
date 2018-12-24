using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot
{
    public partial class Form1 : Form
    {
        Client tcpClient;
        controller controller;
        public Form1()
        {
            InitializeComponent();
            tcpClient = new Client();
        }

        internal void UpdateMessageBox(string msg)
        {
            MessageBoxText.Text += msg + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcpClient.SendThePassword(PasswordTextBox.Text);
        }

        internal void setController(controller mController)
        {
            controller = mController;
            tcpClient.setController(mController);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tcpClient.SetPortNumber(PortTextBox.Text) && tcpClient.SetServerIPAddress(IpTextBox.Text))
            {
                tcpClient.StartHackingProcess();
            }
        }
    }
}
