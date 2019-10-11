using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BASINSDS
{
    public partial class MainForm : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl shell;
        public MainForm()
        {
            InitializeComponent();

            if (DesignMode) return;
            shell = this;
            appManager.LoadExtensions();
        }
    }
}
