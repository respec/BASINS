using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    public interface IFTableOperations
    {
        ArrayList GenerateFTable();
    }
    public interface IFTableOperationsOC
    {
        ArrayList GenerateFTableOC();
    }
    public class XSectionStation
    {
        public double x;
        public double y;
    }
    public abstract class FTableCalculator
    {
        public enum ChannelType
        {
            NONE, CIRCULAR, RECTANGULAR, TRIANGULAR, TRAPEZOIDAL, PARABOLIC, NATURAL
        };
        public enum ControlDeviceType
        {
            WeirTriVNotch, WeirTrapeCipolletti, WeirBroadCrest, WeirRectangular, OrificeUnderdrain, OrificeRiser
        };

        public enum BMPType
        {
           DryWell,
           InfiltrationTrench,
           InfiltrationBasin,
           RainBarrel,
           RainGardenBioretention,
           RoofGarden,
           SandFilter,
           StorageTank,
           VegetatedSwale,
           Wetland
        }

        /*Definitions for the control structure input panels*/
        //public static string[] VNotchWeirInputs = { "Vnotch Weir Vertex Angle (deg)", "Vnotch Weir Invert", "Discharge Coefficent" };
        //public static string[] VNotchWeirInputsUnits = { "", "", "" };

        //public static string[] TrapezoidalWeirInputs = { "Trapezoidal Weir Width", "Trapezoidal Weir Depth", "Discharge Coefficent" };
        //public static string[] TrapezoidalWeirUnits = { "", "", "" };

        //public static string[] SharpCrestedWeirInputs = { "Trapezoidal Weir Width", "Trapezoidal Weir Depth", "Discharge Coefficent" };
        //public static string[] SharpCrestedWeirUnits = { "", "", "" };

        //public static string[] BroadCrestedWeirInputs = { "Broad Crested Weir Crest Width", "Broad Crested Weir Invert Depth", "Discharge Coefficent" };
        //public static string[] BroadCrestedWeirUnits = { "", "", "" };

        //public static string[] RectangularWeirInputs = { "Rectangular Weir Crest Width", "Rectangular Weir Invert Depth", "Discharge Coefficeint" };
        //public static string[] RectangularWeirUnits = { "", "", "" };

        //public static string[] UnderdrainOrificeInputs = { "Underdrain Orifice Diameter", "Underdrain Orifice Invert Depth", "Discharge Coefficient" };
        //public static string[] UnderdrainOrificeUnits = { "", "", "" };

        //public static string[] RiserOrificeInputs = { "Riser Orifice Diameter", "Riser Orifice Depth", "Discharge Coefficient" };
        //public static string[] RiserOrificeUnits = { "", "", "" };

        public static Dictionary<BMPType, string> BMPTypeNames = new Dictionary<BMPType,string>() {
            {BMPType.DryWell, "Dry Well"},
            {BMPType.InfiltrationBasin, "Infiltration Basin"},
            {BMPType.InfiltrationTrench, "Infiltration Trench"},
            {BMPType.RainBarrel, "Rain Barrel"},
            {BMPType.RainGardenBioretention, "Rain Garden/Bioretention"},
            {BMPType.RoofGarden, "Roof Garden"},
            {BMPType.SandFilter, "Sand Filter"},
            {BMPType.StorageTank, "Storage Tank"},
            {BMPType.VegetatedSwale, "Vegetated Swale"},
            {BMPType.Wetland, "Wetland"}
        };

        public static Dictionary<BMPType, ChannelType> LookupBMPTypeChannel = new Dictionary<BMPType, ChannelType>() {
            {BMPType.DryWell, ChannelType.RECTANGULAR},
            {BMPType.InfiltrationBasin, ChannelType.PARABOLIC},
            {BMPType.InfiltrationTrench, ChannelType.RECTANGULAR},
            {BMPType.RainBarrel, ChannelType.CIRCULAR},
            {BMPType.RainGardenBioretention, ChannelType.PARABOLIC},
            {BMPType.RoofGarden, ChannelType.RECTANGULAR},
            {BMPType.SandFilter, ChannelType.RECTANGULAR},
            {BMPType.StorageTank, ChannelType.CIRCULAR},
            {BMPType.VegetatedSwale, ChannelType.TRAPEZOIDAL},
            {BMPType.Wetland, ChannelType.NATURAL}
        };

        public enum ChannelGeomInput
        {
            Length, Width, TopWidth, Depth, MaximumDepth, Diameter, LongitudinalSlope, SideSlope, HeightIncrement, ManningsN, Profile
        }

        //User interface should use these texts as TextBox control's name for easy data retrieval
        public static Dictionary<ChannelGeomInput, string> LookupChannelGeomInput = new Dictionary<ChannelGeomInput, string>() {
           {ChannelGeomInput.Length,            "Length,txtGeomLength"},
           {ChannelGeomInput.Width,             "Width,txtGeomWidth"},
           {ChannelGeomInput.TopWidth,          "Top Width,txtGeomTopWidth"},
           {ChannelGeomInput.Depth,             "Depth,txtGeomDepth"},
           {ChannelGeomInput.MaximumDepth,      "Maximum Depth,txtGeomMaxDepth"},
           {ChannelGeomInput.Diameter,          "Diameter,txtGeomDiam"},
           {ChannelGeomInput.LongitudinalSlope, "Longitudinal Slope (ft/ft),txtGeomLSlope"},
           {ChannelGeomInput.SideSlope,         "Side Slope (H:V),txtGeomSideSlope"},
           {ChannelGeomInput.HeightIncrement,   "Height Increment,txtGeomHInc"},
           {ChannelGeomInput.ManningsN,         "Manning's N,txtGeomMannN"}
        };
        public string[] geomInputLongNames;
        public ChannelGeomInput[] geomInputs;
        protected ArrayList vectorRowData;
        protected ArrayList vectorColNames;
        public ChannelType CurrentType = ChannelType.NONE;

        public static double TotalDepth;

        public FTableCalculator()
        {
            vectorColNames = new ArrayList();
        }
        public ArrayList GetColumnNames()
        {
            return vectorColNames;
        }

        protected bool hasDepthEverBeenFound = false;
        /**
 * 
 *<P>Merges the two data vectors. The open channel vector gets set in the setOpenChannelVector
 *method.  The Hashtable contains a key and value, which is the result vector from the control device (CD) calculations.  </P>
 *<P><B>Limitations:</B>  </P>
 * @param controlDeviceVectors
 * @return
 */
        public ArrayList mergeCalculators(ArrayList openChannelVector, Dictionary<FTableCalculator.ControlDeviceType, ArrayList> controlDeviceVectors)
        {
            double lEpslon = 0.0000001;
            //I have the open channel depth vector and the different control devices.
            ArrayList gridVector = new ArrayList();
            ArrayList oldRowVector;
            double currentDepth = 0;
            double lastFlowReading = 0;
            hasDepthEverBeenFound = false;
            //FTableCalculatorConstants.DepthCheckFinal=-1;

            for (int i = 0; i < openChannelVector.Count; i++)
            {
                oldRowVector = (ArrayList)openChannelVector[i]; //get a control device

                //get the depth on that row
                double.TryParse(oldRowVector[0].ToString(), out currentDepth);

                //go over each selected control device and find out if its depth is > the depth of the stream
                //for (Enumeration e = controlDeviceVectors.keys();e.hasMoreElements();){

                // FTableCalculatorConstants.DepthCheckFinal=0;
                foreach (FTableCalculator.ControlDeviceType lOCType in controlDeviceVectors.Keys)
                {
                    ArrayList cdGridVector = null;
                    if (controlDeviceVectors.TryGetValue(lOCType, out cdGridVector))
                    {
                        bool foundDepth = false;
                        //FTableCalculatorConstants.DepthCheck=0; // sri-08-27-2012
                        for (int j = 0; j < cdGridVector.Count; j++)
                        {
                            // used to indicate that control devices are added
                            FTableCalculatorConstants.DepthCheck = 1;
                            ArrayList cdRowVector = (ArrayList)cdGridVector[j];
                            double controlDeviceDepth = 0;
                            double.TryParse(cdRowVector[0].ToString(), out controlDeviceDepth);
                            if (Math.Abs(controlDeviceDepth - currentDepth) < lEpslon)
                            {
                                // FTableCalculatorConstants.DepthCheck=1;  // sri-08-27-2012
                                foundDepth = true;
                                double lVal = 0;
                                double.TryParse(cdRowVector[3].ToString(), out lVal);
                                oldRowVector.Add(lVal);
                                lastFlowReading = lVal;
                                hasDepthEverBeenFound = true;
                                //since you found the depth, go to the next control device
                                break;
                            }
                        }
                        //this code was also added to make the merge results code work.
                        if (!foundDepth)
                        {
                            //A matching depth for a control device was found in previous iterations, but
                            //now the depth is not found.  So if the control device is an orifice, just keep 
                            //repeating the maximum flow for that orifice.  This situation should never happen for 
                            //a control device like a wier.

                            // sri- removed this if condition
                            //					if (hasDepthEverBeenFound == true && calculatorTypeString.indexOf("Orifice")>=0){
                            //						oldRowVector.add(lastFlowReading);
                            //					}
                            //					else{
                            oldRowVector.Add(0.00d);
                            //}
                            // to capture depth mismatch in any one of the multiple control devices used
                            //                           if(FTableCalculatorConstants.DepthCheck==0)
                            //                                FTableCalculatorConstants.DepthCheckFinal=0;
                        }
                        //end code to fix merge bug - BED
                    }
                }
                gridVector.Add(oldRowVector);
            }
            return gridVector;
        }

        public static double CalculateMaxVolume(ArrayList aFtableResultChannel, out string aMsg)
        {
            aMsg = "";
            if (aFtableResultChannel == null) return -999;
            //sri- 09-11-2012
            // calculating maximum storage which is similar to modifytableforinfiltration in ftablepanel
            // used by all other shapes. Since this panel doesn't do infiltration similar calculation is done here
            ArrayList lastR = (ArrayList)aFtableResultChannel[aFtableResultChannel.Count - 1];
            string volStr = (string)lastR[2];
            double[] convr = { Math.Pow(10, 6), 43560 };
            double conv = convr[FTableCalculatorConstants.programunits];//FTableCalculatorConstants.Aunit;
            try
            {
                double maxVol = double.Parse(volStr) * conv;
                string maxVolstring = string.Format("{0:#,##0}", (object)maxVol);
                string unit = FTableCalculatorConstants.UnitSystemLabels[0, FTableCalculatorConstants.programunits];
                unit = unit.Trim(new char[] { '(', ')' });
                aMsg = "Maximum Storage Capacity: " + maxVolstring + " " + unit + "^3";
                return maxVol;
            }
            catch (FormatException nfe)
            {
                return -999;
            }
        }
    }
}
