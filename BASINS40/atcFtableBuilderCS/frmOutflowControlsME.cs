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
            foreach (string lOCTypeName in FTableCalculator.OCTypeNames.Values)
            {
                cboOCTypes.Items.Add(lOCTypeName);
            }
            cboOCTypes.SelectedIndex = 0;

            PopulateOCTree();

            txtOCParm_0.TextChanged += new EventHandler(txtOCParamChanged);
            txtOCParm_1.TextChanged += new EventHandler(txtOCParamChanged);
            txtOCDisCoeff.TextChanged += new EventHandler(txtOCParamChanged);

            pLoaded = true;
        }

        private void cboOCTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            string lSelectedOCName = cboOCTypes.SelectedItem.ToString();
            string lName = "";
            FTableCalculator.ControlDeviceType lSelectedOCType = FTableCalculator.ControlDeviceType.None;
            foreach (FTableCalculator.ControlDeviceType lOCType in FTableCalculator.OCTypeNames.Keys)
            {
                if (FTableCalculator.OCTypeNames.TryGetValue(lOCType, out lName))
                {
                    if (lSelectedOCName == lName)
                    {
                        lSelectedOCType = lOCType;
                        break;
                    }
                }
            }

           Dictionary<string, double> lDefaults = null;
            switch (lSelectedOCType)
            {
                case FTableCalculator.ControlDeviceType.WeirTriVNotch:
                    lDefaults = FTableCalcOCWeirTriVNotch.ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.WeirTrapeCipolletti:
                    lDefaults = FTableCalcOCWeirTrapeCipolletti.ParamValueDefaults();
                   break;
                case FTableCalculator.ControlDeviceType.WeirRectangular:
                    lDefaults = FTableCalcOCWeirRectangular.ParamValueDefaults();
                   break;
                case FTableCalculator.ControlDeviceType.WeirBroadCrest:
                    lDefaults = FTableCalcOCWeirBroad.ParamValueDefaults();
                   break;
                case FTableCalculator.ControlDeviceType.OrificeUnderdrain:
                    lDefaults = FTableCalcOCOrificeUnderflow.ParamValueDefaults();
                   break;
                case FTableCalculator.ControlDeviceType.OrificeRiser:
                    lDefaults = FTableCalcOCOrificeRiser.ParamValueDefaults();
                   break;
            };

            List<string> lParamLbls = lDefaults.Keys.ToList<string>();
            List<double> lParamVals = lDefaults.Values.ToList<double>();

            lblOCParm0.Text = lParamLbls[0];
            lblOCParm1.Text = lParamLbls[1];

            txtOCParm_0.Text = lParamVals[0].ToString();
            txtOCParm_1.Text = lParamVals[1].ToString();
            txtOCDisCoeff.Text = lParamVals[2].ToString();

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
                    foreach (string lKey in lOCs.Keys) //FTableCalculator lOC in lOCs) 
                    { //add OCs
                        //lKey is like OC class name plus an id, eg. FTableOCWeirRectangular_2
                        FTableCalculator lOC = (FTableCalculator)lOCs.get_ItemByKey(lKey);
                        FTableCalculator.ControlDeviceType lCDType = lOC.ControlDevice;
                        switch (lCDType)
                        {
                            case FTableCalculator.ControlDeviceType.None:
                                break;
                            case FTableCalculator.ControlDeviceType.OrificeRiser:
                                break;
                            case FTableCalculator.ControlDeviceType.OrificeUnderdrain:
                                break;
                            case FTableCalculator.ControlDeviceType.WeirBroadCrest:
                                break;
                            case FTableCalculator.ControlDeviceType.WeirRectangular:
                                break;
                            case FTableCalculator.ControlDeviceType.WeirTrapeCipolletti:
                                break;
                            case FTableCalculator.ControlDeviceType.WeirTriVNotch:
                                break;
                        }
                        string className = lKey.Substring(0, lKey.IndexOf("_"));
                        string Ids = lKey.Substring(lKey.IndexOf("_") + 1);
                        int Id = 0;
                        int.TryParse(Ids, out Id);
                        if (className.StartsWith("FTableCalcOC"))
                        {
                            if (className.Contains("WeirBroad"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirBroadCrest, out lOCName);
                                lOC = (FTableCalcOCWeirBroad)lOCs.get_ItemByKey(lKey);
                            }
                            else if (className.Contains("WeirRect"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirRectangular, out lOCName);
                                lOC = (FTableCalcOCWeirRectangular)lOCs.get_ItemByKey(lKey);
                            }
                            else if (className.Contains("WeirTrape"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTrapeCipolletti, out lOCName);
                                lOC = (FTableCalcOCWeirTrapeCipolletti)lOCs.get_ItemByKey(lKey);
                            }
                            else if (className.Contains("WeirTriVNotch"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTriVNotch, out lOCName);
                                lOC = (FTableCalcOCWeirTriVNotch)lOCs.get_ItemByKey(lKey);
                            }
                            else if (className.Contains("OrificeRiser"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeRiser, out lOCName);
                                lOC = (FTableCalcOCOrificeRiser)lOCs.get_ItemByKey(lKey);
                            }
                            else if (className.Contains("OrificeUnderflow"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeUnderdrain, out lOCName);
                                lOC = (FTableCalcOCOrificeUnderflow)lOCs.get_ItemByKey(lKey);
                            }
                            TreeNode lOCNode = lExitNode.Nodes.Add(lKey, lOCName + "_" + Id.ToString());
                            //add parameter values
                            Dictionary<string, double> lParamValues = lOC.ParamValues();
                            List<string> lParamNames = lParamValues.Keys.ToList<string>();
                            List<double> lParamVals = lParamValues.Values.ToList<double>();
                            for (int z = 0; z < lParamNames.Count; z++)
                            {
                                lOCNode.Nodes.Add(lParamNames[z], lParamNames[z] + "(" + lParamVals[z] + ")");
                            }
                        }
                    }
                }
            }
        }

        private void txtOCParamChanged(object sender, EventArgs e)
        {
            double lParam0 = -99;
            double lParam1 = -99;
            double lParamDC = -99;

            if (double.TryParse(txtOCParm_0.Text, out lParam0) ||
                double.TryParse(txtOCParm_1.Text, out lParam1) ||
                double.TryParse(txtOCDisCoeff.Text, out lParamDC))
            {
                if (lParam0 <= 0 || lParam1 <= 0 || lParamDC <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid parameter(s) for control device." + Environment.NewLine +
                        "Parameter(s) value cannot be less than or equal to zero.", "Control Device Setup");
                    return;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid parameter(s) for control device.", "Control Device Setup");
                return;
            }
        }

        
    }
}
