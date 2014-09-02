using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace atcFtableBuilder
{
    public partial class frmOutflowControlsME : Form
    {
        private bool pLoaded = false;
        public frmOutflowControlsME()
        {
            InitializeComponent();
        }

        private void frmOutflowControlsME_Load(object sender, EventArgs e)
        {
            foreach (string lOCTypeName in clsGlobals.OCTypeNames.Values)
            {
                cboOCTypes.Items.Add(lOCTypeName);
            }
            cboOCTypes.SelectedIndex = 0;

            PopulateOCTree();

            pLoaded = true;
        }

        private void cboOCTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            string lSelectedOCName = cboOCTypes.SelectedItem.ToString();
            string lName = "";
            clsGlobals.OCTypes lSelectedOCType = clsGlobals.OCTypes.None;
            foreach (clsGlobals.OCTypes lOCType in clsGlobals.OCTypeNames.Keys)
            {
                if (clsGlobals.OCTypeNames.TryGetValue(lOCType, out lName))
                {
                    if (lSelectedOCName == lName)
                    {
                        lSelectedOCType = lOCType;
                        break;
                    }
                }
            }

            string lParmLbl0 = "";
            string lParmLbl1 = "";
            string lParmVal0 = "";
            string lParmVal1 = "";
            string lParmVal2 = "";
            switch (lSelectedOCType)
            {
                case clsGlobals.OCTypes.OCWeirTriVnotch:
                    lParmLbl0 = clsGlobals.gOCWeirTriVnotchLbl[0];
                    lParmLbl1 = clsGlobals.gOCWeirTriVnotchLbl[1];

                    lParmVal0 = clsGlobals.DefaultsWeirTriVnotch[0];
                    lParmVal1 = clsGlobals.DefaultsWeirTriVnotch[1];
                    lParmVal2 = clsGlobals.DefaultsWeirTriVnotch[2];

                    break;
                case clsGlobals.OCTypes.OCWeirTrape:
                    lParmLbl0 = clsGlobals.gOCWeirTrapeLbl[0];
                    lParmLbl1 = clsGlobals.gOCWeirTrapeLbl[1];

                    lParmVal0 = clsGlobals.DefaultsWeirTrape[0];
                    lParmVal1 = clsGlobals.DefaultsWeirTrape[1];
                    lParmVal2 = clsGlobals.DefaultsWeirTrape[2];

                    break;
                case clsGlobals.OCTypes.OCWeirRect:
                    lParmLbl0 = clsGlobals.gOCWeirRectLbl[0];
                    lParmLbl1 = clsGlobals.gOCWeirRectLbl[1];

                    lParmVal0 = clsGlobals.DefaultsWeirRect[0];
                    lParmVal1 = clsGlobals.DefaultsWeirRect[1];
                    lParmVal2 = clsGlobals.DefaultsWeirRect[2];

                    break;
                case clsGlobals.OCTypes.OCWeirBroad:
                    lParmLbl0 = clsGlobals.gOCWeirBroadLbl[0];
                    lParmLbl1 = clsGlobals.gOCWeirBroadLbl[1];
                    
                    lParmVal0 = clsGlobals.DefaultsWeirBroad[0];
                    lParmVal1 = clsGlobals.DefaultsWeirBroad[1];
                    lParmVal2 = clsGlobals.DefaultsWeirBroad[2];

                    break;
                case clsGlobals.OCTypes.OCOrificeUnd:
                    lParmLbl0 = clsGlobals.gOCOrificeUndLbl[0];
                    lParmLbl1 = clsGlobals.gOCOrificeUndLbl[1];

                    lParmVal0 = clsGlobals.DefaultsOrifice[0];
                    lParmVal1 = clsGlobals.DefaultsOrifice[1];
                    lParmVal2 = clsGlobals.DefaultsOrifice[2];

                    break;
                case clsGlobals.OCTypes.OCOrificeRiser:
                    lParmLbl0 = clsGlobals.gOCOrificeRiserLbl[0];
                    lParmLbl1 = clsGlobals.gOCOrificeRiserLbl[1];

                    lParmVal0 = clsGlobals.DefaultsOrifice[0];
                    lParmVal1 = clsGlobals.DefaultsOrifice[1];
                    lParmVal2 = clsGlobals.DefaultsOrifice[2];

                    break;
            };
            lblOCParm0.Text = lParmLbl0;
            lblOCParm1.Text = lParmLbl1;

            txtOCParm_0.Text = lParmVal0;
            txtOCParm_1.Text = lParmVal1;
            txtOCDisCoeff.Text = lParmVal2;

            if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
            {
                rdoExit2.Checked = true;
            }
            else
            {
                rdoExit1.Checked = true;
            }
        }

        private void PopulateOCTree()
        {
            string lOCName = "";
            for (int i = 1; i <= 5; i++)
            {
                if (clsGlobals.gExitOCSetup.Keys.Contains(i))
                { //add exit
                    TreeNode lExitNode = treeExitControls.Nodes.Add(i.ToString(), "Exit " +i.ToString());
                    atcUtility.atcCollection lOCs = (atcUtility.atcCollection) clsGlobals.gExitOCSetup.get_ItemByKey(i);
                    foreach (clsGlobals.OutflowControl lOC in lOCs)
                    { //add OCs
                        clsGlobals.OCTypeNames.TryGetValue(lOC.myOCType, out lOCName);
                        lExitNode.Nodes.Add(lOC.myOCType.ToString(), lOCName);

                        //add Parm values

                        //or just display them on the right hand side when clicked on


                    }


                }
            }
        }
    }
}
