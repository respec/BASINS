using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcOCWeirTriVNotch : FTableCalculator, IFTableOperationsOC
    {
        private double prvStot = 0;
        private double prvAcr = 0;

        public double WeirAngle = -999;
        public double Height = -999;
        private double pWeirInvert = -999;
        public double WeirInvert
        {
            get { return pWeirInvert; }
            set { pWeirInvert = value; }
        }
        public double DischargeCoefficient = -999;

        public FTableCalcOCWeirTriVNotch()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH");
            vectorColNames.Add("AREA");
            vectorColNames.Add("VOLUME");
            vectorColNames.Add("OUTFLOW");
        }

        public ArrayList GenerateFTableOC()
        {
            return GenerateFTable(WeirAngle, Height, WeirInvert, DischargeCoefficient);
        }

        public ArrayList GenerateFTable(double aWeirAngle, double aChannelDiameter, double aWeirInvert, double aDischargeCoefficient)
        {
            if (aWeirAngle < 0 || aChannelDiameter < 0 || aWeirInvert == -999 || aDischargeCoefficient < 0) return null;
            //Note that channel diameter and the 'Height' input from the independent GUI 
            //must be the same.  Channel Diameter gets set to the DT variable. -BED
            vectorRowData = new ArrayList();
            //Flow Area Calculations
            double L = aWeirAngle;
            double N = aWeirInvert;
            double DT = aChannelDiameter;
            //double S = longitudalChannelSlope;
            //double w = topChannelWidth;		      

            Double Cd = aDischargeCoefficient;

            ArrayList row = new ArrayList();

            double QC = 0.0;
            double acr = 0.0;
            double stot = 0.0;

            double prevAcr = 0.0; ;
            double prevStot = 0.0;
            double CF = 0.0;
            double R1 = 0.0;
            double CD = 0.0;
            double Angle = 0.0;

            Angle = Math.Tan(0.5 * L * 3.14 / 180.0);
            //Cd = 0.585;
            CD = Cd * (8.0 / 15.0);

            // cd * sqrt(2*g)  //english units g= 32.2
            // conversion option added by sri
            if (FTableCalculatorConstants.programunits == 1)  // if units selected are metric  cfs
                CF = CD * Math.Sqrt(2.0 * 32.2) * Angle;

            if (FTableCalculatorConstants.programunits == 0)  // if units selected are metric  cms
                CF = CD * Math.Sqrt(2.0 * 9.81) * Angle;

            string lFormat = "{0:0.00000}";
            string sDepth = "";
            string sArea = "";
            string sVolume = "";
            string sOutFlow = "";

            for (double g = N; g < 1000.0; g += FTableCalculatorConstants.calculatorIncrement)
            {
                if (g > (N - 0.1))
                {
                    R1 = g - N;
                }

                QC = CF * Math.Pow(R1, 2.5);

                //These variables are never used.//////////-BED
                prevStot = stot;
                prevAcr = acr;
                //////////////////////////////////////////         
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
