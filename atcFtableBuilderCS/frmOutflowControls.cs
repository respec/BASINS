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
    public partial class frmOutflowControls : Form
    {
        private bool pLoaded = false;
        //private string [] OrificeDefaults = {"5","2","3.1"};
        private string[] DefaultsOrifice = { "5", "2", "0.6" };  //sri-jul 18 2012
        private string[] DefaultsWeir = { "10", "5", "2" };
        private string[] DefaultsWeirTriVnotch = { "10", "5", "0.585" };
        private string[] DefaultsWeirTrape = { "10", "5", "3.367" };
        private string[] DefaultsWeirBroad = { "10", "5", "3.0" };
        private string[] DefaultsWeirRect = { "10", "5", "3.33" }; // Need to check this value

        public frmOutflowControls()
        {
            InitializeComponent();

            foreach (Control lControl in this.Controls)
            {
                //Console.WriteLine(lControl.Name);
                if (lControl.Name.StartsWith("frame"))
                {
                    foreach (Control lChild in lControl.Controls)
                    {
                        if (lChild.Name.StartsWith("txt"))
                        {
                            lChild.TextChanged += new EventHandler(txtInputsChanged);
                        }
                        else if (lChild.Name.StartsWith("chk"))
                        {
                            ((CheckBox)lChild).CheckedChanged += new EventHandler(OCSelectedChanged);
                        }
                    }
                }
            }
        }

        /**
 * 
 *<P>Checks that the optional inputs are also valid.  It will only check textboxes that have a checkbox checked.</P>
 *<P><B>Limitations:</B>  </P>
 * @return
 */
        public bool checkOptionalInputsPanelInputs()
        {
            bool inputsOk = true;
            //first, reset all error marks on the controls.
            string lControlName = "";
            IEnumerator e = this.Controls.GetEnumerator();
            while (e.MoveNext())
            {
                lControlName = ((Control)e.Current).Name;
                if (lControlName.StartsWith("lbl") ||
                    lControlName.StartsWith("chk") ||
                    lControlName.StartsWith("frame")) continue;
                //resetInputErrorMarks();
            }
            ArrayList lCheckBoxes = new ArrayList();
            lCheckBoxes.Add(chkOCWeirTri);
            lCheckBoxes.Add(chkOCWeirTrape);
            lCheckBoxes.Add(chkOCWeirRect);
            lCheckBoxes.Add(chkOCWeirBroad);
            lCheckBoxes.Add(chkOCOrificeUnd);
            lCheckBoxes.Add(chkOCOrificeRiser);
            object[] controlCheckBoxes = lCheckBoxes.ToArray();
            for (int i = 0; i < controlCheckBoxes.Length; i++)
            {
                CheckBox lchkOC = (CheckBox)controlCheckBoxes[i];
                if (lchkOC.Checked)
                {
                    //String checkBoxName = lchkOC.Name;
                    ////{"Vnotch Weir","Sharp Crested Weir","Broad Crested Weir","Rectangular Weir","Underdrain Orifice","Riser Orifice"};
                    //if (checkBoxName.ToLower().Equals("triangular vnotch weir", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    //colName = "v_notchwr(cfs)";
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Triangular Vnotch Weir")).checkPanelInputs(
                    //            convertStringArrayToArrayList(this.VNotchWeirInputs));
                    //}
                    //else if (checkBoxName.toLowerCase().equals("trapezoidal weir (cipoletti)"))
                    //{
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Trapezoidal Weir (Cipoletti)")).checkPanelInputs(
                    //             convertStringArrayToArrayList(this.TrapezoidalWeirInputs));
                    //}
                    //else if (checkBoxName.toLowerCase().equals("broad crested weir"))
                    //{
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Broad Crested Weir")).checkPanelInputs(
                    //            convertStringArrayToArrayList(this.BroadCrestedWeirInputs));
                    //}
                    //else if (checkBoxName.toLowerCase().equals("rectangular weir"))
                    //{
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Rectangular Weir")).checkPanelInputs(
                    //            convertStringArrayToArrayList(this.RectangularWeirInputs));
                    //}
                    //else if (checkBoxName.toLowerCase().equals("underdrain orifice"))
                    //{
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Underdrain Orifice")).checkPanelInputs(
                    //            convertStringArrayToArrayList(this.UnderdrainOrificeInputs));
                    //}
                    //else if (checkBoxName.toLowerCase().equals("riser orifice"))
                    //{
                    //    inputsOk = ((OptionalInputPanel)this.controlDeviceInputPanels.get("Riser Orifice")).checkPanelInputs(
                    //            convertStringArrayToArrayList(this.RiserOrificeInputs));
                    //}


                    //the panel knows which check box is selected
                }//close the is selected condition  

            }
            return inputsOk;
        }

        public bool InputsAreOk()
        {
            bool lInputsAreOk = true;
            foreach (Control lFrame in this.Controls)
            {
                foreach (Control lComp in lFrame.Controls)
                {
                    if (lComp.Name.StartsWith("chk"))
                    {
                        if (((CheckBox)lComp).Checked)
                        {
                            double putValue;
                            foreach (Control lC in lFrame.Controls)
                            {
                                if (lC.Name.StartsWith("txt"))
                                {
                                    bool isProblematic = false;
                                    if (!double.TryParse(lC.Text, out putValue))
                                    {
                                        isProblematic = true;
                                    }
                                    else if (double.IsNaN(putValue))
                                    {
                                        isProblematic = true;
                                    }
                                    else if ((putValue < 0d)
                                       && (!lC.Name.Contains("Weir Invert"))
                                       && (!lC.Name.Contains("Orifice Height"))
                                       && (!lC.Name.Contains("Riser Height")))
                                    {
                                        isProblematic = true;
                                    }
                                    else
                                    {

                                    }
                                    if (isProblematic)
                                    {
                                        lInputsAreOk = false;
                                        handleBadInput(lC);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lInputsAreOk;
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

        private void frmOutflowControls_Load(object sender, EventArgs e)
        {
            //Initiate default value into a global entity
            clsGlobals.gOCWeirTriVnotch[0] = DefaultsWeirTriVnotch[0];
            clsGlobals.gOCWeirTriVnotch[1] = DefaultsWeirTriVnotch[1];
            clsGlobals.gOCWeirTriVnotch[2] = DefaultsWeirTriVnotch[2];

            clsGlobals.gOCWeirTrape[0] = DefaultsWeirTrape[0];
            clsGlobals.gOCWeirTrape[1] = DefaultsWeirTrape[1];
            clsGlobals.gOCWeirTrape[2] = DefaultsWeirTrape[2];

            clsGlobals.gOCWeirBroad[0] = DefaultsWeirBroad[0];
            clsGlobals.gOCWeirBroad[1] = DefaultsWeirBroad[1];
            clsGlobals.gOCWeirBroad[2] = DefaultsWeirBroad[2];

            clsGlobals.gOCWeirRect[0] = DefaultsWeirRect[0];
            clsGlobals.gOCWeirRect[1] = DefaultsWeirRect[1];
            clsGlobals.gOCWeirRect[2] = DefaultsWeirRect[2];

            clsGlobals.gOCOrificeUnd[0] = DefaultsOrifice[0];
            clsGlobals.gOCOrificeUnd[1] = DefaultsOrifice[1];
            clsGlobals.gOCOrificeUnd[2] = DefaultsOrifice[2];

            clsGlobals.gOCOrificeRiser[0] = DefaultsOrifice[0];
            clsGlobals.gOCOrificeRiser[1] = DefaultsOrifice[1];
            clsGlobals.gOCOrificeRiser[2] = DefaultsOrifice[2];

            //load default values
            txtOCWeirTriVertexAng.Text = clsGlobals.gOCWeirTriVnotch[0];
            txtOCWeirTriInvert.Text = clsGlobals.gOCWeirTriVnotch[1];
            txtOCWeirTriDisCoeff.Text = clsGlobals.gOCWeirTriVnotch[2];

            txtOCWeirTrapeWidth.Text = clsGlobals.gOCWeirTrape[0];
            txtOCWeirTrapeDepth.Text = clsGlobals.gOCWeirTrape[1];
            txtOCWeirTrapeDisCoeff.Text = clsGlobals.gOCWeirTrape[2];

            txtOCWeirBroadWidth.Text = clsGlobals.gOCWeirBroad[0];
            txtOCWeirBroadInvertDepth.Text = clsGlobals.gOCWeirBroad[1];
            txtOCWeirBroadDisCoeff.Text = clsGlobals.gOCWeirBroad[2];

            txtOCWeirRectWidth.Text = clsGlobals.gOCWeirRect[0];
            txtOCWeirRectInvertDepth.Text = clsGlobals.gOCWeirRect[1];
            txtOCWeirRectDisCoeff.Text = clsGlobals.gOCWeirRect[2];

            txtOCOrificeUndDiam.Text = clsGlobals.gOCOrificeUnd[0];
            txtOCOrificeUndInvertDepth.Text = clsGlobals.gOCOrificeUnd[1];
            txtOCOrificeUndDisCoeff.Text = clsGlobals.gOCOrificeUnd[2];

            txtOCOrificeRiserDiam.Text = clsGlobals.gOCOrificeRiser[0];
            txtOCOrificeRiserDepth.Text = clsGlobals.gOCOrificeRiser[1];
            txtOCOrificeRiserDisCoeff.Text = clsGlobals.gOCOrificeRiser[2];

            clsGlobals.gOCSelectedWeirTri = chkOCWeirTri.Checked;
            clsGlobals.gOCSelectedWeirTrape = chkOCWeirTrape.Checked;
            clsGlobals.gOCSelectedWeirBroad = chkOCWeirBroad.Checked;
            clsGlobals.gOCSelectedWeirRect = chkOCWeirRect.Checked;
            clsGlobals.gOCSelectedOrificeUnd = chkOCOrificeUnd.Checked;
            clsGlobals.gOCSelectedOrificeRiser = chkOCOrificeRiser.Checked;

            pLoaded = true;
        }

        //This input change event handler routine really depends on 
        //the 'Tag' attribute of each TextBox to be preset at design time
        //and NOT EDITED during program execution!!!
        private void txtInputsChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            double lVal;
            bool isProblematic = false;
            TextBox lThisTB = (TextBox)sender;
            if (double.TryParse(lThisTB.Text, out lVal))
            {
                if (double.IsNaN(lVal))
                {
                    isProblematic = true;
                } else if ((lVal < 0d)
                                   && (!lThisTB.Name.Contains("Weir Invert"))
                                   && (!lThisTB.Name.Contains("Orifice Height"))
                                   && (!lThisTB.Name.Contains("Riser Height")))
                {
                    isProblematic = true;
                }
                if (isProblematic)
                {
                    handleBadInput(lThisTB);
                    return;
                }
                int TagInd = int.Parse(lThisTB.Tag.ToString());
                if (lThisTB.Name.Contains("Tri"))
                {
                    clsGlobals.gOCWeirTriVnotch[TagInd] = lThisTB.Text;
                }
                else if (lThisTB.Name.Contains("Trape"))
                {
                    clsGlobals.gOCWeirTrape[TagInd] = lThisTB.Text;
                }
                else if (lThisTB.Name.Contains("Broad"))
                {
                    clsGlobals.gOCWeirBroad[TagInd] = lThisTB.Text;
                }
                else if (lThisTB.Name.Contains("Rect"))
                {
                    clsGlobals.gOCWeirRect[TagInd] = lThisTB.Text;
                }
                else if (lThisTB.Name.Contains("Und"))
                {
                    clsGlobals.gOCOrificeUnd[TagInd] = lThisTB.Text;
                }
                else if (lThisTB.Name.Contains("Riser"))
                {
                    clsGlobals.gOCOrificeRiser[TagInd] = lThisTB.Text;
                }

                //Since it gets past testing, reset errors
                lThisTB.BackColor = Color.White;
                toolTip1.SetToolTip(lThisTB, "");
            }
        }

        private void OCSelectedChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            CheckBox lThisCB = (CheckBox)sender;
            if (lThisCB.Checked)
            {
                lThisCB.Parent.BackColor = Color.DarkGray;
            }
            else
            {
                lThisCB.Parent.BackColor = Control.DefaultBackColor;
            }

            string lName = lThisCB.Name;
            if (lName.Contains("Weir"))
            {
                if (lName.Contains("Tri"))
                    clsGlobals.gOCSelectedWeirTri = lThisCB.Checked;
                else if (lName.Contains("Trape"))
                    clsGlobals.gOCSelectedWeirTrape = lThisCB.Checked;
                else if (lName.Contains("Rect"))
                    clsGlobals.gOCSelectedWeirRect = lThisCB.Checked;
                else if (lName.Contains("Broad"))
                    clsGlobals.gOCSelectedWeirBroad = lThisCB.Checked;
            }
            else if (lName.Contains("Orifice"))
            {
                if (lName.Contains("Und"))
                    clsGlobals.gOCSelectedOrificeUnd = lThisCB.Checked;
                else if (lName.Contains("Riser"))
                    clsGlobals.gOCSelectedOrificeRiser = lThisCB.Checked;
            }
        }

        private void frmOutflowControls_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!InputsAreOk())
            {
                System.Windows.Forms.MessageBox.Show("There are still errors in parameters for chosen outflow controls.\n" +
                    "Please correct these errors before exiting this dialog.");
                e.Cancel = true;
                return;
            }
            else
                return;

        }
    }
}