using System.Collections;
//Main.java
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
    public class FTableCalcEllipse : FTableCalculator, IFTableOperations
    {
        public double inpChannelLength;
        public double inpChannelWidth;
        public double inpChannelDepth;
        public double inpChannelManningsValue;
        public double inpChannelSlope;
        public double inpHeightIncrement;

        public FTableCalcEllipse()
        {
            vectorColNames.Clear();
            vectorColNames.Add("depth");
            vectorColNames.Add("area");
            vectorColNames.Add("volume");
            vectorColNames.Add("outflow1");
            geomInputLongNames = new string[] { 
                "Channel Length", 
                "Channel Width", 
                "Channel Depth", 
                "Channel Mannings Value", 
                "Longitudinal Slope", 
                "Height Increment" };  //sri 07-23-2012};
            geomInputs = new ChannelGeomInput[] {
                ChannelGeomInput.Length,
                ChannelGeomInput.Width,
                ChannelGeomInput.Depth,
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

            if (aInputs[ChannelGeomInput.Depth] != null)
                inpChannelDepth = (double)aInputs[ChannelGeomInput.Depth];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.Width] != null)
                inpChannelWidth = (double)aInputs[ChannelGeomInput.Width];
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

        public ArrayList GenerateFTable()
        {
            return GenerateFTable(inpChannelLength, inpChannelDepth, inpChannelManningsValue, inpChannelSlope, inpChannelWidth, inpHeightIncrement);
        }

        private ArrayList GenerateFTable(double channelLength, double maxChannelDepth,
                double channelManningsValue, double longitudalChannelSlope, double topChannelWidth, double Increment)
        {

            vectorRowData = new ArrayList();
            //Flow Area Calculations
            double L = channelLength;
            double N = channelManningsValue;
            double TD = maxChannelDepth;
            double S = longitudalChannelSlope;
            double w = topChannelWidth;

            ArrayList row = new ArrayList();

            double A = 0.0;
            double wd = 0.0;
            double hr = 0.0;
            double C = 0.0;
            double QC = 0.0;
            double acr = 0.0;
            //double GH = 0.0;
            //double GJ = 0.0;
            double stot = 0.0;

            double prevAcr = 0.0; ;
            double prevStot = 0.0;
            double prevG = 0.0;

            double TW = 0d;
            double CONS = FTableCalculatorConstants.Cunit;
            double ACONS = FTableCalculatorConstants.Aunit;

            double g = 0.0;
            //for (double g = 0.00; g<=TD;g+=Increment)
            //formatter  was added since while incrementing value for ex for last depth value
            //instead of 6 the value is incremented to 6.000000000025, because of which the last value is missed
            // few times.
            //Replace this loop with a do-while loop
            //DecimalFormat myFormatter = new DecimalFormat("0.000");
            //for (g =0.00; double.parseDouble(myFormatter.format(g)) <=TD; g +=Increment)  // sri jul 31 2012
            //{
            //}

            string lFormattedVal = "";
            string lFormatResult = "{0:0.00000}";
            double lFormattedg = 0.0;
            string sDepth = "";
            string sArea = "";
            string sVolume = "";
            string sOutFlow = "";
            g = 0;
            do
            {
                lFormattedVal = string.Format("{0:0.000}", g);
                if (double.TryParse(lFormattedVal, out lFormattedg))
                {
                    if (lFormattedg > TD)
                    {
                        break; //jump out do-while loop
                    }

                    //do work here
                    A = (2.0 / 3.0) * w * g;
                    wd = w + (8.0 * g * g) / (3.0 * w);

                    hr = A / wd;
                    //w*w*g / (1.5*W*W+4.0*g*g);            

                    C = (CONS * System.Math.Sqrt(S)) / N;
                    QC = A * System.Math.Pow(hr, (2.0 / 3.0)) * C;

                    // change from infiltration
                    TW = A * 1.5 / TD;

                    //acr = (L * (w*g/TD)) / 43560.0;
                    acr = TW * L / ACONS;

                    //stot = (prevAcr + acr)/2 * (g - prevG) + prevStot;
                    stot = A * L / ACONS;
                    if (FTableCalculatorConstants.programunits == 0)
                    {
                        acr = acr / (System.Math.Pow(10, 4));
                        stot = stot / (System.Math.Pow(10, 6));
                    }

                    prevG = g;
                    prevStot = stot;
                    prevAcr = acr;

                    row = new ArrayList();

                    sDepth   = string.Format(lFormatResult, (object)g);
                    sArea    = string.Format(lFormatResult, (object)acr);
                    sVolume  = string.Format(lFormatResult, (object)stot);
                    sOutFlow = string.Format(lFormatResult, (object)QC);
                    row.Add(sDepth);
                    row.Add(sArea);
                    row.Add(sVolume);
                    row.Add(sOutFlow);

                    vectorRowData.Add(row);
                }
                else
                {
                    break; //jump out do-while loop
                }
                g += Increment;

            } while (true);
            return vectorRowData;
        }
    }
}

