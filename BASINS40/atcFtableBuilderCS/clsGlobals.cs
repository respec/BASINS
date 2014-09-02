using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using atcUCI;

namespace atcFtableBuilder
{
    public class clsGlobals
    {
        public enum ToolType
        {
            Gray, Green
        }

        public static ToolType gToolType = ToolType.Green;
        public static FTableCalculator gCalculator = null;

        //Default values for various channel types
        //check: geoInputValues
        public static double InfilBackfillDepth = 0;
        public static double InfilBackfillPore = 0.4;
        public static double GeomManningN = 0.05;
        public static double GeomChannelLengthGray = 4000;
        //CIRCULAR
        public static double GeomCircLength = 4000;
        public static double GeomCircDiam = 10;
        public static double GeomCircLSlope = 0.025;
        public static double GeomCircHInc = 0.1;
        //RECTANGULAR
        public static double GeomRectLength = 400;
        public static double GeomRectTopWidth = 36.0;
        public static double GeomRectMaxDepth = 6.0;
        public static double GeomRectLSlope = 0.025; //Java source code says 2???
        public static double GeomRectHInc = 0.1;
        //TRIANGULAR
        public static double GeomTriLength = 400;
        public static double GeomTriMaxDepth = 6.0;
        public static double GeomTriSideSlope = 3.0; //interface says 3.0???
        public static double GeomTriLSlope = 0.025;
        public static double GeomTriHInc = 0.1;
        //TRAPEZOIDAL
        public static double GeomTrapeLength = 400;
        public static double GeomTrapeTopWidth = 36.0;
        public static double GeomTrapeMaxDepth = 6.0;
        public static double GeomTrapeSideSlope = 1.0;
        public static double GeomTrapeLSlope = 0.025;
        public static double GeomTrapeHInc = 0.1;
        //PARABOLIC
        public static double GeomParabLength = 400;
        public static double GeomParabWidth = 36.0;
        public static double GeomParabDepth = 6.0;
        public static double GeomParabLSlope = 0.025;
        public static double GeomParabHInc = 0.1;
        //NATURAL
        public static double GeomNaturalLength = 400;
        //public static double GeomNaturalMaxDepth = 6.0;
        //public static double GeomNaturalWidth = 36.0;
        public static double GeomNaturalLSlope = 0.025;
        public static double GeomNaturalHInc = 1.0;
        //NATURAL FP
        public static double GeomNaturalFPChLength = 4000;
        public static double GeomNaturalFPLOBLength = 4000;
        public static double GeomNaturalFPROBLength = 4000;
        public static double GeomNaturalFPChMannN = 0.05;
        public static double GeomNaturalFPLOBMannN = 0.05;
        public static double GeomNaturalFPROBMannN = 0.05;
        public static double GeomNaturalFPChLSlope = 0.00025;
        public static double GeomNaturalFPLOBX = 0;
        public static double GeomNaturalFPROBX = 100;

        //Infiltration Calculator Soil Types
        public static string[] soilTypes = {
            "Sand",
			"Loamy Sand",
			"Sand Loam",
			"Loam",
			"Silty Loam",
			"Sandy Clay Loam",
			"Clay Loam",
			"Silty Clay Loam",
			"Sandy Clay",
			"Silty Clay",
			"Clay" };

        //Backfill for infiltration rate calculation
        public static double BackfillDepth = 0;
        public static double BackfillPorosity = 0.4;
        public static bool HasBackfill = false;
        public static FTableCalculator.BMPType gBMPType = FTableCalculator.BMPType.None;

        public static atcUCI.HspfFtable pFTable;

        #region Outflow Control

        public static string[] gOCOrificeUnd = { "0", "0", "0" };
        public static string[] gOCOrificeRiser = { "0", "0", "0" };
        public static string[] gOCWeirTriVnotch = { "0", "0", "0" };
        public static string[] gOCWeirTrape = { "0", "0", "0" };
        public static string[] gOCWeirBroad = { "0", "0", "0" };
        public static string[] gOCWeirRect = { "0", "0", "0" };

        public static string[] DefaultsOrifice = { "5", "2", "0.6" };  //sri-jul 18 2012
        public static string[] DefaultsWeir = { "10", "5", "2" };
        public static string[] DefaultsWeirTriVnotch = { "10", "5", "0.585" };
        public static string[] DefaultsWeirTrape = { "10", "5", "3.367" };
        public static string[] DefaultsWeirBroad = { "10", "5", "3.0" };
        public static string[] DefaultsWeirRect = { "10", "5", "3.33" }; // Need to check this value

        public static bool gOCSelectedWeirTri = false;
        public static bool gOCSelectedWeirTrape = false;
        public static bool gOCSelectedWeirBroad = false;
        public static bool gOCSelectedWeirRect = false;
        public static bool gOCSelectedOrificeUnd = false;
        public static bool gOCSelectedOrificeRiser = false;

        //For multiple exit version
        public enum OCTypes
        {
            None, OCWeirTrape, OCWeirTriVnotch, OCWeirBroad, OCWeirRect, OCOrificeUnd, OCOrificeRiser
        }

        public static Dictionary<OCTypes, String> OCTypeNames = new Dictionary<OCTypes, string>()
        {
            {OCTypes.None, "None"},
            {OCTypes.OCWeirTriVnotch, "Triangular Vnotch Weir"},
            {OCTypes.OCWeirTrape, "Trapezoidal Weir (Cipoletti)"},
            {OCTypes.OCWeirRect, "Rectangular Weir"},
            {OCTypes.OCWeirBroad, "Broad Crested Weir"},
            {OCTypes.OCOrificeUnd, "Underdrain Orifice"},
            {OCTypes.OCOrificeRiser, "Riser Orifice"}
        };
        public static string[] gOCParmLbl = { "Parameter", "Parameter", "Discharge Coefficient" };
        public static string[] gOCOrificeUndLbl = { "Orifice Diameter", "Orifice Invert Depth", "Discharge Coefficient" };
        public static string[] gOCOrificeRiserLbl = { "Orifice Diameter", "Orifice Depth", "Discharge Coefficient" };
        public static string[] gOCWeirTriVnotchLbl = { "Weir Vertex Angle (deg)", "Weir Invert Depth", "Discharge Coefficient" };
        public static string[] gOCWeirTrapeLbl = { "Weir Width", "Weir Depth", "Discharge Coefficient" };
        public static string[] gOCWeirBroadLbl = { "Weir Crest Width", "Weir Invert Depth", "Discharge Coefficient" };
        public static string[] gOCWeirRectLbl = { "Weir Crest Width", "Weir Invert Depth", "Discharge Coefficient" };

        public struct OutflowControl
        {
            public int RchExit;
            public double Parm0;
            public double Parm1;
            public double Parm2;
            public OCTypes myOCType;
        }

        //a collection of OutflowControl(s) keyed on exit id (1 - 5)
        public static atcUtility.atcCollection gExitOCSetup = new atcUtility.atcCollection();

        #endregion

        #region Natural Profile
        //public enum ProfileDataType
        //{
        //    Elevation, Depth
        //}
        public static ArrayList gProfileStations;
        //public static ProfileDataType gProfileDataType;
        public static FTableCalculatorConstants.UnitSystem gProfileDataUnit; //0: SI, 1: US
        #endregion

        #region Utils
        public static string SettingFilename = "";
        public static string ToolNameGray = "Sewer and Open Channel Design Tool";
        public static string ToolNameGreen = "Low Impact Development (LID) Controls Tool";

        #endregion
    }
}
