// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="My Own">
//   
// </copyright>
// <summary>
//   Defines the Form1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScreenSaverWorkAround
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
            Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() => { ScreenSaverApp.App(); }, atp);
            }).ContinueWith(cont => { Console.WriteLine("Finished!"); });

            this.DoubleBuffered = true;
            
           this.timer.Interval = 5000;
           this.timer.Tick += this.timer_Tick;

           this.lblSaved.Visible = false;
           this.cboTimes.DataSource = this.GetTimes();
           this.LoadCheckBoxes();
           this.LoadSelectedTime();
        }

        private void LoadSelectedTime()
        {
            cboTimes.SelectedIndex = Properties.Settings.Default.SelectedTimeIndex;
        }

        private void LoadCheckBoxes()
        {
            chkSunday.Checked = Properties.Settings.Default.chkSunday;
            chkMonday.Checked = Properties.Settings.Default.chkMonday;
            chkTuesday.Checked = Properties.Settings.Default.chkTuesday;
            chkWednesday.Checked = Properties.Settings.Default.chkWednesday;
            chkThursday.Checked = Properties.Settings.Default.chkThursday;
            chkFriday.Checked = Properties.Settings.Default.chkFriday;
            chkSaturday.Checked = Properties.Settings.Default.chkSaturday;
        }

        private List<TimeSpan> GetTimes()
        {
            var returnValue = new List<TimeSpan>();
            for (TimeSpan time = new TimeSpan(0, 0, 0);
                 time < new TimeSpan(23, 59, 59);
                 time = time.Add(new TimeSpan(0, 15, 0)))
            {
                returnValue.Add(time);
            }

            return returnValue;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.chkSunday = this.chkSunday.Checked;
            Properties.Settings.Default.chkMonday = this.chkMonday.Checked;
            Properties.Settings.Default.chkTuesday = this.chkTuesday.Checked;
            Properties.Settings.Default.chkWednesday = this.chkWednesday.Checked;
            Properties.Settings.Default.chkThursday = this.chkThursday.Checked;
            Properties.Settings.Default.chkFriday = this.chkFriday.Checked;
            Properties.Settings.Default.chkSaturday = this.chkSaturday.Checked;

            if (this.cboTimes.SelectedItem != null)
            {
                Properties.Settings.Default.SelectedTimeIndex = this.cboTimes.SelectedIndex;
            }
            
            if (!String.IsNullOrEmpty(cboTimes.Text))
            {
                Properties.Settings.Default.SelectedTime = TimeSpan.Parse(cboTimes.Text);
            }

            Properties.Settings.Default.Save();

            FadeConfirmLabel();
        }

        private void FadeConfirmLabel()
        {
            lblSaved.Visible = true;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblSaved.Visible = false;
            timer.Stop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.ShowInTaskbar = this.WindowState != FormWindowState.Minimized;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private bool showBallonTipOnlyOnce = true;

        private bool contextMenuExitClicked = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!contextMenuExitClicked)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                if (showBallonTipOnlyOnce)
                {
                    this.notifyIcon1.ShowBalloonTip(
                        1000, string.Empty, "Screen Saver App Still Running", ToolTipIcon.Info);
                    showBallonTipOnlyOnce = false;
                }
                e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuExitClicked = true;
            this.Close();
        }
    }
}
