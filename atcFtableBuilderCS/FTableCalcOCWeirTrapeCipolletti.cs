using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcOCWeirTrapeCipolletti : FTableCalculator, IFTableOperationsOC
    {
        public double WeirWidth = -999;
        public double Height = -999;
        private double pWeirInvert = -999;
        public double WeirInvert
        {
            get { return pWeirInvert; }
            set { pWeirInvert = value; }
        }
        int pExit;
        public override int myExit
        {
            get { return pExit; }
            set { pExit = value; }
        }
        public double DischargeCoefficient = -999;
        private string[] gOCWeirTrapeLbl = { "Weir Width", "Weir Depth", "Discharge Coefficient" };
        private double[] DefaultsWeirTrape = { 10, 5, 3.367 };
        public override ControlDeviceType ControlDevice
        {
            get
            {
                return ControlDeviceType.WeirTrapeCipolletti;
            }
        }

        public FTableCalcOCWeirTrapeCipolletti()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH(ft)");
            vectorColNames.Add("AREA(acres)");
            vectorColNames.Add("VOLUME(ac-ft)");
            vectorColNames.Add("OUTFLOW(cfs)");
        }

        public Dictionary<string, double> ParamValueDefaults()
        {
            Dictionary<string, double> defaults = new Dictionary<string, double>();
            for (int i = 0; i <= gOCWeirTrapeLbl.Length - 1; i++)
            {
                defaults.Add(gOCWeirTrapeLbl[i], DefaultsWeirTrape[i]);
            }
            return defaults;
        }
        public override Dictionary<string, double> ParamValues()
        {
            double[] CurrentParamValues = { WeirWidth, WeirInvert, DischargeCoefficient };
            Dictionary<string, double> defaults = new Dictionary<string, double>();
            for (int i = 0; i <= gOCWeirTrapeLbl.Length - 1; i++)
            {
                defaults.Add(gOCWeirTrapeLbl[i], CurrentParamValues[i]);
            }
            return defaults;
        }
        public override bool SetParamValues(Dictionary<string, double> aParams)
        {
            int lUpdateCtr = 0;
            double lValue = 0;
            foreach (string lKey in aParams.Keys)
            {
                if (lKey == gOCWeirTrapeLbl[0])
                {
                    if (aParams.TryGetValue(lKey, out lValue))
                    {
                        WeirWidth = lValue;
                        lUpdateCtr += 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (lKey == gOCWeirTrapeLbl[1])
                {
                    if (aParams.TryGetValue(lKey, out lValue))
                    {
                        WeirInvert = lValue;
                        lUpdateCtr += 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (lKey == gOCWeirTrapeLbl[2])
                {
                    if (aParams.TryGetValue(lKey, out lValue))
                    {
                        DischargeCoefficient = lValue;
                        lUpdateCtr += 1;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (lUpdateCtr == gOCWeirTrapeLbl.Length)
                return true;
            else
                return false;
        }

        public override FTableCalculator Clone()
        {
            FTableCalcOCWeirTrapeCipolletti lClone = new FTableCalcOCWeirTrapeCipolletti();
            lClone.myExit = this.myExit;
            lClone.WeirWidth = this.WeirWidth;
            lClone.WeirInvert = this.WeirInvert;
            lClone.DischargeCoefficient = this.DischargeCoefficient;
            return lClone;
        }

        public override ArrayList GenerateFTableOC()
        {
            return GenerateFTable(WeirWidth, Height, WeirInvert, DischargeCoefficient);
        }
        public ArrayList GenerateFTable(double aWeirLength, double aChannelDiameter, double aWeirInvert, double aDischargeCoefficient)
        {
            if (WeirWidth < 0 || aChannelDiameter < 0 || aWeirInvert == -999 || aDischargeCoefficient < 0) return null;
            ArrayList vectorRowData = new ArrayList();
            //Flow Area Calculations
            double L = aWeirLength;
            double N = aWeirInvert;
            double DT = aChannelDiameter;
            double Cw = aDischargeCoefficient;
            //double S = longitudalChannelSlope;
            //double w = topChannelWidth;		      

            ArrayList row = new ArrayList();

            double QC = 0.0;
            double acr = 0.0;
            double stot = 0.0;
            double prevAcr = 0.0; ;
            double prevStot = 0.0;
            double R1 = 0.0;

            string sDepth = "";
            string sArea = "";
            string sVolume = "";
            string sOutFlow = "";
            string lFormat = "{0:0.00000}";
            for (double g = N; g < DT; g += FTableCalculatorConstants.calculatorIncrement)
            {
                if (g > N)
                {
                    R1 = g - N;
                }

                //Cw = US,(3.367), SI (1.86)
                QC = Cw * Math.Pow(R1, 1.5) * L;
                //QC = 3.33 * Math.Pow(R1, 1.5)* L ;  
                //System.out.print(g);
                //System.out.println("      "); 

                prevStot = stot;
                prevAcr = acr;

                row = new ArrayList();

                sDepth = string.Format(lFormat, (object)g);
                sArea = string.Format(lFormat, (object)acr);
                sVolume = string.Format(lFormat, (object)stot);
                sOutFlow = string.Format(lFormat, (object)QC);
                row.Add(sDepth);
                row.Add(sArea);
                row.Add(sVolume);
                row.Add(sOutFlow);

                vectorRowData.Add(row);
            }
            return vectorRowData;
        }
    }
}
