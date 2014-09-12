using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
                rdoExit1.Enabled = false;
            else
                rdoExit1.Enabled = true;

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
            if (lSelectedOCName == "None") return;
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
            FTableCalculator lCalc = null;
            switch (lSelectedOCType)
            {
                case FTableCalculator.ControlDeviceType.WeirTriVNotch:
                    lCalc = new FTableCalcOCWeirTriVNotch();
                    lDefaults = ((FTableCalcOCWeirTriVNotch)lCalc).ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.WeirTrapeCipolletti:
                    lCalc = new FTableCalcOCWeirTrapeCipolletti();
                    lDefaults = ((FTableCalcOCWeirTrapeCipolletti)lCalc).ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.WeirRectangular:
                    lCalc = new FTableCalcOCWeirRectangular();
                    lDefaults = ((FTableCalcOCWeirRectangular)lCalc).ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.WeirBroadCrest:
                    lCalc = new FTableCalcOCWeirBroad();
                    lDefaults = ((FTableCalcOCWeirBroad)lCalc).ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.OrificeUnderdrain:
                    lCalc = new FTableCalcOCOrificeUnderflow();
                    lDefaults = ((FTableCalcOCOrificeUnderflow)lCalc).ParamValueDefaults();
                    break;
                case FTableCalculator.ControlDeviceType.OrificeRiser:
                    lCalc = new FTableCalcOCOrificeRiser();
                    lDefaults = ((FTableCalcOCOrificeRiser)lCalc).ParamValueDefaults();
                    break;
            };

            List<string> lParamLbls = lDefaults.Keys.ToList<string>();
            List<double> lParamVals = lDefaults.Values.ToList<double>();

            lblOCParm0.Text = lParamLbls[0];
            lblOCParm1.Text = lParamLbls[1];

            txtOCParm_0.Text = lParamVals[0].ToString();
            txtOCParm_1.Text = lParamVals[1].ToString();
            txtOCDisCoeff.Text = lParamVals[2].ToString();

            //if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
            //{
            //    rdoExit2.Checked = true;
            //}
            //else
            //{
            //    rdoExit1.Checked = true;
            //}
        }

        private void PopulateOCTree()
        {
            for (int i = 1; i <= 5; i++)
            {
                //add Exit node
                string lExitNodeKey = i.ToString();
                string lExitNodeText = "Exit " + i.ToString();
                TreeNode lExitNodeUI = null;
                if (treeExitControls.Nodes.ContainsKey(lExitNodeKey))
                    lExitNodeUI = treeExitControls.Nodes[lExitNodeKey];
                else
                    lExitNodeUI = treeExitControls.Nodes.Add(lExitNodeKey, lExitNodeText);

                //add control device node(s)
                if (i == 1 && clsGlobals.gToolType == clsGlobals.ToolType.Gray)
                {
                    string lOutflowKey = "Outflow1";
                    TreeNode lOutflowNode = lExitNodeUI.Nodes.Add(lOutflowKey, lOutflowKey);
                    continue;
                }

                //add OC nodes
                TreeNode lExitNodeProg = clsGlobals.gExitOCSetup[i - 1];
                if (lExitNodeProg == null)
                    continue;

                foreach (TreeNode lOCNode in lExitNodeProg.Nodes)
                {
                    TreeNode lOCNodeUI = new TreeNode();
                    lOCNodeUI.Text = lOCNode.Text;
                    lExitNodeUI.Nodes.Add(lOCNodeUI);

                    foreach (TreeNode lOCParamNode in lOCNode.Nodes)
                    {
                        TreeNode lOCParamNodeUI = new TreeNode();
                        lOCParamNodeUI.Text = lOCParamNode.Text;
                        lOCNodeUI.Nodes.Add(lOCParamNodeUI);
                    }
                }
            }
        }

        private void txtOCParamChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (sender == null) return;

            double lParam0 = -99;

            TextBox lTB = (TextBox)sender;

            if (double.TryParse(lTB.Text, out lParam0))
            {
                if (lParam0 <= 0)
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string lErrMsg = ParamsOK();
            if (!string.IsNullOrEmpty(lErrMsg))
            {
                System.Windows.Forms.MessageBox.Show("Please address the following error before proceeding:" + Environment.NewLine + lErrMsg,
                    "Adding New Control Devices");
                return;
            }


            int lSelectedExit = SelectedExit();
            FTableCalculator lCalc = GetControlDeviceFromForm();

            if (lCalc == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a proper control device type.", "Adding New Control Devices");
                return;
            }

            //Add OC node
            string lOCCommonName = FTableCalculator.OCTypeNames[lCalc.ControlDevice];
            int typeCounter = -99;
            TreeNode lExitNode = treeExitControls.Nodes[lSelectedExit - 1];
            foreach (TreeNode lOCNode in lExitNode.Nodes)
            {
                string lOCAlias = lOCNode.Text.Substring(0, lOCNode.Text.IndexOf("_"));
                string lOCId = lOCNode.Text.Substring(lOCNode.Text.IndexOf("_") + 1);
                int liOCId = int.Parse(lOCId);

                if (lOCAlias == lOCCommonName)
                {
                    if (typeCounter < liOCId)
                        typeCounter = liOCId;
                }
            }

            if (typeCounter < 0)
                typeCounter = 0;
            else
                typeCounter ++;

            string lnewOCName = lOCCommonName + "_" + typeCounter.ToString();

            TreeNode lnewOCNode = lExitNode.Nodes.Add(lnewOCName);

            //Add OC's param nodes
            Dictionary<string, double> lParams = lCalc.ParamValues();
            List<string> lParamNames = lParams.Keys.ToList<string>();
            List<double> lParamValues = lParams.Values.ToList<double>();
            foreach (string lParamName in lParams.Keys)
            {
                TreeNode lParamNode = new TreeNode();
                lParamNode.Text = lParamName + "(" + lParams[lParamName].ToString() + ")";
                lnewOCNode.Nodes.Add(lParamNode);
            }

            //Refresh tree
            //AddOCtoTree(lCalc);
            AdjustNodeNames(lExitNode);
        }

        private void AdjustNodeNames(TreeNode aExitNode)
        {
            if (aExitNode == null || !aExitNode.Text.StartsWith("Exit")) return;
            if (aExitNode.Nodes.Count == 0) return;

            atcUtility.atcCollection lCDTypes = new atcUtility.atcCollection();
            foreach (TreeNode lCDNode in aExitNode.Nodes)
            {
                string lCDName = lCDNode.Text.Substring(0, lCDNode.Text.IndexOf("_"));
                if (!lCDTypes.Keys.Contains(lCDName)) lCDTypes.Add(lCDName, 0);
            }
            foreach (TreeNode lCDNode in aExitNode.Nodes)
            {
                string lCDName = lCDNode.Text.Substring(0, lCDNode.Text.IndexOf("_"));
                int lId = (int)lCDTypes.get_ItemByKey(lCDName);
                lId++;
                string lCDNameNew = lCDName + "_" + lId.ToString();
                lCDNode.Text = lCDNameNew;
                lCDTypes.set_ItemByKey(lCDName, lId);
            }
        }

        private bool AddOCtoTree(FTableCalculator aCalc)
        {
            bool lCalcAdded = false;

            FTableCalculator.ControlDeviceType lOCType = aCalc.ControlDevice;
            int lExit = aCalc.myExit; 
            Dictionary<string, double> lParamValues = aCalc.ParamValues();
            string lOCDisplayName = "";
            FTableCalculator.OCTypeNames.TryGetValue(aCalc.ControlDevice, out lOCDisplayName);

            TreeNode lExitNode = null;
            if (treeExitControls.Nodes.ContainsKey(lExit.ToString())) 
                lExitNode = treeExitControls.Nodes[lExit.ToString()]; //.Add(lKey, lOCName + "_" + Id.ToString());
            else
                lExitNode = treeExitControls.Nodes.Add(lExit.ToString(), "Exit " + lExit.ToString());

            //add OC node
            string lOCNameClass = aCalc.GetType().Name;
            int Ctr = 0;
            string lOCNodeName = lOCDisplayName + "_" + Ctr.ToString();
            string lOCNodeKey = lOCNameClass + "_" + Ctr.ToString();
            while (lExitNode.Nodes.ContainsKey(lOCNodeKey))
            {
                Ctr++;
                lOCNodeKey = lOCNameClass + "_" + Ctr.ToString();
                lOCNodeName = lOCDisplayName + "_" + Ctr.ToString();
            }
            TreeNode lOCNode = lExitNode.Nodes.Add(lOCNodeKey, lOCNodeName);

            //add parameter value nodes
            List<string> lParamNames = lParamValues.Keys.ToList<string>();
            List<double> lParamVals = lParamValues.Values.ToList<double>();
            for (int z = 0; z < lParamNames.Count; z++)
            {
                lOCNode.Nodes.Add(lParamNames[z], lParamNames[z] + "(" + lParamVals[z] + ")");
            }
            lCalcAdded = true;

            return lCalcAdded;
        }

        private string ParamsOK()
        {
            string lErrMsg = "";
            if (cboOCTypes.SelectedItem.ToString() == "None") lErrMsg = "Need to select a control device first." + Environment.NewLine;

            double lParam0 = -99;
            double lParam1 = -99;
            double lParamDC = -99;

            if (double.TryParse(txtOCParm_0.Text, out lParam0) &&
                double.TryParse(txtOCParm_1.Text, out lParam1) &&
                double.TryParse(txtOCDisCoeff.Text, out lParamDC))
            {
                if (lParam0 <= 0 || lParam1 <= 0 || lParamDC <= 0)
                {
                    lErrMsg += "Parameter(s) value cannot be less than or equal to zero." + Environment.NewLine;
                }
            }
            else
            {
                lErrMsg += "Parameter value needs to be numeric." + Environment.NewLine;
            }

            if (!rdoExit1.Checked && !rdoExit2.Checked && !rdoExit3.Checked && !rdoExit4.Checked && !rdoExit5.Checked)
                lErrMsg += "Need to select an exit.";

            return lErrMsg;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboOCTypes.SelectedIndex = 0;
            txtOCParm_0.Text = "";
            txtOCParm_1.Text = "";
            txtOCDisCoeff.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            TreeNode lSelectedNode = treeExitControls.SelectedNode;
            if (lSelectedNode == null || lSelectedNode.Text.StartsWith("Exit") || !lSelectedNode.Parent.Text.StartsWith("Exit") )
            {
                System.Windows.Forms.MessageBox.Show("Select a Control Device for update.", "Update Control Device");
                return;
            }
            string lSelectedOCAlias = lSelectedNode.Text.Substring(0, lSelectedNode.Text.IndexOf("_"));
            int lSelectedOCIndex = int.Parse(lSelectedNode.Text.Substring(lSelectedNode.Text.IndexOf("_") + 1));
            string lParentNodeName = lSelectedNode.Parent.Text;
            string lExitNo = lParentNodeName.Substring(lParentNodeName.IndexOf(" ") + 1);
            int lExitNum = int.Parse(lExitNo);

            FTableCalculator lCalc = GetControlDeviceFromForm();
            if (lSelectedOCAlias == FTableCalculator.OCTypeNames[lCalc.ControlDevice].ToString())
            {
                Dictionary<string, double> lParams = lCalc.ParamValues();
                double lVal = 0;
                foreach (TreeNode lParamNode in lSelectedNode.Nodes)
                {
                    string lParamName = lParamNode.Text.Substring(0, lParamNode.Text.IndexOf("("));
                    lVal = lParams[lParamName];
                    lParamNode.Text = lParamName + "(" + lVal.ToString() + ")";
                }
                this.treeExitControls.MouseUp -= this.treeExitControls_MouseUp;
                AdjustNodeNames(lSelectedNode.Parent);
                lSelectedNode.Expand();
                treeExitControls.SelectedNode = lSelectedNode;
                this.treeExitControls.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeExitControls_MouseUp);
            }
            if (lExitNum != SelectedExit())
            {
                TreeNode lNewNode = new TreeNode();
                lNewNode.Text = lSelectedNode.Text;
                foreach (TreeNode lParamNode in lSelectedNode.Nodes)
                {
                    TreeNode lnewParamNode = new TreeNode();
                    lnewParamNode.Text = lParamNode.Text;
                    lNewNode.Nodes.Add(lnewParamNode);
                }
                lSelectedNode.Remove();

                foreach (TreeNode lExitNode in treeExitControls.Nodes)
                {
                    if (lExitNode.Text.Contains(SelectedExit().ToString()))
                    {
                        lExitNode.Nodes.Add(lNewNode);
                        break;
                    }
                }

                this.treeExitControls.MouseUp -= this.treeExitControls_MouseUp;
                AdjustNodeNames(lNewNode.Parent);
                lNewNode.Expand();
                treeExitControls.SelectedNode = lNewNode;
                this.treeExitControls.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeExitControls_MouseUp);
            }
        }

        private int SelectedExit()
        {
            if (rdoExit1.Checked)
                return 1;
            else if (rdoExit2.Checked)
                return 2;
            else if (rdoExit3.Checked)
                return 3;
            else if (rdoExit4.Checked)
                return 4;
            else if (rdoExit5.Checked)
                return 5;
            else
                return 0;
        }

        private FTableCalculator GetControlDeviceFromForm()
        {
            List<FTableCalculator.ControlDeviceType> listOCTypes = FTableCalculator.OCTypeNames.Keys.ToList<FTableCalculator.ControlDeviceType>();
            List<string> listOCCommonNames = FTableCalculator.OCTypeNames.Values.ToList<string>();
            FTableCalculator.ControlDeviceType lSelectedOCType = FTableCalculator.ControlDeviceType.None;
            for (int i = 0; i < listOCTypes.Count; i++)
            {
                if (cboOCTypes.SelectedItem.ToString() == listOCCommonNames[i])
                {
                    lSelectedOCType = listOCTypes[i];
                    break;
                }
            }

            int lSelectedExit = SelectedExit();

            double lParam0 = 0;
            double lParam1 = 0;
            double lDisCoe = 0;

            double.TryParse(txtOCParm_0.Text, out lParam0);
            double.TryParse(txtOCParm_1.Text, out lParam1);
            double.TryParse(txtOCDisCoeff.Text, out lDisCoe);

            FTableCalculator lCalc = null;
            switch (lSelectedOCType)
            {
                case FTableCalculator.ControlDeviceType.OrificeRiser:
                    lCalc = new FTableCalcOCOrificeRiser();
                    ((FTableCalcOCOrificeRiser)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCOrificeRiser)lCalc).OrificePipeDiameter = lParam0;
                    ((FTableCalcOCOrificeRiser)lCalc).OrificeDepth = lParam1;
                    ((FTableCalcOCOrificeRiser)lCalc).OrificeDischargeCoefficient = lDisCoe;
                    break;
                case FTableCalculator.ControlDeviceType.OrificeUnderdrain:
                    lCalc = new FTableCalcOCOrificeUnderflow();
                    ((FTableCalcOCOrificeUnderflow)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCOrificeUnderflow)lCalc).OrificePipeDiameter = lParam0;
                    ((FTableCalcOCOrificeUnderflow)lCalc).OrificeInvertDepth = lParam1;
                    ((FTableCalcOCOrificeUnderflow)lCalc).OrificeDischargeCoefficient = lDisCoe;
                    break;
                case FTableCalculator.ControlDeviceType.WeirBroadCrest:
                    lCalc = new FTableCalcOCWeirBroad();
                    ((FTableCalcOCWeirBroad)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCWeirBroad)lCalc).WeirWidth = lParam0;
                    ((FTableCalcOCWeirBroad)lCalc).WeirInvert = lParam1;
                    ((FTableCalcOCWeirBroad)lCalc).DischargeCoefficient = lDisCoe;
                    break;
                case FTableCalculator.ControlDeviceType.WeirRectangular:
                    lCalc = new FTableCalcOCWeirRectangular();
                    ((FTableCalcOCWeirRectangular)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCWeirRectangular)lCalc).WeirWidth = lParam0;
                    ((FTableCalcOCWeirRectangular)lCalc).WeirInvert = lParam1;
                    ((FTableCalcOCWeirRectangular)lCalc).DischargeCoefficient = lDisCoe;
                    break;
                case FTableCalculator.ControlDeviceType.WeirTrapeCipolletti:
                    lCalc = new FTableCalcOCWeirTrapeCipolletti();
                    ((FTableCalcOCWeirTrapeCipolletti)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCWeirTrapeCipolletti)lCalc).WeirWidth = lParam0;
                    ((FTableCalcOCWeirTrapeCipolletti)lCalc).WeirInvert = lParam1;
                    ((FTableCalcOCWeirTrapeCipolletti)lCalc).DischargeCoefficient = lDisCoe;
                    break;
                case FTableCalculator.ControlDeviceType.WeirTriVNotch:
                    lCalc = new FTableCalcOCWeirTriVNotch();
                    ((FTableCalcOCWeirTriVNotch)lCalc).myExit = lSelectedExit;
                    ((FTableCalcOCWeirTriVNotch)lCalc).WeirAngle = lParam0;
                    ((FTableCalcOCWeirTriVNotch)lCalc).WeirInvert = lParam1;
                    ((FTableCalcOCWeirTriVNotch)lCalc).DischargeCoefficient = lDisCoe;
                    break;
            }
            return lCalc;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!pLoaded) return;

            TreeNode lSelectedNode = treeExitControls.SelectedNode;
            if (lSelectedNode == null || lSelectedNode.Text.StartsWith("Exit") || !lSelectedNode.Parent.Text.StartsWith("Exit"))
            {
                System.Windows.Forms.MessageBox.Show("Select a Control Device to delete.", "Delete Control Device");
                return;
            }

            lSelectedNode.Remove();
        }

        private void treeExitControls_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Point where mouse is clicked
            System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);
            // Go to the node that the user clicked
            TreeNode lSelectedNode = treeExitControls.GetNodeAt(p);

            if (!(lSelectedNode == null))
            {
                if (lSelectedNode.Text.StartsWith("Exit")) return;
                if (!lSelectedNode.Parent.Text.StartsWith("Exit")) return;
            }
            else
                return;

            treeExitControls.SelectedNode = lSelectedNode;

            string lExitNodeName = lSelectedNode.Parent.Text;
            string lExitNo = lExitNodeName.Substring(lExitNodeName.IndexOf(" ") + 1);
            switch (lExitNo)
            {
                case "1": rdoExit1.Checked = true; break; 
                case "2": rdoExit2.Checked = true; break; 
                case "3": rdoExit3.Checked = true; break; 
                case "4": rdoExit4.Checked = true; break; 
                case "5": rdoExit5.Checked = true; break; 
            }

            string lCDAlias = lSelectedNode.Text.Substring(0, lSelectedNode.Text.IndexOf("_"));
            this.cboOCTypes.SelectedIndexChanged -= this.cboOCTypes_SelectedIndexChanged;
            for (int i = 0; i < cboOCTypes.Items.Count; i++)
            {
                if (cboOCTypes.Items[i].ToString() == lCDAlias)
                {
                    cboOCTypes.SelectedIndex = i;
                    break;
                }
            }
            this.cboOCTypes.SelectedIndexChanged += new System.EventHandler(this.cboOCTypes_SelectedIndexChanged);

            Regex lReg = new Regex(@"\(([^\)]*)\)");
            foreach (TreeNode lParamNode in lSelectedNode.Nodes)
            {
                MatchCollection lMatches = lReg.Matches(lParamNode.Text);
                string lsValue = lMatches[0].Groups[0].Value;
                lsValue = lsValue.TrimStart(new char[]{'('});
                lsValue = lsValue.TrimEnd(new char[]{')'});

                string lParamName = lParamNode.Text.Substring(0, lParamNode.Text.IndexOf("("));
                if (lParamName == lblOCParm0.Text)
                    txtOCParm_0.Text = lsValue;
                else if (lParamName == lblOCParm1.Text)
                    txtOCParm_1.Text = lsValue;
                else if (lParamName == lblOCParm3.Text)
                    txtOCDisCoeff.Text = lsValue;
            }
        }

        private void frmOutflowControlsME_FormClosing(object sender, FormClosingEventArgs e)
        {
            Array.Clear(clsGlobals.gExitOCSetup, 0, clsGlobals.gExitOCSetup.Length);
            treeExitControls.Nodes.CopyTo(clsGlobals.gExitOCSetup, 0);
        }
    }
}
