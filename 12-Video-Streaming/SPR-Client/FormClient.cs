using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
////////////////////////////
using MyNetwork;

namespace SPR_Client
{
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
        }
        Net socket = new Net();
        private void Form1_Load(object sender, EventArgs e)
        {
            string ip = "127.0.0.1";
            int port = 6060;
            socket.createClient(ip, port);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Send
            string msg = textBox1.Text;
            socket.sendMsg(msg);
            listBox1.Items.Add("YOU: " + msg);
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Receive
            string msg = socket.receiveMsg();
            listBox1.Items.Add("HIM: " + msg);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Send File
            openFileDialog1.ShowDialog();
            string file = openFileDialog1.FileName;
            socket.sendFile(file);
            MessageBox.Show("Done");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Recive File
            saveFileDialog1.ShowDialog();
            string file = saveFileDialog1.FileName;
            socket.receiveFile(file);
            MessageBox.Show("Done");
        }

        IWebCam webCam = null;
        private void button5_Click(object sender, EventArgs e)
        {
            if (webCam == null)
            {
                webCam = new IWebCam(this.Handle);
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Image img = webCam.iWebCam_Image;
            if (img != null)
            {
                pictureBox1.Image = img;
                socket.sendImg(img);
            }
        }
    }
}
