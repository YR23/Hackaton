using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    class UDPBot
    {
        controller controller;
        int MyPortNum;
        int ShobPortNum;
        UdpClient Shob;
        UdpClient Bot;
        public UDPBot()
        {
            ShobPortNum = 31337;
        }

        internal void setController(controller mController)
        {
            controller = mController;
        }

        public void BotAnnouncment(string port)
        {
            MyPortNum = Int32.Parse(port);
            Shob = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), ShobPortNum); // endpoint where server is listening
            Shob.Connect(ep);

           
            // send data
           
            controller.UpdateMessageBox("Bot is listening on port " + MyPortNum);

            SendBotAnnouncment();
            //Waiting thread to wait for attack
            //var receivedData = Shob.Receive(ref ep);



        }

        private void SendBotAnnouncment()
        {
            byte[] PortIn2ByteArray = Create2byteArray(MyPortNum);
            Shob.Send(PortIn2ByteArray, 2);
        }

        private byte[] Create2byteArray(int portNum)
        {
            byte[] PortInByte2 = BitConverter.GetBytes(portNum).ToArray();
            byte[] res = new byte[2];
            for (int i=0;i< res.Length; i++)
            {
                res[i] = PortInByte2[i];
            }
            return res;
        }

        internal byte[] WaitForAttack()
        {
            Bot = new UdpClient(MyPortNum);
            var remoteEP = new IPEndPoint(IPAddress.Any, MyPortNum);
            var data = Bot.Receive(ref remoteEP);
            return data;
        }
    }
}
