using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    /**
 * 
 * <P>The idea behind this class is that it will take a default depth for the control device 
 * and a depth for the open channel.  This action is handled
 * in the mergeCalculators method. It will fill the result vector with 0's for 
 * flow until the depth for the control device is reached. If the device is an orifice,
 * once the maximum flow is reached, it will simply repeat that flow.</P>
 * <P><B>Created:</B> Apr 12, 2007 </P>
 * <P><B>Revised:</B>   </P>
 * <P><B>Limitations:</B>  </P>
 * <P><B>Dependencies:</B>  </P>
 * <P><B>References:</B>   </P>

 * @author CSC for EPA/ORD/NERL
 *
 */
    class ControlDeviceMiddleware
    {
        public FTableCalculator.ControlDeviceType myCalcType;
        //private java.util.Hashtable calcInputs;
        private Hashtable calcInputs;
        private string[] calcInputDefs;
        private ArrayList openChannelArrayList;
        bool hasDepthEverBeenFound = false;
        public ControlDeviceMiddleware()
        {
        }
        /**
         * The inputs parameter must be put in the correct order that calcuator expects
         * @param calculatorType
         * @param inputs
         */
        public ControlDeviceMiddleware(FTableCalculator.ControlDeviceType calculatorType)
        {
            this.myCalcType = calculatorType;
        }
        /**
         * 
         *<P>Get the control device result form the calculators by calling the GenerateFTable method in each of them.  </P>
         *<P><B>Limitations:</B>  </P>
         * @return The result vector from the calcualtion.s 
         */
        public ArrayList getControlDeviceResult()
        {
            ArrayList cdResult = new ArrayList();

            string lInput1 = "";
            string lInput2 = "";
            string lInput3 = "";
            string lInput4 = "";
            if (myCalcType == FTableCalculator.ControlDeviceType.WeirBroadCrest)
            {
                FTableCalcOCWeirBroad bft = new FTableCalcOCWeirBroad();
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                lInput4 = (string)calcInputs[calcInputDefs[3]];
                cdResult = bft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3),
                    double.Parse(lInput4));
            }
            else if (myCalcType == FTableCalculator.ControlDeviceType.WeirTrapeCipolletti)
            {
                FTableCalcOCWeirTrapeCipolletti cft = new FTableCalcOCWeirTrapeCipolletti();
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                lInput4 = (string)calcInputs[calcInputDefs[3]];
                cdResult = cft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3),
                    double.Parse(lInput4));
            }
            else if (myCalcType == FTableCalculator.ControlDeviceType.WeirRectangular)
            {
                FTableCalcOCWeirRectangular rft = new FTableCalcOCWeirRectangular();
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                lInput4 = (string)calcInputs[calcInputDefs[3]];
                cdResult = rft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3),
                    double.Parse(lInput4));
            }
            else if (myCalcType == FTableCalculator.ControlDeviceType.OrificeRiser)
            {
                FTableCalcOCOrificeRiser rft = new FTableCalcOCOrificeRiser();
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                cdResult = rft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3));
            }
            else if (myCalcType == FTableCalculator.ControlDeviceType.OrificeUnderdrain)
            {
                FTableCalcOCOrificeUnderflow uft = new FTableCalcOCOrificeUnderflow();
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                cdResult = uft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3));
            }
            else if (myCalcType == FTableCalculator.ControlDeviceType.WeirTriVNotch)
            {
                FTableCalcOCWeirTriVNotch vft = new FTableCalcOCWeirTriVNotch();
                //this should still work as long as the string array input defs are still in order.
                lInput1 = (string)calcInputs[calcInputDefs[0]];
                lInput2 = (string)calcInputs[calcInputDefs[1]];
                lInput3 = (string)calcInputs[calcInputDefs[2]];
                lInput4 = (string)calcInputs[calcInputDefs[3]];
                cdResult = vft.GenerateFTable(
                    double.Parse(lInput1),
                    double.Parse(lInput2),
                    double.Parse(lInput3),
                    double.Parse(lInput4));
            }
            return cdResult;
        }
        public void setCalcInputs(Hashtable calcInputs, string[] inputDefs)
        {
            this.calcInputs = calcInputs;
            this.calcInputDefs = inputDefs;
        }
        public void setOpenChannelArrayList(ArrayList openChannelArrayList)
        {
            this.openChannelArrayList = openChannelArrayList;
        }
        /**
         * 
         *<P>Merges the two data vectors. The open channel vector gets set in the setOpenChannelArrayList
         *method.  The Hashtable contains a key and value, which is the result vector from the control device (CD) calculations.  </P>
         *<P><B>Limitations:</B>  </P>
         * @param controlDeviceArrayLists
         * @return
         */
        public ArrayList mergeCalculators(Dictionary<string, ArrayList> controlDeviceArrayLists)
        {
            //I have the open channel depth vector and the different control devices.
            ArrayList gridArrayList = new ArrayList();
            ArrayList oldRowArrayList;
            double currentDepth = 0;
            string calculatorTypeString = "";
            double lastFlowReading = 0;
            hasDepthEverBeenFound = false;
            //FTableCalculatorConstants.DepthCheckFinal=-1;

            for (int i = 0; i < openChannelArrayList.Count; i++)
            {
                oldRowArrayList = (ArrayList)openChannelArrayList[i]; //get a control device

                //get the depth on that row
                currentDepth = double.Parse(oldRowArrayList[0].ToString());

                //go over each selected control device and find out if its depth is > the depth of the stream
                //for (Enumeration e = controlDeviceArrayLists.keys();e.hasMoreElements();){

                // FTableCalculatorConstants.DepthCheckFinal=0;
                for (int z = 0; z < controlDeviceArrayLists.Count; z++)
                {
                    //take the control device grid
                    //calculatorTypestring = e.nextElement().ToString();

                    Object[] objKeys = controlDeviceArrayLists.Keys.ToArray<string>();
                    calculatorTypeString = objKeys[z].ToString();

                    ArrayList cdGridArrayList = null;
                    controlDeviceArrayLists.TryGetValue(calculatorTypeString, out cdGridArrayList);
                    bool foundDepth = false;

                    //FTableCalculatorConstants.DepthCheck=0; // sri-08-27-2012
                    for (int j = 0; j < cdGridArrayList.Count; j++)
                    {
                        // used to indicate that control devices are added
                        FTableCalculatorConstants.DepthCheck = 1;
                        ArrayList cdRowArrayList = (ArrayList)cdGridArrayList[j];
                        double controlDeviceDepth = double.Parse(cdRowArrayList[0].ToString());
                        if (controlDeviceDepth == currentDepth)
                        {
                            // FTableCalculatorConstants.DepthCheck=1;  // sri-08-27-2012
                            foundDepth = true;
                            oldRowArrayList.Add(double.Parse(cdRowArrayList[3].ToString()));
                            lastFlowReading = double.Parse(cdRowArrayList[3].ToString());
                            hasDepthEverBeenFound = true;
                            //since you found the depth, go to the next control device
                            break;
                        }
                    }
                    //this code was also added to make the merge results code work.
                    if (foundDepth == false)
                    {
                        //A matching depth for a control device was found in previous iterations, but
                        //now the depth is not found.  So if the control device is an orifice, just keep 
                        //repeating the maximum flow for that orifice.  This situation should never happen for 
                        //a control device like a wier.

                        // sri- removed this if condition
                        //					if (hasDepthEverBeenFound == true && calculatorTypestring.indexOf("Orifice")>=0){
                        //						oldRowArrayList.add(lastFlowReading);
                        //					}
                        //					else{
                        oldRowArrayList.Add(0.00d);
                        //}
                        // to capture depth mismatch in any one of the multiple control devices used
                        //                           if(FTableCalculatorConstants.DepthCheck==0)
                        //                                FTableCalculatorConstants.DepthCheckFinal=0;
                    }
                    //end code to fix merge bug - BED
                }
                gridArrayList.Add(oldRowArrayList);
            }
            return gridArrayList;
        }
    }
}