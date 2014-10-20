using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        string chooseDir ="";
        bool mes = true;
        public Form1()
        {
            InitializeComponent();
          }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                txtFileName.Text = File.ReadAllText(Application.StartupPath + "\\directive.txt");
            }
            catch (Exception)
            {
                File.Create(Application.StartupPath + "\\directive.txt"); 
            }
            finally
            {
                txtFileName.Text = File.ReadAllText("directive.txt");
                chooseDir = txtFileName.Text.Replace("\r\n", "");
                if (chooseDir != "")
                {
                    try
                    {
                        FileAttributes attr = File.GetAttributes(chooseDir);
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writeOpen();
                        }
                    }
                    catch (Exception)
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("Directory does not exist!");
                    }
                }
            }
        }

        private void getLastFile()
        {
            chooseDir = txtFileName.Text.Replace("\r\n", "");
            string lastFile = GetLastFileInDirectory(chooseDir, "*.csv");
            if (lastFile != string.Empty)
            {
                lblTemp.Text=readLastLine(chooseDir, lastFile);
            }
            else
            {
                timer1.Enabled = false;
                MessageBox.Show("Directory is empty!");
            }
        }

        private string readLastLine(string directory, string lastFile)
        {
            using (TextFieldParser tfp = new TextFieldParser(directory+"\\"+lastFile))
            {
                string[] last = new string[] {"","0","" };
                tfp.Delimiters = new string[] { "," };
                while (!tfp.EndOfData)
                {
                    last = tfp.ReadFields();
                }                    
                if (Convert.ToDouble(last[1]) >= 40)
                {
                    if (mes)
                    {
                        mes = false;
                        MessageBox.Show("Temperature is higher than 40°C!", "TEMPERATURE SENSOR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        mes = true;
                    }

                }
                return last[1];
            }
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFileName.Text = open.SelectedPath;
                chooseDir = open.SelectedPath;
                writeOpen();
            }
        }

        private void writeOpen()
        {
            string[] lines = { chooseDir };
            File.WriteAllLines(Application.StartupPath + "\\directive.txt", lines);
            getLastFile();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            notifyIcon1.Text = "Temperature: " + lblTemp.Text;
        }
    }
}
