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
        private bool pChannelGeomInputChangeComplete;
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //set up something
            rdoUnitSI.CheckedChanged += new EventHandler(Unit_CheckedChanged);
            rdoUnitUS.CheckedChanged += new EventHandler(Unit_CheckedChanged);

            rdoChCirc.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChRect.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChTri.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChPara.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChNatural.CheckedChanged += new EventHandler(ChannelSelectionChanged);
            rdoChTrape.CheckedChanged += new EventHandler(ChannelSelectionChanged);

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

            if (grdChProfile.Source == null)
            {
                grdChProfile.Source = new atcControls.atcGridSource();
                grdChProfile.Source.Columns = 2;
                grdChProfile.Source.FixedRows = 1;
                //set header
                grdChProfile.Source.set_CellValue(0, 0, "X=Distance From Left Bank");
                grdChProfile.Source.set_CellValue(0, 1, "Y=Relative Depth from Thalweg");
                grdChProfile.Initialize(grdChProfile.Source);
                grdChProfile.SizeAllColumnsToContents();
                grdChProfile.Refresh();
            }

            frameNaturalChFP.Visible = false;
            if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
            {
                rdoChNaturalFP.Visible = true;
                txtGeomNFP_ChLSlope.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_ChLength.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_ChMannN.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_LOBLength.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_LOBMannN.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_ROBLength.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_ROBMannN.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_LOBX.TextChanged += new EventHandler(txtGeomChanged);
                txtGeomNFP_ROBX.TextChanged += new EventHandler(txtGeomChanged);
                rdoChNaturalFP.CheckedChanged += new EventHandler(ChannelSelectionChanged);

                gbInputInfil.Enabled = false;
                gbInputInfil.Visible = false;
                btnShowInfilCalc.Visible = false;
                lblBMPList.Visible = false;
                lblBMPMsg.Visible = false;
                cboBMPTypes.Visible = false;

                this.Text += " - " + clsGlobals.ToolNameGray;
            }
            else
            {
                //load BMP types
                cboBMPTypes.Items.Add("");
                foreach (FTableCalculator.BMPType lBMPType in FTableCalculator.BMPTypeNames.Keys)
                {
                    cboBMPTypes.Items.Add(FTableCalculator.BMPTypeNames[lBMPType]);
                }
                chkBackfill_CheckedChanged(null, null);
                txtBackfillDepth.Text = clsGlobals.BackfillDepth.ToString();
                txtBackfillPore.Text = clsGlobals.BackfillPorosity.ToString();
 
                rdoChNaturalFP.Visible = false;
                this.Text += " - " + clsGlobals.ToolNameGreen;
            }

            pLoaded = true;
       }

        private void Unit_CheckedChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (!((RadioButton)sender).Checked) return;
            if (rdoUnitSI.Checked)
            {
                FTableCalculatorConstants.unitssel = 0;
                FTableCalculatorConstants.Cunit = 1.0;
                FTableCalculatorConstants.Aunit = 1.0;
                FTableCalculatorConstants.programunits = 0;
                FTableCalculatorConstants.infiltrationConversion = Math.Pow(36, -1);
                //Infiltration_Panel infpanel = new Infiltration_Panel((DocumentListener));
                //infpanel.txtSaturatedHydraulicConductivity.setText("35");
                //infpanel.txtsuctionHead.setText("35");
                ////Change the units of the discharge coefficient
                //FTableCalculatorConstants.controlDeviceInputPanels.get("Triangular Vnotch Weir").setDischargeCoefficient("Triangulr Vnotch Weir", "100");
                //FTableCalculatorConstants.txtfield.setText("M");
            }
            else if (rdoUnitUS.Checked)
            {
                FTableCalculatorConstants.unitssel = 1;
                FTableCalculatorConstants.Cunit = 1.486;
                FTableCalculatorConstants.Aunit = 43560.0;  // sq feet to acres factor; 1 square foot = 2.29568411 10-5 acres
                FTableCalculatorConstants.programunits = 1; //for conversion of area to acres
                FTableCalculatorConstants.infiltrationConversion = 1.008333; // acres * in/hr to cf/s
                //FTableCalculatorConstants.controlDeviceInputPanels.get(" Triangular Vnotch Weir").setDischargeCoefficient(" Triangular Vnotch Weir", "200");
                //FTableCalculatorConstants.txtfield.setText("E");
                //Infiltration_Panel infpanel1 = new Infiltration_Panel((DocumentListener));
                //infpanel1.txtSaturatedHydraulicConductivity.setText("53");
                //infpanel1.txtsuctionHead.setText("53");
            }
            System.Windows.Forms.MessageBox.Show("All input should be in " + FTableCalculatorConstants.UnitSystemNames[FTableCalculatorConstants.unitssel], "Unit System Change");
            return;
        }

        private void bmpSketch1_MouseHover(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            string ltip = "";
            switch (bmpSketch1.CurrentChannelType)
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
            clsGlobals.gCalculator = null;
            pChannelGeomInputChangeComplete = false;
            frameNaturalChFP.Visible = false;
            if (rdoChCirc.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcCircle();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.CIRCULAR;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.CIRCULAR;
                txtGeomLength.Text = clsGlobals.GeomCircLength.ToString();
                txtGeomDiam.Text = clsGlobals.GeomCircDiam.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString(); //hidden but use default
                txtGeomLSlope.Text = clsGlobals.GeomCircLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomCircHInc.ToString();
                bmpSketch1._diameter = clsGlobals.GeomCircDiam;
            }
            else if (rdoChRect.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcRectangular();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.RECTANGULAR;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.RECTANGULAR;
                txtGeomLength.Text = clsGlobals.GeomRectLength.ToString();
                txtGeomTopWidth.Text = clsGlobals.GeomRectTopWidth.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomRectMaxDepth.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomRectLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomRectHInc.ToString();
                bmpSketch1._width = clsGlobals.GeomRectTopWidth;
                bmpSketch1._depth = clsGlobals.GeomRectMaxDepth;
            }
            else if (rdoChTri.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcTri();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.TRIANGULAR;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.TRIANGULAR;
                txtGeomLength.Text = clsGlobals.GeomTriLength.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomTriMaxDepth.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString();
                txtGeomSideSlope.Text = clsGlobals.GeomTriSideSlope.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomTriLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomTriHInc.ToString();
                bmpSketch1._sideslope = clsGlobals.GeomTriSideSlope;
                bmpSketch1._depth = clsGlobals.GeomTriMaxDepth;
            }
            else if (rdoChTrape.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcTrape();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.TRAPEZOIDAL;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.TRAPEZOIDAL;
                txtGeomLength.Text = clsGlobals.GeomTrapeLength.ToString();
                txtGeomTopWidth.Text = clsGlobals.GeomTrapeTopWidth.ToString();
                txtGeomMaxDepth.Text = clsGlobals.GeomTrapeMaxDepth.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString();
                txtGeomSideSlope.Text = clsGlobals.GeomTrapeSideSlope.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomTrapeLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomTrapeHInc.ToString();
                bmpSketch1._sideslope = clsGlobals.GeomTrapeSideSlope;
                bmpSketch1._depth = clsGlobals.GeomTrapeMaxDepth;
                bmpSketch1._width = clsGlobals.GeomTrapeTopWidth;
            }
            else if (rdoChPara.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcEllipse();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.PARABOLIC;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.PARABOLIC;
                txtGeomLength.Text = clsGlobals.GeomParabLength.ToString();
                txtGeomWidth.Text = clsGlobals.GeomParabWidth.ToString();
                txtGeomDepth.Text = clsGlobals.GeomParabDepth.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomParabLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomParabHInc.ToString();
                bmpSketch1._width = clsGlobals.GeomParabWidth;
                bmpSketch1._depth = clsGlobals.GeomParabDepth;
            }
            else if (rdoChNatural.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcNatural();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.NATURAL;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NATURAL;
                txtGeomLength.Text = clsGlobals.GeomNaturalLength.ToString();
                txtGeomMannN.Text = clsGlobals.GeomManningN.ToString();
                txtGeomLSlope.Text = clsGlobals.GeomNaturalLSlope.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomHInc.Text = clsGlobals.GeomNaturalHInc.ToString();
            }
            else if (clsGlobals.gToolType == clsGlobals.ToolType.Gray && rdoChNaturalFP.Checked)
            {
                clsGlobals.gCalculator = new FTableCalcNaturalFP();
                clsGlobals.gCalculator.CurrentType = FTableCalculator.ChannelType.NATURALFP;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NATURALFP;
                frameNaturalChFP.Visible = true;
                txtGeomNFP_ChLength.Text = clsGlobals.GeomNaturalFPChLength.ToString();
                txtGeomNFP_LOBLength.Text = clsGlobals.GeomNaturalFPLOBLength.ToString();
                txtGeomNFP_ROBLength.Text = clsGlobals.GeomNaturalFPROBLength.ToString();
                txtGeomNFP_ChMannN.Text = clsGlobals.GeomNaturalFPChMannN.ToString();
                txtGeomNFP_LOBMannN.Text = clsGlobals.GeomNaturalFPLOBMannN.ToString();
                txtGeomNFP_ROBMannN.Text = clsGlobals.GeomNaturalFPROBMannN.ToString();
                txtGeomNFP_LOBX.Text = clsGlobals.GeomNaturalFPLOBX.ToString();
                txtGeomNFP_ROBX.Text = clsGlobals.GeomNaturalFPROBX.ToString();
                pChannelGeomInputChangeComplete = true;
                txtGeomNFP_ChLSlope.Text = clsGlobals.GeomNaturalFPChLSlope.ToString();
                //txtGeomNFP_HInc.Text = clsGlobals.GeomNaturalFPHInc.ToString();
            }
            else
            {
                clsGlobals.gCalculator = null;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NONE;
            }

            if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
                txtGeomLength.Text = clsGlobals.GeomChannelLengthGray.ToString();

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

            btnImportChProfile.Enabled = false;
            btnClearProfile.Enabled = false;
            btnUndoClear.Enabled = false;
            grdChProfile.Enabled = false;

            if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.CIRCULAR)
            {
                txtGeomDiam.Visible = true;
                lblGeomDiam.Visible = true;
                txtGeomLSlope.Visible = true;
                lblGeomLSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.RECTANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.TRIANGULAR)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.TRAPEZOIDAL)
            {
                txtGeomMaxDepth.Visible = true;
                lblGeomMaxDepth.Visible = true;
                txtGeomTopWidth.Visible = true;
                lblGeomTopWidth.Visible = true;
                txtGeomSideSlope.Visible = true;
                lblGeomSideSlope.Visible = true;
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.PARABOLIC)
            {
                txtGeomWidth.Visible = true;
                lblGeomWidth.Visible = true;
                txtGeomDepth.Visible = true;
                lblGeomDepth.Visible = true;
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.NATURAL ||
                     bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.NATURALFP)
            {
                btnImportChProfile.Enabled = true;
                btnClearProfile.Enabled = true;
                btnUndoClear.Enabled = true;
                grdChProfile.Enabled = true;
            }

            if (clsGlobals.gToolType == clsGlobals.ToolType.Gray)
            {
                lblGeomLSlope.Visible = true;
                txtGeomLSlope.Visible = true;
                lblGeomMannN.Visible = true;
                txtGeomMannN.Visible = true;
            }
            else
            {
            }
        }

        private void chkBackfill_CheckedChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            clsGlobals.HasBackfill = chkBackfill.Checked;
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
            if (clsGlobals.gCalculator == null) return;
            if (!pChannelGeomInputChangeComplete) return;

            ArrayList lInputNames = null;
            if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.CIRCULAR)
            {
                lInputNames = new ArrayList(((FTableCalcCircle)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.RECTANGULAR)
            {
                lInputNames = new ArrayList(((FTableCalcRectangular)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.TRIANGULAR)
            {
                lInputNames = new ArrayList(((FTableCalcTri)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.TRAPEZOIDAL)
            {
                lInputNames = new ArrayList(((FTableCalcTrape)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.PARABOLIC)
            {
                lInputNames = new ArrayList(((FTableCalcEllipse)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.NATURAL)
            {
                lInputNames = new ArrayList(((FTableCalcNatural)clsGlobals.gCalculator).geomInputs);
            }
            else if (bmpSketch1.CurrentChannelType == FTableCalculator.ChannelType.NATURALFP)
            {
                lInputNames = new ArrayList(((FTableCalcNaturalFP)clsGlobals.gCalculator).geomInputs);
            }
            InputsAreOk(lInputNames);
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

        private void btnCalcFtable_Click(object sender, EventArgs e)
        {
            if (clsGlobals.gCalculator == null) return;
            if (!InputsAreOk(new ArrayList(clsGlobals.gCalculator.geomInputs))) return;

            if (!RedrawShapes()) return;

            ArrayList colNames = new ArrayList();
            //Get the inputs from the geometric data and put them into a hashtable.
            Hashtable inputHash = getInputValues(clsGlobals.gCalculator.geomInputs);

            colNames.Add("depth" + FTableCalculatorConstants.UnitSystemLabels[0, FTableCalculatorConstants.programunits]);
            //note:  depth was used because it was the default column
            //name for the other calcualtors.  This may need revision for this calculator.
            colNames.Add("area" + FTableCalculatorConstants.UnitSystemLabels[1, FTableCalculatorConstants.programunits]);
            colNames.Add("volume" + FTableCalculatorConstants.UnitSystemLabels[2, FTableCalculatorConstants.programunits]);
            // sri- 09-11-2012
            colNames.Add("outflow1" + FTableCalculatorConstants.UnitSystemLabels[3, FTableCalculatorConstants.programunits]);

            //If any control structures were checked for calculation, append their
            //column names to the open channel column names so that their output can be displayed.

            colNames = appendCheckedCalculatorColumnNames(colNames);
            //The next procedure gets the vector of vectors from the calculator.
            //See the calculator code for details of how it compiles the output.
            //FTableCalculator.ChannelGeomInput[] lInputIds = clsGlobals.gCalculator.geomInputs;
            ArrayList lFtableResultChannel = null;
            frmFTableResult lFTableGrid = new frmFTableResult();
            switch (clsGlobals.gCalculator.CurrentType)
            {
                case FTableCalculator.ChannelType.CIRCULAR:
                    //The following will describe how the calculation results are retrieved from
                    //the calculator and connected to the jTable for display.
                    //Please note that the data comes in the form of a Vector.  The Vector, data, is 
                    //actually a Vector of Vectors.  Each row in the grid is a vector object.

                    if (((FTableCalcCircle)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                       lFtableResultChannel = ((FTableCalcCircle)(clsGlobals.gCalculator)).GenerateFTable();
                    /*
                        (double)inputHash[lInputIds[0]],
                        (double)inputHash[lInputIds[1]],
                        (double)inputHash[lInputIds[2]],
                        (double)inputHash[lInputIds[3]],
                        (double)inputHash[lInputIds[4]]);
                    */

                    //Tong: set diameter again??? purpose???
                    bmpSketch1._diameter = (double)inputHash[FTableCalculator.ChannelGeomInput.Diameter];

                    //Tong: purpose???
                    //set the open channel depth so that the control device middleware 
                    //will know how to set the DT variable for the calculators. -BED
                    //setTotalDepth(bmpSketch1._diameter);//Double.parseDouble(inputHash.get(geoInputNames[1]).toString()));
                    FTableCalculator.TotalDepth = bmpSketch1._diameter;

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcCircle)clsGlobals.gCalculator;

                    break;
                case FTableCalculator.ChannelType.RECTANGULAR:
                    if (((FTableCalcRectangular)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                        lFtableResultChannel = ((FTableCalcRectangular)(clsGlobals.gCalculator)).GenerateFTable();

                    FTableCalculator.TotalDepth = (double)inputHash[FTableCalculator.ChannelGeomInput.MaximumDepth];

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcRectangular)clsGlobals.gCalculator;
                    break;
                case FTableCalculator.ChannelType.TRIANGULAR:
                    if (((FTableCalcTri)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                        lFtableResultChannel = ((FTableCalcTri)(clsGlobals.gCalculator)).GenerateFTable();

                    FTableCalculator.TotalDepth = (double)inputHash[FTableCalculator.ChannelGeomInput.MaximumDepth];

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcTri)clsGlobals.gCalculator;
                    break;
                case FTableCalculator.ChannelType.TRAPEZOIDAL:
                    /*
                double topWidth = 0;
				double sideSlope = 0;
				double depthVal = 0;
				topWidth = Double.parseDouble(inputHash.get(geoInputNames[1]).toString());
				sideSlope = Double.parseDouble(inputHash.get(geoInputNames[2]).toString());
				depthVal = Double.parseDouble(inputHash.get(geoInputNames[0]).toString());
				double bw = topWidth - (2 * sideSlope * depthVal);
				trapChannel.SetTopWidth(topWidth);
				trapChannel.SetBottomWidth(bw);
				trapChannel.SetDepth(depthVal);
                     */
                    if (((FTableCalcTrape)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                        lFtableResultChannel = ((FTableCalcTrape)(clsGlobals.gCalculator)).GenerateFTable();

                    FTableCalculator.TotalDepth = (double)inputHash[FTableCalculator.ChannelGeomInput.MaximumDepth];

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcTrape)clsGlobals.gCalculator;
                    break;
                case FTableCalculator.ChannelType.PARABOLIC:
                    /*
                Double width;
				Double depth;
				width = Double.parseDouble(inputHash.get(geoInputNames[1]).toString());
				depth = Double.parseDouble(inputHash.get(geoInputNames[2]).toString());
				ellipseChannel.setDepth(depth.doubleValue());
				ellipseChannel.setWidth(width.doubleValue());
                     */
                    if (((FTableCalcEllipse)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                        lFtableResultChannel = ((FTableCalcEllipse)(clsGlobals.gCalculator)).GenerateFTable();

                    FTableCalculator.TotalDepth = (double)inputHash[FTableCalculator.ChannelGeomInput.Depth];

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcEllipse)clsGlobals.gCalculator;
                    break;
                case FTableCalculator.ChannelType.NATURAL:
                    if (clsGlobals.gProfileStations == null || clsGlobals.gProfileStations.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Need to import channel cross section profile data first.", "Natural Channel Input");
                        return;
                    }

                    if (inputHash.ContainsKey(FTableCalculator.ChannelGeomInput.Profile))
                        inputHash.Remove(FTableCalculator.ChannelGeomInput.Profile);
                    inputHash.Add(FTableCalculator.ChannelGeomInput.Profile, clsGlobals.gProfileStations);
                    if (((FTableCalcNatural)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                    {
                        lFtableResultChannel = ((FTableCalcNatural)(clsGlobals.gCalculator)).GenerateFTable();
                    }

                    //When finding the total depth for the control device calculators,
				    //the system must scan the channel profile to get the biggest y-value.
				    //The following line gets that value.
                    FTableCalculator.TotalDepth = ((FTableCalcNatural)(clsGlobals.gCalculator)).ChannelProfileYMaxDepth;

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012

                    lFTableGrid.Calc = (FTableCalcNatural)clsGlobals.gCalculator;
                    break;
                case FTableCalculator.ChannelType.NATURALFP:
                    if (clsGlobals.gProfileStations == null || clsGlobals.gProfileStations.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Need to import channel cross section profile data first.", "Natural Channel with FP Input");
                        return;
                    }

                    if (inputHash.ContainsKey(FTableCalculator.ChannelGeomInput.Profile))
                        inputHash.Remove(FTableCalculator.ChannelGeomInput.Profile);
                    inputHash.Add(FTableCalculator.ChannelGeomInput.Profile, clsGlobals.gProfileStations);
                    if (((FTableCalcNaturalFP)(clsGlobals.gCalculator)).SetInputParameters(inputHash))
                    {
                        lFtableResultChannel = ((FTableCalcNaturalFP)(clsGlobals.gCalculator)).GenerateFTable();
                    }

                    //When finding the total depth for the control device calculators,
                    //the system must scan the channel profile to get the biggest y-value.
                    //The following line gets that value.
                    FTableCalculator.TotalDepth = ((FTableCalcNaturalFP)(clsGlobals.gCalculator)).ChannelProfileYMaxDepth;

                    // storing the user defined increment value to use in ControlDeviceFtableCalculator classes
                    //FTableCalculatorConstants.calculatorIncrement = (double)inputHash[FTableCalculator.ChannelGeomInput.NFP_HeightIncrement]; //Double.parseDouble(inputHash.get(geoInputNames[4]).toString());  // sri-09-11-1012
                    FTableCalculatorConstants.calculatorIncrement = ((FTableCalcNaturalFP)(clsGlobals.gCalculator)).inpHeightIncrement;

                    lFTableGrid.Calc = (FTableCalcNaturalFP)clsGlobals.gCalculator;
                    break;
            }

            //The vector, data, represents the flow of the open stream channel.
            //The control structure calcuators must run and merge their results with
            //the open stream channel results.  The next line accomplishes such a task.
            //Please see the comments in the doControlStructureCalcualtions for
            //how the merge is accomplished.

            Dictionary<FTableCalculator.ControlDeviceType, ArrayList> lFtableResultControlDevice = doControlStructureCalculations();
            clsGlobals.gCalculator.mergeCalculators(lFtableResultChannel, lFtableResultControlDevice);

            //if there is negative output in the vector, return the negative message
            //from the FTableCalculatorConstants
            string newResultMessage = FTableCalculatorConstants.returnMessageIfNegativeValue(lFtableResultChannel);

            //this line will modify the data in the vectors if
            //the control structures are included. (removes the open flow calculation)
            //(these comments also apply to other shape calculators)
            ArrayList[] finalResults = FTableCalculatorConstants.modifyResultsToAccomodateControlStructures(colNames, lFtableResultChannel);
            colNames = finalResults[0];
            lFtableResultChannel = finalResults[1];

            //Tong: backfill is checked already by this point and check if infiltration is done
            double infilRate = 0;
            if (clsGlobals.gToolType == clsGlobals.ToolType.Green) // && double.TryParse(txtInfilRate.Text, out infilRate))
            {
                if (string.IsNullOrEmpty(txtInfilRate.Text))
                {
                    infilRate = 0;
                }
                else
                {
                    if (double.TryParse(txtInfilRate.Text, out infilRate))
                    {
                        if (infilRate < 0) infilRate = 0;
                    }
                }
                double backfillDepth = double.Parse(txtBackfillDepth.Text);
                double backfillPoros = double.Parse(txtBackfillPore.Text);
                string lMsg;
                finalResults = clsGlobals.gCalculator.modifyTableForInfiltration(lFtableResultChannel, colNames, backfillDepth, backfillPoros, infilRate, out lMsg);
                colNames = finalResults[0];
                lFtableResultChannel = finalResults[1];
                lFTableGrid.FTableMessage = lMsg;
            }

            if (newResultMessage.Equals(FTableCalculatorConstants.JTableNegativeValueMessage))
            {
                //jTable.setVisible(false); //hide the jtable if negative results are present
                //jTableMessageLabel.setText(newResultMessage);
                lFTableGrid.FTableMessage = newResultMessage;
            }
            else
            {
                lFTableGrid.FTableResults = lFtableResultChannel;
                lFTableGrid.FTableColumnNames = colNames;
            }
            lFTableGrid.ShowDialog();
        }

        private bool RedrawShapes()
        {
            bool lDimsAreGood = true;
            string lMsg = "";
            if (rdoChCirc.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.CIRCULAR;
                bmpSketch1._diameter = clsGlobals.GeomCircDiam;
                double lDiam = 0.0;
                if (double.TryParse(txtGeomDiam.Text, out lDiam) && lDiam > 0)
                    bmpSketch1._diameter = lDiam;
                else
                    lDimsAreGood = false;

                if (!lDimsAreGood)
                    lMsg = "CIRCULAR: revise diameter.";

            }
            else if (rdoChRect.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.RECTANGULAR;
                bmpSketch1._width = clsGlobals.GeomRectTopWidth;
                bmpSketch1._depth = clsGlobals.GeomRectMaxDepth;
                double ltopWidth = 0; double lmaxDepth = 0;
                if (double.TryParse(txtGeomTopWidth.Text, out ltopWidth) && double.TryParse(txtGeomMaxDepth.Text, out lmaxDepth))
                {
                    if (ltopWidth > 0 && lmaxDepth > 0)
                    {
                        bmpSketch1._width = ltopWidth;
                        bmpSketch1._depth = lmaxDepth;
                    }
                    else
                        lDimsAreGood = false;
                }
                else
                    lDimsAreGood = false;

                if (!lDimsAreGood)
                    lMsg = "RECTANGULAR: revise top width or max. depth";
            }
            else if (rdoChTri.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.TRIANGULAR;
                bmpSketch1._sideslope = clsGlobals.GeomTriSideSlope;
                bmpSketch1._depth = clsGlobals.GeomTriMaxDepth;

                double lmaxDepth = 0; double lsideSlope = 0;
                if (double.TryParse(txtGeomSideSlope.Text, out lsideSlope) && double.TryParse(txtGeomMaxDepth.Text, out lmaxDepth))
                {
                    if (lsideSlope > 0 && lmaxDepth > 0)
                    {
                        bmpSketch1._sideslope = lsideSlope;
                        bmpSketch1._depth = lmaxDepth;
                    }
                    else
                        lDimsAreGood = false;
                }
                else
                    lDimsAreGood = false;

                if (!lDimsAreGood)
                    lMsg = "TRIANGULAR: revise side slope or max. depth";

            }
            else if (rdoChTrape.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.TRAPEZOIDAL;
                bmpSketch1._sideslope = clsGlobals.GeomTrapeSideSlope;
                bmpSketch1._depth = clsGlobals.GeomTrapeMaxDepth;
                bmpSketch1._width = clsGlobals.GeomTrapeTopWidth;

                double lmaxDepth = 0; double ltopWidth = 0; double lsideSlope = 0;
                if (double.TryParse(txtGeomTopWidth.Text, out ltopWidth) && 
                    double.TryParse(txtGeomMaxDepth.Text, out lmaxDepth) &&
                    double.TryParse(txtGeomSideSlope.Text, out lsideSlope))
                {
                    if (ltopWidth > 0 && lmaxDepth > 0 && lsideSlope > 0)
                    {
                        double lbotWidth = ltopWidth - 2 * (lmaxDepth * lsideSlope);
                        if (lbotWidth > 0)
                        {
                            bmpSketch1._width = ltopWidth;
                            bmpSketch1._depth = lmaxDepth;
                            bmpSketch1._sideslope = lsideSlope;
                        }
                        else
                            lDimsAreGood = false;
                    }
                    else
                        lDimsAreGood = false;
                }
                else
                    lDimsAreGood = false;

                if (!lDimsAreGood)
                    lMsg = "TRAPEZOIDAL: revise top width, max. depth, or side slope";
 
            }
            else if (rdoChPara.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.PARABOLIC;
                bmpSketch1._width = clsGlobals.GeomParabWidth;
                bmpSketch1._depth = clsGlobals.GeomParabDepth;

                double lwidth = 0; double ldepth = 0;
                if (double.TryParse(txtGeomWidth.Text, out lwidth) && double.TryParse(txtGeomDepth.Text, out ldepth))
                {
                    if (lwidth > 0 && ldepth > 0)
                    {
                        bmpSketch1._width = lwidth;
                        bmpSketch1._depth = ldepth;
                    }
                    else
                        lDimsAreGood = false;
                }
                else
                    lDimsAreGood = false;

                if (!lDimsAreGood)
                    lMsg = "PARABOLIC: revise width or depth";
            }
            else if (rdoChNatural.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NATURAL;
            }
            else if (clsGlobals.gToolType == clsGlobals.ToolType.Gray && rdoChNaturalFP.Checked)
            {
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NATURALFP;
            }
            else
            {
                clsGlobals.gCalculator = null;
                bmpSketch1.CurrentChannelType = FTableCalculator.ChannelType.NONE;
            }

            if (lDimsAreGood)
            {
                bmpSketch1.Invalidate(true);
                bmpSketch1.Refresh();
            }

            if (!string.IsNullOrEmpty(lMsg))
                System.Windows.Forms.MessageBox.Show(lMsg, "Problematic Channel Dimension");

            return lDimsAreGood;
        }

        /**
 * 
 *<P>Pass in an ArrayList of names which can be quickly compared against component names.
 *Put the results in a hash table with the key value pair <InputName, Value></P>
 *<P><B>Limitations:</B>  </P>
 * @param geoInputNames
 * @return
 */
        public Hashtable getInputValues(FTableCalculator.ChannelGeomInput[] channelInputs)
        {
            Hashtable newHash = new Hashtable();
            TextBox lInputTxtField = null;
            double lVal = 0;
            foreach (FTableCalculator.ChannelGeomInput lChannelInputId in channelInputs)
            {
                try
                {
                    if (FTableCalculator.LookupChannelGeomInput.ContainsKey(lChannelInputId))
                    {
                        string txtFieldName = FTableCalculator.LookupChannelGeomInput[lChannelInputId];
                        txtFieldName = txtFieldName.Substring(txtFieldName.IndexOf(",") + 1);
                        lInputTxtField = (TextBox)this.Controls.Find(txtFieldName, true)[0];
                        if (lInputTxtField != null)
                        {
                            double.TryParse(lInputTxtField.Text, out lVal);
                            if (!newHash.ContainsKey(lChannelInputId)) newHash.Add(lChannelInputId, lVal);
                        }
                    }
               }
                catch (System.InvalidCastException e)
                {
                    //do nothing for now
                }

            }

            //int k = 0;
            //for (int j = 0; j < this.getComponentCount(); j++)
            //{
            //    if (inputNames.contains(this.getComponent(j).getName()))
            //    {
            //        try
            //        {
            //            double putValue = Double.parseDouble(((JTextField)this.getComponent(j)).getText());
            //            plotdata.value[k] = putValue; //xyang
            //            k = k + 1;
            //            newHash.put(this.getComponent(j).getName(), putValue);
            //            //System.out.println(this.getComponent(j).getName());
            //        }
            //        catch (ClassCastException cce)
            //        {
            //            newHash.put(this.getComponent(j).getName(), Double.NaN);
            //        }
            //    }
            //}
            return newHash;
        }

        /**
 * 
 *<P>This method cycles through the checkboxes on the panel.  If a checkbox is selected,
 *then the appropriate control device calculator is called to perform a calcuation based on
 *its inputs.  The vectors that are returned from the control device calculators
 *are appended to the open channel calcualtor resutls.  Note:  each calculator is
 *run independently.  i.e. The results of the open channel calculator will not 
 *be affected by activating a control structure.</P>
 *<P><B>Limitations:</B>  </P>
 * @param openChannelResult The vector from the calcuations of the open channel calculator.
 * @return The vector of results from the open channel and the control devices.  
 * 
 */
        protected Dictionary<FTableCalculator.ControlDeviceType, ArrayList> doControlStructureCalculations()
        {
            //    	the user wants to run a calculation.
            //LinkedHashMap<String, Vector> outputHash = new LinkedHashMap<String, Vector>();
            Dictionary<FTableCalculator.ControlDeviceType, ArrayList> outputHash = new Dictionary<FTableCalculator.ControlDeviceType, ArrayList>();
            //ControlDeviceMiddleware cdmiddlewr = null;
            Hashtable inputHash = new Hashtable();
            //java.util.Hashtable<String, Vector> outputHash = new Hashtable<String, Vector>(); //this will hold all the vector results of calcuations;
            if (clsGlobals.gOCSelectedWeirTri)
            { //controlCheckBoxes.get(i).getName() == "Triangular Vnotch Weir")
                FTableCalcOCWeirTriVNotch lWeirTri = new FTableCalcOCWeirTriVNotch();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCWeirTriVnotch[0], out lInputVal))
                    lWeirTri.WeirAngle = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirTriVnotch[1], out lInputVal))
                    lWeirTri.WeirInvert = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirTriVnotch[2], out lInputVal))
                    lWeirTri.DischargeCoefficient = lInputVal;
                lWeirTri.Height = FTableCalculator.TotalDepth;
                ArrayList lFTCResult = lWeirTri.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.WeirTriVNotch, lFTCResult);
            }
            if (clsGlobals.gOCSelectedWeirTrape)
            { //controlCheckBoxes.get(i).getName() == "Trapezoidal Weir (Cipoletti)")
                FTableCalcOCWeirTrapeCipolletti lWeirTrape = new FTableCalcOCWeirTrapeCipolletti();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCWeirTrape[0], out lInputVal))
                    lWeirTrape.WeirWidth = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirTrape[1], out lInputVal))
                    lWeirTrape.WeirInvert = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirTrape[2], out lInputVal))
                    lWeirTrape.DischargeCoefficient = lInputVal;
                lWeirTrape.Height = FTableCalculator.TotalDepth;
                ArrayList lFTCResult = lWeirTrape.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.WeirTrapeCipolletti, lFTCResult);
            }
            if (clsGlobals.gOCSelectedWeirBroad)
            { //controlCheckBoxes.get(i).getName() == "Broad Crested Weir")
                FTableCalcOCWeirBroad lWeirBroad = new FTableCalcOCWeirBroad();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCWeirBroad[0], out lInputVal))
                    lWeirBroad.WeirWidth = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirBroad[1], out lInputVal))
                    lWeirBroad.WeirInvert = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirBroad[2], out lInputVal))
                    lWeirBroad.DischargeCoefficient = lInputVal;
                lWeirBroad.Height = FTableCalculator.TotalDepth;
                ArrayList lFTCResult = lWeirBroad.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.WeirBroadCrest, lFTCResult);
            }

            if (clsGlobals.gOCSelectedWeirRect)
            { //controlCheckBoxes.get(i).getName() == "Rectangular Weir")
                FTableCalcOCWeirRectangular lWeirRect = new FTableCalcOCWeirRectangular();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCWeirRect[0], out lInputVal))
                    lWeirRect.WeirWidth = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirRect[1], out lInputVal))
                    lWeirRect.WeirInvert = lInputVal;
                if (double.TryParse(clsGlobals.gOCWeirRect[2], out lInputVal))
                    lWeirRect.DischargeCoefficient = lInputVal;
                lWeirRect.Height = FTableCalculator.TotalDepth;
                ArrayList lFTCResult = lWeirRect.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.WeirRectangular, lFTCResult);
            }
            if (clsGlobals.gOCSelectedOrificeUnd)
            { //controlCheckBoxes.get(i).getName() == "Underdrain Orifice")
                FTableCalcOCOrificeUnderflow lOrificeUnd = new FTableCalcOCOrificeUnderflow();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCOrificeUnd[0], out lInputVal))
                    lOrificeUnd.OrificePipeDiameter = lInputVal;
                if (double.TryParse(clsGlobals.gOCOrificeUnd[1], out lInputVal))
                    lOrificeUnd.OrificeInvertDepth = lInputVal;
                if (double.TryParse(clsGlobals.gOCOrificeUnd[2], out lInputVal))
                    lOrificeUnd.OrificeDischargeCoefficient = lInputVal;
                ArrayList lFTCResult = lOrificeUnd.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.OrificeUnderdrain, lFTCResult);
            }
            if (clsGlobals.gOCSelectedOrificeRiser)
            { //controlCheckBoxes.get(i).getName() == "Riser Orifice")

                FTableCalcOCOrificeRiser lOrificeRiser = new FTableCalcOCOrificeRiser();
                double lInputVal = 0;
                if (double.TryParse(clsGlobals.gOCOrificeRiser[0], out lInputVal))
                    lOrificeRiser.OrificePipeDiameter = lInputVal;
                if (double.TryParse(clsGlobals.gOCOrificeRiser[1], out lInputVal))
                    lOrificeRiser.OrificeDepth = lInputVal;
                if (double.TryParse(clsGlobals.gOCOrificeRiser[2], out lInputVal))
                    lOrificeRiser.OrificeDischargeCoefficient = lInputVal;
                ArrayList lFTCResult = lOrificeRiser.GenerateFTableOC();
                //One can do some checking before putting it in the outputHash
                outputHash.Add(FTableCalculator.ControlDeviceType.OrificeRiser, lFTCResult);
            }
            return outputHash;
            /*
            cdmiddlewr = new ControlDeviceMiddleware(null); //calculator type is irrelevant for this operation
            cdmiddlewr.setOpenChannelVector(openChannelResult);
            return cdmiddlewr.mergeCalculators(outputHash);
            */
        }

        /**
	 * 
	 *<P>Returns a vector of column names by appending to the vector of column names from the calculator class.  </P>
	 *<P><B>Limitations:</B>  </P>
	 * @param defaultColNames
	 * @return
	 */
        protected ArrayList appendCheckedCalculatorColumnNames(ArrayList defaultColNames)
        {
            string[] unit = { " (cms)", " (cfs)" };
            String colName = "";
            int punit = FTableCalculatorConstants.programunits;
            if (clsGlobals.gOCSelectedWeirTri)
            {
                colName = "v_notchwr" + unit[punit];
                defaultColNames.Add(colName);
            }
            if (clsGlobals.gOCSelectedWeirTrape)
            {
                colName = "trapwr" + unit[punit];
                defaultColNames.Add(colName);
            }
            if (clsGlobals.gOCSelectedWeirBroad)
            {
                colName = "bdcrstdwr" + unit[punit];
                defaultColNames.Add(colName);
            }
            if (clsGlobals.gOCSelectedWeirRect)
            {
                colName = "rctnglrwr" + unit[punit];
                defaultColNames.Add(colName);
            }
            if (clsGlobals.gOCSelectedOrificeUnd)
            {
                colName = "udrdrnorf" + unit[punit];
                defaultColNames.Add(colName);
            }
            if (clsGlobals.gOCSelectedOrificeRiser)
            {
                colName = "riserorf" + unit[punit];
                defaultColNames.Add(colName);
            }
            return defaultColNames;
        }

        private bool InputsAreOk(ArrayList inputNames)
        {
            bool allinputsOK = true;
            // first clone names list and add the names that this class has added as inputs

            double lVal;
            bool lOneInputIsOK;
            foreach (FTableCalculator.ChannelGeomInput lInputType in inputNames)
            {
                string lControlName = "";
                if (FTableCalculator.LookupChannelGeomInput.TryGetValue(lInputType, out lControlName))
                {
                    lControlName = lControlName.Substring(lControlName.IndexOf(",") + 1);
                    TextBox ltxtField = (TextBox)this.Controls.Find(lControlName, true)[0];
                    lOneInputIsOK = true;
                    if (ltxtField != null)
                    {
                        if (!double.TryParse(ltxtField.Text, out lVal) || lVal < 0)
                        {
                            allinputsOK = false;
                            lOneInputIsOK = false;
                            handleInputAppearance(ltxtField, false);
                        }
                        else
                        {
                            switch (lInputType)
                            {
                                case FTableCalculator.ChannelGeomInput.Length:
                                    break;
                                case FTableCalculator.ChannelGeomInput.Diameter:
                                case FTableCalculator.ChannelGeomInput.MaximumDepth:
                                    if ((lVal > 100) || (lVal < 0.1))
                                    {
                                        allinputsOK = false;
                                        lOneInputIsOK = false;
                                        handleInputAppearance(ltxtField, false);
                                    }
                                    break;
                                case FTableCalculator.ChannelGeomInput.NFP_BankLeftManningsN:
                                case FTableCalculator.ChannelGeomInput.NFP_BankRightManningsN:
                                case FTableCalculator.ChannelGeomInput.NFP_ChannelManningsN:
                                case FTableCalculator.ChannelGeomInput.ManningsN:
                                    if (lVal == 0)
                                    {
                                        allinputsOK = false;
                                        lOneInputIsOK = false;
                                        handleInputAppearance(ltxtField, false);
                                    }
                                    break;
                                case FTableCalculator.ChannelGeomInput.NFP_BankRightEndX:
                                    if (clsGlobals.gProfileStations != null && clsGlobals.gProfileStations.Count > 0)
                                    {
                                        XSectionStation lastStation = (XSectionStation)clsGlobals.gProfileStations[clsGlobals.gProfileStations.Count - 1];
                                        double lastStationX = lastStation.x;
                                        if (lVal > lastStationX)
                                        {
                                            allinputsOK = false;
                                            lOneInputIsOK = false;
                                            handleInputAppearance(ltxtField, false);
                                        }
                                    }
                                    break;
                            }
                        }
                        if (lOneInputIsOK)
                        {
                            //reset to normal appearance
                            handleInputAppearance(ltxtField, true);
                        }
                    }
                }

            }

            if (chkBackfill.Checked)
            {
                lOneInputIsOK = true;
                if (double.TryParse(txtBackfillPore.Text, out lVal))
                {
                    if (lVal > 1d || lVal < 0d)
                    {
                        allinputsOK = false;
                        lOneInputIsOK = false;
                        handleInputAppearance(txtBackfillPore, false);
                    }
                }
                else
                {
                    allinputsOK = false;
                    lOneInputIsOK = false;
                    handleInputAppearance(txtBackfillPore, false);
                }
                if (lOneInputIsOK)
                    handleInputAppearance(txtBackfillPore, true);

                lOneInputIsOK = true;
                double lvalMaxChannelDepth, lvalBackfillDepth;
                if (double.TryParse(txtBackfillDepth.Text, out lvalBackfillDepth))
                {
                    if (txtGeomMaxDepth.Visible)
                    {
                        if (double.TryParse(txtGeomMaxDepth.Text, out lvalMaxChannelDepth))
                        {
                            if (lvalBackfillDepth > lvalMaxChannelDepth)
                            {
                                allinputsOK = false;
                                lOneInputIsOK = false;
                                handleInputAppearance(txtBackfillDepth, false);
                            }
                        }

                    }
                }
                else
                {
                    allinputsOK = false;
                    lOneInputIsOK = false;
                    handleInputAppearance(txtBackfillDepth, false);
                }
                if (lOneInputIsOK)
                    handleInputAppearance(txtBackfillDepth, true);
            }

            //resetInputErrorMarks();
            if (!allinputsOK)
            {
                System.Windows.Forms.MessageBox.Show(
                        "Some fields contain inputs that are not valid.  \n"
                      + "Please check the indicated field's tool tips for further information.",
                        "Invalid Input");
            }
            return allinputsOK;
        }

        private bool handleInputAppearance(Control cpt, bool isOK)
        {
            string ltooltipMsg = "";
            if (isOK)
            {
                cpt.BackColor = Color.White;
            }
            else
            {
                cpt.BackColor = FTableCalculatorConstants.error_color;
                ltooltipMsg = cpt.Text + " is not a valid input for " + cpt.Name + ".";
            }

            try
            {
                toolTip1.SetToolTip(cpt, ltooltipMsg);
                return true;
            }
            catch (System.Exception cce)
            {
                return false;
            }
        }

        private void btnShowOptControls_Click(object sender, EventArgs e)
        {
            //frmOutflowControls lOutflowControlDialog = new frmOutflowControls();
            //lOutflowControlDialog.ShowDialog();
            frmOutflowControlsME lOutflowControlDialog = new frmOutflowControlsME();
            lOutflowControlDialog.ShowDialog();
            int CDCount = 0;
            for (int i = 0; i < 5; i++)
            {
                CDCount += clsGlobals.gExitOCSetup[i].Nodes.Count;
            }
            btnShowOptControls.Text = "Show optional control devices";
            if (CDCount > 0 && !(clsGlobals.gToolType == clsGlobals.ToolType.Gray && CDCount == 1))
                btnShowOptControls.Text += " (...)"; //" (" + CDCount + " CDs)";

            //using (Graphics cg = this.CreateGraphics())
            //{
            //    SizeF size = cg.MeasureString(lnewText, btnShowOptControls.Font);
            //    btnShowOptControls.Padding = new System.Windows.Forms.Padding(2);
            //    btnShowOptControls.Width = (int)size.Width;
            //    btnShowOptControls.Text = lnewText;
            //}
        }

        private void cboBMPTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lBMPNameSelected = (string)cboBMPTypes.SelectedItem;
            foreach (FTableCalculator.BMPType lBMPType in FTableCalculator.BMPTypeNames.Keys)
            {
                string lBMPName = FTableCalculator.BMPTypeNames[lBMPType];
                if (lBMPName == lBMPNameSelected)
                {
                    clsGlobals.gBMPType = lBMPType;
                    FTableCalculator.ChannelType lChannelTypeMatch = FTableCalculator.LookupBMPTypeChannel[lBMPType];
                    switch (lChannelTypeMatch)
                    {
                        case FTableCalculator.ChannelType.NONE:
                            return;
                        case FTableCalculator.ChannelType.CIRCULAR:
                            rdoChCirc.Checked = true;
                            return;
                        case FTableCalculator.ChannelType.TRIANGULAR:
                            rdoChTri.Checked = true;
                            return;
                        case FTableCalculator.ChannelType.RECTANGULAR:
                            rdoChRect.Checked = true;
                            return;
                        case FTableCalculator.ChannelType.TRAPEZOIDAL:
                            rdoChTrape.Checked = true;
                            return;
                        case FTableCalculator.ChannelType.PARABOLIC:
                            rdoChPara.Checked = true;
                            return;
                        case FTableCalculator.ChannelType.NATURAL:
                            rdoChNatural.Checked = true;
                            return;
                    }
                }
            }
        }

        private void btnImportChProfile_Click(object sender, EventArgs e)
        {
            frmImportProfile lImport = new frmImportProfile();
            lImport.NewProfileDataRead += this.NewProfileDataRead;
            lImport.ShowDialog();
            lImport.NewProfileDataRead -= this.NewProfileDataRead;
            lImport.Dispose();
            lImport = null;
        }

        private void NewProfileDataRead(object sender, NewProfileEventArgs e)
        {
            if (!e.ReadSuccess)
                return;
            if (grdChProfile.Source == null)
                return;
            else
                ClearProfileGrid();

            PopulateProfileGrid();
        }

        private void PopulateProfileGrid()
        {
            int lgrdRow = 1;
            string lFormat = "{0:0.000000}";
            XSectionStation lStation;
            for (int i = 0; i < clsGlobals.gProfileStations.Count; i++)
            {
                lStation = (XSectionStation)clsGlobals.gProfileStations[i]; //get a point or station on the cross section profile
                grdChProfile.Source.set_CellValue(lgrdRow, 0, string.Format(lFormat, (object)lStation.x));
                grdChProfile.Source.set_CellValue(lgrdRow, 1, string.Format(lFormat, (object)lStation.y));
                lgrdRow++;
            }

            grdChProfile.Initialize(grdChProfile.Source);
            grdChProfile.SizeAllColumnsToContents();
            grdChProfile.Refresh();
        }

        private void ClearProfileGrid()
        {
            for (int i = 1; i < grdChProfile.Source.Rows; i++)
            {
                for (int j = 0; j < grdChProfile.Source.Columns; j++)
                {
                    grdChProfile.Source.set_CellValue(i, j, "");
                }
            }
            grdChProfile.Refresh();
        }

        private void btnUndoClear_Click(object sender, EventArgs e)
        {
            if (grdChProfile.Source == null)
                return;

            if (clsGlobals.gProfileStations != null && clsGlobals.gProfileStations.Count > 0) 
                PopulateProfileGrid();
        }

        private void btnClearProfile_Click(object sender, EventArgs e)
        {
            if (grdChProfile.Source == null)
                return;
            ClearProfileGrid();
        }
    }
}
