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
            string lOCName = "";
            for (int i = 1; i <= 5; i++)
            {
                //add Exit node
                string lExitNodeKey = i.ToString();
                string lExitNodeText = "Exit " + i.ToString();
                TreeNode lExitNode = null;
                if (treeExitControls.Nodes.ContainsKey(lExitNodeKey))
                    lExitNode = treeExitControls.Nodes[lExitNodeKey];
                else
                    lExitNode = treeExitControls.Nodes.Add(lExitNodeKey, lExitNodeText);

                if (!clsGlobals.gExitOCSetup.Keys.Contains(i))
                    clsGlobals.gExitOCSetup.Add(i, new atcUtility.atcCollection());

                //add control device node(s)
                if (i == 1 && clsGlobals.gToolType == clsGlobals.ToolType.Gray)
                {
                    string lOutflowKey = "Outflow1";
                    TreeNode lOutflowNode = lExitNode.Nodes.Add(lOutflowKey, lOutflowKey);

                    atcUtility.atcCollection lOCs = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(i);
                    if (!lOCs.Keys.Contains(lOutflowKey))
                        lOCs.Add(lOutflowKey, lOutflowKey);

                    continue;
                }

                if (clsGlobals.gExitOCSetup.Keys.Contains(i))
                { //add exit

                    atcUtility.atcCollection lOCs = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(i);
                    //lExitNode.Text += " (" + lOCs.Count + " CDs)";

                    foreach (string lKey in lOCs.Keys) //FTableCalculator lOC in lOCs) 
                    { //add OCs
                        //lKey is like OC class name plus an id, eg. FTableOCWeirRectangular_2
                        FTableCalculator lOC = (FTableCalculator)lOCs.get_ItemByKey(lKey);
                        string className = lKey.Substring(0, lKey.IndexOf("_"));
                        string Ids = lKey.Substring(lKey.IndexOf("_") + 1);
                        int Id = 0;
                        int.TryParse(Ids, out Id);
                        if (className.StartsWith("FTableCalcOC"))
                        {
                            Dictionary<string, double> lParamValues = null;
                            if (className.Contains("WeirBroad"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirBroadCrest, out lOCName);
                                lOC = (FTableCalcOCWeirBroad)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCWeirBroad)lOC).ParamValues();
                            }
                            else if (className.Contains("WeirRect"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirRectangular, out lOCName);
                                lOC = (FTableCalcOCWeirRectangular)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCWeirRectangular)lOC).ParamValues();
                            }
                            else if (className.Contains("WeirTrape"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTrapeCipolletti, out lOCName);
                                lOC = (FTableCalcOCWeirTrapeCipolletti)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCWeirTrapeCipolletti)lOC).ParamValues();
                            }
                            else if (className.Contains("WeirTriVNotch"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTriVNotch, out lOCName);
                                lOC = (FTableCalcOCWeirTriVNotch)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCWeirTriVNotch)lOC).ParamValues();
                            }
                            else if (className.Contains("OrificeRiser"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeRiser, out lOCName);
                                lOC = (FTableCalcOCOrificeRiser)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCOrificeRiser)lOC).ParamValues();
                            }
                            else if (className.Contains("OrificeUnderflow"))
                            {
                                FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeUnderdrain, out lOCName);
                                lOC = (FTableCalcOCOrificeUnderflow)lOCs.get_ItemByKey(lKey);
                                lParamValues = ((FTableCalcOCOrificeUnderflow)lOC).ParamValues();
                            }
                            TreeNode lOCNode = lExitNode.Nodes.Add(lKey, lOCName + "_" + Id.ToString());
                            //add parameter values

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

            List<FTableCalculator.ControlDeviceType> listOCs = FTableCalculator.OCTypeNames.Keys.ToList<FTableCalculator.ControlDeviceType>();
            List<string> listOCNames = FTableCalculator.OCTypeNames.Values.ToList<string>();
            FTableCalculator.ControlDeviceType lSelectedOCType = FTableCalculator.ControlDeviceType.None;
            for (int i = 0; i < listOCs.Count; i++)
            {
                if (cboOCTypes.SelectedItem.ToString() == listOCNames[i])
                {
                    lSelectedOCType = listOCs[i];
                    break;
                }
            }

            int lSelectedExit = 0;
            if (rdoExit1.Checked) lSelectedExit= 1;
            else if (rdoExit2.Checked) lSelectedExit= 2;
            else if (rdoExit3.Checked) lSelectedExit= 3;
            else if (rdoExit4.Checked) lSelectedExit= 4;
            else if (rdoExit5.Checked) lSelectedExit= 5;

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

            if (clsGlobals.gExitOCSetup == null)
                clsGlobals.gExitOCSetup = new atcUtility.atcCollection();
            atcUtility.atcCollection lExitSetup = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(lSelectedExit);

            if (lExitSetup == null)
            {
                lExitSetup = new atcUtility.atcCollection();
                clsGlobals.gExitOCSetup.Add(lSelectedExit, lExitSetup);
            }

            string lOCName = lCalc.GetType().Name;
            int typeCounter = 0;
            string lOCNameCtr = lOCName + "_" + typeCounter.ToString();
            while (lExitSetup.Keys.Contains(lOCNameCtr))
            {
                typeCounter++;
                lOCNameCtr = lOCName + "_" + typeCounter.ToString();
            }

            lExitSetup.Add(lOCNameCtr, lCalc);

            //Refresh tree
            AddOCtoTree(lCalc);
        }

        private bool AddOCtoTree(FTableCalculator aCalc)
        {
            bool lCalcAdded = false;
            string className = aCalc.GetType().Name;
            //FTableCalculator.ControlDeviceType lCDType = lOC.ControlDevice;
            int lExit = 0;
            Dictionary<string, double> lParamValues = null;
            string lOCDisplayName = "";
            switch (className)
            {
                case "FTableCalculator":
                    break;
                case "FTableCalcOCWeirTriVNotch":
                    lExit = ((FTableCalcOCWeirTriVNotch)aCalc).myExit;
                    lParamValues = ((FTableCalcOCWeirTriVNotch)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTriVNotch, out lOCDisplayName);
                    break;
                case "FTableCalcOCWeirTrapeCipolletti":
                    lExit = ((FTableCalcOCWeirTrapeCipolletti)aCalc).myExit;
                    lParamValues = ((FTableCalcOCWeirTrapeCipolletti)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirTrapeCipolletti, out lOCDisplayName);
                    break;
                case "FTableCalcOCWeirRectangular":
                    lExit = ((FTableCalcOCWeirRectangular)aCalc).myExit;
                    lParamValues = ((FTableCalcOCWeirRectangular)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirRectangular, out lOCDisplayName);
                    break;
                case "FTableCalcOCWeirBroad":
                    lExit = ((FTableCalcOCWeirBroad)aCalc).myExit;
                    lParamValues = ((FTableCalcOCWeirBroad)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.WeirBroadCrest, out lOCDisplayName);
                    break;
                case "FTableCalcOCOrificeUnderflow":
                    lExit = ((FTableCalcOCOrificeUnderflow)aCalc).myExit;
                    lParamValues = ((FTableCalcOCOrificeUnderflow)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeUnderdrain, out lOCDisplayName);
                    break;
                case "FTableCalcOCOrificeRiser":
                    lExit = ((FTableCalcOCOrificeRiser)aCalc).myExit;
                    lParamValues = ((FTableCalcOCOrificeRiser)aCalc).ParamValues();
                    FTableCalculator.OCTypeNames.TryGetValue(FTableCalculator.ControlDeviceType.OrificeRiser, out lOCDisplayName);
                    break;
            }

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
            if (lSelectedNode == null || !lSelectedNode.Parent.Text.StartsWith("Exit") || lSelectedNode.Text.StartsWith("Exit") )
            {
                System.Windows.Forms.MessageBox.Show("Select a Control Device for update.", "Update Control Device");
                return;
            }
            lSelectedNode.BackColor = Color.Blue;
            string lSelectedOCAlias = lSelectedNode.Text.Substring(0, lSelectedNode.Text.IndexOf("_"));
            int lSelectedOCIndex = int.Parse(lSelectedNode.Text.Substring(lSelectedNode.Text.IndexOf("_") + 1));
            string lParentNodeName = lSelectedNode.Parent.Text;
            string lExitNo = lParentNodeName.Substring(lParentNodeName.IndexOf(" ") + 1);
            int lExitNum = int.Parse(lExitNo);
            atcUtility.atcCollection lExitCDs = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(lExitNum);
            string lAlias = "";
            FTableCalculator lCD = null;
            atcUtility.atcCollection lExitChanged = new atcUtility.atcCollection();
            int lSelectedExit = SelectedExit();
            foreach (string lCDKey in lExitCDs.Keys)
            {
                string lCDClassName = lCDKey.Substring(0, lCDKey.IndexOf("_"));
                int lCDIndex = int.Parse(lCDKey.Substring(lCDKey.IndexOf("_") + 1));
                lCD = (FTableCalculator)lExitCDs.get_ItemByKey(lCDKey);
                FTableCalculator.OCTypeNames.TryGetValue(lCD.ControlDevice, out lAlias);
                int myExit = lCD.myExit;
                if (lSelectedOCAlias == lAlias && lSelectedOCIndex == lCDIndex)
                {
                    //update starts
                    //each CD type needs a update params method
                    Dictionary<string, double> lParams = lCD.ParamValues();
                    double lValue;
                    foreach (string lKey in lParams.Keys)
                    {
                        if (lKey == lblOCParm0.Text && double.TryParse(txtOCParm_0.Text, out lValue))
                        {
                            lParams.Remove(lKey);
                            lParams.Add(lKey, lValue);
                        }
                        else if (lKey == lblOCParm1.Text && double.TryParse(txtOCParm_1.Text, out lValue))
                        {
                            lParams.Remove(lKey);
                            lParams.Add(lKey, lValue);
                        }
                        else if (lKey == lblOCParm3.Text && double.TryParse(txtOCDisCoeff.Text, out lValue))
                        {
                            lParams.Remove(lKey);
                            lParams.Add(lKey, lValue);
                        }
                    }
                    lCD.SetParamValues(lParams);
                    break;
                }
                else
                    lCD = null;

                if (!(lCD == null))
                {
                    //it's been updated
                    if (lCD.myExit != lSelectedExit)
                    {
                        FTableCalculator lCDNew = lCD.Clone();
                        lCDNew.myExit = lSelectedExit;
                        atcUtility.atcCollection lExitCDsNew = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(lSelectedExit);
                        int lOCCtr = -90;
                        string lCDNewName = "";
                        foreach (string lzKey in lExitCDsNew.Keys)
                        {
                            lCDNewName = lCDNew.GetType().Name;
                            if (lzKey.StartsWith(lCDNewName))
                            {
                                int lOCId = int.Parse(lCDNewName.Substring(lCDNewName.IndexOf("_") + 1));
                                if (lOCCtr < lOCId)
                                    lOCCtr = lOCId;
                            }
                        }
                        lCDNewName = lCDNew.GetType().Name + "_" + (lOCCtr + 1).ToString();
                        lExitCDsNew.Add(lCDNewName, lCDNew);

                        lExitChanged.Add(lCD);
                    }

                }
            }
            foreach (FTableCalculator lCDExitChanged in lExitChanged)
            {
                lExitCDs.Remove(lCDExitChanged);
            }

            lSelectedNode.BackColor = Color.White;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeExitControls.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Select a Control Device to delete.", "Delete Control Device");
                return;
            }

        }

        private void treeExitControls_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Point where mouse is clicked
            System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);
            // Go to the node that the user clicked
            TreeNode lSelectedNode = treeExitControls.GetNodeAt(p);
            if (!(lSelectedNode == null))
            {
                treeExitControls.SelectedNode = lSelectedNode;
                if (lSelectedNode.Text.StartsWith("Exit")) return;
                if (!lSelectedNode.Parent.Text.StartsWith("Exit")) return;

                string lExitName = lSelectedNode.Parent.Text;
                string lExitNo = lExitName.Substring(lExitName.IndexOf(" ") + 1);

                int lExitNum = int.Parse(lExitNo);
                atcUtility.atcCollection lExitCDs = (atcUtility.atcCollection)clsGlobals.gExitOCSetup.get_ItemByKey(lExitNum);
                foreach (FTableCalculator lCD in lExitCDs)
                {
                    int myExit = lCD.myExit;
                    string lOCAlias = "";
                    FTableCalculator.OCTypeNames.TryGetValue(lCD.ControlDevice, out lOCAlias);
                    if (lSelectedNode.Text.StartsWith(lOCAlias))
                    {
                        Dictionary<string, double> lParams = lCD.ParamValues();
                        List<string> lParamNames = lParams.Keys.ToList<string>();
                        List<double> lParamValues = lParams.Values.ToList<double>();
                        lblOCParm0.Text = lParamNames[0];
                        lblOCParm1.Text = lParamNames[1];
                        lblOCParm3.Text = lParamNames[2];
                        txtOCParm_0.Text = lParamValues[0].ToString();
                        txtOCParm_1.Text = lParamValues[1].ToString();
                        txtOCDisCoeff.Text = lParamValues[2].ToString();
                        switch (lExitNo)
                        {
                            case "1": rdoExit1.Checked = true; break;
                            case "2": rdoExit2.Checked = true; break;
                            case "3": rdoExit3.Checked = true; break;
                            case "4": rdoExit4.Checked = true; break;
                            case "5": rdoExit5.Checked = true; break;
                        }

                        for (int i = 0; i < cboOCTypes.Items.Count; i++)
                        {
                            if (lOCAlias == cboOCTypes.Items[i].ToString())
                            {
                                this.cboOCTypes.SelectedIndexChanged -= this.cboOCTypes_SelectedIndexChanged;
                                cboOCTypes.SelectedIndex = i;
                                this.cboOCTypes.SelectedIndexChanged += new System.EventHandler(this.cboOCTypes_SelectedIndexChanged);
                                break;
                            }
                        }

                    }



                }

            }

        }
    }
}
