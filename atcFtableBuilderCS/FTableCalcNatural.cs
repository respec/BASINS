using MapWinUtility;
using System.Collections;
using System.Collections.Generic;

namespace atcFtableBuilder
{
    //This version is the 'NaturalChannelCalc' class in the original Java program
    public class FTableCalcNatural : FTableCalculator, IFTableOperations
    {
        public double inpChannelLength;
        public double inpChannelManningsValue;
        public double inpChannelSlope;
        public double inpHeightIncrement;
        public ArrayList inpChannelProfile = null; //this is an arraylist of XSectionStation(s)

        public double ChannelProfileYMaxDepth = -999; //a holder of the most depth in the x-section profile

        public FTableCalcNatural()
        {
            geomInputLongNames = new string[] {"Channel Length","Mannings Value","Longitudinal Slope","Height Increment"};
            geomInputs = new ChannelGeomInput[]{
                ChannelGeomInput.Length,
                ChannelGeomInput.ManningsN,
                ChannelGeomInput.LongitudinalSlope,
                ChannelGeomInput.HeightIncrement
            };
        }

        public bool SetInputParameters(Hashtable aInputs)
        {
            bool AllInputsFound = true;
            if (aInputs[ChannelGeomInput.Length] != null)
                inpChannelLength = (double)aInputs[ChannelGeomInput.Length];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.ManningsN] != null)
                inpChannelManningsValue = (double)aInputs[ChannelGeomInput.ManningsN];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.LongitudinalSlope] != null)
                inpChannelSlope = (double)aInputs[ChannelGeomInput.LongitudinalSlope];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.HeightIncrement] != null)
                inpHeightIncrement = (double)aInputs[ChannelGeomInput.HeightIncrement];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.Profile] != null)
                inpChannelProfile = (ArrayList)aInputs[ChannelGeomInput.Profile];
            else
                AllInputsFound = false;

            return AllInputsFound;
        }

        public ArrayList GenerateFTable()
        {
            if (inpChannelProfile == null) return null;
            return GenerateFTable(inpChannelProfile, inpChannelLength, inpChannelManningsValue, inpChannelSlope, inpHeightIncrement);
        }

        private ArrayList GenerateFTable(ArrayList channelProfile, double channelLength,
                double channelManningsValue, double channelSlope, double heightIncrement)
        {
            ArrayList resultVector = new ArrayList();

            int i = 0;
            int n = 0;
            int j = 0;
            double ymin = 0.0;
            double ymax = 0.0;

            //By oversizing the array (the original dimension was 100), an array out of bounds
            //error is avoided.

            int larraySize = 1000;
            double[] x = new double[larraySize];
            double[] y = new double[larraySize];
            double[] xx = new double[larraySize];
            double[] yy = new double[larraySize];
            double[] h = new double[larraySize];
            double[] P = new double[larraySize];
            double[] A = new double[larraySize];
            double[] R = new double[larraySize];
            double[] q = new double[larraySize];
            double[] ac = new double[larraySize];
            double[] ghl = new double[larraySize];
            double[] VOLL = new double[larraySize];
            double[] top = new double[larraySize];
            double[] LT1 = new double[larraySize];
            double[] RT1 = new double[larraySize];
            double[] ILL = new double[larraySize];
            double[] IRR = new double[larraySize];
            double CONS = FTableCalculatorConstants.Cunit;
            double ACONS = FTableCalculatorConstants.Aunit;
            int counter = 0;
            int counterxl = 0;
            int counterxr = 0;
            double ll = 0.0;
            double nN = 0.0;
            double s = 0.0;

            i = 0;
            int k = 0;
            double nh = 0.0;
            int iL = 0;
            int iR = 0;
            int m = 0;
            double xL = 0.0;
            double xR = 0.0;
            double yL = 0.0;
            double hmin = 0.0;
            double hmax = 0.0;
            double Dh = 0.0; // input
            ll = channelLength;
            s = channelSlope;
            nN = channelManningsValue;

            //x = this.makeDoubleArrayFromVectorArray(channelProfile, 0);
            //y = this.makeDoubleArrayFromVectorArray(channelProfile, 1);
            //ymin = 100000000.0;
            //xmin = ymin;
            //xmax = -ymin;
            /*Determine the maximum x and y coordinates*/
            //xmax = FTableCalculatorConstants.getMaxValueFromVector(0, channelProfile);
            //xmin = FTableCalculatorConstants.getMinValueFromVector(0, channelProfile);
            //ymax = FTableCalculatorConstants.getMaxValueFromVector(1, channelProfile);
            //ymin = FTableCalculatorConstants.getMinValueFromVector(1, channelProfile);

            ymin = double.MaxValue;
            ymax = double.MinValue;
            XSectionStation lStation;
            for (i = 0; i < channelProfile.Count; i++)
            {
                lStation = (XSectionStation)channelProfile[i];
                if (lStation.y < ymin) ymin = lStation.y;
                if (lStation.y > ymax) ymax = lStation.y;
                x[i + 1] = lStation.x;
                y[i + 1] = lStation.y;
            }

            ChannelProfileYMaxDepth = ymax;

            Dh = heightIncrement;  // must accept by the user
            n = 1;

            ////TODO: System.out.print(xmax);
            //'Calculate minimum and maximum depths
            hmin = 0.0;
            hmax = ymax - ymin;

            //'Get depth increment and calculate array of depths
            //  myMessage = "The maximum depth is " + hmax.string + ".  ";
            //  myMessage = myMessage + "Enter depth increment for table:";
            //   Dh = Val(InputBox(myMessage))

            nh = (hmax / Dh);

            for (i = 0; i < channelProfile.Count; i++)
            {
                lStation = (XSectionStation)channelProfile[i];
            }

            for (i = 1; i <= nh; i++)
            {
                h[i] = i * Dh;
            }

            while (n <= channelProfile.Count)
            {
                n = n + 1;
            }


            n = n - 1;

            for (k = 1; k <= nh; k++)
            {
                counterxl = 0;
                counterxr = 0;
                counter = 0;
                P[k] = 0.0;
                A[k] = 0.0;
                top[k] = 0.0;

                yL = ymin + h[k];

                // 'Obtain points where current depth h(k) intersects x-section
                for (i = 1; i <= n - 1; i++)
                {
                    if ((yL > y[i + 1]) && (yL <= y[i]))
                    {
                        xL = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1] - y[i]);
                        iL = i;
                        counterxl = counterxl + 1;
                    }
                    if ((yL > y[i]) && (yL <= y[i + 1]))
                    {
                        xR = x[i] + (yL - y[i]) * (x[i + 1] - x[i]) / (y[i + 1] - y[i]);
                        iR = i;
                        counterxr = counterxr + 1;
                    }

                    LT1[counterxl] = xL;
                    RT1[counterxr] = xR;
                    ILL[counterxl] = iL;
                    IRR[counterxr] = iR;
                }

                if (counterxr >= counterxl)
                {
                    counter = counterxr;
                }
                else counter = counterxl;

                iL = 0;
                iR = 0;
                xR = 0;
                xL = 0;

                //'Load vectors of x and y included below current depth
                for (j = 1; j <= counter; j++)
                {
                    xL = LT1[j];
                    xR = RT1[j];
                    iL = (int)ILL[j];
                    iR = (int)IRR[j];

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
                    for (i = 1; i <= m + 1; i++)
                    {
                        P[k] = P[k] + System.Math.Sqrt(System.Math.Pow(xx[i] - xx[i + 1], 2) + System.Math.Pow(yy[i] - yy[i + 1], 2));
                        top[k] = top[k] + System.Math.Abs(xx[i] - xx[i + 1]);
                    }
                    for (i = 1; i <= m + 2; i++)
                    {
                        A[k] = A[k] + xx[i] * yy[i + 1] - xx[i + 1] * yy[i];
                    }
                }
                A[k] = 0.5 * System.Math.Abs(A[k]);
                R[k] = A[k] / P[k];
            }

            for (i = 1; i <= nh; i++)
            {
                q[i] = A[i] * System.Math.Pow(R[i], .677) * (1.486 / nN) * System.Math.Sqrt(s);
                ac[i] = (ll * top[i] / ACONS);
                //ac[i] = ac[i] + (ll * top[i])/43560.0;
            }


            // ' this section calculates the volume
            //for (i=1;i<=m+2;i++)    
            for (i = 0; i <= nh; i++)
            {
                q[i] = A[i] * System.Math.Pow(R[i], (2.0 / 3.0)) * (CONS / nN) * System.Math.Sqrt(s);
                ghl[i] = Dh * ((ac[i] + ac[i + 1])) * 0.5;
                VOLL[i + 1] = ghl[i] + VOLL[i];
                VOLL[0] = 0;
                //TODO: System.out.print(h[i]);
                //TODO: System.out.println("      ");  
                //TODO: System.out.print(VOLL[i]); 
                //TODO: System.out.print(x[i+1]);
            }
            /**CSC added this code to construct the result vector.
             * It is based on the for loops written above
             */
            //ArrayList resultVector = new ArrayList();
            if (FTableCalculatorConstants.programunits == 0)
            {
                for (i = 0; i <= nh; i++)
                {
                    ac[i] = ac[i] / System.Math.Pow(10, 4);
                    VOLL[i] = VOLL[i] / (System.Math.Pow(10, 6));
                }
            }
            string lFormat = "{0:0.000000}";
            for (i = 0; i <= nh; i++)
            {
                ArrayList rowData = new ArrayList();
                rowData.Add(string.Format(lFormat, (object)h[i]));
                rowData.Add(string.Format(lFormat, (object)ac[i]));
                rowData.Add(string.Format(lFormat, (object)VOLL[i]));
                rowData.Add(string.Format(lFormat, (object)q[i]));

                resultVector.Add(rowData);
            }

            larraySize = 0;
            x = new double[larraySize];
            y = new double[larraySize];
            xx = new double[larraySize];
            yy = new double[larraySize];
            h = new double[larraySize];
            P = new double[larraySize];
            A = new double[larraySize];
            R = new double[larraySize];
            q = new double[larraySize];
            ac = new double[larraySize];
            ghl = new double[larraySize];
            VOLL = new double[larraySize];
            top = new double[larraySize];
            LT1 = new double[larraySize];
            RT1 = new double[larraySize];
            ILL = new double[larraySize];
            IRR = new double[larraySize];

            return resultVector;
        }
    }
}