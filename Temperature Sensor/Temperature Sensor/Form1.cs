using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temperature_Sensor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            getLastFile();
        }

        private void getLastFile()
        {
            string directory = @"C:\Users\pc\Documents\PCsensor\TEMPer V24.3\Data";
            string lastFile = GetLastFileInDirectory(directory, "*.csv");
            readLastLine(directory, lastFile);
        }

        private static void readLastLine(string directory, string lastFile)
        {
            if (lastFile != string.Empty)
            {
                using (TextFieldParser tfp = new TextFieldParser(directory+"\\"+lastFile))
                {
                    string[] last = new string[] { };
                    tfp.Delimiters = new string[] { "," };
                    while (!tfp.EndOfData)
                    {
                        last = tfp.ReadFields();
                    }
                    if (Convert.ToDouble(last[1]) >= 40)
                    {
                        MessageBox.Show("Temperature is higher than 40°C!","TEMPERATURE SENSOR",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else { MessageBox.Show("Directory is empty!"); }
        }
        public string GetLastFileInDirectory(string directory, string pattern)
        {
            if (directory.Trim().Length == 0)
                return string.Empty; //Error handler can go here

            if ((pattern.Trim().Length == 0) || (pattern.Substring(pattern.Length - 1) == "."))
                return string.Empty; //Error handler can go here

            if (Directory.GetFiles(directory, pattern).Length == 0)
                return string.Empty; //Error handler can go here
            
            var dirInfo = new DirectoryInfo(directory);
            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

            return file.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getLastFile();
        }

    }
}
