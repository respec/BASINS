using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    //This is the version that actually referenced in the original Java project by ControlDeviceMiddleware.java
    class FTableCalcRectangular : FTableCalcTrape, IFTableOperations
    {
        public FTableCalcRectangular()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH");
            vectorColNames.Add("AREA");
            vectorColNames.Add("VOLUME");
            vectorColNames.Add("OUTFLOW");
            geomInputLongNames = new string[]{ "Maximum Channel Depth ", 
                                          "Top Channel Width ", 
                                          "Channel Length ", 
                                          "Channel Mannings Value", 
                                          "Longitudinal Slope (ft/ft)", 
                                          "Height Increment" };  //sri 07-23-2012
            geomInputs = new ChannelGeomInput[]{
                ChannelGeomInput.MaximumDepth,
                ChannelGeomInput.TopWidth,
                ChannelGeomInput.Length,
                ChannelGeomInput.ManningsN,
                ChannelGeomInput.LongitudinalSlope,
                ChannelGeomInput.HeightIncrement
            };
        }

        public new bool SetInputParameters(Hashtable aInputs)
        {
            bool AllInputsFound = true;
            if (aInputs[ChannelGeomInput.Length] != null)
                inpChannelLength = (double)aInputs[ChannelGeomInput.Length];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.MaximumDepth] != null)
                inpChannelMaxDepth = (double)aInputs[ChannelGeomInput.MaximumDepth];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.TopWidth] != null)
                inpChannelTopWidth = (double)aInputs[ChannelGeomInput.TopWidth];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.ManningsN] != null)
                inpChannelManningsValue = (double)aInputs[ChannelGeomInput.ManningsN];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.LongitudinalSlope] != null)
                inpChannelSlope = (double)aInputs[ChannelGeomInput.LongitudinalSlope];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.HeightIncrement] != null)
                inpHeightIncrement = (double)aInputs[ChannelGeomInput.HeightIncrement];
            else
                AllInputsFound = false;

            return AllInputsFound;
        }

        public new ArrayList GenerateFTable()
        {
            return GenerateFTable(inpChannelLength, inpChannelMaxDepth, inpChannelManningsValue, inpChannelSlope, inpChannelTopWidth, inpHeightIncrement);
        }

        private ArrayList GenerateFTable( //GenerateRectangularFTable(
            double channelLength,
            double maxChannelDepth,
            double channelManningsValue,
            double longitudalChannelSlope,
            double topChannelWidth,
            double Increment) // sri-07-23-2012
        {
            double z1 = 0.0;
            //string Shape = "Rectangle";

            return GenerateFTable(channelLength, maxChannelDepth,
               channelManningsValue, longitudalChannelSlope, topChannelWidth, z1, Increment); //, Shape); // sri-07-23-2012
        }

        public ArrayList GenerateFTable_ToBeDiscussed(double channelLength, double Height,
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
