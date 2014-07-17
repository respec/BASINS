using atcUtility;
using System.Collections;
using System.Collections.Generic;
// the final returned values will be in in/hr

public class Infiltration_Calc
{
    //methods in this class emulate VB code received from client
    private double result;
    private double drainTime;
    //    private double  Var2;

    private double fttocm = 2.54d * 12d;

    private double cmtoin = 1.0d / 2.54d;
    //private Map<string,string> soilprop = new HashMap<string, string>();
    private Dictionary<string, string> soilprop = new Dictionary<string, string>();
    static ArrayList tablerows = new ArrayList();
    //make soilpropfile look like a file
    static string soilpropfile =
    "Soil Type,Suction Head (in or cm),Pore Size Distribution,Porosity,Residual Water Content,Effective Porosity,Hydraulic Head (cm),Hydraulic Conductivity (in/hr or cm/hr),Typical Water Capacity,Typical Infilteration Rate (in/hr),Hydrologic Soil Group\n" +
    "Sand,4.95,0.69,0.437,0.02,0.417,4.95,11.78,0.35,8.27,A\n" +
    "Loamy Sand,6.13,0.553,0.437,0.035,0.401,6.13,2.99,0.31,2.41,A\n" +
    "Sand Loam,11.01,0.378,0.453,0.041,0.412,11.01,1.09,0.25,1.02,B\n" +
    "Loam,8.89,0.252,0.463,0.027,0.434,8.89,0.34,0.19,0.52,B\n" +
    "Silty Loam,16.68,0.234,0.501,0.015,0.486,16.68,0.65,0.17,0.27,C\n" +
    "Sandy Clay Loam,21.85,0.319,0.398,0.068,0.33,21.85,0.15,0.14,0.17,C\n" +
    "Clay Loam,20.88,0.242,0.464,0.075,0.309,20.88,0.1,0.14,0.09,D\n" +
    "Silty Clay Loam,27.30,0.177,0.471,0.04,0.432,27.3,0.1,0.11,0.06,D\n" +
    "Sandy Clay,23.90,0.223,0.43,0.109,0.321,23.9,0.06,0.09,0.05,D\n" +
    "Silty Clay,29.22,0.15,0.479,0.0056,0.423,29.22,0.05,0.09,0.04,D\n" +
    "Clay,31.63,0.165,0.475,0.09,0.385,31.63,0.03,0.08,0.02,D\n";

    static string soilpropfile_eng =
    "Soil Type,Suction Head (in or cm),Pore Size Distribution,Porosity,Residual Water Content,Effective Porosity,Hydraulic Head (cm),Hydraulic Conductivity (in/hr or cm/hr),Typical Water Capacity,Typical Infilteration Rate (in/hr),Hydrologic Soil Group\n" +
    "Sand,1.95,0.69,0.437,0.02,0.417,4.95,4.638,0.35,8.27,A\n" +
    "Loamy Sand,2.41,0.553,0.437,0.035,0.401,6.13,1.177,0.31,2.41,A\n" +
    "Sand Loam,4.33,0.378,0.453,0.041,0.412,11.01,0.429,0.25,1.02,B\n" +
    "Loam,3.5,0.252,0.463,0.027,0.434,8.89,0.134,0.19,0.52,B\n" +
    "Silty Loam,6.57,0.234,0.501,0.015,0.486,16.68,0.256,0.17,0.27,C\n" +
    "Sandy Clay Loam,8.6,0.319,0.398,0.068,0.33,21.85,0.059,0.14,0.17,C\n" +
    "Clay Loam,8.22,0.242,0.464,0.075,0.309,20.88,0.039,0.14,0.09,D\n" +
    "Silty Clay Loam,10.75,0.177,0.471,0.04,0.432,27.3,0.039,0.11,0.06,D\n" +
    "Sandy Clay,9.41,0.223,0.43,0.109,0.321,23.9,0.024,0.09,0.05,D\n" +
    "Silty Clay,11.5,0.15,0.479,0.0056,0.423,29.22,0.02,0.09,0.04,D\n" +
    "Clay,12.45,0.165,0.475,0.09,0.385,31.63,0.012,0.08,0.02,D\n";

    //holder of Infiltration calculation results
    public static double ResultInfiltrationRate;
    public static double ResultInfiltrationDepth;
    public static double ResultInfiltrationDrainTime;

