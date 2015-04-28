using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////////////////////////
using System.IO;

namespace SPR_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] drives = Directory.GetLogicalDrives();//C:  D:  E: WIN API
            listBox1.Items.AddRange(drives);
            listBox2.Items.AddRange(drives);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, MouseEventArgs e)
        {
            int index = listBox1.SelectedIndex;//UI
            string path = listBox1.Items[index].ToString();//UI
            //////////////////////////////////////
            string[] dirs = Directory.GetDirectories(path);//WIN API
            string[] files = Directory.GetFiles(path);//WIN API
            //////////////////////////////////////
            listBox1.Items.Clear();
            listBox1.Items.AddRange(dirs); //UI
            listBox1.Items.AddRange(files);//UI
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox2.SelectedIndex;//UI
            string path = listBox2.Items[index].ToString();//UI
            //////////////////////////////////////
            string[] dirs = Directory.GetDirectories(path);//WIN API
            string[] files = Directory.GetFiles(path);//WIN API
            //////////////////////////////////////
            listBox2.Items.Clear();
            listBox2.Items.AddRange(dirs); //UI
            listBox2.Items.AddRange(files);//UI
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index1 = listBox1.SelectedIndex;//UI
            string path1 = listBox1.Items[index1].ToString();//UI

            int index2 = listBox2.SelectedIndex;//UI
            string path2 = listBox2.Items[index2].ToString();//UI
            string filename = Path.GetFileName(path1);
            File.Copy(path1, path2 + "\\" + filename);
        }
    }
}
