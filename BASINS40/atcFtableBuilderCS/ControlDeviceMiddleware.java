package gov.epa.nerl.athens.ftable.controldevices;

import java.util.Enumeration;
import java.util.LinkedHashMap;
import java.util.Set;
import java.util.Vector;

import sun.font.GlyphLayout.GVData;

import com.sun.org.apache.xalan.internal.xsltc.runtime.Hashtable;
import gov.epa.nerl.athens.ftable.FTableCalculatorConstants;

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
public class ControlDeviceMiddleware {

	private Class myCalcType;
	private java.util.Hashtable calcInputs;
	private String [] calcInputDefs;
	private Vector openChannelVector;
	boolean hasDepthEverBeenFound = false;
	/**
	 * The inputs parameter must be put in the correct order that calcuator expects
	 * @param calculatorType
	 * @param inputs
	 */
	public ControlDeviceMiddleware(Class calculatorType){
		
		this.myCalcType = calculatorType;
			
	}
	/**
	 * 
	 *<P>Get the control device result form the calculators by calling the GenerateFTable method in each of them.  </P>
	 *<P><B>Limitations:</B>  </P>
	 * @return The result vector from the calcualtion.s 
	 */
	public Vector getControlDeviceResult(){
		Vector cdResult = new Vector();
		
		if (myCalcType == BroadFTableCalc.class){
			BroadFTableCalc bft = new BroadFTableCalc();
			cdResult = bft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString()),
                                        Double.parseDouble(calcInputs.get(calcInputDefs[3]).toString()));
		}
		else if (myCalcType == CipolettiFTableCalc.class){
			CipolettiFTableCalc cft = new CipolettiFTableCalc();
                      String a  = calcInputs.get(calcInputDefs[0]).toString();
			cdResult = cft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString()),
                                        Double.parseDouble(calcInputs.get(calcInputDefs[3]).toString()));
		}
		else if (myCalcType == RectangularFTableCalc.class){
			RectangularFTableCalc rft = new RectangularFTableCalc();
			cdResult = rft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString()),
                                        Double.parseDouble(calcInputs.get(calcInputDefs[3]).toString()));
		}
		else if (myCalcType == RiserFTableCalc.class){
			RiserFTableCalc rft = new RiserFTableCalc();
			cdResult = rft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()),
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()),
					Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString()));

                }
		else if (myCalcType == UnderflowFTableCalc.class){
			UnderflowFTableCalc uft = new UnderflowFTableCalc();
			cdResult = uft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()),
                                       Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString() ));
		}
		else if (myCalcType == VNotchFTableCalc.class){
			VNotchFTableCalc vft = new VNotchFTableCalc();
			//this should still work as long as the string array input defs are still in order.
			cdResult = vft.GenerateFTable(Double.parseDouble(calcInputs.get(calcInputDefs[0]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[1]).toString()), 
					Double.parseDouble(calcInputs.get(calcInputDefs[2]).toString()),
                                          Double.parseDouble(calcInputs.get(calcInputDefs[3]).toString()));
		}
		return cdResult;
	}
	public void setCalcInputs(java.util.Hashtable calcInputs,String [] inputDefs) {
		this.calcInputs = calcInputs;
		this.calcInputDefs = inputDefs;
	}
	public void setOpenChannelVector(Vector openChannelVector) {
		this.openChannelVector = openChannelVector;
	}
	/**
	 * 
	 *<P>Merges the two data vectors. The open channel vector gets set in the setOpenChannelVector
	 *method.  The Hashtable contains a key and value, which is the result vector from the control device (CD) calculations.  </P>
	 *<P><B>Limitations:</B>  </P>
	 * @param controlDeviceVectors
	 * @return
	 */
	public Vector mergeCalculators(LinkedHashMap<String, Vector> controlDeviceVectors){
		//I have the open channel depth vector and the different control devices.
		Vector gridVector = new Vector();
		Vector oldRowVector;
		double currentDepth = 0;
		String calculatorTypeString = "";
		double lastFlowReading = 0;
		hasDepthEverBeenFound = false;
                //FTableCalculatorConstants.DepthCheckFinal=-1;
                 
		for (int i=0;i<openChannelVector.size();i++){
			oldRowVector = (Vector)openChannelVector.get(i); //get a control device
			
			//get the depth on that row
			currentDepth = Double.parseDouble(oldRowVector.get(0).toString());
			
			//go over each selected control device and find out if its depth is > the depth of the stream
			//for (Enumeration e = controlDeviceVectors.keys();e.hasMoreElements();){
			
                       // FTableCalculatorConstants.DepthCheckFinal=0;
                        for (int z = 0; z < controlDeviceVectors.size();z++){
				//take the control device grid
				//calculatorTypeString = e.nextElement().toString();
                               
				Object [] objKeys = controlDeviceVectors.keySet().toArray();
				calculatorTypeString = objKeys[z].toString();
				
				Vector cdGridVector = (Vector)controlDeviceVectors.get(calculatorTypeString);
				boolean foundDepth = false;

                                //FTableCalculatorConstants.DepthCheck=0; // sri-08-27-2012
				for (int j =0; j < cdGridVector.size();j++){
                                    // used to indicate that control devices are added
                                     FTableCalculatorConstants.DepthCheck=1;
					Vector cdRowVector = (Vector)cdGridVector.get(j);
					double controlDeviceDepth =Double.parseDouble(cdRowVector.get(0).toString()); 
					if (controlDeviceDepth == currentDepth){
                                               // FTableCalculatorConstants.DepthCheck=1;  // sri-08-27-2012
						foundDepth = true;
						oldRowVector.add(Double.parseDouble(cdRowVector.get(3).toString()));
						lastFlowReading = Double.parseDouble(cdRowVector.get(3).toString());
						hasDepthEverBeenFound = true;
						//since you found the depth, go to the next control device
						break;
					}
				}
				//this code was also added to make the merge results code work.
				if (foundDepth == false){
					//A matching depth for a control device was found in previous iterations, but
					//now the depth is not found.  So if the control device is an orifice, just keep 
					//repeating the maximum flow for that orifice.  This situation should never happen for 
					//a control device like a wier.

                                    // sri- removed this if condition
//					if (hasDepthEverBeenFound == true && calculatorTypeString.indexOf("Orifice")>=0){
//						oldRowVector.add(lastFlowReading);
//					}
//					else{
						oldRowVector.add(0.00d);
					//}
                               // to capture depth mismatch in any one of the multiple control devices used
//                           if(FTableCalculatorConstants.DepthCheck==0)
//                                FTableCalculatorConstants.DepthCheckFinal=0;
				}
				//end code to fix merge bug - BED

                      

			}


		
			gridVector.add(oldRowVector);
		}

		return gridVector;
	}
	
	
}
