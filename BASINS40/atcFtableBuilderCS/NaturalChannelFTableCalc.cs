using System.Collections;



public class NaturalChannelFTableCalc 
{
	ArrayList vectorRowData;
	ArrayList vectorColNames;
	double[] x = null;
	double[] y = null;
	double _totalWidth;
	double _maxDepth;
	int MAXSIZE = 1000;
		  
	public NaturalChannelFTableCalc()
	{
		vectorColNames = new ArrayList();
		vectorColNames.Add("depth");
		vectorColNames.Add("area");
		vectorColNames.Add("volume");
		vectorColNames.Add("outflow1");
	}
		  
	public ArrayList GetColumnNames()
	{
		return vectorColNames;
	}

	public ArrayList GenerateFTable(ArrayList channelProfile, double channelLength, 
	              			     double channelManningsValue,double channelSlope)
	{
		
		if (channelProfile == null)
			return null;
		
		getParamsFromTable(channelProfile);
		
		//int count = channelProfile.size();
        int count = x.Length;
		
		if (count < 1)
			return null;
		
	    //Assign channel length 
		double ll = channelLength;
		//Assign channel slope
	    double s = channelSlope;
	    //Assign Mannings N
	    double nN = channelManningsValue;
	    
	    //Will be passed in
	    double Dh = 1.0;  // must accept by the user
		
        // TODO code application logic here
        int n = 0;
        double xmin = 0 ;
        double xmax = 0 ;
     
        double ymin = 0;
        double ymax = 0;
        //double [] x = new double[MAXSIZE];   
        //double [] x = new double[count];        
	    //double [] y = new double[count]; 
	    double [] xx = new double[MAXSIZE];   
	    double [] yy = new double[MAXSIZE]; 
   
	    double [] P = new double[MAXSIZE]; 
	    double [] A = new double[MAXSIZE];   
	    double [] R = new double[MAXSIZE];
	    double [] q = new double[MAXSIZE];   
	    double [] ac = new double[MAXSIZE]; 
	    double [] ghl = new double[MAXSIZE];   
	    double [] VOLL = new double[MAXSIZE];
        double [] top = new double[MAXSIZE];
	    double CONS = FTableCalculatorConstants.Cunit;
        double ACONS = FTableCalculatorConstants.Aunit;
	//  int i =0;
	    
	    double nh=0 ;
	    int iL = 0;
	    int iR =0;
	    int m =0;
	    double xL =0;
	    double xR =0;
	    double yL=0;
	    double hmin = 0;
	    double hmax =0;	    

	    ymin = 100000000.0;
	    ymax = -999999.0;
	    xmin = ymin;
     // xmax = -ymin;

	    n = 1;
      //while(n < 7)
	    for (int idx=0;idx<count;idx++)
	    {
	    	if(y[idx] < ymin) 	    	
	    		ymin= y[idx];  
	    	
	    	if (y[idx] > ymax)
	    		ymax = y[idx];

	    	if (x[idx] < xmin)	    	
	    		xmin = x[idx];
	    	
	    	if (x[idx] > xmax)	    	
	    	xmax = x[idx]; 	   
 			//TODO: System.out.println("      ");  
		    //TODO: System.out.print(ymax); 
	    }
 
	    n = n - 1;
 
		//'Calculate minimum and maximum depths
		hmin = 0;
		hmax = ymax - ymin;
		 
		//'Get depth increment and calculate array of depths
		//  myMessage = "The maximum depth is " + hmax.string + ".  ";
		//  myMessage = myMessage + "Enter depth increment for table:";
		//   Dh = Val(InputBox(myMessage))
		
		nh = (hmax/Dh);
	    double [] h = new double[(int)nh];
 
		for (int idx = 0; idx<nh; idx++)
		{
			h[idx] = idx * Dh;  			
		}    
   
		//'Calculate cross-sectional properties
		for (int k=0;k<nh; k++)
		{
			yL = ymin + h[k]; 
         
			//'Obtain points where current depth h(k) intersects x-section
			for (int i = 0; i< y.Length-1;i++)
			{
				if ((yL > y[i + 1]) && (yL <= y[i]))
				{
					xL = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1] - y[i]);
					iL = i;              
				}
				if ((yL > y[i]) && (yL<= y[i + 1]))
				{
					xR = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1]- y[i]);
					iR = i;            
				}
			}
    
			//'Load vectors of x and y included below current depth
			m = iR - iL;
			xx[1] = xL;
			yy[1] = yL;
			for (int i = 0; i < m; i++)
			{ 
				xx[i + 1] = x[iL + i];
				yy[i + 1] = y[iL + i];
			}
			xx[m + 2] = xR;
			yy[m + 2] = yL;
			xx[m + 3] = xx[1];
			yy[m + 3] = yy[1];
     
			// 'Calculate wetted perimeter, area, and hydraulic radius
			P[k] = 0;        
			A[k] = 0;
			for(int i = 0; i < m + 1; i++)
			{
				P[k] = P[k] + System.Math.Sqrt(System.Math.Pow(xx[i] - xx[i + 1], 2) + System.Math.Pow(yy[i] - yy[i + 1],2));
		               top[k] = top[k]+ System.Math.Abs(xx[i]-xx[i+1]);
                        }
			for (int i = 0;i < m + 2; i++)
			{
				A[k] = A[k] + xx[i] * yy[i + 1] - xx[i + 1] * yy[i];
			}
			A[k] = 0.5 * System.Math.Abs(A[k]);
			R[k] = A[k] / P[k];
                      
 
		}
 
 
		for(int i = 0; i < nh-1; i++)
		{      
		//	top = Math.abs(x[i] - x[i + 1]);
			q[i] = A[i] * System.Math.Pow(R[i],.677) * (CONS / nN) * System.Math.Sqrt(s);
			ac[i] = ac[i] + (ll * top[i])/ACONS;
		}

		// ' this section calculates the volume
		for(int i = 0;i < nh-1;i++)
		{
			ghl[i] = Dh * ((ac[i] + ac[i + 1])) * 0.5;
			VOLL[i + 1] = ghl[i] + VOLL[i];
			//TODO: System.out.print(q[i]);
			//TODO: System.out.println("      ");  
			//TODO: System.out.print(P[k]);  
		}	

        if(FTableCalculatorConstants.programunits==0)
        {
            for(int i = 0;i < nh;i++)
            {
                ac[i] = ac[i]/System.Math.Pow(10,4);
                VOLL[i] = VOLL[i]/(System.Math.Pow(10,6));
            }
        }


		ArrayList results = new ArrayList();
		ArrayList rowData = null;
        string sDepth = "";
        string sArea = "";
        string sVolume = "";
        string sOutFlow = "";
		for(int i = 0;i < nh;i++)
		{
			rowData = new ArrayList();
            sDepth   = string.Format("     %.5f", h[i]);
            sArea    = string.Format("%.5f", ac[i]);
            sVolume  = string.Format("%.5f", VOLL[i]);
            sOutFlow = string.Format("%.5f", q[i]);

	        rowData.Add(sDepth);
	        rowData.Add(sArea);
	        rowData.Add(sVolume);
	        rowData.Add(sOutFlow);
	        
			results.Add(rowData);	
		}
		return results;
	}
	
	private void Temp()
	{
	}
	
	private void getParamsFromTable(ArrayList channelProfile)
	{
		ArrayList row;
		
		double depth = 0.0;
		double dist = 0.0;

        int numRows = channelProfile.Count;
		
		x = new double[numRows];
		y = new double[numRows];
		string strDist = "";
		string strDepth = "";
		int count = 0;
		
		for (int i=0;i<numRows;i++)
		{
			row = (ArrayList)channelProfile[i];
			
			strDist = row[0].ToString();
			strDepth = row[1].ToString();
			if ((strDist == null) || (strDist == ""))
				break;
			
			if ((strDepth == null) || (strDepth == ""))
				break;
						
			count++;
			dist = double.Parse(strDist);
			x[i] = dist;
						
			depth = double.Parse(strDepth);
			y[i] = depth;
						
			if (dist > _totalWidth)
				_totalWidth = dist;
			
			if (depth > _maxDepth)
				_maxDepth = depth;						
		}						
		x = resizeArray(x, count);
		y = resizeArray(y, count);
	}
	
	private double[] resizeArray (double[] oldArray, int newSize) 
	{
		int oldSize = oldArray.Length;
		
		int preserveLength = System.Math.Min(oldSize,newSize);
		double[] newArray = new double[preserveLength];
		if (preserveLength > 0)
		      System.Array.Copy (oldArray,0,newArray,0,preserveLength);
		
		return newArray; 
	}
}