    public enum GAMPPARAM
    {
        ResWater,
        InitWater,
        Porosity,
        HydCond,
        SuctionHead,
        EffPorosity,
        SoilDepth
    }
    public enum INFILCALCMETHODS
    {
        MARYLANDLOOKUP,
        GREENAMPT
    }

    public Infiltration_Calc()
    {
        // TODO Auto-generated constructor stub
        //maybe pass hash values here, check for valid user input here, save as class variables 
    }

    public Dictionary<string, string> MDMethod(string soiltype)
    {
        //table lookup for soil properties and infiltration rate
        tablerows = readCSVsoil_prop(); //read the file (if necessary)
        soilprop = getsoil(soiltype);	//find the soil properties
        return soilprop; 	//return hashtable of soil properties
    }

    private Dictionary<string, string> getsoil(string soiltype)
    {
        // return a hashtable of soil properties for passed soiltype
        ArrayList v = new ArrayList();
        ArrayList values = new ArrayList();
        IEnumerator e1 = tablerows.GetEnumerator();

        //find the vector with the correct soiltype
        while (e1.MoveNext())
        {
            v = (ArrayList)e1.Current;
            IEnumerator e2 = v.GetEnumerator();
            while (e2.MoveNext())
            {
                string soil = e2.Current.ToString();
                if (soil.ToLower().Equals(soiltype.ToLower()))
                {
                    //TODO: System.out.println(soil + " matches " + soiltype);
                    values = v;
                    break;
                }
                else { break; }
            }
            if (values.Count > 0)
                break;
        }
        //build and return the hashtable
        ArrayList keys = (ArrayList)tablerows[0];
        IEnumerator ekeys = keys.GetEnumerator();
        IEnumerator evals = values.GetEnumerator();
        //Assuming the evals and ekeys are one-to-one
        if (soilprop != null) soilprop.Clear();
        while (evals.MoveNext())
        {
            if (ekeys.MoveNext())
            {
                soilprop.Add((string)ekeys.Current, (string)evals.Current);
            }
        }

        /*
		for (Enumeration evals = values.elements(); evals.hasMoreElements();)
        {
			soilprop.put((string)ekeys.nextElement(), (string)evals.nextElement());
		}
        */
        return soilprop;
    }

    private ArrayList readCSVsoil_prop()
    {
        if (tablerows == null) tablerows = new ArrayList();
        ArrayList vals;
        //read soil properties into memory
        string lSoilTableString = "";
        if (FTableCalculatorConstants.unitssel == 0)  //sri
            lSoilTableString = soilpropfile;
        else if (FTableCalculatorConstants.unitssel == 1)  // sri
            lSoilTableString = soilpropfile_eng;

        string[] lSoilPropLines = lSoilTableString.Split(new char[] { '\n' }, System.StringSplitOptions.None);
        if (lSoilPropLines == null || lSoilPropLines.Length == 0)
        {
            return null;
        }

        bool lAddnew = true;
        if (tablerows.Count > 0) lAddnew = false;

        try
        {
            if (lSoilPropLines.Length > 0)
            {
                IEnumerator lSoilPropEnum = lSoilPropLines.GetEnumerator();
                string[] lSoilRecord = null;
                int lSoilTabRowIndex = 0;
                while (lSoilPropEnum.MoveNext())
                {
                    if (!(lSoilPropEnum.Current.ToString().Trim() == ""))
                    {
                        lSoilRecord = lSoilPropEnum.Current.ToString().Split(new char[] { ',' }, System.StringSplitOptions.None);

                        vals = new ArrayList();
                        for (int i = 0; i < lSoilRecord.Length; i++)
                            vals.Add(lSoilRecord[i]);

                        if (lAddnew)
                            tablerows.Add(vals);
                        else
                            tablerows[lSoilTabRowIndex++] = vals;
                    }
                }
            }
        }
        catch (System.IO.FileNotFoundException e)
        {
            // TODO Auto-generated catch block
            System.Windows.Forms.MessageBox.Show(e.StackTrace);
        }
        catch (System.IO.IOException e)
        {
            // TODO Auto-generated catch block
            System.Windows.Forms.MessageBox.Show(e.StackTrace);
        }
        catch (System.Exception e)
        {
            System.Windows.Forms.MessageBox.Show(e.StackTrace);
        }
        finally
        {
            if (lSoilPropLines != null)
            {
                lSoilPropLines = null;
            }
        }
        return tablerows;
    }

