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
        string ShobName;


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

        public void SetShobName(string name)
        {
            ShobName = name;
        }

        public bool SetServerIPAddress(string _IPAddressServer)
        {
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(_IPAddressServer, out ipaddr))
            {
                controller.UpdateMessageBox("Wrong II my Friend!");
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
             ConnectToServer();   
        }

        public async Task ConnectToServer()
        {
            try
            {
                await mClient.ConnectAsync(mServerIPAddress, mServerPort);
                StreamReader clientStreamReader = new StreamReader(mClient.GetStream());

                char[] buff = new char[64];

                    int readByteCount = 0;
                    readByteCount =  clientStreamReader.Read(buff, 0, buff.Length);
                    if (readByteCount <= 0)
                    {
                        controller.UpdateMessageBox("Disconnected from server");
                    }
                    string msg = new string(buff);
                    controller.UpdateMessageBox(msg);
                    Array.Clear(buff, 0, buff.Length);
                
            }
            catch (Exception e)
            {

            }
            
        }

        public int SendThePassword(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                controller.UpdateMessageBox("please Enter A valid Password!");
                return 0;
            }
            if (mClient != null)
            {
                if (mClient.Connected)
                {
                    StreamReader reader = null;
                    NetworkStream nwStream = mClient.GetStream();
                    //creating the buffer message
                    byte[] buffMessage = Encoding.ASCII.GetBytes(pass);
                    char[] buff = new char[64];
                    reader = new StreamReader(nwStream);
                    int nRet = reader.Read(buff, 0, buff.Length);
                    string receivedText = new string(buff);
                    string finalText = clearAllzeroes(receivedText);
                    controller.UpdateMessageBox("please Enter A Password!");
                    nwStream.Write(buffMessage, 0, buffMessage.Length);
                    //waiting for response
                    char[] buff2 = new char[64];
                    nRet = reader.Read(buff2, 0, buff2.Length);
                    receivedText = new string(buff2);
                    finalText = clearAllzeroes(receivedText);
                    Array.Clear(buff, 0, buff.Length);
                    if (finalText.Contains("Access Granted"))
                    {
                        byte[] buf = Encoding.ASCII.GetBytes("Hacked By " + ShobName + "!");

                        //sending the message to the client
                        nwStream.Write(buf, 0, buf.Length);
                    }
                    int readByteCount = 0;
                    nRet = reader.Read(buff, 0, buff.Length);
                    if (nRet <= 0)
                    {
                        controller.UpdateMessageBox("\r\n" + "Disconnected from server");
                        mClient.Close();
                        return -1;

                    }
                }
            }
            return 0;
        }

        private string clearAllzeroes(string receivedText)
        {
            string s = "";
            for (int i = 0; i < receivedText.Length; i++)
            {
                if ((receivedText[i] >= 'a' && receivedText[i] <= 'z') || (receivedText[i] >= 'A' && receivedText[i] <= 'Z') ||(receivedText[i] == ' '))
                    s += receivedText[i];
            }
            return s;
        }

    }
 }

