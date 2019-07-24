using CefSharp;
using CefSharp.WinForms;
using Overlay.NET.GTAV.Directx;
using System;
using System.Windows.Forms;

namespace VRSA_GTA_V
{
    public partial class frmMain : Form
    {
        Timer t;

        ChromiumWebBrowser browser;

        string processName = "mspaint";

        public frmMain()
        {
            InitializeComponent();
            t = new Timer();
            t.Interval = 8 * 1000;
            t.Tick += T_Tick;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            new DirectXOverlay(processName).Start();

            Cef.Initialize(new CefSettings()
            {
                RemoteDebuggingPort = 8089
            });

            browser = new ChromiumWebBrowser("http://vrsa.epizy.com/")
            {
                Location = new System.Drawing.Point(0,0),
                Dock = DockStyle.Fill
            };

            pnlBrowser.Controls.Add(browser);

            this.BringToFront();
        }

        private void BtnCleanSession_Click(object sender, EventArgs e)
        {
            var success = ProcessManager.SuspendProcess(processName);
            if (success)
            {
                Text = "VRSA GTAV - Cleaning Session";
                t.Start();
            }
            else
            {
                MessageBox.Show("An error occured Suspending Process " + processName + ".exe");
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            var success = ProcessManager.ResumeProcess(processName);
            if (success)
            {
                Text = "VRSA GTAV - Session Clean";
                t.Stop();
            }
            else
            {
                MessageBox.Show("An error occured Resuming Process " + processName + ".exe");
            }
        }
    }
}
