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
        public FTableCalcTrape()
        {
            vectorColNames.Clear();
            vectorColNames.Add("depth");
            vectorColNames.Add("area");
            vectorColNames.Add("volume");
            vectorColNames.Add("outflow1");
            geoInputNames = new string[]{"Maximum Channel Depth",
                                         "Top Channel Width",
                                         "Channel Side Slope (H:V)", 
                                         "Channel Length",
                                         "Channel Mannings Value",
                                         "Longitudinal Slope",
                                         "Height Increment"}; // sri-07-23-2012
        }

        public ArrayList GenerateTriangularFTable(double channelLength, double maxChannelDepth,
                        double channelManningsValue, double longitudalChannelSlope,
                        double sideChannelSlope_HorzToVert, double Increment)  // sri-07-23-2012
        {
            double z1 = sideChannelSlope_HorzToVert;
            double topChannelWidth = z1 * 2.0 * maxChannelDepth;
            string Shape = "Triangle";

            return GenerateFTable(channelLength, maxChannelDepth, channelManningsValue,
                       longitudalChannelSlope, topChannelWidth, z1, Increment, Shape);  // sri-07-23-2012
        }


        public ArrayList GenerateRectangularFTable(double channelLength, double maxChannelDepth,
                     double channelManningsValue, double longitudalChannelSlope,
                     double topChannelWidth, double Increment) // sri-07-23-2012
        {
            double z1 = 0.0;
            string Shape = "Rectangle";

            return GenerateFTable(channelLength, maxChannelDepth,
         channelManningsValue, longitudalChannelSlope, topChannelWidth, z1, Increment, Shape); // sri-07-23-2012
        }

        public ArrayList GenerateFTable(double channelLength, double maxChannelDepth,
                          double channelManningsValue, double longitudalChannelSlope,
                          double topChannelWidth, double sideChannelSlope_HorzToVert, double Increment, string Shape) // sri-09-11-2012
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

            for (double g = 0.00; g <= TD + 0.01; g += Increment)  // sri-07-23-2012
            {
                A = BW * g + (z1 * g * g);
                wd = BW + 2.0 * g * System.Math.Sqrt(1.0 + z1 * z1);

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

                if (Shape.ToLower() == "rectangle")
                {
                    sDepth   = string.Format(lFormatRect, (object)g);
                    sArea    = string.Format(lFormatRect, (object)acr);
                    sVolume  = string.Format(lFormatRect, (object)stot);
                    sOutFlow = string.Format(lFormatRect, (object)QC);
                }
                else
                {
                    sDepth   = string.Format(lFormat, (object)g);
                    sArea    = string.Format(lFormat, (object)acr);
                    sVolume  = string.Format(lFormat, (object)stot);
                    sOutFlow = string.Format(lFormat, (object)QC);
                }
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