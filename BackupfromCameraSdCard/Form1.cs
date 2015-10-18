using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupfromCameraSdCard
{
    public partial class Form1 : Form, IHandleEvent
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


     

        private bool isBusy;

        private object locker = new object();

        private string Source = string.Empty;

        private void ReportProgress(CopyEvent info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(delegate
                {
                    ReportProgress(info);
                }));
                return;
            }
            this.progressBar1.PerformStep();
            richTextBox1.AppendText(string.Format("Copied='{2}' {0} => {1} ", info.Source, info.Dest, info.Copied));
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }


        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cboSrouce.SelectedItem == null)
            {
                MessageBox.Show("Please select a source", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sources = new SourceFilesBuilder(cboSrouce.SelectedItem.ToString());

            var destinations = new List<DestinationLocationBuilder>();
            if (!string.IsNullOrWhiteSpace(cboDest1.Text))
            {
                destinations.Add(new DestinationLocationBuilder(cboDest1.Text, txtCopyTo.Text));
            }

            if (!string.IsNullOrWhiteSpace(cboDest2.Text))
            {
                destinations.Add(new DestinationLocationBuilder(cboDest2.Text, txtCopyTo.Text));
            }


            var destination = new DestinationLocationBuilder(cboDest1.Text, txtCopyTo.Text);
            var destination2 = new DestinationLocationBuilder(cboDest2.Text, txtCopyTo.Text);

            this.btnCopy.Enabled = false;

            var copier = new FileCopyFacade(sources, destinations);

            progressBar1.Maximum = copier.FilesCount();
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            Thread thd_main = new Thread(copier.CopyAll);
            thd_main.Start();

            Thread thread3 = new Thread(delegate (object x)
            {
                while (thd_main.IsAlive)
                {
                    lock (this.locker)
                    {
                        Monitor.Wait(this.locker, TimeSpan.FromSeconds(2.0));
                    }
                }
                this.EnableBack();
            });
            lock (this.locker)
            {
                Monitor.Wait(this.locker, TimeSpan.FromSeconds(2.0));
            }
            thread3.Start();
        }



        private void EnableBack()
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new Action(delegate
                {
                    this.EnableBack();
                }));
                return;
            }
            this.btnCopy.Enabled = true;
            this.isBusy = false;
            MessageBox.Show(string.Format("Copied {0} files ", this.progressBar1.Maximum), "Completed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = this.isBusy;
            if (this.isBusy)
            {
                MessageBox.Show("Threads are still busy copying files, pls be patient!", "Calm down!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            base.OnClosing(e);
        }


        private void OnLoadAsync()
        {
           
            txtCopyTo.Text = DateTime.Now.ToString("ddMMMyyyy");

            DriveInfo.GetDrives()
                .Where(x => x.IsReady && x.DriveType == DriveType.Removable && x.DriveType != DriveType.CDRom)
                            .Where(x => x.DriveFormat.Equals("FAT32", StringComparison.CurrentCultureIgnoreCase))
                            .Where(x => x.RootDirectory.GetDirectories("DCIM", SearchOption.AllDirectories).Any())
                 .ToList()
                 .ForEach(x => cboSrouce.Items.Add(x.RootDirectory.Name));


            var drives = DriveInfo.GetDrives()
               .Where(x => x.IsReady && x.DriveType != DriveType.CDRom)
                .Where(x => !x.DriveFormat.Equals("FAT32", StringComparison.CurrentCultureIgnoreCase))
                .Where(x => File.Exists(Path.Combine(x.Name, "CAMERA_FILE_COPIER.exe")))
                          .ToList();

            cboDest1.Items.Clear();
            cboDest2.Items.Clear();

            if (drives.Any())
            {

                cboDest1.Text = Path.Combine(drives.First().Name, "Camera");
                if (drives.Count > 1)
                {
                    cboDest2.Text = Path.Combine(drives.Skip(1).First().Name, "Camera");
                }
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            Broadcaster.GetObject().Register(this);

           
            this.OnLoadAsync();
            base.OnLoad(e);
        }



        public void Notify(CopyEvent info)
        {
            this.ReportProgress(info);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            OnLoadAsync();
        }
    }
}