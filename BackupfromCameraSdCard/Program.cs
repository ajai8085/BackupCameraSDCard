using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupfromCameraSdCard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string str = "filecopiermainentrypointsigleinstancekey311343e1360d466c92d2340a5217dba9";
            using (Mutex mutex = new Mutex(false, "Global\\" + str))
            {
                if (!mutex.WaitOne(10, false))
                {
                    MessageBox.Show("Already running !", "Instance already running", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}
