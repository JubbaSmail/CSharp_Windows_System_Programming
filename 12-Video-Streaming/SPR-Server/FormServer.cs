using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
///////////////////////////////////
using MyNetwork;

namespace SPR_Server
{
    public partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
        }
        Net socket = new Net();
        private void Form1_Load(object sender, EventArgs e)
        {
            int port = 6060;
            socket.creartServer(port);
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
            socket.receiveMsg(this, msg_received);
        }

        public void msg_received(string msg)
        {
            listBox1.Items.Add("HIM: " + msg);
            socket.receiveMsg(this, msg_received);
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
        //10.4.44.73 :6060
        private void button5_Click(object sender, EventArgs e)
        {
            socket.receiveImg(this, received_img);
        }

        public void received_img(Image img)
        {
            pictureBox1.Image = img;
            pictureBox1.Refresh();
            socket.receiveImg(this, received_img);
        }
    }
}
