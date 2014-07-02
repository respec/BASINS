using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcOCOrificeUnderflow : FTableCalculator, IFTableOperationsOC
    {
        public double OrificePipeDiameter = -999;
        public double pOrificeInvertDepth = -999;
        public double OrificeInvertDepth
        {
            get { return pOrificeInvertDepth; }
            set { pOrificeInvertDepth = value; }
        }
        public double OrificeDischargeCoefficient = -999;

        public FTableCalcOCOrificeUnderflow()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH(ft)");
            vectorColNames.Add("AREA(acres)");
            vectorColNames.Add("VOLUME(ac-ft)");
            vectorColNames.Add("OUTFLOW(cfs)");
        }

        public ArrayList GenerateFTableOC()
        {
            return GenerateFTable(OrificePipeDiameter, OrificeInvertDepth, OrificeDischargeCoefficient);
        }

        public ArrayList GenerateFTable(double PipeDiameter,
                                     double channelManningsValue, double DischargeCoefficient)
        {
            ArrayList vectorRowData = new ArrayList();
            //Flow Area Calculations
            //double L = channelLength;
            double N = channelManningsValue;
            double DT = PipeDiameter;
            //double S = longitudalChannelSlope;
            //double w = topChannelWidth;		      

            double Co = DischargeCoefficient;

            ArrayList row = new ArrayList();

            double QC = 0.0;
            double acr = 0.0;
            double stot = 0.0;
            double prevAcr = 0.0; ;
            double prevStot = 0.0;
            double R1 = 0.0;
            double Area = 0.0;

            string sDepth = "";
            string sArea = "";
            string sVolume = "";
            string sOutFlow = "";
            string lFormat = "{0:0.00000}";
            for (double g = N; g < 1000.0; g += FTableCalculatorConstants.calculatorIncrement)
            {
                if (g >= N)
                {
                    R1 = g - N;
                }
                Area = 3.14159 * (Math.Pow(DT, 2) / 4);

                if (FTableCalculatorConstants.programunits == 1)  // if units selected are metric cf
                  //QC = 0.6* Area * Math.Pow(64.4, 0.5) * Math.Pow(R1, 0.5);
                    QC = Co * Area * Math.Pow(64.4, 0.5) * Math.Pow(R1, 0.5);  //sri

                // conversion option added by sri
                if (FTableCalculatorConstants.programunits == 0)  // if units selected are metric  cms
                  //QC = 0.6 *Area * Math.Pow(19.62, 0.5) * Math.Pow(R1, 0.5);
                    QC = Co * Area * Math.Pow(19.62, 0.5) * Math.Pow(R1, 0.5);  //sri

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