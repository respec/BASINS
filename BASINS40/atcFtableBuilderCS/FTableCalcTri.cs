using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcTri : FTableCalcTrape, IFTableOperations
    {
        public FTableCalcTri()
        {
            //No "Top Channel Width"
            geomInputLongNames = new string[]{
                "Maximum Channel Depth ", 
                "Channel Side Slope (H:V)", 
                "Channel Length", 
                "Channel Mannings Value", 
                "Longitudinal Slope", 
                "Height Increment" };  //sri 07-23-2012

            geomInputs = new ChannelGeomInput[] {
                ChannelGeomInput.MaximumDepth,
                ChannelGeomInput.SideSlope,
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

            if (aInputs[ChannelGeomInput.SideSlope] != null)
                inpChannelSideSlope = (double)aInputs[ChannelGeomInput.SideSlope];
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
            return GenerateFTable(inpChannelLength, inpChannelMaxDepth, inpChannelManningsValue, inpChannelSlope, inpChannelSideSlope, inpHeightIncrement);
        }

        private ArrayList GenerateFTable ( //GenerateTriangularFTable (
            double channelLength, double maxChannelDepth,
            double channelManningsValue, double longitudalChannelSlope,
            double sideChannelSlope_HorzToVert, double Increment)  // sri-07-23-2012
        {
            double z1 = sideChannelSlope_HorzToVert;
            double topChannelWidth = z1 * 2.0 * maxChannelDepth;
            //string Shape = "Triangle";

            return GenerateFTable(channelLength, maxChannelDepth, channelManningsValue,
                       longitudalChannelSlope, topChannelWidth, z1, Increment); //, Shape);  // sri-07-23-2012
        }
    }
}
