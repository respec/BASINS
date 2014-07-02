using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    class FTableCalcOCOrificeRiser : FTableCalculator, IFTableOperationsOC
    {
        public double OrificePipeDiameter = -999;
        public double pOrificeDepth = -999;
        public double OrificeDepth
        {
            get { return pOrificeDepth; }
            set { pOrificeDepth = value; }
        }
        public double OrificeDischargeCoefficient = -999;

        public FTableCalcOCOrificeRiser()
        {
            vectorColNames.Clear();
            vectorColNames.Add("DEPTH");
            vectorColNames.Add("AREA");
            vectorColNames.Add("VOLUME");
            vectorColNames.Add("OUTFLOW");
        }

        public ArrayList GenerateFTableOC()
        {
            return GenerateFTable(OrificePipeDiameter, OrificeDepth, OrificeDischargeCoefficient);
        }

        public ArrayList GenerateFTable(double PipeDiameter,
                                     double WeirinvertValue, double DischargeCoefficient)
        {
            ArrayList vectorRowData = new ArrayList();
            //Flow Area Calculations
            //double L = channelLength;
            double N = WeirinvertValue;
            double DT = PipeDiameter;
            //double S = longitudalChannelSlope;
            //double w = topChannelWidth;		      

            double Co = DischargeCoefficient;

            ArrayList row = new ArrayList();
            double Area = 0.0;
            double QC = 0.0;
            double acr = 0.0;
            double stot = 0.0;
            double QO = 0.0;
            double QV = 0.0;
            double prevAcr = 0.0; ;
            double prevStot = 0.0;
            double R1 = 0.0;
            double[] QW;
            QW = new double[1000];

            string sDepth = "";
            string sArea = "";
            string sVolume = "";
            string sOutFlow = "";
            string lFormat = "{0:0.00000}";
            for (double g = N; g < 100; g += FTableCalculatorConstants.calculatorIncrement)
            {
                if (g > N)
                {
                    R1 = g - N;
                }
                Area = 3.14159 * (Math.Pow(DT, 2) / 4);
                if (FTableCalculatorConstants.programunits == 1)  // if units selected are metric cfs
                {
                  //QO = 0.6* Area * Math.Pow(64.4, 0.5) * Math.Pow(R1, 0.5);
                    QO = Co * Area * Math.Pow(64.4, 0.5) * Math.Pow(R1, 0.5);  //sri
                }

                if (FTableCalculatorConstants.programunits == 0)  // if units selected are metric  cms
                {
                    //sqrt(2*acc due to gravity)
                  //QO = 0.6* Area * Math.Pow(19.62, 0.5) * Math.Pow(R1, 0.5);
                    QO = Co * Area * Math.Pow(19.62, 0.5) * Math.Pow(R1, 0.5);  //sri
                }

                //QW(g)= QO;
                //Co = 3.087 (US)
                QV = Co * (DT * 3.14159) * Math.Pow(R1, 1.5);

                //If QO < QV then QC = QO else QC = QV;   
                if (QO < QV)
                {
                    QC = QO;
                }
                else
                {
                    QC = QV;
                }

                prevStot = stot;
                prevAcr = acr;

                row = new ArrayList();

                sDepth = string.Format(lFormat, (object)g);
                sArea = string.Format(lFormat, (object)acr);
                sVolume = string.Format(lFormat, (object)stot);
                sOutFlow = string.Format(lFormat, (object)QC);
                row.Add(sDepth);
                row.Add(sArea);
                row.Add(sVolume);
                row.Add(sOutFlow);

                vectorRowData.Add(row);
            }
            return vectorRowData;
        }
    }
}
