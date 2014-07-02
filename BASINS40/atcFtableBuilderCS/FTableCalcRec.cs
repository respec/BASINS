using System.Collections;
using System.Collections.Generic;

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
    public class FTableCalcRec : FTableCalculator, IFTableOperations
    {
        public FTableCalcRec()
        {
            vectorColNames.Clear();
            vectorColNames.Add("depth");
            vectorColNames.Add("area");
            vectorColNames.Add("volume");
            vectorColNames.Add("outflow1");
        }

        public ArrayList GenerateFTable()
        {
            return GenerateFTable(0, 0, 0, 0, 0);
        }

        public ArrayList GenerateFTable(double channelLength, double channelDepth,
                                        double channelWidth, double channelManningsValue,
                                        double channelSlope)
        {
            vectorRowData = new ArrayList();

            double depth = channelDepth;
            double width = channelWidth;
            double length = channelLength;
            double slope = channelSlope;
            double manningsValue = channelManningsValue;
            double CONS = FTableCalculatorConstants.Cunit;
            double ACONS = FTableCalculatorConstants.Aunit;
            int depthIdx = (int)depth * 10;
            double g = 0.0;
            double bw = width;

            for (int d = 0; d < depthIdx; d++)
            {
                g = d / 10;
            }
            return vectorRowData;
        }
    }
}


