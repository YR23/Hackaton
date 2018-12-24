using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public class Client
    {

        controller controller;
        TcpClient mClient;
        int mServerPort;
        IPAddress mServerIPAddress;
        string ShobName = "Laliz";

        internal void setController(controller mController)
        {
            controller = mController;
        }

        public IPAddress ServerIPAddress
        {
            get
            {
                return mServerIPAddress;
            }
        }

        public bool SetServerIPAddress(string _IPAddressServer)
        {
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(_IPAddressServer, out ipaddr))
            {
                controller.UpdateMessageBox("Wrong II mt Friend!");
                return false;
            }

            mServerIPAddress = ipaddr;
            return true;
        }

        public int ServerPort
        {
            get
            {
                return mServerPort;
            }
        }

        public bool SetPortNumber(string _ServerPort)
        {
            int portNumber = 0;

            if (!int.TryParse(_ServerPort.Trim(), out portNumber))
            {
                controller.UpdateMessageBox("Port number must be a number");
                return false;
            }

            if (portNumber <= 0 || portNumber > 65535)
            {
                controller.UpdateMessageBox("Port number must be in range 0-65535");
                return false;
            }

            mServerPort = portNumber;

            return true;
        }

        public async void StartHackingProcess()
        {
            if (mClient == null)
            {
                mClient = new TcpClient();
            }
            await ConnectToServer();   
        }

        public async Task ConnectToServer()
        {
            try
            {
                await mClient.ConnectAsync(mServerIPAddress, mServerPort);
                controller.UpdateMessageBox("Attacking Victim!");
                StreamReader clientStreamReader = new StreamReader(mClient.GetStream());

                char[] buff = new char[64];
                int readByteCount = 0;

                while (true)
                {
                    readByteCount = await clientStreamReader.ReadAsync(buff, 0, buff.Length);

                    if (readByteCount <= 0)
                    {
                        controller.UpdateMessageBox("Disconnected from server");
                        break;
                    }

                    string msg = new string(buff);
                    controller.UpdateMessageBox(msg);
                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception e)
            {

            }
            
        }

        public async void SendThePassword(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                controller.UpdateMessageBox("please Enter A valid Password!");
                return;
            }
            if (mClient != null)
            {
                if (mClient.Connected)
                {
                    StreamReader reader = null;
                    NetworkStream nwStream = mClient.GetStream();
                    //creating the buffer message
                    byte[] buffMessage = Encoding.ASCII.GetBytes(pass);

                    //sending the message to the client
                    nwStream.Write(buffMessage, 0, buffMessage.Length);
                    //waiting for response
                    reader = new StreamReader(nwStream);
                    char[] buff = new char[64];
                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);
                    string receivedText = new string(buff);
                    Array.Clear(buff, 0, buff.Length);
                    if (receivedText == "Access Granted!")
                    {
                        byte[] buf = Encoding.ASCII.GetBytes("Hacked By "+ShobName + "!");

                        //sending the message to the client
                        nwStream.Write(buf, 0, buf.Length);
                    }
                }
            }
        }
           
    }
 }

