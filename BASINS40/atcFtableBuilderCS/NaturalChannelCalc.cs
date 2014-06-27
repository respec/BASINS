using MapWinUtility;
using System.Collections;
using System.Collections.Generic;

public class NaturalChannelCalc {

	public ArrayList GenerateFTable(ArrayList channelProfile, double channelLength, 
			double channelManningsValue,double channelSlope,double heightIncrement)

	{
		// TODO code application logic here
		ArrayList resultVector = new ArrayList();
		int profileArraySize = 0;
		string strX = "";
		string strY = "";
		double d = 0;
		int i =0;
		for (i=0; i<channelProfile.Count;i++)
		{
			strX = (string) (((ArrayList) channelProfile[i])[0]);
			strY = (string) (((ArrayList) channelProfile[i])[1]);
			if ((strX == "") || (strY == ""))
			{
				profileArraySize = i;
				break;
			}
            if (!double.TryParse(strX.Trim(), out d) || !double.TryParse(strY.Trim(), out d))
            {
                return resultVector;
            }
		}
		int n = 0;
        int j =0;
		double xmin = 0.0 ;
		double xmax = 0.0 ;
		double ymin = 0.0;
		double ymax = 0.0;

		//By oversizing the array (the original dimension was 100), an array out of bounds
		//error is avoided.
		double [] x = new double[1000];
		double [] y = new double[1000]; 
		x = this.makeDoubleArrayFromVectorArray(channelProfile, 0);
		y = this.makeDoubleArrayFromVectorArray(channelProfile, 1);
		double [] xx = new double[1000];   
		double [] yy = new double[1000]; 
		double [] h = new double[1000];   
		double [] P = new double[1000]; 
		double [] A = new double[1000];   
		double [] R = new double[1000];
		double [] q = new double[1000];   
		double [] ac = new double[1000]; 
        double [] ghl = new double[1000];   
		double [] VOLL = new double[1000];
		double [] top = new double [1000];
        double [] LT1 = new double[1000]; 
		double [] RT1 = new double[1000];   
		double [] ILL = new double[1000];
		double [] IRR = new double [1000];
                
	    double CONS = FTableCalculatorConstants.Cunit;
        double ACONS = FTableCalculatorConstants.Aunit;
        int counter=0;
        int counterxl =0;
        int counterxr =0;
		double AC1 =0.0;
		double ll = 0.0;
		double nN =0.0;
		double s =0.0;

		i =0;
		int k =0;
		double nh=0.0 ;
		int iL = 0;
		int iR =0;
		int m =0;
		double xL =0.0;
		double xR =0.0;
		double yL=0.0;
		double hmin =0.0;
		double hmax =0.0;
		double Dh =0.0; // input
		double filename =0.0;
		ll = channelLength;
		s = channelSlope;
		nN = channelManningsValue;

		ymin = 100000000.0;
		xmin = ymin;
		xmax = -ymin;
		Dh = heightIncrement;  // must accept by the user
		n = 1;

		/*Determine the maximum x and y coordinates*/

		xmax = FTableCalculatorConstants.getMaxValueFromVector(0, channelProfile);
		xmin = FTableCalculatorConstants.getMinValueFromVector(0, channelProfile);
		ymax = FTableCalculatorConstants.getMaxValueFromVector(1, channelProfile);
		ymin = FTableCalculatorConstants.getMinValueFromVector(1, channelProfile);
		////TODO: System.out.print(xmax);
		//'Calculate minimum and maximum depths
		hmin = 0.0;
		hmax = ymax - ymin;

		//'Get depth increment and calculate array of depths
		//  myMessage = "The maximum depth is " + hmax.string + ".  ";
		//  myMessage = myMessage + "Enter depth increment for table:";
		//   Dh = Val(InputBox(myMessage))

		nh = (hmax/Dh);

		for (i = 1; i<= nh; i++)
		{
			h[i] = i * Dh;  
		}    

		while(n <= profileArraySize)
		{
			n = n + 1;
		}


		n = n - 1;

		for (k = 1; k <=nh; k++)
		{
			counterxl =0;
            counterxr=0;
            counter=0;
            P[k] = 0.0;        
			A[k] = 0.0;
            top[k]= 0.0;
                        
            yL = ymin + h[k]; 

			// 'Obtain points where current depth h(k) intersects x-section
			for (i = 1; i <= n - 1;i++)
			{
				if ((yL > y[i + 1]) && (yL <= y[i]))
				{
					xL = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1] - y[i]);
					iL = i;
                    counterxl =counterxl + 1;
				}
				if ((yL > y[i]) && (yL<= y[i + 1]))
				{
					xR = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1]- y[i]);
					iR = i;
                    counterxr=counterxr +1;
				}
			
                LT1[counterxl] = xL;
                RT1[counterxr] = xR;
                ILL[counterxl] = iL;
                IRR[counterxr] = iR;
            }
         
            if(counterxr >= counterxl)
            {
                counter = counterxr ;
            }
           else counter =counterxl;
                    
           iL = 0;
           iR = 0;
           xR = 0;
           xL = 0;
                        
			//'Load vectors of x and y included below current depth
           for (j = 1; j <= counter; j++)
           {
                xL = LT1[j];
                xR = RT1[j];
                iL = (int) ILL[j];
                iR = (int) IRR[j];
                     
			    m = iR - iL;
			    xx[1] = xL;
			    yy[1] = yL;
			    for (i = 1; i <= m; i++)
			    { 
				    xx[i + 1] = x[iL + i];
				    yy[i + 1] = y[iL + i];
			    }
			    xx[m + 2] = xR;
			    yy[m + 2] = yL;
			    xx[m + 3] = xx[1];
			    yy[m + 3] = yy[1];

			// 'Calculate wetted perimeter, area, and hydraulic radius
			    for(i = 1; i <= m + 1; i++)
			    {
                    P[k] = P[k] + System.Math.Sqrt(System.Math.Pow(xx[i] - xx[i + 1], 2) + System.Math.Pow(yy[i] - yy[i + 1],2));
				    top[k] = top[k]+ System.Math.Abs(xx[i]-xx[i+1]);
			    }
			    for (i = 1;i <= m + 2; i++)
			    {
				    A[k] = A[k] + xx[i] * yy[i + 1] - xx[i + 1] * yy[i];
			    }
           }
           A[k] = 0.5 * System.Math.Abs(A[k]);
           R[k] = A[k] / P[k];
		}

		for(i = 1; i <= nh; i++)
        {
			q[i] = A[i] * System.Math.Pow(R[i],.677) * (1.486 / nN) * System.Math.Sqrt(s);
			ac[i] =  (ll * top[i]/ACONS);
	      //ac[i] = ac[i] + (ll * top[i])/43560.0;
		}
 

		// ' this section calculates the volume
		//for (i=1;i<=m+2;i++)    
		for(i =0; i <= nh;i++)
		{
            q[i] = A[i] * System.Math.Pow(R[i],(2.0/3.0)) * (CONS / nN) * System.Math.Sqrt(s);
			ghl[i] = Dh* ((ac[i] + ac[i + 1])) * 0.5;
			VOLL[i + 1] = ghl[i] + VOLL[i];
            VOLL[0] =0;   
		  	//TODO: System.out.print(h[i]);
			//TODO: System.out.println("      ");  
			//TODO: System.out.print(VOLL[i]); 
            //TODO: System.out.print(x[i+1]);
		}
		/**CSC added this code to construct the result vector.
		 * It is based on the for loops written above
		 */
		//ArrayList resultVector = new ArrayList();
        if (FTableCalculatorConstants.programunits==0)
        {
            for( i = 0;i <= nh;i++)
            {
                ac[i] = ac[i]/System.Math.Pow(10,4);
                VOLL[i] = VOLL[i]/(System.Math.Pow(10,6));
            }
        }
		for (i = 0;i<=nh;i++)
        {
			 ArrayList rowData = new ArrayList();
			 rowData.Add(makeStringForVectorResult("%.6f", h[i]));
			 rowData.Add(makeStringForVectorResult("%.6f", ac[i]));
			 rowData.Add(makeStringForVectorResult("%.6f", VOLL[i]));
			 rowData.Add(makeStringForVectorResult("%.6f", q[i]));
			 
			 resultVector.Add(rowData);
		}

		return resultVector;
	}
	/**
	 * 
	 *<P></P>
	 *<P><B>Limitations:</B>  </P>
	 * @param dataVector
	 * @param index
	 * @return
	 */
	private double [] makeDoubleArrayFromVectorArray(ArrayList dataVector,int index)
    {
		//The vector i'll be passing in is a data grid vector. It is a vector of vectors.
		ArrayList dataArrayList = new ArrayList();
        string lval = "";
        double lvald = 0.0;
        bool lvalIsBad = false;
		for (int i = 0; i < dataVector.Count; i++)
        {
			ArrayList rowVector = (ArrayList) dataVector[i];
            lval = rowVector[index].ToString();
            lvalIsBad = false;
            if (!string.IsNullOrEmpty(lval))
            {
                if (double.TryParse(lval, out lvald))
                {
                    dataArrayList.Add(lvald);
                }
                else
                {
                    dataArrayList.Add(double.NaN);
                    lvalIsBad = true;
                }
            }
            else
            {
                dataArrayList.Add(double.NaN);
                lvalIsBad = true;
            }
            if (lvalIsBad)
            {
			    //TODO: System.out.println("Tried to convert data " + rowVector.get(index) + 
			    //          " in column " + index + " at row " + i);
            }
		}

		object [] obArray = dataArrayList.ToArray();
 	  //double [] resultArray = new double [obArray.length];
		double [] resultArray = new double [1000];
		//Given code was written for arrays that start at index 1.
		//This loop is adapted to fit that code.
		for (int i = 1; i <= obArray.Length;i++)
        {
		  //resultArray[i] = double.parseDouble(obArray[i-1].toString());
            resultArray[i] = (double) obArray[i - 1];
		}
		return resultArray;
	}

	/**
	 * 
	 *<P>This code is based on the code that changes the values from the calcualtor into 
	 *formatted strings for the result grid.  </P>
	 *<P><B>Limitations:</B>  </P>
	 * @param format
	 * @return
	 */
	private string makeStringForVectorResult(string format, double value)
    {
		//Object [] vals = new Object[]{new double(value.ToString())};
        string result = string.Format(format, value);
		return result;
	}
}