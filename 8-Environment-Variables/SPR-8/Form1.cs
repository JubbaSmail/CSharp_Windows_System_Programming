using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/////////////////////////////
using System.Collections;
using System.IO;

namespace SPR_8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void ENV2FILE()
        {
            //Environment.GetEnvironmentVariables()
            //Dictionary<string, string> stds = new Dictionary<string, string>();
            //stds.Add("Ahmad", "99");
            //stds.Add("Samer", "98");
            //stds.Add("Walid", "97");
            //stds.Add("Rama", "96");
            //MessageBox.Show(stds["Ahmad"].ToString());
            //int x = 1; object x2 = 1;
            //var z = "String"; object z2 = "String";
            //foreach (KeyValuePair<string,string> item in stds)
            //{
            //    MessageBox.Show(item.Key + "," + item.Value);
            //}
            //Hashtable ht = new Hashtable(
            //    new Dictionary<string, int>()
            //    );
            //ht.Add("Ahmad", 75);
            //ht.Add("Samer", 59);
            //ht.Add("Walid", 38);
            //ht.Add("Rama", 0);
            Hashtable env = (Hashtable)Environment.GetEnvironmentVariables();
            string[] keys_val = new string[1000];
            int index=0;
            foreach (DictionaryEntry item in env)
                keys_val[index++] = item.Key + "---->" + item.Value;
            File.WriteAllLines("x.txt", keys_val);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FILE2ENV();
        }

        public void FILE2ENV()
        {
            string[] key_vals = File.ReadAllLines("x.txt");
            for (int i = 0; i < key_vals.Length; i++)
            {
                string key_val = key_vals[i];
                string[] split = new string[1] { "-->"};
                string key = key_val.Split(split,StringSplitOptions.RemoveEmptyEntries)[0];
                string value = key_val.Split(split, StringSplitOptions.RemoveEmptyEntries)[1];
                Environment.SetEnvironmentVariable(key, value,EnvironmentVariableTarget.User);
            }
            MessageBox.Show("Done");
        }

    }
}
