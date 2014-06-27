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
public class CircleFTableCalc
{
  ArrayList vectorRowData;
  ArrayList vectorColNames;

  public CircleFTableCalc()
  {
//    vectorColNames = new ArrayList();
//    vectorColNames.add("depth(ft)");
//    vectorColNames.add("area(acres)");
//    vectorColNames.add("volume(ac-ft)");
//    vectorColNames.add("outflow1(cfs)");

      // sri-09-11-2012
    vectorColNames = new ArrayList();
	vectorColNames.Add("depth");
	vectorColNames.Add("area");
	vectorColNames.Add("volume");
    // sri- 09-11-2012
	//vectorColNames.add("outflow1");
  }

  public ArrayList GetColumnNames()
  {
    return vectorColNames;
  }

  public ArrayList GenerateFTable(double channelLength, double channelDiameter,
                             double channelManningsValue, double averageChannelSlope,
                             double Increment)  // sri-09-11-2012
  {
	  vectorRowData = new ArrayList();
      //Flow Area Calculations
      double L = channelLength;
      double NP = channelManningsValue;
      double SP = averageChannelSlope;
      double DT = channelDiameter;

      double dblDepthIncrement = DT / 10.0;
      double dblDepth = 0;
      double b1 = 0;
      double b2 = 0;
      double b3 = 0;
      double b4 = 0;
      double b5 = 0;
      double t1 = 0;
      double V1 = 0;
      double V2 = 0;
      double V3 = 0;
      double R = DT /2.0;
      double wp1 = 0;
      double wP = 0;
      double dblArea = 0;
      double dblOutFlow = 0;
      double dblVolume = 0;
      double t2 = 0;
      double t = 0;
      double dblVelocity = 0;
      //sri-09-11-2012
      double CONS = FTableCalculatorConstants.Cunit;
      double ACONS = FTableCalculatorConstants.Aunit;

    //dblDepth += dblDepthIncrement;
      //depth starts at .01, both calculator and control structure must start
      //at the same relative depth.
      dblDepth = 0.00;

      string sDepth   = "";
      string sArea    = "";
      string sVolume  = "";
      string sOutFlow = "";
      while (dblDepth <= DT)
      {
          //Flow Area Calculations
          b1 = DT * DT / 8.0;
          b2 = 1.0 - (2.0 * dblDepth / DT);
          b3 = 2.0 * (2.0 * System.Math.Atan(1.0) - System.Math.Atan(b2 / System.Math.Sqrt(1.0 - b2 * b2)));
          b4 = System.Math.Sin(b3);
          b5 = 2.0 * (2.0 * System.Math.Atan(1.0) - System.Math.Atan(b2 / System.Math.Sqrt(1.0 - b2 * b2)));
          dblArea = b1 * (b5 - b4);
          //Top Width Calculations
          t1 = (( 2.0 * dblDepth - DT) / DT);
          t2 = 2.0 * System.Math.Atan(1.0) - System.Math.Atan(t1 / System.Math.Sqrt(1.0 - t1 * t1));
          t = DT * System.Math.Sin(t2);
          wp1 = ( 1.0 - ( 2.0 * dblDepth / DT));
          wP = DT * ( 2.0 * System.Math.Atan(1.0) - System.Math.Atan(wp1 / System.Math.Sqrt( 1.0 - wp1 * wp1)));
       // dblOutFlow = 1.486 * dblArea * System.Math.Pow(( dblArea / wP), (2.0/3.0)) * System.Math.Sqrt(SP) / NP;
          dblOutFlow = CONS * dblArea * System.Math.Pow(( dblArea / wP), (2.0/3.0)) * System.Math.Sqrt(SP) / NP;
          //Volume calculations
          V1 = (R - dblDepth) * System.Math.Sqrt( 2.0 * R * dblDepth - dblDepth * dblDepth);
          V2 = (R - dblDepth) / R;
          V3 = 2.0 * System.Math.Atan(1.0) - System.Math.Atan( V2 / System.Math.Sqrt(1.0 - V2 * V2));
        //dblVolume = (L * ( (0.5 * DT) * (0.5 * DT) * V3 - V1)) / 43560.0;
          dblVolume = (L * ( (0.5 * DT) * (0.5 * DT) * V3 - V1)) / ACONS;
          //Valocity Calculatios
        //dblVelocity = 1.486 * System.Math.Pow(( dblArea / wP), (2.0/3.0)) * System.Math.Sqrt(SP) / NP;
          dblVelocity = CONS * System.Math.Pow(( dblArea / wP), (2.0/3.0)) * System.Math.Sqrt(SP) / NP;
          //Area Calculations
        // dblArea = (L * t) / 43560.0;
           dblArea = (L * t) / ACONS;
         if (dblDepth == 0.0){ dblOutFlow = 0.0;}

           //SRI-09-11-2012
           if(FTableCalculatorConstants.programunits==0)  // SI UNITS SELECTED
            {
                dblArea = dblArea/(System.Math.Pow(10,4));
                dblVolume = dblVolume/(System.Math.Pow(10,6));
            }
          //TODO: System.out.println("      ");  
	      //TODO: System.out.print(dblOutFlow); 

          ArrayList row = new ArrayList();
          
          sDepth   = string.Format("%.6f", dblDepth);
          sArea    = string.Format("%.6f", dblArea);
          sVolume  = string.Format("%.6f", dblVolume);
          sOutFlow = string.Format("%.6f", dblOutFlow);
          row.Add(sDepth);
          //row.add("1");
          row.Add(sArea);
          //row.add("2");
          row.Add(sVolume);
          //row.add("3");
          row.Add(sOutFlow);
          //row.add("4");

          vectorRowData.Add(row);

        //dblDepth += 0.02;
       // dblDepth += FTableCalculatorConstants.calculatorIncrement;
          dblDepth += Increment;  // sri-09-11-2012
        }      
        return vectorRowData;
  }
}