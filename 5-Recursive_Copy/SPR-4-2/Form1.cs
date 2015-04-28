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
using System.IO;

namespace SPR_4_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] directory = Directory.GetLogicalDrives();
            listBox1.Items.AddRange(directory);
            listBox2.Items.AddRange(directory);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string path = listBox1.Items[index].ToString();

            string[] dir = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            listBox1.Items.Clear();
            listBox1.Items.AddRange(dir);
            listBox1.Items.AddRange(files);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox2.SelectedIndex;
            string path = listBox2.Items[index].ToString();

            string[] dir = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            listBox2.Items.Clear();
            listBox2.Items.AddRange(dir);
            listBox2.Items.AddRange(files);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index1 = listBox1.SelectedIndex;
            string path1 = listBox1.Items[index1].ToString();
            int index2 = listBox2.SelectedIndex;
            string path2 = listBox2.Items[index2].ToString();

            if (Directory.Exists(path1))
            {
                copyDirectory(path1,path2);
                return;
            }
            
            //C:\\tools\\easybcd.exe --> easybcd.exe
            string file_name = Path.GetFileName(path1);
            File.Copy(path1, path2 + "\\" + file_name);
        }

        private void copyDirectory(string path1, string path2)
        {
            string dir_name1 = Path.GetFileName(path1);
            Directory.CreateDirectory(path2 + "\\" + dir_name1);
            path2 += "\\" + dir_name1;
            
            string[] dir = Directory.GetDirectories(path1);
            string[] files = Directory.GetFiles(path1);

            for (int i = 0; i < files.Length; i++)
            {
                string file_name = Path.GetFileName(files[i]);
                File.Copy(files[i], path2 + "\\" + file_name);
            }
            for (int i = 0; i < dir.Length; i++)
            {
                string dir_name = Path.GetFileName(dir[i]);
                copyDirectory(dir[i], path2 + "\\" + dir_name);
            }
        }
    }
}
