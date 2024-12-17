using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    public class FTableCalcNaturalFP : FTableCalculator, IFTableOperations
    {
        public double inpChannelSlope;
        public double inpChannelLength;
        public double inpChannelManningsValue;
        public double inpBankLeftLength; //left bank downstream length i.e. LOB in original Java interface
        public double inpBankRightLength; //right bank downstream lengt i.e. ROB in original Java interfaceh
        public double inpBankLeftManningsValue;
        public double inpBankRightManningsValue;
        public double inpHeightIncrement = 0.5; //default to 0.5 in original Java

        public double inpBankLeftStartingXCoord = 0;
        public double inpBankRightEndingXCoord = -99;

        public ArrayList inpChannelProfile = null; //this is an arraylist of XSectionStation(s)

        public double ChannelProfileYMaxDepth = -999; //a holder of the most depth in the x-section profile

        public FTableCalcNaturalFP()
        {
            geomInputLongNames = new string[] { "Channel Length", "Channel Mannings N", "Channel Longitudinal Slope", 
                "Left Bank Downstream Length", "Left Bank Mannings N", "Left Bank Start X coord",
                "Right Bank Downstream Length", "Right Bank Mannings N", "Right Bank End X coord",
                "Height Increment" };
            geomInputs = new ChannelGeomInput[]{
                ChannelGeomInput.NFP_ChannelLength,
                ChannelGeomInput.NFP_ChannelManningsN,
                ChannelGeomInput.NFP_ChannelSlope,
                ChannelGeomInput.NFP_BankLeftLength,
                ChannelGeomInput.NFP_BankLeftManningsN,
                ChannelGeomInput.NFP_BankLeftStartX,
                ChannelGeomInput.NFP_BankRightLength,
                ChannelGeomInput.NFP_BankRightManningsN,
                ChannelGeomInput.NFP_BankRightEndX,
                ChannelGeomInput.NFP_HeightIncrement
            };
        }

        public bool SetInputParameters(Hashtable aInputs)
        {
            bool AllInputsFound = true;
            if (aInputs[ChannelGeomInput.NFP_ChannelLength] != null)
                inpChannelLength = (double)aInputs[ChannelGeomInput.NFP_ChannelLength];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_ChannelManningsN] != null)
                inpChannelManningsValue = (double)aInputs[ChannelGeomInput.NFP_ChannelManningsN];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_ChannelSlope] != null)
                inpChannelSlope = (double)aInputs[ChannelGeomInput.NFP_ChannelSlope];
            else
                AllInputsFound = false;

            //if (aInputs[ChannelGeomInput.HeightIncrement] != null)
            //    inpHeightIncrement = (double)aInputs[ChannelGeomInput.HeightIncrement];
            //else
            //    AllInputsFound = false;

            if (aInputs[ChannelGeomInput.Profile] != null)
            {
                inpChannelProfile = (ArrayList)aInputs[ChannelGeomInput.Profile];
                if (inpChannelProfile.Count == 0)
                    AllInputsFound = false;
            }
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankLeftLength] != null)
                inpBankLeftLength = (double)aInputs[ChannelGeomInput.NFP_BankLeftLength];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankLeftManningsN] != null)
                inpBankLeftManningsValue = (double)aInputs[ChannelGeomInput.NFP_BankLeftManningsN];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankLeftStartX] != null)
            {
                inpBankLeftStartingXCoord = (double)aInputs[ChannelGeomInput.NFP_BankLeftStartX];
                if (inpBankLeftStartingXCoord < 0)
                    AllInputsFound = false;
            }
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankRightLength] != null)
                inpBankRightLength = (double)aInputs[ChannelGeomInput.NFP_BankRightLength];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankRightManningsN] != null)
                inpBankRightManningsValue = (double)aInputs[ChannelGeomInput.NFP_BankRightManningsN];
            else
                AllInputsFound = false;

            if (aInputs[ChannelGeomInput.NFP_BankRightEndX] != null)
            {
                inpBankRightEndingXCoord = (double)aInputs[ChannelGeomInput.NFP_BankRightEndX];
                if (inpBankRightEndingXCoord < 0)
                    AllInputsFound = false;
                if (AllInputsFound)
                {
                    if (((XSectionStation)inpChannelProfile[inpChannelProfile.Count - 1]).x < inpBankRightEndingXCoord)
                        AllInputsFound = false;
                }
            }
            else
                AllInputsFound = false;

            return AllInputsFound;
        }

        public ArrayList GenerateFTable()
        {
            if (inpChannelProfile == null) return null;
            return GenerateFTable(inpChannelProfile, inpBankLeftLength, inpChannelLength, inpBankRightLength, inpBankLeftManningsValue, inpChannelManningsValue, inpBankRightManningsValue,
                inpHeightIncrement, inpChannelSlope, inpBankLeftStartingXCoord, inpBankRightEndingXCoord);
        }

        public ArrayList GenerateFTable(ArrayList channelProfile, double llL, double llC,
                    double llR, double nNL, double nNC, double nNR, double heightIncrement,
                    double channelSlope, double LeftBank, double RightBank)
        {
            double LFbank = LeftBank;//9.0 ;// X of the left bank
            double RTBank = RightBank;
            // TODO code application logic here
            ArrayList resultVector = new ArrayList();

            int profileArraySize = 0;
            string strX = "";
            string strY = "";
            double d = 0;
            int i = 0;
            //for (i = 0; i < channelProfile.Count; i++)
            //{
            //    //strX = (string)((ArrayList)channelProfile[i])[0];
            //    //strY = (string)((ArrayList)channelProfile[i])[1];
            //    strX = ((XSectionStation)channelProfile[i]).x.ToString();
            //    strY = ((XSectionStation)channelProfile[i]).y.ToString();
            //    if ((strX == "") || (strY == ""))
            //    {
            //        profileArraySize = i;
            //        break;
            //    }
            //    try
            //    {
            //        d = double.Parse(strX.Trim());
            //        d = double.Parse(strY.Trim());
            //    }
            //    catch (FormatException nfe)
            //    {
            //        return resultVector;
            //    }
            //}
            int n = 0;
            double j = 0.0;
            double xmin = 0.0;
            double xmax = 0.0;
            double ymin = 0.0;
            double ymax = 0.0;


            //By oversizing the array (the original dimension was 100), an array out of bounds
            //error is avoided.
            int lSize = 10000;
            double[] x = new double[lSize];
            double[] y = new double[lSize];

            //x = this.makeDoubleArrayFromVectorArray(channelProfile, 0);
            //y = this.makeDoubleArrayFromVectorArray(channelProfile, 1);
            xmax = double.MinValue;
            ymax = double.MinValue;
            xmin = double.MaxValue;
            ymin = double.MaxValue;
            XSectionStation lStation;
            for (i = 0; i < channelProfile.Count; i++)
            {
                lStation = (XSectionStation)channelProfile[i];

                if (lStation.y < ymin) ymin = lStation.y;
                if (lStation.y > ymax) ymax = lStation.y;
                if (lStation.x < xmin) xmin = lStation.x;
                if (lStation.x > xmax) xmax = lStation.x;

                x[i + 1] = lStation.x;
                y[i + 1] = lStation.y;
            }

            ChannelProfileYMaxDepth = ymax;

            double[] xx = new double[lSize];
            double[] yy = new double[lSize];
            double[] h = new double[lSize];

            double[] R = new double[lSize];
            double[] q = new double[lSize];
            double[] ac = new double[lSize];
            double[] ghl = new double[lSize];
            double[] VOLL = new double[lSize];
            double[] top = new double[lSize];
            double[] LT1 = new double[lSize];
            double[] RT1 = new double[lSize];
            double[] ILL = new double[lSize];
            double[] IRR = new double[lSize];

            int lSize1 = 1000;
            //double [] hh = new double[lSize1];
            double[,] BGW = new double[lSize1, lSize1];
            double[,] BOW = new double[lSize1, lSize1];
            double[] topp = new double[lSize1];
            double[] AA = new double[lSize1];
            double QQL, AREA, RRC, RRL, RRR, ALPHA, QQC;

            double AC1 = 0.0;
            //double ll = llL;     // #################################
            //double nN =nNL;  // channelManningsValue currently same value uis used to be chnaged#####
            double s = 0.0;
            double top1 = 0.0;

            //int i =0;
            //int k =0;
            double nh = 0.0;
            int iL = 0;
            int iR = 0;
            int m = 0;
            double xL = 0.0;
            double xR = 0.0;
            double yL = 0.0;
            double hmin = 0.0;
            double hmax = 0.0;
            double Dh = 0.0; // #################input
            double filename = 0.0;
            double KALK4;
            double GLM;
            Double GLT;
            double GLM1, GLM2, GLM3, KLM, yjjk, k8, TELL, kalk2, ASUM;
            double mm, GM, GOF, YKL, GOW, TUUR, GAB, YKF, DAALO1, DUN, GOH, DUN1, SIGMA, ncount, VELL, GTO, GHH, VELC, VELR;
            int im = 0; ;
            double AREAC, AREAR, AREAl;
            Double renum, GHHTL, GHHTR, GHHTC, ESTEP, GTOPR, QUMBE, BF, GTOPL, GTOPC, GAR1, GAR2, GAR3;
            Double LAAD, RAAD, CAAD;
            Double QQR, KKKL, KKKC, KKKR, KKTOT, Qtotal, SS, QTOT, RADH;
            double[] pp = new double[1000];
            double AREATOTAL, SHEAR1, SHEAR2, SHEAR3, SHEARTOT;
            double STREAMP1, STREAMP2, STREAMP3, STREAMPTOT;
            //           CAAD = 0.0;
            //ll = channelLength;
            s = channelSlope;  // ####################################
            //nN = channelManningsValue;
            //Manning n value for differnt sections : These values have to be
            //input by the user
            /*double nNL = nN;
            double nNC = nN;
            double nNR = nN;o

            //Downstream length for different sections
            double llL = ll;
            double llR = ll;
            double llC = ll;*/

            double CONS = FTableCalculatorConstants.Cunit;
            double ACONS = FTableCalculatorConstants.Aunit;

            //ymin = 100000000.0;
            //xmin = ymin;
            //xmax = -ymin;
            Dh = heightIncrement;  // must accept by the user
            Dh = 0.5; // ###########TEMP ......REMOVE/CHNAGE LATER..............
            n = 1;

            QUMBE = 0.0;
            GHH = 0.0;
            GTO = 0.0;

            /*Determine the maximum x and y coordinates*/
            //xmax = FTableCalculatorConstants.getMaxValueFromVector(0, channelProfile);
            //xmin = FTableCalculatorConstants.getMinValueFromVector(0, channelProfile);
            //ymax = FTableCalculatorConstants.getMaxValueFromVector(1, channelProfile);
            //ymin = FTableCalculatorConstants.getMinValueFromVector(1, channelProfile);

            //System.out.print(xmax);
            //'Calculate minimum and maximum depths
            hmin = 0.0;
            hmax = ymax - ymin;

            //'Get depth increment and calculate array of depths
            //  myMessage = "The maximum depth is " + hmax.String + ".  ";
            //  myMessage = myMessage + "Enter depth increment for table:";
            //   Dh = Val(InputBox(myMessage))


            int lSize2 = 100000;
            double[] yoy = new double[lSize2];
            //double [] xjj = new double[lSize2];
            double[] xjj = new double[1000000];// sri- jul 18 2012
            //double [] yoga = new double[100000];
            double[] yoga = new double[1000000]; // sri- jul 18 2012
            double[] yoy1 = new double[lSize2];

            double[] AR = new double[lSize]; //10000

            double nnh = 0.0;
            double[] hh = new double[1000];
            double[] top_Renamed = new double[1000];
            double[] P = new double[1000];
            double[] A = new double[1000];
            double[] AC3 = new double[1000];
            double[] AL = new double[1000];
            double[] HHH = new double[1000];
            double[] VOLL1 = new double[1000];
            int[] shut = new int[1000];
            int kbj = 0;
            int k = 0;
            double dhh = 0.5;  // ################# check double/int  and to be chnaged to 0.5 ?
            int counter = 0;
            int counterxl = 0;
            int counterxr = 0;
            double f = 0;  // **************

            double[] ACUMX, ACUMY, hk1 = new double[10000];
            //double [] YK, XK, YJK, HHHH, HHH, YJK2, YJK1  = new double [1000];
            double YLLL, DHL4, daf, DFG0, DFG, DFG1, DFG2, KOL, KOL1,
                    KOL3, DFG4, DFG6, DFG3, cc3, dar;
            int DFG7 = 0;
            int mak = 0;
            int mak1 = 0;
            int nhj = 0;
            double DFG5 = 0.0;
            double GL = 0.0;
            double YDIF = 0.0;// *********************
            int DFG8 = 0;
            im = 0;
            DFG2 = 0.0;
            //mak1 =0.0;
            nnh = (hmax / dhh);

            for (i = 1; i <= nnh; i++)
            {
                hh[i] = i * dhh;
            }

            while (n <= channelProfile.Count) //profileArraySize)
            {
                n = n + 1;
            }

            n = n - 1;
            for (i = 1; i <= n; i++)
            {
                dar = y[i] - ymin;
                yoy1[i] = dar;
                y[i] = yoy1[i];
            }

            for (kbj = 1; kbj <= nnh; kbj++)
            {
                // summing up intervals to get actual depth
                YLLL = ymin + hh[kbj];

                /*Obtain points where current depth h(k)intersects x-section
        calculations of left and right bank are made separately  xL and xR */

                AR[k] = 0.0;
                A[k] = 0.0;
                P[k] = 0.0;
                AC3[k] = 0.0;
                AL[k] = 0.0;

                counter = 0;
                counterxl = 0;
                counterxr = 0;

                /* This section must be able to do multiple values if the criteria finds multiple values
                 Extenstion to divided channel is needed.*/
                double dary, dfg9, DIFF1, DIFF2, DFG12, KOL4, KOL5,
                        DIFF3, DUFFy, lul1, lul, DIFF4, KOL6, KOL9, RANK, KOL7, KOL2, dammy;

                int countery = 0;

                DIFF4 = 0.0;
                DFG12 = 0.0;
                YLLL = ymin + hh[kbj];

                for (i = 1; i <= n; i++)
                {
                    DFG6 = 0;
                    DFG1 = 0;
                    lul = 0;

                    HHH[i] = Math.Abs(i * dhh - ymax);

                    DFG3 = Math.Abs(y[i + 1] - y[i]);

                    DFG = ((x[i + 1] - x[i]) * dhh);

                    DFG4 = DFG3 * dhh;
                    DFG0 = DFG / (DFG3 + ymin); //' the program has very slight error on the loop
                    lul = (int)(DFG3);    // ########## check
                    lul1 = DFG3 - lul;
                    KOL9 = DIFF4 - dhh;
                    KOL = (int)(DFG3 / dhh);
                    KOL1 = (DFG3 / dhh);
                    KOL2 = KOL1 - KOL;
                    KOL3 = y[i] - dhh;
                    KOL5 = (int)(KOL1);
                    KOL6 = KOL5 - KOL1;
                    DIFF3 = (int)(y[i + 1]);
                    DIFF1 = (int)(y[i]);
                    DIFF2 = y[i] - DIFF1;
                    DIFF4 = y[i + 1] - DIFF3;
                    dfg9 = y[i] - y[i + 1];
                    KOL4 = DIFF2 - dhh;

                    RANK = (x[i + 1] - x[i]);
                    KOL7 = Math.Abs(DIFF1 - DIFF3);
                    DUFFy = Math.Abs(DIFF2 - DIFF4);
                    cc3 = 0;
                    dary = 0;
                    countery = 0;

                    if (y[i] >= y[i + 1])
                        dary = y[i];
                    else
                        dary = y[i + 1];

                    dammy = dary;

                    double end = dary + Dh;
                    double step = dhh;

                    int ite = (int)(end / step);

                    // for(j=dhh ;j<=dary+Dh;j=j+dhh)  // ########## check dhh value
                    for (int xy = 1; xy <= ite; xy++)
                    {
                        j = xy * dhh;
                        if (y[i] > j && y[i + 1] < j)
                            countery = countery + 1;
                        if (y[i] < j && y[i + 1] > j)
                            countery = countery + 1;    // ######### check what if == j ??
                    }

                    cc3 = countery / 2.0;

                    if (countery == 0 && DFG3 > dhh)
                        cc3 = DFG3;
                    if ((y[i] - y[i + 1]) == 0)
                        cc3 = 0;


                    int ite1 = (int)(cc3 / dhh);

                    // for(f=0; f<=cc3;f=f+dhh)  // ############ check cc3 and dhh values
                    for (int xz = 0; xz <= ite1; xz++)  // &***************
                    {
                        f = xz * dhh;
                        DFG8 = DFG8 + 1;
                        DFG7 = DFG7 + 1;

                        // CHANNEL DEPTH ADJUSTMENT

                        if (f == 0)
                            DFG5 = y[i];

                        if (f != 0 && dfg9 > 0)
                        {
                            if (f == dhh && DIFF2 == dhh)
                                DFG5 = y[i] - dhh;
                            if (f == dhh && DIFF2 > dhh)
                                DFG5 = y[i] - (DIFF2 - dhh);
                            if (f == dhh && DIFF2 < dhh && DIFF2 != 0)
                                DFG5 = y[i] - DIFF2;
                            if (f == dhh && DIFF2 == 0)
                                DFG5 = y[i] - dhh;
                            if (f != 0 && f != dhh)
                                DFG5 = DFG6 - dhh;
                        }

                        if (f != 0 && dfg9 < 0)
                        {
                            if (f == dhh && DIFF2 == dhh)
                                DFG5 = y[i] + dhh;
                            if (f == dhh && DIFF2 < dhh)
                                DFG5 = y[i] + (dhh - DIFF2);
                            if (f == dhh && DIFF2 > dhh)
                                DFG5 = y[i] + (dhh - KOL4);
                            if (f != 0 && f != dhh)
                                DFG5 = DFG6 + dhh;
                        }

                        DFG6 = DFG5;
                        yoga[DFG7] = DFG5;

                        //CHANNEL WIDTH ADJUSTMENT

                        if (f == 0)
                            DFG2 = x[i];

                        if (f != 0 && dfg9 > 0)
                        {
                            if (f == dhh && DIFF2 > dhh)
                                DFG2 = x[i] + ((KOL4) / (DFG3)) * RANK;
                            if (f == dhh && DIFF2 == dhh)
                                DFG2 = x[i] + (((dhh)) / (DFG3)) * RANK;
                            if (f == dhh && DIFF2 < dhh && DIFF2 != 0)
                                DFG2 = x[i] + (DIFF2 / DFG3) * RANK;
                            if (f == dhh && DIFF2 == 0)
                                DFG2 = x[i] + DFG / DFG3;
                            if (f > dhh)
                                DFG2 = DFG12 + (Math.Abs(f - dhh) / DFG3) * RANK;
                        }

                        if (f != 0 && dfg9 < 0)
                        {
                            if (f == dhh && DIFF2 > dhh)
                                DFG2 = x[i] + ((dhh - KOL4) / (DFG3)) * RANK;

                            if (f == dhh && DIFF2 == dhh)
                                DFG2 = x[i] + (DIFF2 / DFG3) * RANK;

                            if (f == dhh && DIFF2 < dhh && DIFF2 != 0)
                                DFG2 = x[i] + ((Math.Abs((dhh - DIFF2))) / (DFG3)) * RANK;

                            if (f == dhh && DIFF2 == 0)
                                DFG2 = x[i] + (dhh / DFG3) * RANK;

                            if (f > dhh)
                                DFG2 = DFG12 + (Math.Abs(f - dhh) / DFG3) * RANK;

                        }

                        if (f == dhh)
                            DFG12 = DFG2;

                        DFG1 = DFG1 + DFG0;
                        xjj[DFG7] = DFG2;

                        if (xmax == xjj[DFG7])
                        {
                            mak = DFG7;
                            mak1 = mak1 + 1;
                        }

                        if (mak1 == 1)
                            shut[1] = mak;

                        mak = shut[1];

                    }  // f loop

                }  // i loop

            }  // kbj loop

            double[] SAY = new double[10000];
            double[] YGT2 = new double[10000];
            double[] short2 = new double[10000];
            double[] ETOP = new double[10000];
            double[] SHORT3 = new double[10000];
            double[] Yline = new double[10000];
            double[] XXLINE = new double[10000];
            double[] Xline = new double[10000];
            double[] EYL = new double[10000];
            double[] YJJK = new double[10000];
            double[] yjun = new double[10000];
            double[] yaho = new double[10000];
            double[] short1 = new double[10000];
            double[] JET1 = new double[10000];
            double[] XLLL = new double[10000];

            Double VOLALL = 0.0;
            int dalmar = 0;
            Double QQLL = 0.0;
            Double QQCC = 0.0;
            Double QQRR = 0.0;

            double[] XLL1 = new double[10000];
            double[] XLL2 = new double[10000];
            double[] XLL3 = new double[1000];
            double[] XLM1 = new double[1000];
            double[] XLM2 = new double[1000];
            double[] XLM3 = new double[1000];
            double[] VOLL4 = new double[1000];
            double[] VOLL5 = new double[1000];
            double[] VOLL6 = new double[1000];

            for (int j1 = 1; j1 <= mak; j1++)  // ************* j chnaged to j1
            {
                Yline[j1] = yoga[j1];
                XXLINE[j1] = Math.Abs(xjj[j1 + 1] - xjj[j1]);
                Yline[mak] = yoga[mak];

                XXLINE[mak] = x[n];
            }
            nhj = (int)(hmax / dhh); //depth intervals

            for (i = 1; i <= nhj; i++)
            {
                hh[i] = i * dhh;
            }

            //   'Calculate cross-sectional properties
            int jk = 0;

            int jkend = (int)(nhj - dhh);
            //for(jk =0;jk<=nhj-dhh;jk++)
            for (jk = 0; jk <= jkend; jk++)  //**********chnaged from nhj-dhh to jkend
            {
                YDIF = (ymin + hh[jk] - ymin);
                GLM1 = 0.0;
                //ETOP[YDIF] = 0.0;
                ETOP[jk] = 0.0;
                // EYL[YDIF] = 0.0;
                EYL[jk] = 0.0;
                RRL = 0;
                RRC = 0;
                RRR = 0;
                XLL1[im] = 0.0;
                XLL2[im] = 0.0;
                XLL3[im] = 0.0;
                VOLL5[im] = 0.0;
                VOLL6[im] = 0.0;
                KKKL = 0.0;
                KKKC = 0.0;
                KKKR = 0.0;
                GHHTL = 0.0;
                GHHTR = 0.0;
                GHHTC = 0.0;
                GTOPC = 0.0;
                GTOPR = 0.0;
                GTOPL = 0.0;
                GAR1 = 0.0;
                GAR2 = 0.0;
                GAR3 = 0.0;
                GOH = 0.0;
                GL = 0.0;
                GAB = 0.0;
                DUN1 = 0.0;
                LAAD = 0.0;
                RAAD = 0.0;
                CAAD = 0.0;
                GOF = 0.0;
                GOH = 0.0;
                GAB = 0.0;
                GOW = 0.0;
                AREAl = 0.0;
                AREAC = 0.0;
                AREAR = 0.0;
                ASUM = 0.0;
                GLM1 = 0.0;

                Double dal = 0.0;

                for (im = 1; im <= mak - 1; im++)
                {

                    YJJK[im] = (Yline[im] + Yline[im + 1]) * (0.5);
                    yaho[im] = Math.Abs((ymax + dhh) - YJJK[im]) - (ymax - YDIF);
                    dal = yaho[im];
                    AA[im] = Math.Abs(XXLINE[im] * (1));
                    if (yaho[im] >= 0)
                        //yjun[YDIF] = AA[im] * yaho[im];
                        yjun[jk] = AA[im] * yaho[im];
                    else
                        // yjun[YDIF] = 0.0;
                        yjun[jk] = 0.0;
                    if (yaho[im] < 0)
                        // yjun[YDIF] = 0.0;
                        yjun[jk] = 0.0;
                    if (yaho[im] < 0)
                        yaho[im] = 0.0;
                    if (XXLINE[im] == 0)
                        yaho[im] = 0.0;

                    //GLM1 = GLM1 + yjun[YDIF];
                    GLM1 = GLM1 + yjun[jk];

                    if (Yline[im] <= YDIF + dhh && Yline[im + 1] <= YDIF + dhh)
                        //ETOP[YDIF] = ETOP[YDIF] +topp[im];
                        ETOP[jk] = ETOP[jk + 1] + topp[im];
                    pp[im] = Math.Sqrt(Math.Pow(XXLINE[im], 2) + Math.Pow(Yline[im + 1] - Yline[im], 2));

                    if (XXLINE[im] == 0)
                        pp[im] = 0;

                    if (Yline[im] <= YDIF + dhh && Yline[im + 1] <= YDIF + dhh)
                        //EYL[YDIF] = EYL[YDIF] + pp[im];
                        EYL[jk] = EYL[jk] + pp[im];

                    if (pp[im] < 0)
                        pp[im] = 0;

                    ASUM = ASUM + (XXLINE[im]);

                    if (Yline[im] <= YDIF + dhh && Yline[im + 1] <= YDIF + dhh)
                    {
                        if (XXLINE[im] == XXLINE[im + 1])
                            pp[im + 1] = pp[im];
                        if (ASUM <= LFbank)
                            GHHTL = GHHTL + pp[im];
                        if (ASUM > LFbank && ASUM <= RTBank)
                            GHHTC = GHHTC + pp[im];
                        if (ASUM > RTBank)
                            GHHTR = GHHTR + pp[im];
                    }

                    if (Yline[im] <= YDIF + dhh && Yline[im + 1] <= YDIF + dhh)
                    {
                        topp[im] = Math.Abs(XXLINE[im]);

                        if (XXLINE[im] == 0)
                            topp[im] = 0;

                        if (ASUM <= LFbank)
                            GTOPL = GTOPL + topp[im];

                        if (ASUM > LFbank && ASUM <= RTBank)
                            GTOPC = GTOPC + topp[im];

                        if (ASUM > RTBank)
                            GTOPR = GTOPR + topp[im];
                    }

                    if (Yline[im] <= YDIF + dhh && Yline[im + 1] <= YDIF + dhh)
                    {
                        if (ASUM <= LFbank)
                            //GAR1 = GAR1 +yjun[YDIF];
                            GAR1 = GAR1 + yjun[jk];

                        if (ASUM > LFbank && ASUM <= RTBank)
                            //GAR3 = GAR3 + yjun[YDIF];
                            GAR3 = GAR3 + yjun[jk];

                        if (ASUM > RTBank)
                            //GAR2 = GAR2 + yjun[YDIF];
                            GAR2 = GAR2 + yjun[jk];
                    }

                    GHH = GHHTC + GHHTR + GHHTL;
                    QUMBE = GAR1 + GAR2 + GAR3;
                    GTO = GTOPC + GTOPR + GTOPL;

                    //XLL1[YDIF + dhh] = XLL1[im] + (llL * GTOPL / ACONS);
                    XLL1[jk + 1] = XLL1[im] + (llL * GTOPL / ACONS);
                    //XLL2[YDIF + dhh] = XLL2[im] + (llC * GTOPC / ACONS);
                    XLL2[jk + 1] = XLL2[im] + (llC * GTOPC / ACONS);
                    //XLL3[YDIF + dhh] = XLL3[im] + (llR * GTOPR / ACONS);
                    XLL3[jk + 1] = XLL3[im] + (llR * GTOPR / ACONS);

                    //System.out.println("XLL" + "\t"+XLL1[YDIF + dhh]+"\t"+XLL2[YDIF + dhh]+"\t"+XLL3[YDIF + dhh]);

                }  // im loop

                LAAD = LAAD + GAR1;
                RAAD = RAAD + GAR2;
                CAAD = CAAD + GAR3;

                if (CAAD == 0)
                    RRC = 0;
                else
                    RRC = CAAD / GHHTC;

                if (LAAD == 0)
                    RRL = 0;
                else
                    RRL = LAAD / GHHTL;
                if (RAAD == 0)
                    RRR = 0;
                else
                    RRR = RAAD / GHHTR;

                RADH = (LAAD + CAAD + (RAAD)) / (GHHTL + GHHTC + GHHTR);// 'This new code calculates total hydraulic radius
                AREA = LAAD + CAAD + RAAD;
                QQL = (LAAD * Math.Pow(RRL, (2.0 / 3.0)) * CONS / nNL) * Math.Pow(s, 0.5);
                QQC = (CAAD * Math.Pow(RRC, (2.0 / 3.0)) * CONS / nNC) * Math.Pow(s, 0.5);
                QQR = (RAAD * Math.Pow(RRR, (2.0 / 3.0)) * CONS / nNR) * Math.Pow(s, 0.5);
                KKKL = (CONS / nNL) * (Math.Pow(RRL, (2.0 / 3.0)) * LAAD); // ' Conveyance
                KKKC = (CONS / nNC) * (Math.Pow(RRC, (2.0 / 3.0)) * CAAD);  // ' Conveyance
                KKKR = (CONS / nNR) * (Math.Pow(RRR, (2.0 / 3.0)) * RAAD); // ' Conveyance
                KKTOT = KKKL + KKKC + KKKR;
                Qtotal = KKTOT * Math.Pow(s, 0.5);

                QQLL = KKKL * Math.Pow(s, 0.5);
                QQCC = KKKC * Math.Pow(s, 0.5);
                QQRR = KKKR * Math.Pow(s, 0.5);

                Qtotal = QQL + QQC + QQR;

                SS = Math.Pow(Qtotal, 2) / Math.Pow(KKTOT, 2);

                ALPHA = (Math.Pow(AREA, 2) / Math.Pow(KKTOT, 3)) * ((Math.Pow(KKKL, 3) / Math.Pow(LAAD, 2)) + (Math.Pow(KKKC, 3) / Math.Pow(CAAD, 2)) + (Math.Pow(KKKR, 3) / Math.Pow(RAAD, 2)));

                if (KKKC == 0 || KKKL == 0 || KKKR == 0)
                    ALPHA = 1;

                QTOT = (AREA * Math.Pow(RADH, (2.0 / 3.0)) * CONS / nNC) * Math.Pow(s, 0.5);

                // ##################### check what value of N to be used here

                VELL = QQL / LAAD;
                VELC = QQC / CAAD;
                VELR = QQR / RAAD;

                AREAl = AREAl + (llL * GTOPL / ACONS);
                AREAC = AREAC + (llC * GTOPC / ACONS);
                AREAR = AREAR + (llR * GTOPR / ACONS);

                AREATOTAL = (AREAl + AREAC + AREAR);

                //VOLL1[YDIF + dhh] = (YDIF + dhh) * AREATOTAL * 0.5 ; //' Calculates volume column of the FTABLE
                VOLL1[jk + 1] = (YDIF + dhh) * AREATOTAL * 0.5; //' Calculates volume column of the FTABLE

                // HERE WE CALCULATE SHEAR STRESS 
                SHEAR1 = 62.4 * RRL * s;
                SHEAR2 = 62.4 * RRC * s;
                SHEAR3 = 62.4 * RRR * s;
                SHEARTOT = 62.4 * RADH * s;

                // HERE WE CALCULATE STREAM POWER
                STREAMP1 = (62.4 * QQL * s) / GTOPL;
                STREAMP2 = (62.4 * QQC * s) / GTOPC;
                STREAMP3 = (62.4 * QQR * s) / GTOPR;
                STREAMPTOT = (62.4 * QTOT * s) / GTO;

                //            q[jk] = Qtotal; // Summing the outflow from the three sections;
                //            h[jk] = YDIF+dhh;
                //            ac[jk] = AREATOTAL;
                //            System.out.println(YDIF+dhh+"\t"+ GHH+"\t"+GTO+"\t"+QUMBE+"\t"+Qtotal);

                dalmar = dalmar + 1;
                //XLM1[dalmar + 1] = XLL1[YDIF + dhh];
                XLM1[dalmar + 1] = XLL1[jk + 1];
                //XLM2[dalmar + 1]= XLL2[YDIF + dhh];
                XLM2[dalmar + 1] = XLL2[jk + 1];
                //XLM3[dalmar + 1]= XLL3[YDIF + dhh];
                XLM3[dalmar + 1] = XLL3[jk + 1];

                //VOLL4[YDIF + dhh]= VOLL4[YDIF]+ dhh * (XLL1[YDIF + dhh] + XLM1[dalmar]) * 0.5;
                VOLL4[jk + 1] = VOLL4[jk] + dhh * (XLL1[jk + 1] + XLM1[dalmar]) * 0.5;
                //VOLL5[YDIF + dhh] = VOLL5[YDIF]+ dhh * (XLL2[YDIF + dhh] + XLM2[dalmar]) * 0.5;
                VOLL5[jk + 1] = VOLL5[jk] + dhh * (XLL2[jk + 1] + XLM2[dalmar]) * 0.5;
                //VOLL6[YDIF + dhh] = VOLL6[YDIF] + dhh * (XLL3[YDIF + dhh] + XLM3[dalmar]) * 0.5;
                VOLL6[jk + 1] = VOLL6[jk] + dhh * (XLL3[jk + 1] + XLM3[dalmar]) * 0.5;

                // VOLALL = VOLL4[YDIF + dhh] + VOLL5[YDIF + dhh]+ VOLL6[YDIF + dhh];
                VOLALL = VOLL4[jk + 1] + VOLL5[jk + 1] + VOLL6[jk + 1];

                q[jk] = Qtotal;
                h[jk] = YDIF + dhh;
                ac[jk] = AREATOTAL;
                VOLL[jk] = VOLALL;

            }  // jk loop


            if (FTableCalculatorConstants.programunits == 0)
            {
                for (i = 0; i < nhj; i++)
                {
                    ac[i] = ac[i] / Math.Pow(10, 4);
                    VOLL[i] = VOLL[i] / (Math.Pow(10, 6));
                }
            }
            string lFormat = "{0:0.00}"; //makeStringForVectorResult("%.2f", 0.0)
            for (i = 0; i <= nhj; i++)
            {
                ArrayList rowData = new ArrayList();
                if (i == 0)
                {
                    //rowData.Add(makeStringForVectorResult("%.6f", 0.0));
                    //rowData.Add(makeStringForVectorResult("%.6f", 0.0));
                    //rowData.Add(makeStringForVectorResult("%.6f", 0.0));
                    //rowData.Add(makeStringForVectorResult("%.6f", 0.0));

                    string lzeros = string.Format(lFormat, 0);
                    rowData.Add(lzeros);
                    rowData.Add(lzeros);
                    rowData.Add(lzeros);
                    rowData.Add(lzeros);
                    //resultVector.add(rowData);
                }
                else
                {
                    //rowData.Add(makeStringForVectorResult("%.6f", h[i-1]));
                    //rowData.Add(makeStringForVectorResult("%.6f", ac[i-1]));
                    //rowData.Add(makeStringForVectorResult("%.6f", VOLL[i-1]));
                    //rowData.Add(makeStringForVectorResult("%.6f", q[i-1]));
                    rowData.Add(string.Format(lFormat, h[i - 1]));
                    rowData.Add(string.Format(lFormat, ac[i - 1]));
                    rowData.Add(string.Format(lFormat, VOLL[i - 1]));
                    rowData.Add(string.Format(lFormat, q[i - 1]));
                }
                resultVector.Add(rowData);
            }
            return resultVector;
        }

    }
}
