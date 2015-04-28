using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////////////////////
using Microsoft.Win32;

namespace SRP_6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser;//HKEY_CURRENT_USER
            string [] keys = rk.GetSubKeyNames();
            listBox1.Items.AddRange(keys);
        }
        string global_key = "";
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string key = listBox1.Items[index].ToString();
            if(global_key != "")
                global_key += "\\"+ key;
            else
                global_key += key;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(global_key);

            string[] keys = rk.GetSubKeyNames();
            string[] value_names = rk.GetValueNames();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(keys);
            listBox2.Items.Clear();
            listBox2.Items.AddRange(value_names);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(global_key);
            int index = listBox2.SelectedIndex;
            string value_name = listBox2.Items[index].ToString();
            string value_data = rk.GetValue(value_name).ToString();
            string value_kind = rk.GetValueKind(value_name).ToString();

            MessageBox.Show(value_kind + " -- " + value_data);
        }
    }
}
