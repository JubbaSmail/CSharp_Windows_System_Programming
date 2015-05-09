using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////////////////////////////////
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace MyNetwork
{
    public class Net
    {
        TcpClient client;
        TcpListener server;

        public bool creartServer(int port)
        {
            try
            {
                server = new TcpListener(port);
                server.Start();
                Thread t1 = new Thread(AcceptTcpClient);
                t1.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AcceptTcpClient()
        {
            client = server.AcceptTcpClient();
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
        
        public delegate void pointer_to_funcation(string msg);
        public void receiveMsg(Form main,pointer_to_funcation f)
        {
            Thread t2 = new Thread(receiveMsg2);
            t2.Start(new object[2]{main,f});//call the thread+params
        }
        public void receiveMsg2(object obj)
        {
            object[] objs = (object[])obj;
            Form main = (Form)objs[0];
            pointer_to_funcation f = (pointer_to_funcation)objs[1];
            string msg = "";
            try
            {
                NetworkStream ns = client.GetStream();
                //byte[] binary = ns.Read();
                byte[] binary = new byte[100];
                ns.Read(binary, 0, binary.Length);
                msg = UnicodeEncoding.Unicode.GetString(binary);
                main.Invoke(f, msg);
                //return msg;
            }
            catch
            {
                return;
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

        public bool sendImg(Image img)
        {
            try
            {
                //img 2D -> 1D array of byte[]
                byte[] img_1D = IImage.StreamFromImage(img);
                //send size to server
                NetworkStream ns = client.GetStream();
                byte[] size_of_img = BitConverter.GetBytes(img_1D.Length);//int -> 4byte
                ns.Write(size_of_img, 0, size_of_img.Length);
                //FileStream -> NetworkStream

                MemoryStream fs = new MemoryStream(img_1D);
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public delegate void pointer_to_funcation2(Image img);
        public void receiveImg(Form main, pointer_to_funcation2 f)
        {
            //Call Thread
            Thread t2 = new Thread(receiveImg2);
            t2.Start(new object[2] { main, f });//call the thread+params
        }

        private void receiveImg2(object obj)
        {
            //Thread Function
            object[] objs = (object[])obj;
            Form main = (Form)objs[0];
            pointer_to_funcation2 f = (pointer_to_funcation2)objs[1];
            try
            {
                NetworkStream ns = client.GetStream();
                byte[] size_buffer = new byte[4];
                ns.Read(size_buffer, 0, size_buffer.Length);
                int img_size = BitConverter.ToInt32(size_buffer, 0);

                byte[] img_1D = new byte[img_size];
                MemoryStream fs = new MemoryStream(img_1D);
               
                byte[] buffer = new byte[1024];
                int readbytes = 0;
                while (img_size > 0)
                {
                    readbytes = ns.Read(buffer, 0, buffer.Length);
                    img_size -= readbytes;
                    fs.Write(buffer, 0, readbytes);
                    buffer[0] = 1;
                    ns.Write(buffer, 0, 1);//msg to server [cont]
                }
                fs.Close();
                Image img = IImage.ImageFromStream(img_1D);
                main.Invoke(f, img);
            }
            catch
            {
                return;
            }
        }
    }
}
