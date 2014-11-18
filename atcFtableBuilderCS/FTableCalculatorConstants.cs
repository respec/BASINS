using System.Collections;
using System.Collections.Generic;
using System.Drawing;

/**
 * 
 * <P>The FTableCalculators in this project have no superclass, thus
 * this class is used to keep constants and methods that are needed throughout the package. </P>
 * <P><B>Created:</B> Apr 13, 2007 </P>
 * <P><B>Revised:</B>   </P>
 * <P><B>Limitations:</B>  </P>
 * <P><B>Dependencies:</B>  </P>
 * <P><B>References:</B>   </P>

 * @author CSC for EPA/ORD/NERL
 *
 */
namespace atcFtableBuilder
{
    public class FTableCalculatorConstants
    {
        /**
         * The increment for each calcualtor (open channel and control structure) so that they can run congruently. 
         */
        public static double calculatorIncrement = .1;

        // if ==1 user specified increment to be used in control device calculation i.e user is using natural channel
        public static int NaturalIncrement = 0;
        // sri-08-27-2012
        public static int DepthCheck = -1;  // 0- depth increment did not matched, 1- depth increment matched
        public static int DepthCheckFinal = -1;  // 0- depth increment did not matched, 1- depth increment matched

        public static string JTableDefaultMessage = "Right click the grid for more options.";
        public static string JTableNegativeValueMessage = "There is invalid input data or an invalid channel.";
        public static string badInputs2NaturalChannelMessage = "The dataset was empty.  Please make sure you have proper numeric data in the input grid above.";
        //This color is the color of the selected JTab (RGB) value.
        public static Color selectedBackGroundColor = Color.FromArgb(184, 207, 229); //this is the color code for the jtabs

        public static int outflow_1_index = 3;
        public static Color error_color = Color.FromArgb(255, 180, 180);
        //  static Double Cunit = 1.0;
        //  static Double Aunit = 1.0;
        public static double Cunit = 1.486;  // sri
        public static double Aunit = 43560.0;
        //sq feet to acres factor; 1 square foot = 2.29568411 � 10-5 acres
        // Tells the program what sort of elevation data the user is bringing in ;
        static int choice = 0;
        //Tells the program what unit the data is in ;
        static int importunits = 0;

        public static int programunits = 1;  //sri - to make english units default

        public static double infiltrationConversion = System.Math.Pow(3600 * 100, -1);

        public static int unitssel = 1;  // sri-to store the int value of the units selected, default eng lish units

        public enum UnitSystem
        {
            SI = 0,
            US = 1
        }
        public static string[] UnitSystemNames = { "S.I.Units", "U.S.Units" };  //UI dropdown list should be in same order for easy access

        public static string[,] UnitSystemLabels = new string[,]{
             {"(m)",              "(ft)"},
             {"(ha.)",            "(acres)"},
             {"(milllion cu. m)", "(ac-ft)"},
             {"(cms)",            "(cfs)"}};
        //static double infiltrationConversion = 1.008333;  // // acres * in/hr to cf/s


        // if units selected are metric then conversion value will be chnaged to 1/36.0 in BMPType panel actionperformed....

