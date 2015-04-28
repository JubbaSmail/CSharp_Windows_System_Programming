using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////////////////////////////////
using System.Net.Sockets;
using System.IO;

namespace MyNetwork
{
    public class Net
    {
        TcpClient client;
        public bool creartServer(int port)
        {
            try
            {
                TcpListener server = new TcpListener(port);
                server.Start();
                client = server.AcceptTcpClient();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createClient(string ip,int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                return true;
            }
            catch
            {
                return false;
            }
        }
    
        public bool sendMsg(string msg)
        {
            try
            {
                byte[] binary = UnicodeEncoding.Unicode.GetBytes(msg);
                NetworkStream ns = client.GetStream();
                ns.Write(binary, 0, binary.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string receiveMsg()
        {
            string msg = "";
            try
            {
                NetworkStream ns = client.GetStream();
                //byte[] binary = ns.Read();
                byte[] binary = new byte[100];
                ns.Read(binary, 0, binary.Length);
                msg = UnicodeEncoding.Unicode.GetString(binary);
                return msg;
            }
            catch
            {
                return msg;
            }
        }

        public bool sendFile(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[1024];
                int readbytes = 0;
                while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ns.Write(buffer, 0, readbytes);
                    ns.Read(buffer, 0, 1);
                    if (buffer[0] != 1)
                        break;
                }
                fs.Close();
                ns.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool receiveFile(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[1024];
                int readbytes = 0;
                while ((readbytes = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, readbytes);
                    buffer[0] = 1;
                    ns.Write(buffer, 0, 1);//msg to server [cont]
                }
                fs.Close();
                ns.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
