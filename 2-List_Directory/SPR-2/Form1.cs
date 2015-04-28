using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////////////////////////////
using System.IO;

namespace SPR_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("..");
            try
            {
                string[] folders = Directory.GetDirectories(comboBox1.Text);
                for (int i = 0; i < folders.Length; i++)
                {
                    listBox1.Items.Add(folders[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string[] drives = Directory.GetLogicalDrives();
            comboBox1.Items.AddRange(drives);
            comboBox1.SelectedIndex = 0;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string path = listBox1.Items[index].ToString();
            //MessageBox.Show(path);
            listBox1.Items.Clear();
            listBox1.Items.Add("..");
            try
            {
                string[] folders = Directory.GetDirectories(path);
                for (int i = 0; i < folders.Length; i++)
                {
                    listBox1.Items.Add(folders[i]);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
