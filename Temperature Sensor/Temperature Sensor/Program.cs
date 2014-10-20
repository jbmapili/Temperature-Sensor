using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temperature_Sensor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName(
                       System.IO.Path.GetFileNameWithoutExtension(
                           System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1
                   )
            {
                MessageBox.Show("すでにプログラムが動作中です。", "多重起動エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else 
            { 
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.Contains("TEMPerV21"))
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    }
                }
                MessageBox.Show("Temperature sensor is close!");
            }
        }
    }
}
