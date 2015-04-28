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
using System.IO;//Input Output

namespace MyNotepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //File -> Save
            //1. Open File Dialog
            saveFileDialog1.ShowDialog();//Open windows
            string path = saveFileDialog1.FileName;//File path
            //2. Create File
            //3. Text -> File
            string[] text = richTextBox1.Lines;
            File.WriteAllLines(path, text);//Create+Write+Close
            //4. Close File
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1- Open Dialog
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            // 2- Read File -> Text
            string[] text = File.ReadAllLines(path);
            richTextBox1.Lines = text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
