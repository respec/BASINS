using System.Collections;

/**
 * <p>Title: FTable</p>
 *
 * <p>Description: </p>
 *
 * <p>Copyright: Copyright (c) 2006</p>
 *
 * <p>Company: </p>
 *
 * @author Kurt Wolfe
 * @version 1.0
 */

namespace atcFtableBuilder
{
    public class FTableCalcTrape : FTableCalculator, IFTableOperations
    {
        public double inpChannelLength;
        public double inpChannelMaxDepth;
        public double inpChannelManningsValue;
        public double inpChannelTopWidth;
        public double inpChannelSlope;
        public double inpChannelSideSlope;
        public double inpHeightIncrement;

        public FTableCalcTrape()
        {
            vectorColNames.Clear();
            vectorColNames.Add("depth");
            vectorColNames.Add("area");
            vectorColNames.Add("volume");
            vectorColNames.Add("outflow1");
            geomInputLongNames = new string[]{"Maximum Channel Depth",
                                         "Top Channel Width",
                                         "Channel Side Slope (H:V)", 
                                         "Channel Length",
                                         "Channel Mannings Value",
                                         "Longitudinal Slope",
                                         "Height Increment"}; // sri-07-23-2012
            geomInputs = new ChannelGeomInput[]{
                ChannelGeomInput.MaximumDepth,
                ChannelGeomInput.TopWidth,
                ChannelGeomInput.SideSlope,
                ChannelGeomInput.Length,
                ChannelGeomInput.ManningsN,
                ChannelGeomInput.LongitudinalSlope,
                ChannelGeomInput.HeightIncrement
            };
        }

        public bool SetInputParameters(Hashtable aInputs)
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

            if (aInputs[ChannelGeomInput.SideSlope] != null)
                inpChannelSideSlope = (double)aInputs[ChannelGeomInput.SideSlope];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.HeightIncrement] != null)
                inpHeightIncrement = (double)aInputs[ChannelGeomInput.HeightIncrement];
            else
                AllInputsFound = false;

            return AllInputsFound;
        }

        public ArrayList GenerateFTable()
        {
            return GenerateFTable(inpChannelLength, inpChannelMaxDepth, inpChannelManningsValue, inpChannelSlope, inpChannelTopWidth, inpChannelSideSlope, inpHeightIncrement);
        }

        protected ArrayList GenerateFTable(
            double channelLength, 
            double maxChannelDepth,
            double channelManningsValue, 
            double longitudalChannelSlope,
            double topChannelWidth, 
            double sideChannelSlope_HorzToVert, 
            double Increment) //,  string Shape) // sri-09-11-2012
        {
            vectorRowData = new ArrayList();

            //Flow Area Calculations
            double L = channelLength;
            double N = channelManningsValue;
            double TD = maxChannelDepth;
            double S = longitudalChannelSlope;
            double w = topChannelWidth;
            double z1 = sideChannelSlope_HorzToVert;

            double BW = w - (2.0 * z1 * TD);

            ArrayList row = new ArrayList();

            double A = 0.0;
            double wd = 0.0;
            double hr = 0.0;
            double C = 0.0;
            double QC = 0.0;
            double acr = 0.0;
            double V = 0.0;
            double stot = 0.0;
            double CONS = FTableCalculatorConstants.Cunit;
            double ACONS = FTableCalculatorConstants.Aunit;
            double prevAcr = (L * BW) / ACONS;
            double prevStot = 0.0;
            double prevG = 0.0;

            double lEpslon = 0.0000001;
            for (double g = 0.00; g <= TD + 0.01; g += Increment)  // sri-07-23-2012
            {
                A = BW * g + (z1 * g * g);
                wd = BW + 2.0 * g * System.Math.Sqrt(1.0 + z1 * z1);

                if (System.Math.Abs(wd - 0) < lEpslon)
                    hr = 0;
                else
                    hr = A / wd;
                w = BW + (2.0 * z1 * g);

                C = (CONS * System.Math.Sqrt(S)) / N;
                QC = C * A * System.Math.Pow(hr, (2.0 / 3.0));
                V = QC / A;
                acr = (L * w) / ACONS;

                stot = (prevAcr + acr) / 2.0 * (g - prevG) + prevStot;
                prevG = g;
                prevStot = stot;
                prevAcr = acr;

                if (FTableCalculatorConstants.programunits == 0)
                {
                    acr = acr / (System.Math.Pow(10, 4));
                    stot = stot / (System.Math.Pow(10, 6));
                }

                //TODO: System.out.println("      ");  
                //TODO: System.out.print(V); 
                row = new ArrayList();

                //sri-09-11-2012
                string sDepth = "";
                string sArea = "";
                string sVolume = "";
                string sOutFlow = "";

                string lFormat     = "{0:0.00000}";
                string lFormatRect = "{0:0.000000}";

                //if (Shape.ToLower() == "rectangle")
                //{
                //    sDepth   = string.Format(lFormatRect, (object)g);
                //    sArea    = string.Format(lFormatRect, (object)acr);
                //    sVolume  = string.Format(lFormatRect, (object)stot);
                //    sOutFlow = string.Format(lFormatRect, (object)QC);
                //}
                //else
                //{
                sDepth = string.Format(lFormat, (object)g);
                sArea = string.Format(lFormat, (object)acr);

                if (stot > 0 && stot < 0.000001)
                    sVolume = string.Format(clsGlobals.NumberFormatSci, (object)stot);
                else
                    sVolume = string.Format(lFormatRect, (object)stot);

                sOutFlow = string.Format(lFormat, (object)QC);
                //}
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