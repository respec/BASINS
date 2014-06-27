using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    //This is the version that actually referenced in the original Java project by ControlDeviceMiddleware.java
    class FTableCalcRectangular : FTableCalculator, IFTableOperations
    {
        public FTableCalcRectangular()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH");
            vectorColNames.Add("AREA");
            vectorColNames.Add("VOLUME");
            vectorColNames.Add("OUTFLOW");
            geoInputNames = new string[]{ "Maximum Channel Depth ", 
                                          "Top Channel Width ", 
                                          "Channel Length ", 
                                          "Channel Mannings Value", 
                                          "Longitudinal Slope (ft/ft)", 
                                          "Height Increment" };  //sri 07-23-2012
    
        }

        public ArrayList GenerateFTable(double channelLength, double Height,
                                        double channelManningsValue, double DischargeCoefficient)
        {
            ArrayList vectorRowData = new ArrayList();
            //Flow Area Calculations
            double L = channelLength;
            double N = channelManningsValue;
            double DT = Height;
            //double S = longitudalChannelSlope;
            //double w = topChannelWidth;		      
            double Cw = DischargeCoefficient;

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
                if (g >= N)
                {
                    R1 = g - N;
                }

                // with full contraction
                QC = Cw * System.Math.Pow(R1, 1.5) * (L - 0.2 * R1);
                prevStot = stot;
                prevAcr = acr;

                row = new ArrayList();

                sDepth   = String.Format(lFormat, (object)g);
                sArea    = String.Format(lFormat, (object)acr);
                sVolume  = String.Format(lFormat, (object)stot);
                sOutFlow = String.Format(lFormat, (object)QC);
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