    public double[] GreemAmptMethod(Dictionary<GAMPPARAM, double> values)
    {
        //estimate infiltration rate from soil hydraulic properties
        //hash values are user input and/or table lookup from form
        //double [] pond = new double [100];
        try
        {
            double resH2Omin = 0; //residual water content
            double H2Ocontent = 0; //water content
            double satH2Omax = 0; //saturated water content aka porosity
            double satHydraulicCond = 0; //saturated hydraulic conductivity
            double suctionHead = 0; //bubbling pressure
            double effporosity = 0; //effective porosity
            double soilDepth = 0; //max layer depth (cm)
            //double pondedDepth      = 0; //ponding depth

            double lVal = 0.0;
            /* Tong, a new way below
        if (values.TryGetValue("Residual Water Content", out lVal)) resH2Omin = double.Parse(lVal);
		if (values.TryGetValue("Initial Water Content", out lVal)) H2Ocontent = double.Parse(lVal);
		if (values.TryGetValue("Porosity", out lVal)) satH2Omax = double.Parse(lVal);
        if (values.TryGetValue("Hydraulic Conductivity (in/hr or cm/hr)", out lVal)) satHydraulicCond = double.Parse(lVal);
        if (values.TryGetValue("Suction Head (in or cm)", out lVal)) suctionHead = double.Parse(lVal);
        if (values.TryGetValue("Effective Porosity", out lVal)) effporosity = double.Parse(lVal);
        if (values.TryGetValue("Underlaying Soil Depth (mts or feet)", out lVal)) soilDepth = double.Parse(lVal);
      //if (values.TryGetValue("Ponding Depth (ft)", out lVal)) pondedDepth = double.Parse(lVal);
            */

            if (values.TryGetValue(GAMPPARAM.ResWater, out lVal)) resH2Omin = lVal;
            if (values.TryGetValue(GAMPPARAM.InitWater, out lVal)) H2Ocontent = lVal;
            if (values.TryGetValue(GAMPPARAM.Porosity, out lVal)) satH2Omax = lVal;
            if (values.TryGetValue(GAMPPARAM.HydCond, out lVal)) satHydraulicCond = lVal;
            if (values.TryGetValue(GAMPPARAM.SuctionHead, out lVal)) suctionHead = lVal;
            if (values.TryGetValue(GAMPPARAM.EffPorosity, out lVal)) effporosity = lVal;
            if (values.TryGetValue(GAMPPARAM.SoilDepth, out lVal)) soilDepth = lVal;
            //if (values.TryGetValue(GAMPPARAM.PondingDepth, out lVal)) pondedDepth     = lVal;

            //pond[99] = pondedDepth;
            double effSaturation = (H2Ocontent - resH2Omin) / (satH2Omax - resH2Omin);
            int ctr = 0;
            double sum = 0.0d;
            double sum1 = 0.0d;
            double Ftotal = 0.0d;
            double td = 0.0d;
            double time = 0.0d;
            double mj = 0.0d;
            double fj, fr, frate, f;

            double[] retVals = new double[3];
            if (FTableCalculatorConstants.unitssel == 0) //BMPTypePanel.cmbunits.getSelectedIndex()==0)  //sri -SI
            {
                fttocm = 100d;  // convert depth meters to cm
                //cmtoin = 1d/39.37007d;  // meters to inches
            }
            //       if(BMPTypePanel.cmbunits.getSelectedIndex()==1)  //sri- english
            //       {
            //              suctionHead = suctionHead/2.54;  // inches to cm
            //              satHydraulicCond = satHydraulicCond *2.54; //inches to cm/hr
            //
            //       }

            //this needs some error checking or restructuring of algorithm
            for (double depth = .1d; depth < soilDepth + .01d; depth = depth + .01d)
            {
                mj = System.Math.Log((1.0d + (depth * fttocm / suctionHead)));  //cm
                td = ((fttocm * depth * (1.0d - effSaturation) * effporosity) - suctionHead * (1.0d - effSaturation) * effporosity * mj) / satHydraulicCond;
                frate = satHydraulicCond * ((depth * fttocm + suctionHead) / (depth * fttocm));
                //			fj = fttocm * depth * (1.0d-effSaturation) * effporosity;
                //			fr = (suctionHead * (1.0d-effSaturation) * effporosity) + (fttocm * depth * (1.0d-effSaturation) * effporosity);
                //			f = satHydraulicCond * (1.0d+fr) / fj;
                sum += frate;

                //Ftotal = (soilDepth*(1-effSaturation)*effporosity*fttocm)/2.54;
                Ftotal = (soilDepth * (1 - effSaturation) * effporosity * fttocm);  //sri
                //TODO: System.out.println("     ");  
                //TODO: System.out.print(Ftotal);    
                drainTime = td;
                //TODO: System.out.println("     ");  
                //TODO: System.out.print(td);
                // return MyVar2;
            }

            //result = sum / (soilDepth / .01d);
            result = sum / (soilDepth * fttocm);  // cm/hr - check above formula
            //retVals[0] = result*cmtoin;  // inches per hour
            retVals[0] = result;  // 
            retVals[1] = Ftotal;  // sri
            retVals[2] = drainTime;
            //TODO: System.out.println("     ");  
            //TODO: System.out.print(result);
            return retVals;
        }
        catch (System.FormatException converterr)
        { //missing or invalid input
            System.Windows.Forms.MessageBox.Show(converterr.StackTrace);
            //return -1; //what to return?       
            return null; //what to return?
        }
    }

