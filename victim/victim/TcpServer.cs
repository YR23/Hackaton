using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace victim
{
    public class TcpServer
    {
        controller controller;
        string victimPass;
        IPAddress mIP;
        int mPort;
        TcpListener mTCPListener;

        List<TcpClient> mClients;
        Dictionary<int, int> TimeDictionary;

        public bool KeepRunning { get; set; }

        public TcpServer()
        {
            mClients = new List<TcpClient>();
            TimeDictionary = new Dictionary<int, int>();
            
        }

        public async void StartListeningForIncomingConnection(IPAddress ipaddr, int port,string pass)
        {
            victimPass = pass;
            if (ipaddr == null)
            {
                ipaddr = IPAddress.Any;
            }

            if (port <= 0)
            {
                port = 23000;
            }

            mIP = ipaddr;
            mPort = port;
            mTCPListener = new TcpListener(mIP, mPort);
            try
            {
                mTCPListener.Start();
                KeepRunning = true;
                while (KeepRunning)
                {
                    TcpClient client = await mTCPListener.AcceptTcpClientAsync();
                    controller.NewClient(client.Client.RemoteEndPoint);
                    string result = await AskClientForPassword(client);
                    controller.message(result);
                }

            }
            catch (Exception excp)
            {
                
            }
        }

        private async Task<string> AskClientForPassword(TcpClient client)
        {
            StreamReader reader = null;
            NetworkStream nwStream = client.GetStream();
            //creating the buffer message
            byte[] buffMessage = Encoding.ASCII.GetBytes("Please enter your password\r\n");

           //sending the message to the client
            nwStream.Write(buffMessage, 0, buffMessage.Length);

            //waiting for response
            reader = new StreamReader(nwStream);
            char[] buff = new char[64];
            int nRet = await reader.ReadAsync(buff, 0, buff.Length);
            string receivedText = new string(buff);
            Array.Clear(buff, 0, buff.Length);
            return receivedText;
        }

        internal void setController(controller mController)
        {
            controller = mController;
        }

        public void StopServer()
        {
            try
            {
                if (mTCPListener != null)
                {
                    mTCPListener.Stop();
                }

                foreach (TcpClient c in mClients)
                {
                    c.Close();
                }

                mClients.Clear();
            }
            catch (Exception excp)
            {

                Debug.WriteLine(excp.ToString());
            }
        }

        private async void CheckTheTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = paramClient.GetStream();
                reader = new StreamReader(stream);

                char[] buff = new char[64];

                while (KeepRunning)
                {
                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);
                    if (nRet == 0)
                    {
                        RemoveClient(paramClient);

                        System.Diagnostics.Debug.WriteLine("Socket disconnected");
                        break;
                    }

                    string receivedText = new string(buff);

                    System.Diagnostics.Debug.WriteLine("*** RECEIVED: " + receivedText);

                    Array.Clear(buff, 0, buff.Length);


                }

            }
            catch (Exception excp)
            {
                RemoveClient(paramClient);
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }

        }

        private void RemoveClient(TcpClient paramClient)
        {
            if (mClients.Contains(paramClient))
            {
                mClients.Remove(paramClient);
                Debug.WriteLine(String.Format("Client removed, count: {0}", mClients.Count));
            }
        }

        public async void SendToAll(string leMessage)
        {
            if (string.IsNullOrEmpty(leMessage))
            {
                return;
            }

            try
            {
                byte[] buffMessage = Encoding.ASCII.GetBytes(leMessage);

                foreach (TcpClient c in mClients)
                {
                    c.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception excp)
            {
                Debug.WriteLine(excp.ToString());
            }

        }
    }
}
