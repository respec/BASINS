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
    public partial class frmInfiltration : Form
    {
        public event EventHandler<InfiltrationMethodChangeEventArgs> InfiltrationMethodChanged;

        private Infiltration_Calc Calculator = null;
        private bool pLoaded = false;
        public frmInfiltration()
        {
            InitializeComponent();
            rdoInfilMethodGAmp.CheckedChanged += new EventHandler(InfilMethodChanged);
            rdoInfilMethodMaryland.CheckedChanged +=new EventHandler(InfilMethodChanged);
        }

        private void InfilMethodChanged(object sender, EventArgs e)
        {
            if (!pLoaded) return;
            if (rdoInfilMethodGAmp.Checked)
            {
                frameGAmp.Enabled = true;
                InfiltrationMethodChangeEventArgs args = new InfiltrationMethodChangeEventArgs();
                args.InfilMethod = Infiltration_Calc.INFILCALCMETHODS.GREENAMPT;
                //Java "%.5f" -> 5 digits after decimal point
                args.ResultInfiltrationRate = String.Format("{0:0.00000}", Infiltration_Calc.ResultInfiltrationRate);
                args.ResultInfiltrationDepth = String.Format("{0:0.00000}", Infiltration_Calc.ResultInfiltrationDepth);
                args.ResultInfiltrationDrainTime = String.Format("{0:0.00000}", Infiltration_Calc.ResultInfiltrationDrainTime);
                OnInfiltrationMethodChanged(args);
            }
            else if (rdoInfilMethodMaryland.Checked)
            {
                frameGAmp.Enabled = false;
                cboSoilMenu_SelectedIndexChanged(null, null);
            }
        }

        protected virtual void OnInfiltrationMethodChanged(InfiltrationMethodChangeEventArgs e)
        {
            EventHandler<InfiltrationMethodChangeEventArgs> handler = InfiltrationMethodChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RaiseInfilEvent(string aInfilRate, string aInfilDepth, string aInfilDrainTime)
        {
            InfiltrationMethodChangeEventArgs args = new InfiltrationMethodChangeEventArgs();
            if (rdoInfilMethodMaryland.Checked)
                args.InfilMethod = Infiltration_Calc.INFILCALCMETHODS.MARYLANDLOOKUP;
            else if (rdoInfilMethodGAmp.Checked)
                args.InfilMethod = Infiltration_Calc.INFILCALCMETHODS.GREENAMPT;

            //Java "%.5f" -> 5 digits after decimal point
            if (string.IsNullOrEmpty(aInfilRate))
                args.ResultInfiltrationRate = String.Format("{0:0.00000}", (object)Infiltration_Calc.ResultInfiltrationRate);
            else
                args.ResultInfiltrationRate = aInfilRate;

            if (string.IsNullOrEmpty(aInfilDepth))
                args.ResultInfiltrationDepth = String.Format("{0:0.00000}", Infiltration_Calc.ResultInfiltrationDepth);
            else
                args.ResultInfiltrationDepth = aInfilDepth;

            if (string.IsNullOrEmpty(aInfilDrainTime))
                args.ResultInfiltrationDrainTime = String.Format("{0:0.00000}", Infiltration_Calc.ResultInfiltrationDrainTime);
            else
                args.ResultInfiltrationDrainTime = aInfilDrainTime;

            OnInfiltrationMethodChanged(args);
        }

        private void frmInfiltration_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < clsGlobals.soilTypes.Length; i++)
            {
                cboSoilMenu.Items.Add(clsGlobals.soilTypes[i]);
            }

            pLoaded = true;
        }

        private bool InputsAreOK(Dictionary<Infiltration_Calc.GAMPPARAM, double> aGAVarMap)
        {
            bool lInputsAreGood = true;

            if (aGAVarMap == null) aGAVarMap = new Dictionary<Infiltration_Calc.GAMPPARAM, double>();

            if (rdoInfilMethodGAmp.Checked)
            {
                if (!IsFieldValid(txtGAmpEffPorosity.Text))
                {
                    txtGAmpEffPorosity.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpEffPorosity.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.EffPorosity, double.Parse(txtGAmpEffPorosity.Text));
                }

                if (!IsFieldValid(txtGAmpInitWater.Text))
                {
                    txtGAmpInitWater.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpInitWater.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.InitWater, double.Parse(txtGAmpInitWater.Text));
                }

                if (!IsFieldValid(txtGAmpPorosity.Text))
                {
                    txtGAmpPorosity.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpPorosity.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.Porosity, double.Parse(txtGAmpPorosity.Text));
                }

                if (!IsFieldValid(txtGAmpResWater.Text))
                {
                    txtGAmpResWater.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpResWater.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.ResWater, double.Parse(txtGAmpResWater.Text));
                }

                if (!IsFieldValid(txtGAmpSatHydCond.Text))
                {
                    txtGAmpSatHydCond.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpSatHydCond.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.HydCond, double.Parse(txtGAmpSatHydCond.Text));
                }

                if (!IsFieldValid(txtGAmpSoilDepth.Text))
                {
                    txtGAmpSoilDepth.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpSoilDepth.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.SoilDepth, double.Parse(txtGAmpSoilDepth.Text));
                }

                if (!IsFieldValid(txtGAmpSuction.Text))
                {
                    txtGAmpSuction.BackColor = FTableCalculatorConstants.error_color;
                    lInputsAreGood = false;
                }
                else
                {
                    txtGAmpSuction.BackColor = Color.White;
                    aGAVarMap.Add(Infiltration_Calc.GAMPPARAM.SuctionHead, double.Parse(txtGAmpSuction.Text));
                }
            }
            else if (rdoInfilMethodMaryland.Checked)
            {

            }

            if (!lInputsAreGood)
            {
                aGAVarMap.Clear();
                aGAVarMap = null;
            }

            return lInputsAreGood;
        }

        private bool IsFieldValid(string aParam)
        {
            bool lIsOK = true;
            double lVal;
            if (!double.TryParse(aParam.Trim(), out lVal))
                lIsOK = false;
            else
            {
                if (lVal < 0)
                    lIsOK = false;
            }

            return lIsOK;
        }

        private void btnCalcInfil_Click(object sender, EventArgs e)
        {
            Dictionary<Infiltration_Calc.GAMPPARAM, double> gavarmap = new Dictionary<Infiltration_Calc.GAMPPARAM,double>();
            if (!InputsAreOK(gavarmap))
            {
                System.Windows.Forms.MessageBox.Show("Some fields contain inputs that are not valid.  \n" +
                "Enter values for the indicated parameters.", "Invalid Input");
                return;
            }

            if (Calculator == null)
                Calculator = new Infiltration_Calc();

            //Do GAmp calculation
            double[] InfiltrationResults = Calculator.GreemAmptMethod(gavarmap);
            //Save results
            Infiltration_Calc.ResultInfiltrationRate = InfiltrationResults[0];
            Infiltration_Calc.ResultInfiltrationDepth = InfiltrationResults[1];
            Infiltration_Calc.ResultInfiltrationDrainTime = InfiltrationResults[2];
            RaiseInfilEvent("", "", "");
        }

        private void cboSoilMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSoilMenu.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Must select a soil type first", "Infiltration Calculator");
                return;
            }
            if (Calculator == null) Calculator = new Infiltration_Calc();
            string lSoilName = cboSoilMenu.SelectedItem.ToString();

            Dictionary<string, string> soilprop = Calculator.MDMethod(lSoilName);

            string lVal = "";
            bool lValIsOK;
            foreach (string lKey in soilprop.Keys)
            {
                lValIsOK = false;
                if (soilprop.TryGetValue(lKey, out lVal)) lValIsOK = true;
                if (lKey.StartsWith("Porosity")) {
                    if (lValIsOK) txtGAmpPorosity.Text = lVal;
                } else if (lKey.StartsWith("Effective Porosity")) {
                    if (lValIsOK) txtGAmpEffPorosity.Text = lVal;
                } else if (lKey.StartsWith("Suction Head")) {
                    if (lValIsOK) txtGAmpSuction.Text = lVal;
                } else if (lKey.StartsWith("Pore Size Distribution")) {
                } else if (lKey.StartsWith("Residual Water Content")) {
                    if (lValIsOK) txtGAmpResWater.Text = lVal;
                } else if (lKey.StartsWith("Hydraulic Head")) {
                } else if (lKey.StartsWith("Hydraulic Conductivity")){
                    if (lValIsOK) txtGAmpSatHydCond.Text = lVal;
                } else if (lKey.StartsWith("Typical Water Capacity")) {
                } else if (lKey.StartsWith("Typical Infilteration Rate")) {
                    if (rdoInfilMethodMaryland.Checked)
                        Infiltration_Calc.ResultInfiltrationRate = double.Parse(lVal);
                } else if (lKey.StartsWith("Hydrologic Soil Group")) {
                }
            }

            if (rdoInfilMethodMaryland.Checked)
            {
                InfiltrationMethodChangeEventArgs args = new InfiltrationMethodChangeEventArgs();
                args.InfilMethod = Infiltration_Calc.INFILCALCMETHODS.MARYLANDLOOKUP;
                args.ResultInfiltrationRate = Infiltration_Calc.ResultInfiltrationRate.ToString(); 
                args.ResultInfiltrationDepth = "";
                args.ResultInfiltrationDrainTime = "";
                OnInfiltrationMethodChanged(args);
            }
        }
    }

    public class InfiltrationMethodChangeEventArgs : EventArgs
    {
        //public int Threshold { get; set; }
        //public DateTime TimeReached { get; set; }
        public Infiltration_Calc.INFILCALCMETHODS InfilMethod;
        public string ResultInfiltrationRate;
        public string ResultInfiltrationDepth;
        public string ResultInfiltrationDrainTime;
    }
}