    // conversion options not added since this method was not used
    double vanGenuchtenMethod(Dictionary<string, string> values)
    {
        try
        {
            double resH2Omin = 0; // residual water content
            double H2Ocontent = 0; // water content
            double satH2Omax = 0; // porosity
            double satHydraulicCond = 0; // saturated hydraulic conductivity
            double suctionHead = 0; // bubbling pressure
            double poreSizeIndex = 0; // pore size index values 

            string lVal = "";
            if (values.TryGetValue("Residual Water Content", out lVal))
                resH2Omin = double.Parse(lVal);

            if (values.TryGetValue("Initial Water Content", out lVal))
                H2Ocontent = double.Parse(lVal);

            if (values.TryGetValue("Porosity", out lVal))
                satH2Omax = double.Parse(lVal);

            if (values.TryGetValue("Hydraulic Conductivity (cm/hr)", out lVal))
                satHydraulicCond = double.Parse(lVal);

            if (values.TryGetValue("Suction Head (cm)", out lVal))
                suctionHead = double.Parse(lVal);

            if (values.TryGetValue("Pore Size Distribution", out lVal))
                poreSizeIndex = double.Parse(lVal);

            //double empcnst1 = 1.0d / suctionHead;
            double empcnstn = poreSizeIndex + 1.0d;
            double empcnstm = poreSizeIndex / empcnstn;
            //double gradient = 1.0d;
            double effSat = (H2Ocontent - resH2Omin) / (satH2Omax - resH2Omin);
            if (effSat > 1.0d) { effSat = 1.0d; }
            double a, b, c, d, q;
            //vb code is:  
            //matric = ((1 / effSat ^ (1 / empcnstm)) - 1) ^ (1 / empcnstn) * suctionHead
            //near as I can tell the translation is:
            a = 1.0d / System.Math.Pow(effSat, (1.0d / empcnstm)) - 1.0d;
            b = 1.0d / effSat;
            double matric = System.Math.Pow(a, b) * suctionHead;
            //and here is yet more of the same:
            //k = satHydraulicCon * effSat ^ 0.5 * ((1 - (1 - effSat ^ (1 / empcnstm)) ^ empcnstm)) ^ 2
            a = 1.0d - System.Math.Pow(effSat, (1.0d / empcnstm));
            b = 1.0d - System.Math.Pow(a, empcnstm);
            c = System.Math.Pow(b, 2.0d);
            d = satHydraulicCond * System.Math.Pow(effSat, 0.5d) * c;
            //q = d * (matric + wettingFrontDepth * fttocm) / (wettingFrontDepth * fttocm);
            return d * cmtoin;
        }
        catch (System.FormatException converterr)
        { //missing or invalid input
            //System.Windows.Forms.MessageBox.Show(converterr.StackTrace);
            return -1; //what to return?
        }
        //	should be a math error catch block as well
    }
}
