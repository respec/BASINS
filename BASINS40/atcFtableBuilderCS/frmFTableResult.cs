using atcControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace atcFtableBuilder
{
    public partial class frmFTableResult : Form
    {
        private atcControls.atcGridSource FTableSource;

        public FTableCalculator Calc = null;

        public ArrayList FTableResults = null;
        public ArrayList FTableColumnNames = null;
        public string FTableMessage = "";

        public frmFTableResult()
        {
            InitializeComponent();
            btnCopyToSpreadsheet.Click += new EventHandler(OperateFTable);
            btnCopyToUCI.Click += new EventHandler(OperateFTable);
            btnClearFTable.Click += new EventHandler(OperateFTable);
        }

        private void OperateFTable(object sender, EventArgs e)
        {
            Button lCommand = (Button)sender;
            switch (lCommand.Name)
            {
                case "btnCopyToSpreadsheet" :
                    CopyToClipboard(true);
                    break;
                case "btnCopyToUCI" :
                    CopyToUCI();
                    break;
                case "btnClearFTable" :
                    ClearFTable();
                    break;
            }
        }

        private void ClearFTable()
        {
            for (int i = 1; i < FTableSource.Rows; i++)
            {
                for (int j = 0; j < FTableSource.Columns; j++)
                {
                    FTableSource.set_CellValue(i, j, "");
                }
            }
            grdFtableResult.Refresh();
        }

        private void CopyToUCI()
        {
            StringBuilder sbf = new StringBuilder();
            string txt = "";
            //write the name of the calculator to the string buffer
            //xyang  sbf.append(" *** " + this.calculatorType + " ***\n");

            ArrayList labels = new ArrayList();
            if (Calc.CurrentType == FTableCalculator.ChannelType.TRAPEZOIDAL) //"TRAPEZOIDAL")
            {
                txt = ((FTableCalcTrape)Calc).inpChannelMaxDepth.ToString();
                string sChDepth = "*** Channel Depth: " + txt;
                txt = ((FTableCalcTrape)Calc).inpChannelTopWidth.ToString();
                string sChWidth = "*** Channel Width: " + txt;
                txt = ((FTableCalcTrape)Calc).inpChannelSideSlope.ToString();
                string sChSideSlope = "*** Channel Side Slope: " + txt;
                txt = ((FTableCalcTrape)Calc).inpChannelLength.ToString();
                string sChLength = "*** Channel Length: " + txt;

                ///*txt = plotdata.trapPanel.lblChannelMannigsValue.getText();
                //sbf.Append(" *** " + txt + "      ");
                //outvalue = plotdata.value[4];
                //sbf.Append(outvalue + "***\n");*/

                ///* txt = plotdata.trapPanel.lblChannelAvgSlope.getText();
                // sbf.Append(" *** " + txt + "      ");
                //outvalue = plotdata.value[5];
                // sbf.Append(outvalue + "***\n");*/
                
                txt = ((FTableCalcTrape)Calc).inpHeightIncrement.ToString();
                string sChHInc = "*** Height Increment: " + txt;
                labels.Add(sChLength); labels.Add(sChWidth); labels.Add(sChDepth); labels.Add(sChSideSlope); labels.Add(sChHInc);
            }
            else if (Calc.CurrentType == FTableCalculator.ChannelType.PARABOLIC) //this.calculatorType == "PARABOLIC")
            {
                txt = ((FTableCalcEllipse)Calc).inpChannelLength.ToString();
                string sChLength = "*** Channel Length: " + txt;
                txt = ((FTableCalcEllipse)Calc).inpChannelWidth.ToString();
                string sChWidth = "*** Channel Width: " + txt;
                txt = ((FTableCalcEllipse)Calc).inpChannelDepth.ToString();
                string sChDepth = "*** Channel Depth: " + txt;
                txt = ((FTableCalcEllipse)Calc).inpChannelSlope.ToString();
                string sChSlope = "*** Channel Slope: " + txt;
                txt = ((FTableCalcEllipse)Calc).inpHeightIncrement.ToString();
                string sChHInc = "*** Height Increment: " + txt;

                /*txt = plotdata.parabolicPanel.lblChannelMannigsValue.getText();
                //sbf.Append(" *** " + txt + "      ");
                //outvalue = plotdata.value[3];
                //sbf.Append(outvalue + "***\n"); */
                labels.Add(sChLength); labels.Add(sChWidth); labels.Add(sChDepth); labels.Add(sChSlope); labels.Add(sChHInc);
            }
            else if (Calc.CurrentType == FTableCalculator.ChannelType.CIRCULAR) //this.calculatorType == "CIRCULAR")
            {
                txt = ((FTableCalcCircle)Calc).inpChannelLength.ToString(); //plotdata.circPanel.lblChannelLength.getText();//xyang
                string sChLength = "*** Channel Length: " + txt; 
                txt = ((FTableCalcCircle)Calc).inpChannelDiameter.ToString();  //plotdata.circPanel.lblChannelDiameter.getText();//xyang
                string sChDiam = "*** Channel Diameter: " + txt;

                /*txt = plotdata.circPanel.lblChannelMannigsValue.getText();//xyang
                sbf.Append(" *** " + txt + "      ");
                outvalue = plotdata.value[2];
                sbf.Append(outvalue + "***\n");*/
                //txt = ((FTableCalcCircle)Calc).ChannelManningsValue.ToString();
                //string sManningsN = "*** Channel Manning's N: " + txt;

                txt = ((FTableCalcCircle)Calc).inpChannelSlope.ToString(); //plotdata.circPanel.lblChannelAvgSlope.getText();//xyang
                string sChSlope = "*** Average Channel Slope: " + txt;
                txt = ((FTableCalcCircle)Calc).inpHeightIncrement.ToString(); //plotdata.circPanel.lblIncrement.getText();//xyang
                string sChHInc = "*** Height Increment: " + txt;

                labels.Add(sChLength); labels.Add(sChDiam); labels.Add(sChSlope); labels.Add(sChHInc);
            }
            else if (Calc.CurrentType == FTableCalculator.ChannelType.RECTANGULAR)  //this.calculatorType == "RECTANGULAR")
            {
                txt = ((FTableCalcRectangular)Calc).inpChannelLength.ToString();
                string sChLength = "*** Channel Length: " + txt;
                txt = ((FTableCalcRectangular)Calc).inpChannelTopWidth.ToString();
                string sChWidth = "*** Channel Width: " + txt;
                txt = ((FTableCalcRectangular)Calc).inpChannelMaxDepth.ToString();
                string sChDepth = "*** Channel Depth: " + txt;
                txt = ((FTableCalcRectangular)Calc).inpChannelSlope.ToString();
                string sChSlope = "*** Channel Slope: " + txt;
                txt = ((FTableCalcRectangular)Calc).inpHeightIncrement.ToString();
                string sChHInc = "*** Height Increment: " + txt;

                /*txt = plotdata.parabolicPanel.lblChannelMannigsValue.getText();
                //sbf.Append(" *** " + txt + "      ");
                //outvalue = plotdata.value[3];
                //sbf.Append(outvalue + "***\n"); */

                labels.Add(sChLength); labels.Add(sChWidth); labels.Add(sChDepth); labels.Add(sChSlope); labels.Add(sChHInc);
            }
            else if (Calc.CurrentType == FTableCalculator.ChannelType.TRIANGULAR) //this.calculatorType == "TRIANGULAR")
            {
                txt = ((FTableCalcTri)Calc).inpChannelLength.ToString();
                string sChLength = "*** Channel Length: " + txt;
                txt = ((FTableCalcTri)Calc).inpChannelTopWidth.ToString();
                string sChWidth = "*** Channel Width: " + txt;
                txt = ((FTableCalcTri)Calc).inpChannelMaxDepth.ToString();
                string sChDepth = "*** Channel Depth: " + txt;
                txt = ((FTableCalcTri)Calc).inpChannelSlope.ToString();
                string sChSlope = "*** Channel Slope: " + txt;
                txt = ((FTableCalcTri)Calc).inpHeightIncrement.ToString();
                string sChHInc = "*** Height Increment: " + txt;

                /*txt = plotdata.parabolicPanel.lblChannelMannigsValue.getText();
                //sbf.Append(" *** " + txt + "      ");
                //outvalue = plotdata.value[3];
                //sbf.Append(outvalue + "***\n"); */
                labels.Add(sChLength); labels.Add(sChWidth); labels.Add(sChDepth); labels.Add(sChSlope); labels.Add(sChHInc);
            }
            else if (Calc.CurrentType == FTableCalculator.ChannelType.NATURAL)
            {
                txt = ((FTableCalcNatural)Calc).inpChannelLength.ToString();
                string sChLength = "*** Channel Length: " + txt;
                txt = ((FTableCalcNatural)Calc).inpChannelSlope.ToString();
                string sChSlope = "*** Channel Slope: " + txt;
                txt = ((FTableCalcNatural)Calc).inpHeightIncrement.ToString();
                string sChHInc = "*** Height Increment: " + txt;

                labels.Add(sChLength); labels.Add(sChSlope); labels.Add(sChHInc);
            }

            SortedList<int, object> lblLens = new SortedList<int, object>();
            int strLen = 0;
            foreach (string lstr in labels)
            {
                strLen = lstr.Length;
                if (!lblLens.Keys.Contains(strLen)) 
                    lblLens.Add(strLen, null);
            }
            int maxLblLength = lblLens.Keys[lblLens.Count - 1];
            foreach (string lstr in labels)
            {
                sbf.AppendLine(lstr.PadRight(maxLblLength) + " ***");
            }

            sbf.AppendLine("  FTABLE    999");
            //sbf.Append("  999's are placeholders for user provided ID 1-3 digits in length***");
            //sbf.Append("\n");

            sbf.AppendLine(" rows cols                               ***");
            //the following section requires me to do a calculation to correctly right justify the rows and cols figures. 

            int numrows = FTableSource.Rows;
            int numcols = FTableSource.Columns;
            sbf.AppendLine(numrows.ToString().PadLeft(5) + numcols.ToString().PadLeft(5));
            for (int i = 0; i < numcols; i++)
            {
                string colName = FTableSource.get_CellValue(0, i);
                colName = colName.Substring(0, colName.IndexOf("(")).Trim();
                sbf.Append(colName.PadLeft(10));
            }
            sbf.AppendLine(" ***");
            for (int i = 0; i < numrows; i++)
            {
                //ToDo: later see if need to only do selected cells
                if (i == 0) continue;
                for (int j = 0; j < numcols; j++)
                {
                    //jTable1.getValueAt(rowsselected[i],colsselected[j]);
                    double newValue = 0;
                    if (double.TryParse(FTableSource.get_CellValue(i, j).ToString(), out newValue))
                    {
                        if (!double.IsNaN(newValue))
                            txt = string.Format("{0:0.00}", (object)newValue);
                        else
                            txt = "0.00";
                    }
                    else
                        txt = "0.00";

                    sbf.Append(txt.PadLeft(10));
                    //if (j<numcols-1) sbf.Append("\t");
                }
                sbf.AppendLine("");
            }
            sbf.AppendLine("  END FTABLE999");

            Clipboard.Clear();
            Clipboard.SetText(sbf.ToString(), TextDataFormat.Text);
            if (clsGlobals.pFTable == null)
            {
                System.Windows.Forms.MessageBox.Show("FTable contents are ready to be pasted into a UCI", "Copy To UCI");
            }
            else
            {
                clsGlobals.pFTable.Ncols = numcols;
                clsGlobals.pFTable.Nrows = numrows-1;

                for (int i = 0; i < numrows; i++)
                {
                    if (i == 0) continue;
                    for (int j = 0; j < numcols; j++)
                    {
                        double newValue = 0.00;
                        double.TryParse(FTableSource.get_CellValue(i, j).ToString(), out newValue);
                        if (double.IsNaN(newValue)) newValue = 0.00;

                        if (j == 0) clsGlobals.pFTable.set_Depth(i, newValue);
                        if (j == 1) clsGlobals.pFTable.set_Area(i, newValue);
                        if (j == 2) clsGlobals.pFTable.set_Volume(i, newValue);
                        if (j == 3) clsGlobals.pFTable.set_Outflow1(i, newValue);
                        if (j == 4) clsGlobals.pFTable.set_Outflow2(i, newValue);
                        if (j == 5) clsGlobals.pFTable.set_Outflow3(i, newValue);
                        if (j == 6) clsGlobals.pFTable.set_Outflow4(i, newValue);
                        if (j == 7) clsGlobals.pFTable.set_Outflow5(i, newValue);
                        
                    }
                }

                System.Windows.Forms.MessageBox.Show("FTable contents have been copied to the UCI", "Copy To UCI");
            }
        }

        private void CopyToClipboard(bool aCopyAll)
        {
            string lCopyString = "";
            bool lFirstValue = true;
            for (int lRow = 0; lRow <= FTableSource.Rows - 1; lRow++)
            {
                bool lStartOfRow = true;
                for (int lCol = 0; lCol <= FTableSource.Columns - 1; lCol++)
                {
                    if (aCopyAll || FTableSource.get_CellSelected(lRow, lCol))
                    {
                        if (lFirstValue)
                        {
                            lCopyString = FTableSource.get_CellValue(lRow, lCol);
                            lFirstValue = false;
                            lStartOfRow = false;
                        }
                        else if (lStartOfRow)
                        {
                            lCopyString = lCopyString + Environment.NewLine + FTableSource.get_CellValue(lRow, lCol);
                            lStartOfRow = false;
                        }
                        else
                        {
                            lCopyString = lCopyString + "\t" + FTableSource.get_CellValue(lRow, lCol);
                        }
                    }
                }
            }
            Clipboard.Clear();
            Clipboard.SetText(lCopyString, TextDataFormat.Text);
            System.Windows.Forms.MessageBox.Show("FTable contents are ready to be pasted into an Excel spreadsheet", "Copy To Spreadsheet");
        }

        public bool PopulateFTableGrid()
        {
            bool TablePopulated = true;

            if (FTableResults == null || FTableColumnNames == null)
            {
                btnCopyToSpreadsheet.Enabled = false;
                btnCopyToUCI.Enabled = false;
                btnClearFTable.Enabled = false;
                if (string.IsNullOrEmpty(FTableMessage) && FTableMessage.Length > 0)
                    lblMessage.Text = "Note: " + FTableMessage;
                return false;
            }

            double lVal;
            FTableSource = new atcControls.atcGridSource();
            FTableSource.Columns = FTableColumnNames.Count;
            FTableSource.FixedRows = 1;
            //set header
            for (int i = 0; i < FTableColumnNames.Count; i++)
            {
                FTableSource.set_CellValue(0, i, FTableColumnNames[i].ToString());
            }
            ArrayList lRow = null;
            int lgrdRow = 1;
            string lFormat = "{0:0.000000}";
            for (int i = 0; i < FTableResults.Count; i++)
            {
                lRow = (ArrayList)FTableResults[i]; //get a FTable row
                for (int c = 0; c < lRow.Count; c++)
                {
                    if (double.TryParse(lRow[c].ToString(), out lVal))
                    {
                        FTableSource.set_CellValue(lgrdRow, c, string.Format(lFormat, (object)lVal));
                    }
                    else
                    {
                        FTableSource.set_CellValue(lgrdRow, c, "0.0");
                    }
                }
                lgrdRow++;
            }

            grdFtableResult.Initialize(FTableSource);
            grdFtableResult.SizeAllColumnsToContents();
            grdFtableResult.Refresh();

            if (string.IsNullOrEmpty(FTableMessage))
            {
                lblMessage.Text = "Note: " + FTableMessage;

                //jTableMessageLabel.setText(newResultMessage);
                //jTable.setVisible(true);
                //// set the data vector in the jTable.    	   
                //dfm.setDataVector(lFtableResultChannel, colNames);

                string lMsg = "";
                double lmaxVol = FTableCalculator.CalculateMaxVolume(FTableResults, out lMsg);
                if (lmaxVol > 0)
                    lblMessage.Text = "Note:\n" + lMsg;
            }
            else
            {
                //this is for Green tool that has infiltration influenced maxVol that is preset
                lblMessage.Text = FTableMessage;
            }

            //modifyTableForInfiltration(data, colNames);
            //sri-09-11-2012
            /* The code below checks if any column has continuous zero values resulted becauses of depth
              * increment mismatch between channel depth and control device depth. If such a situation exist
              in any one of the control devices column the table is not displayed and a error message pops up
              */
            //TableColumnModel clm = jTable.getColumnModel();
            string cdevice = null;
            //         if(colNames.get(3).toString().trim().startsWith("outflow")==false)  // i.e no control device used

            //sri-09-11-2012
            // if control devices used - column zero values check- depth increment check
            if (FTableColumnNames.Count > 3)
            {
                int colcheck = 0;
                double lEpslon = 0.0000001;
                for (int col = 3; col < ((ArrayList)FTableResults[0]).Count; col++)
                {
                    int count = 0;
                    for (int row = 0; row < FTableResults.Count; row++)
                    {
                        lRow = (ArrayList)FTableResults[row];
                        double.TryParse(lRow[col].ToString(), out lVal);

                        if (Math.Abs(lVal - 0.0) < lEpslon)
                            count = count + 1;
                    }

                    if (count == FTableResults.Count)
                    {
                        colcheck = colcheck + 1;
                        if (cdevice == null)
                            cdevice = FTableColumnNames[col].ToString().Trim();
                        else
                            cdevice = cdevice + " , " + FTableColumnNames[col].ToString().Trim(); ;
                    }
                }

                if (colcheck == 0 || cdevice.ToLower().Contains("infiltration"))
                    grdFtableResult.Visible = true;
                else
                {
                    TablePopulated = false;
                    grdFtableResult.Visible = false;
                    lblMessage.Text = "Note:\n" +
                        "ERROR-Depth Increment Mismatch Between Channel and Control Device/s\n" +
                        "Make sure that the depth increment is a multiple of the invert level";
                }

            }  // end- column zero values check- depth increment check
            else
            {
                grdFtableResult.Visible = true;
            }

            bool lOK = grdFtableResult.Visible;
            btnCopyToSpreadsheet.Enabled = lOK;
            btnCopyToUCI.Enabled = lOK;
            btnClearFTable.Enabled = lOK;

            return TablePopulated;
        }

        private void frmFTableResult_Load(object sender, EventArgs e)
        {
            PopulateFTableGrid();
        }
    }
}
