using Overlay.NET.GTAV.Directx;
using System;
using System.Windows.Forms;

namespace VRSA_GTA_V
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            new DirectXOverlay().Start();
        }
    }
}
