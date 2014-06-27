package gov.epa.nerl.athens.ftable.controldevices;
import gov.epa.nerl.athens.ftable.FTableCalculatorConstants;

import java.util.Vector;
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

public class UnderflowCalc 

{
    Vector vectorRowData;
    Vector vectorColNames;

    public UnderflowCalc()
    {
        vectorColNames = new Vector();
        vectorColNames.add("DEPTH");
        vectorColNames.add("AREA");
        vectorColNames.add("VOLUME");
        vectorColNames.add("OUTFLOW");

    }

    public Vector GetColumnNames()
    {
        return vectorColNames;
    }
public Vector GenerateFTable(double WeirLength, double channelDiameter,
                             double WeirInvertValue)
    {
        Vector vectorRowData = new Vector();
        Object[] vals; 
	//Flow Area Calculations
	double L = WeirLength;
	double N = WeirInvertValue;
	double DT = channelDiameter;
	//double S = longitudalChannelSlope;
	//double w = topChannelWidth;		      
	      
	double dblArea = 0.0;
	double dblVolume = 0.0;
	double dblOutFlow = 0.0;
	      
	      
	Vector row = new Vector();
          
        double A = 0.0;
        double wd = 0.0;
        double hr = 0.0;
        double C = 0.0;
        double QC = 0.0;
        double acr = 0.0;
        double GH = 0.0;
        double GJ = 0.0;
        double stot = 0.0;

        double prevAcr = 0.0;;
        double prevStot = 0.0;
        double prevG = 0.0;
        double dia =0.0;
        double JR =0.0;
        double angle =0.0 ;       
        double CF=0.0;
        double R1 = 0.0;
        double CD =0.0;
        double Angle =0.0;
   

          	for (double g = N; g<100;g+=FTableCalculatorConstants.calculatorIncrement)
	{

   if(g > N) 
   {
      
       R1 = g-N;
   } 
         
  // QC = 3.367 * Math.pow(R1,1.5) * L; 
 //  QC = 3.33 * Math.pow(R1,1.5)* L ; 

  dblArea = 3.14159*(Math.pow(DT,2)/4);
   QC =0.6*dblArea*Math.pow(64.4,0.5)*Math.pow(R1,0.5); 
     System.out.print(g);
			System.out.println("      "); 

            prevStot = stot;
            prevAcr = acr;
          
            row = new Vector();

            vals = new Object[]{new Double(g)};
            //String sDepth = String.format("     %.2f",vals);
            String sDepth = String.format("     %.5f",vals);
            row.add(sDepth);

            vals = new Object[]{new Double(acr)};
            //String sArea = String.format(  "%.2f",vals);
            String sArea = String.format(  "%.5f",vals);
            row.add(sArea);

            vals = new Object[]{new Double(stot)};
            //String sVolume = String.format(  "%.2f",vals);
             String sVolume = String.format(  "%.5f",vals);
            row.add(sVolume);

            vals = new Object[]{new Double(QC)};
            //String sOutFlow = String.format(  "%.2f",vals);
            String sOutFlow = String.format(  "%.5f",vals);
            row.add(sOutFlow);

            vectorRowData.add(row);
            
	}	      
	      
	return vectorRowData;
	   		  	      	  
    }
}
