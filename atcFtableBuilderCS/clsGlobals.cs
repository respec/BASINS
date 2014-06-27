using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    public class clsGlobals
    {
        public static FTableCalculator gCalculator = null;

        //Default values for various channel types
        public static double InfilBackfillDepth = 0;
        public static double InfilBackfillPore = 0.4;
        //CIRCULAR
        public static double GeomCircLength = 4000;
        public static double GeomCircDiam = 10;
        public static double GeomCircLSlope = 0.025;
        public static double GeomCircHInc = 0.1;
        //RECTANGULAR
        public static double GeomRectLength = 400;
        public static double GeomRectHInc = 0.1;
        public static double GeomRectMaxDepth = 6.0;
        public static double GeomRectTopWidth = 36.0;
        //TRIANGULAR
        public static double GeomTriLength = 400;
        public static double GeomTriHInc = 0.1;
        public static double GeomTriMaxDepth = 6.0;
        public static double GeomTriSideSlope = 3.0;
        //TRAPEZOIDAL
        public static double GeomTrapeLength = 400;
        public static double GeomTrapeHInc = 0.1;
        public static double GeomTrapeMaxDepth = 6.0;
        public static double GeomTrapeTopWidth = 36.0;
        public static double GeomTrapeSideSlope = 1.0;
        //PARABOLIC
        public static double GeomParabLength = 400;
        public static double GeomParabHInc = 0.1;
        public static double GeomParabDepth = 6.0;
        public static double GeomParabWidth = 36.0;
        //NATURAL
        public static double GeomNaturalLength = 400;
        public static double GeomNaturalHInc = 0.1;

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

        //Backfill
        public static double BackfillDepth = 0;
        public static double BackfillPorosity = 0.4;

        #region Outflow Control
        public static string[] gOCOrificeUnd = { "0", "0", "0" };
        public static string[] gOCOrificeRiser = { "0", "0", "0" };
        public static string[] gOCWeirTriVnotch = { "0", "0", "0" };
        public static string[] gOCWeirTrape = { "0", "0", "0" };
        public static string[] gOCWeirBroad = { "0", "0", "0" };
        public static string[] gOCWeirRect = { "0", "0", "0" };

        public static bool gOCSelectedWeirTri;
        public static bool gOCSelectedWeirTrape;
        public static bool gOCSelectedWeirBroad;
        public static bool gOCSelectedWeirRect;
        public static bool gOCSelectedOrificeUnd;
        public static bool gOCSelectedOrificeRiser;
        #endregion

    }
}