        //public static Hashtable<string, OptionalInputPanel> controlDeviceInputPanels = new java.util.Hashtable<string, OptionalInputPanel>();
        /**
         * 
         *<P>Get the max value from an ArrayList of Vectors.  Each inner vector contains an x and y value.  </P>
         *<P><B>Limitations:</B>  </P>
         * @param index
         * @param channelProfile
         * @return
         */
        public static double getMaxValueFromVector(int index, ArrayList channelProfile)
        {
            return getMinMaxValueFromVector(index, channelProfile, true);


        }
        /**
         * 
         *<P>Get the min value from an ArrayList of Vectors.  Each inner vector contains an x and y value.</P>
         *<P><B>Limitations:</B>  </P>
         * @param index
         * @param channelProfile
         * @return
         */
        public static double getMinValueFromVector(int index, ArrayList channelProfile)
        {
            return getMinMaxValueFromVector(index, channelProfile, false);
        }
        /**
         * 
         *<P>This method will return the maximum or minimum value from a vector.  </P>
         *<P><B>Limitations:</B>  </P>
         * @param index where the needed value is in the dat row vector
         * @param channelProfile the data source
         * @param getMax if true, get the maximum value, if false, get the minimum value
         * @return
         */
        public static double getMinMaxValueFromVector(int index, ArrayList channelProfile, bool getMax)
        {
            double qualifiedValue = 0;
            double sampleValue = 0;
            for (int i = 0; i < channelProfile.Count; i++)
            {
                ArrayList dataRow = (ArrayList)channelProfile[i];
                if (i == 0)
                {
                    if (dataRow[index].ToString().Equals(""))
                    {
                        //this should never happen because the first row in the data vector would have to be blank.
                    }
                    else
                    {
                        qualifiedValue = double.Parse(dataRow[index].ToString());
                    }
                }
                else
                {
                    if (dataRow[index].ToString().Equals(""))
                    {
                        //it's blank, don't do anything
                    }
                    else
                    {
                        sampleValue = double.Parse(dataRow[index].ToString());
                        if (getMax)
                        {
                            if (sampleValue >= qualifiedValue)
                            {
                                qualifiedValue = sampleValue;
                            }
                        }
                        else
                        {
                            if (sampleValue <= qualifiedValue)
                            {
                                qualifiedValue = sampleValue;
                            }
                        }
                    }
                }
            }
            return qualifiedValue;
        }
        /**
         * 
         *<P>  </P>
         *<P><B>Limitations:</B>  </P>
         * @param testVector the first element is the column name vector, and the second element is the result message.
         * @return
         */
        public static string returnMessageIfNegativeValue(ArrayList testVector)
        {
            //the test vector is just a vector of vectors
            IEnumerator e = testVector.GetEnumerator();
            while (e.MoveNext())
            {
                ArrayList indiVector = (ArrayList)e.Current;
                IEnumerator e2 = indiVector.GetEnumerator();
                while (e2.MoveNext())
                {
                    try
                    {
                        double testNum = double.Parse(e2.Current.ToString());
                        if (testNum < 0d)
                        {
                            //if the number is negative, then return the blank vector
                            /*ArrayList newVector = new ArrayList();
						
                            newVector.add("There is an invalid input data or channel.");
                            ArrayList colVector = new ArrayList();
                            colVector.add("Result");
                            ArrayList[] returnVector = {newVector,colVector};
                            return returnVector;*/
                            return JTableNegativeValueMessage;
                        }
                    }
                    catch (System.FormatException nfe)
                    {

                    }
                }
            }
            return JTableDefaultMessage;
        }
        /**
         *<P>If control structures are picked, do not show the open flow control.
         *The 0 element of the array will be the colNames, and the 1 element of the 
         *array will be the data array.</P>
         *<P><B>Limitations:</B>  </P>
         * @param colNames
         * @param data
         * @return
         */
        public static ArrayList[] modifyResultsToAccomodateControlStructures(ArrayList colNames, ArrayList data)
        {
            //if colNames is greater than four, remove the outflow_1 column (open flow)
            //ToDo: for now, see if client like to retain the outflow1 column for the Gray tool
            //      as for infiltration, this column will be removed.
            if (colNames[FTableCalculatorConstants.outflow_1_index].ToString().ToLower().StartsWith("outflow1")) //colNames.Count > FTableCalculatorConstants.outflow_1_index + 1)
            {
                colNames.RemoveAt(FTableCalculatorConstants.outflow_1_index);
                //	the data vector is a vector of vectors.
                //	loop over each element and remove the third element of each element in question
                IEnumerator e = data.GetEnumerator();
                while (e.MoveNext())
                {
                    ArrayList v = (ArrayList)e.Current;
                    v.RemoveAt(FTableCalculatorConstants.outflow_1_index);
                }
            }

            ArrayList lFirstRow = (ArrayList)data[0];
            if (lFirstRow.Count >= 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    string lExitName = "Exit" + (i + 1).ToString();
                    int lCount = clsGlobals.gExitOCSetup[i].Nodes.Count;
                    if (lCount > 0)
                    {
                        lExitName += "CDs" + lCount.ToString();
                        colNames.Add(lExitName);
                    }
                }
            }

            ArrayList[] retVector = { colNames, data };
            return retVector;
        }
    }
}
