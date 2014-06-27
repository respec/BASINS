using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace atcFtableBuilder
{
    public partial class mainForm : Form
    {
        private bool pLoaded = false;
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //set up something
            rdoUnitSI.CheckedChanged += new EventHandler(Unit_CheckedChanged);
            rdoUnitUS.CheckedChanged += new EventHandler(Unit_CheckedChanged);

            rdoChCirc.CheckedChanged    += new EventHandler(ChannelSelectionChanged);
            rdoChRect.CheckedChanged    += new EventHandler(ChannelSelectionChanged);
            rdoChTri.CheckedChanged     += new EventHandler(ChannelSelectionChanged);
            rdoChPara.CheckedChanged    += new EventHandler(ChannelSelectionChanged);
            rdoChNatural.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChTrape.CheckedChanged   += new EventHandler(ChannelSelectionChanged);

            txtGeomDepth.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomDiam.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomHInc.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomLength.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomLSlope.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomMannN.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomMaxDepth.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomSideSlope.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomTopWidth.TextChanged += new EventHandler(txtGeomChanged);
            txtGeomWidth.TextChanged += new EventHandler(txtGeomChanged);

            pLoaded = true;

            chkBackfill_CheckedChanged(null, null);
            txtBackfillDepth.Text = clsGlobals.BackfillDepth.ToString();
            txtBackfillPore.Text = clsGlobals.BackfillPorosity.ToString();
        }

        private void Unit_CheckedChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (rdoUnitSI.Checked)
            {
                FTableCalculatorConstants.unitssel = 0;
            }
            else if (rdoUnitUS.Checked)
            {
                FTableCalculatorConstants.unitssel = 1;
            }
        }

        private void bmpSketch1_MouseHover(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            string ltip = "";
            switch (bmpSketch1.CurrentBMPType)
            {
                case FTableCalculator.ChannelType.NONE:
                    ltip = "Select a channel type with the above Radio buttons";
                    break;
                case FTableCalculator.ChannelType.CIRCULAR:
                    ltip = "Circular Channel Diagram";
                    break;
                case FTableCalculator.ChannelType.RECTANGULAR:
                    ltip = "Rectangular Channel Diagram";
                    break;
                case FTableCalculator.ChannelType.TRIANGULAR:
                    ltip = "Triangular Channel Diagram";
                    break;
                case FTableCalculator.ChannelType.TRAPEZOIDAL:
                    ltip = "Trapezoidal Channel Diagram";
                    break;
                case FTableCalculator.ChannelType.PARABOLIC:
                    ltip = "Parabolic Channel Diagram";
                    break;
                case FTableCalculator.ChannelType.NATURAL:
                    ltip = "Natural Channel Diagram (example only)";
                    break;
            }
            toolTip1.SetToolTip(bmpSketch1, ltip);
        }

        private void ChannelSelectionChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (rdoChCirc.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.CIRCULAR;
                txtGeomDiam.Text = clsGlobals.GeomCircDiam.ToString();
                txtGeomLength.Text = clsGlobals.GeomCircLength.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomCircLSlope.ToString();
                txtGeomHInc.Text = clsGlobals.GeomCircHInc.ToString();
                bmpSketch1._diameter = clsGlobals.GeomCircDiam;
                clsGlobals.gCalculator  = new FTableCalcCircle();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.CIRCULAR;
            }
            else if (rdoChRect.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.RECTANGULAR;
                txtGeomLength.Text = clsGlobals.GeomRectLength.ToString();
                txtGeomHInc.Text = clsGlobals.GeomRectHInc.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomRectMaxDepth.ToString();
                txtGeomTopWidth.Text = clsGlobals.GeomRectTopWidth.ToString();
                clsGlobals.gCalculator  = new FTableCalcRectangular();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.RECTANGULAR;
            }
            else if (rdoChTri.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.TRIANGULAR;
                txtGeomLength.Text = clsGlobals.GeomTriLength.ToString();
                txtGeomHInc.Text = clsGlobals.GeomTriHInc.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomTriMaxDepth.ToString();
                txtGeomSideSlope.Text = clsGlobals.GeomTriSideSlope.ToString();
                //clsGlobals.gCalculator = new FTableCalcRectangular();
                //clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.TRIANGULAR;
            }
            else if (rdoChTrape.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.TRAPEZOIDAL;
                txtGeomLength.Text = clsGlobals.GeomTrapeLength.ToString();
                txtGeomHInc.Text = clsGlobals.GeomTrapeHInc.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomTrapeMaxDepth.ToString();
                txtGeomTopWidth.Text = clsGlobals.GeomTrapeTopWidth.ToString();
                txtGeomSideSlope.Text = clsGlobals.GeomTrapeSideSlope.ToString();
                clsGlobals.gCalculator = new FTableCalcTrape();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.TRAPEZOIDAL;
            }
            else if (rdoChPara.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.PARABOLIC;
                txtGeomLength.Text = clsGlobals.GeomParabLength.ToString();
                txtGeomHInc.Text = clsGlobals.GeomParabHInc.ToString();
                txtGeomDepth.Text = clsGlobals.GeomParabDepth.ToString();
                txtGeomWidth.Text = clsGlobals.GeomParabWidth.ToString();
                //clsGlobals.gCalculator = new FTableCalcRectangular();
                //clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.PARABOLIC;
            }
            else if (rdoChNatural.Checked)
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.NATURAL;
                txtGeomLength.Text = clsGlobals.GeomNaturalLength.ToString();
                txtGeomHInc.Text = clsGlobals.GeomNaturalHInc.ToString();
                clsGlobals.gCalculator = new FTableCalcNatural();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.NATURAL;
            }
            else
            {
                bmpSketch1.CurrentBMPType = FTableCalculator.ChannelType.NONE;
                clsGlobals.gCalculator = null;
            }
            bmpSketch1.Invalidate(true);
            bmpSketch1.Refresh();

            DisplayInputs();
        }

        private void DisplayInputs()
        {
            //length and Lslope are always on

            txtGeomMannN.Visible = false;
            lblGeomMannN.Visible = false;
            txtGeomLSlope.Visible = false;
            lblGeomLSlope.Visible = false;

            txtGeomDiam.Visible = false;
            lblGeomDiam.Visible = false;
            txtGeomMaxDepth.Visible = false;
            lblGeomMaxDepth.Visible = false;
            txtGeomTopWidth.Visible = false;
            lblGeomTopWidth.Visible = false;
            txtGeomSideSlope.Visible = false;
            lblGeomSideSlope.Visible = false;
            txtGeomWidth.Visible = false;
            lblGeomWidth.Visible = false;
            txtGeomDepth.Visible = false;
            lblGeomDepth.Visible = false;

            if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.CIRCULAR)
            {
                txtGeomDiam.Visible = true;
                lblGeomDiam.Visible = true;
                txtGeomLSlope.Visible = true;
                lblGeomLSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.RECTANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.TRIANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.TRAPEZOIDAL)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;
                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.PARABOLIC)
            {
                txtGeomWidth.Visible = true;
                lblGeomWidth.Visible = true;
                txtGeomDepth.Visible = true;
                lblGeomDepth.Visible = true;
            }
        }

        private void chkBackfill_CheckedChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (chkBackfill.Checked)
            {
                txtBackfillDepth.Enabled = true;
                txtBackfillPore.Enabled = true;
            }
            else
            {
                txtBackfillDepth.Enabled = false;
                txtBackfillPore.Enabled = false;
            }
        }

        private void txtGeomChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            //ToDo: finish this later
            return;
            txtGeomMannN.Visible = true;
            lblGeomMannN.Visible = true;
            txtGeomLSlope.Visible = true;
            lblGeomLSlope.Visible = true;

            txtGeomDiam.Visible = false;
            lblGeomDiam.Visible = false;
            txtGeomMaxDepth.Visible = false;
            lblGeomMaxDepth.Visible = false;
            txtGeomTopWidth.Visible = false;
            lblGeomTopWidth.Visible = false;
            txtGeomSideSlope.Visible = false;
            lblGeomSideSlope.Visible = false;
            txtGeomWidth.Visible = false;
            lblGeomWidth.Visible = false;
            txtGeomDepth.Visible = false;
            lblGeomDepth.Visible = false;

            if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.CIRCULAR)
            {
                double lval = 0;
                if (double.TryParse(txtGeomDiam.Text.Trim(), out lval))
                {
                    clsGlobals.GeomCircDiam = lval;
                }
                txtGeomDiam.Text = clsGlobals.GeomCircDiam.ToString();
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.RECTANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;

                txtGeomMannN.Visible = false;
                lblGeomMannN.Visible = false;
                txtGeomLSlope.Visible = false;
                lblGeomLSlope.Visible = false;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.TRIANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;

                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.TRAPEZOIDAL)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;
                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.PARABOLIC)
            {
                txtGeomWidth.Visible = true;
                lblGeomWidth.Visible = true;
                txtGeomDepth.Visible = true;
                lblGeomDepth.Visible = true;
            }
            else if (bmpSketch1.CurrentBMPType == FTableCalculator.ChannelType.NATURAL)
            {
                txtGeomMannN.Visible = false;
                lblGeomMannN.Visible = false;
                txtGeomLSlope.Visible = false;
                lblGeomLSlope.Visible = false;
            }
        }

        private void btnShowInfilCalc_Click(object sender, EventArgs e)
        {
            frmInfiltration lInfilCalculator = new frmInfiltration();
            lInfilCalculator.InfiltrationMethodChanged += this.InfilMethodChanged;
            lInfilCalculator.ShowDialog();
            lInfilCalculator.InfiltrationMethodChanged -= this.InfilMethodChanged;
            lInfilCalculator.Dispose();
            lInfilCalculator = null;
        }

        private void InfilMethodChanged(object sender, InfiltrationMethodChangeEventArgs e)
        {
            txtInfilRate.Text = e.ResultInfiltrationRate;
            txtInfilDepth.Text = e.ResultInfiltrationDepth;
            txtInfilDrainTime.Text = e.ResultInfiltrationDrainTime;
        }

        private void atcGrid1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void btnCalcFtable_Click(object sender, EventArgs e)
        {
            if (clsGlobals.gCalculator == null) return;
            if (!InputsAreOk(new ArrayList(clsGlobals.gCalculator.geoInputNames))) return;
            string[,] units = null;
            ArrayList colNames = new ArrayList();
            switch (clsGlobals.gCalculator.CurrentType)
            {
                case FTableCalculator.ChannelType.CIRCULAR:
                    units = new string[,]{{"(m)",              "(ft)"},
                                          {"(ha.)",            "(acres)"},
                                          {"(milllion cu. m)", "(ac-ft)"},
                                          {"(cms)",            "(cfs)"}};
                    colNames.Add("depth" + units[0, FTableCalculatorConstants.programunits]);
                    //note:  depth was used because it was the default column
                    //name for the other calcualtors.  This may need revision for this calculator.
                    colNames.Add("area" + units[1, FTableCalculatorConstants.programunits]);
                    colNames.Add("volume" + units[2, FTableCalculatorConstants.programunits]);
                    // sri- 09-11-2012
                    //colNames.Add("outflow1" + units[3, FTableCalculatorConstants.programunits]);

                    //If any control structures were checked for calculation, append their
                    //column names to the open channel column names so that their output can be displayed.

                    break;
                case FTableCalculator.ChannelType.TRIANGULAR:
                    break;
            }

        }

        /**
	 * 
	 *<P>Returns a vector of column names by appending to the vector of column names from the calculator class.  </P>
	 *<P><B>Limitations:</B>  </P>
	 * @param defaultColNames
	 * @return
	 */
        protected ArrayList appendCheckecCalculatorColumnNames(ArrayList defaultColNames)
        {
            int j = 1;
            //for (int i = 0; i < controlCheckBoxes.size(); i++)
            //{
            //    if (controlCheckBoxes.get(i).isSelected())
            //    {
            //        String checkBoxName = controlCheckBoxes.get(i).getName();
            //        String colName = "";
            //        String[] unit = { " (cms)", " (cfs)" };
            //        int punit = FTableCalculatorConstants.programunits;
            //        if (checkBoxName.ToLower().Equals("triangular vnotch weir"))
            //        {
            //            colName = "v_notchwr" + unit[punit];
            //        }
            //        else if (checkBoxName.ToLower().Equals("trapezoidal weir (cipoletti)"))
            //        {
            //            colName = "trapwr" + unit[punit];
            //        }
            //        else if (checkBoxName.ToLower().Equals("broad crested weir"))
            //        {
            //            colName = "bdcrstdwr" + unit[punit];
            //        }
            //        else if (checkBoxName.ToLower().Equals("rectangular weir"))
            //        {
            //            colName = "rctnglrwr" + unit[punit];
            //        }
            //        else if (checkBoxName.ToLower().Equals("underdrain orifice"))
            //        {
            //            colName = "udrdrnorf" + unit[punit];
            //        }
            //        else if (checkBoxName.ToLower().Equals("riser orifice"))
            //        {
            //            colName = "riserorf" + unit[punit];
            //        }
            //        defaultColNames.Add(colName);
            //        j++;
            //        //the panel knows which check box is selected
            //    }//close the is selected condition   
            //}
            return defaultColNames;
        }

        private bool InputsAreOk(ArrayList inputNames)
        {
            bool allinputsOK = true;
            // first clone names list and add the names that this class has added as inputs
            ArrayList localList = (ArrayList)inputNames.Clone();
            localList.Add(txtBackfillPore.Name);
            localList.Add(txtBackfillDepth.Name);

            //resetInputErrorMarks();
            for (int j = 0; j < this.Controls.Count; j++)
            {
                Control jComp = this.Controls[j];
                if (!jComp.Enabled || !jComp.Visible)
                {
                    continue;
                }
                String jCName = jComp.Name;
                if (localList.Contains(jCName))
                {
                    try
                    {
                        double putValue = 0.0;
                        double.TryParse(jComp.Text, out putValue);

                        if ((jCName.Equals("Channel Diameter", StringComparison.OrdinalIgnoreCase)) ||
                            (jCName.Equals("Maximum Channel Depth", StringComparison.OrdinalIgnoreCase)))
                        {
                            if ((putValue > 100) || (putValue < 0.1))
                                allinputsOK = handleBadInput(jComp);
                        }
                        if (double.IsNaN(putValue))
                        {
                            allinputsOK = handleBadInput(jComp);
                        }
                        else if ((putValue < 0d)
                              && (!jCName.Equals("Weir Invert", StringComparison.OrdinalIgnoreCase))
                              && (!jCName.Equals("Orifice Height", StringComparison.OrdinalIgnoreCase))
                              && (!jCName.Equals("Riser Height", StringComparison.OrdinalIgnoreCase))
                              && (!jCName.StartsWith("Backfill")))
                        {
                            allinputsOK = handleBadInput(jComp);
                        }
                        else if ((jCName.Equals("Channel Mannings Value", StringComparison.OrdinalIgnoreCase))
                              && (putValue <= 0))
                        {
                            allinputsOK = handleBadInput(jComp);
                        }
                        else if ((jCName.Equals("Backfill porosity", StringComparison.OrdinalIgnoreCase))
                              && (putValue > 1d || putValue < 0d))
                        {
                            allinputsOK = handleBadInput(jComp);
                        }
                        else if (jCName.Equals("Backfill depth", StringComparison.OrdinalIgnoreCase))
                        {
                            for (int tfield = 0; tfield < this.Controls.Count; tfield++)
                            {
                                if (this.Controls[tfield].GetType() == typeof(System.Windows.Forms.TextBox) &&
                                ((TextBox)this.Controls[tfield]).Name.Equals("Maximum Channel Depth", StringComparison.OrdinalIgnoreCase))
                                {
                                    double depth = 0.0;
                                    double.TryParse(this.Controls[tfield].Text, out depth);
                                    if (putValue > depth)
                                    {
                                        allinputsOK = handleBadInput(jComp);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (System.FormatException nfe)
                    {
                        allinputsOK = handleBadInput(jComp);
                    }
                    catch (InvalidCastException cce)
                    {
                        //do nothing
                    }

                }
            }
            if (!allinputsOK)
            {
                System.Windows.Forms.MessageBox.Show(
                        "Some fields contain inputs that are not valid.  \n"
                      + "Please check the indicated field's tool tips for further information.",
                        "Invalid Input");
            }
            return allinputsOK;
        }

        private bool handleBadInput(Control cpt)
        {
            cpt.BackColor = FTableCalculatorConstants.error_color;
            try
            {
                toolTip1.SetToolTip(cpt, cpt.Text + " is not a valid input for " + cpt.Name + ".");
                return true;
            }
            catch (System.Exception cce)
            {
                return false;
            }
        }

        private void btnShowOptControls_Click(object sender, EventArgs e)
        {
            frmOutflowControls lOutflowControlDialog = new frmOutflowControls();
            lOutflowControlDialog.ShowDialog();
        }
    }
}
