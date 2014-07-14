using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HSPFftable
{
    public partial class frmChoice : Form
    {
        public frmChoice()
        {
            InitializeComponent();
        }

        private void rdoToolGray_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoToolGray.Checked)
                atcFtableBuilder.clsGlobals.gToolType = atcFtableBuilder.clsGlobals.ToolType.Gray;
        }

        private void rdoToolGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoToolGreen.Checked)
                atcFtableBuilder.clsGlobals.gToolType = atcFtableBuilder.clsGlobals.ToolType.Green;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (rdoToolGray.Checked || rdoToolGreen.Checked)
                this.Close();
        }
    }
}
