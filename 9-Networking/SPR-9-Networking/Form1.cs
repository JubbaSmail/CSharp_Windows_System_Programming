using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
///////////////////////////////
using System.Net.Sockets;

namespace SPR_9_Networking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient client;
        private void Form1_Load(object sender, EventArgs e)
        {
            int port = 5555;
            TcpListener server = new TcpListener(port);
            server.Start();
            client = server.AcceptTcpClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Send
            NetworkStream ns = client.GetStream();
            UnicodeEncoding ue = new UnicodeEncoding();// String => Byte  &&  Byte => String
            string msg = textBox1.Text;
            byte[] stream = ue.GetBytes(msg);
            ns.Write(stream, 0, stream.Length);
            listBox1.Items.Add("Server: " + msg);
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Receive
            NetworkStream ns = client.GetStream();
            UnicodeEncoding ue = new UnicodeEncoding();// String => Byte  &&  Byte => String
            byte[] stream = new byte[100];
            ns.Read(stream, 0, stream.Length);
            string msg = ue.GetString(stream);
            listBox1.Items.Add("Client: " + msg);
        }
    }
}
