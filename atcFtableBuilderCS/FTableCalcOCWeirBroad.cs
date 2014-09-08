using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcOCWeirBroad : FTableCalculator, IFTableOperationsOC
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
        public new int myExit
        {
            get { return pExit; }
            set { pExit = value; }
        }
        public double DischargeCoefficient = -999;
        private string[] gOCWeirBroadLbl = { "Weir Crest Width", "Weir Invert Depth", "Discharge Coefficient" };
        private double[] DefaultsWeirBroad = { 10, 5, 3.0 };
        public new ControlDeviceType ControlDevice
        {
            get
            {
                return ControlDeviceType.WeirBroadCrest;
            }
        }

        public FTableCalcOCWeirBroad()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH");
            vectorColNames.Add("AREA");
            vectorColNames.Add("VOLUME");
            vectorColNames.Add("OUTFLOW");
        }

        public Dictionary<string, double> ParamValueDefaults()
        {
            Dictionary<string, double> defaults = new Dictionary<string, double>();
            for (int i = 0; i <= gOCWeirBroadLbl.Length - 1; i++)
            {
                defaults.Add(gOCWeirBroadLbl[i], DefaultsWeirBroad[i]);
            }
            return defaults;
        }
        public new Dictionary<string, double> ParamValues()
        {
            double[] CurrentParamValues = { WeirWidth, WeirInvert, DischargeCoefficient };
            Dictionary<string, double> lParams = new Dictionary<string, double>();
            for (int i = 0; i <= gOCWeirBroadLbl.Length - 1; i++)
            {
                lParams.Add(gOCWeirBroadLbl[i], CurrentParamValues[i]);
            }
            return lParams;
        }

        public FTableCalculator Clone()
        {
            FTableCalcOCWeirBroad lClone = new FTableCalcOCWeirBroad();
            lClone.myExit = this.myExit;
            lClone.WeirWidth = this.WeirWidth;
            lClone.WeirInvert = this.WeirInvert;
            lClone.DischargeCoefficient = this.DischargeCoefficient;
            return lClone;
        }

        public ArrayList GenerateFTableOC()
        {
            return GenerateFTable(WeirWidth, Height, WeirInvert, DischargeCoefficient);
        }
        public ArrayList GenerateFTable(double aWeirLength, double aHeight, double aWeirInvert, double aDischargeCoefficient)
        {
            if (WeirWidth < 0 || aHeight < 0 || aWeirInvert == -999 || aDischargeCoefficient < 0) return null;
            ArrayList vectorRowData = new ArrayList();
            //Flow Area Calculations
            double L = aWeirLength;
            double N = aWeirInvert;
            double DT = aHeight;
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
            for (double g = N; g < 1000.0; g += FTableCalculatorConstants.calculatorIncrement)
            {
                if (g > (N))
                {
                    R1 = g - N;
                }
                //Cw = 3.089;
                //   acg= 0.385*Math.pow(32.2*2,0.5);
                QC = Cw * (L) * Math.Pow(R1, 1.5);

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
