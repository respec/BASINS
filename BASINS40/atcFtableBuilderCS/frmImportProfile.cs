using System;
using System.IO;
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
    public partial class frmImportProfile : Form
    {
        public EventHandler<NewProfileEventArgs> NewProfileDataRead;

        private StreamReader pSR = null;
        private string NoticeText = "Import station-elevation or station-depth data from spreadsheet (tab-delimited) or a comma-delimited file." + Environment.NewLine +
            "Data Formatting Rules:" + Environment.NewLine +
            "- Only the first two columns in data files are used, the rest are ignored." + Environment.NewLine +
            "- First column is the X (distance from left bank) and second column is Y (either elevation or depth)" + Environment.NewLine +
            "- If the data are elevations, then it will be converted into depths as shown by the diagram" + Environment.NewLine +
            "- Profile data should be in either meter (SI Unit) or feet (US Unit) and users need to specify below" + Environment.NewLine +
            "- The profile data will be automatically converted to have the same unit system as the selected unit on the main form";
        public frmImportProfile()
        {
            InitializeComponent();
        }

        private void frmImportProfile_Load(object sender, EventArgs e)
        {
            lblNotice.Text = NoticeText;

            //make some decision for the user here
            rdoImportDataExcel.Checked = true;
            if (FTableCalculatorConstants.programunits == (int)FTableCalculatorConstants.UnitSystem.SI)
                rdoImportDataUnitSI.Checked = true;
            else if (FTableCalculatorConstants.programunits == (int)FTableCalculatorConstants.UnitSystem.US)
                rdoImportDataUnitUS.Checked = true;

            rdoDataElev.Checked = true;
        }

        protected virtual void OnNewProfileRead(NewProfileEventArgs e)
        {
            EventHandler<NewProfileEventArgs> handler = NewProfileDataRead;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RaiseNewProfileEvent(bool aReadSuccess, int aNumStations )
        {
            NewProfileEventArgs args = new NewProfileEventArgs();
            args.ReadSuccess = aReadSuccess;
            args.numStations = aNumStations;
            OnNewProfileRead(args);  
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string lFilename = txtChProfileDatafile.Text.Trim();

            if (string.IsNullOrEmpty(lFilename) || !System.IO.File.Exists(lFilename))
                return;
            if ((rdoImportDataCSV.Checked || rdoImportDataExcel.Checked) &&
                (rdoImportDataUnitSI.Checked || rdoImportDataUnitUS.Checked) &&
                (rdoDataDepth.Checked || rdoDataElev.Checked))
            {
                if (rdoImportDataUnitSI.Checked)
                    clsGlobals.gProfileDataUnit = FTableCalculatorConstants.UnitSystem.SI;
                else if (rdoImportDataUnitUS.Checked)
                    clsGlobals.gProfileDataUnit = FTableCalculatorConstants.UnitSystem.US;

                //if (rdoDataDepth.Checked)
                //    clsGlobals.gProfileDataType = clsGlobals.ProfileDataType.Depth;
                //else if (rdoDataElev.Checked)
                //    clsGlobals.gProfileDataType = clsGlobals.ProfileDataType.Elevation;
            }
            else
            {
                return;
            }

            try
            {
                pSR = new StreamReader(lFilename);
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show("Unrecoganized file format:" + Environment.NewLine + lFilename, "Open Import Data File");
                return;
            }

            bool fileImportOK;
            char lDelim = ',';
            if (rdoImportDataCSV.Checked)
                lDelim = ',';
            else if (rdoImportDataExcel.Checked)
                lDelim = '\t';

            fileImportOK = ImportDataFile(lDelim);
            RaiseNewProfileEvent(fileImportOK, clsGlobals.gProfileStations.Count);
        }

        private bool ImportDataFile(char aDelim)
        {
            if (pSR == null) return false;
            bool fileImportOK = true;
            string lOneLine = "";
            string[] lArr = null;
            double lValx = 0;
            double lValy = 0;
            ArrayList lprofileStations = new ArrayList();
            while (!pSR.EndOfStream)
            {
                lOneLine = pSR.ReadLine();
                lArr = lOneLine.Split(new char[] { aDelim }, StringSplitOptions.None);
                if (lArr.Length < 2)
                {
                    System.Windows.Forms.MessageBox.Show("Data File Contains less than 2 columns of data:" + Environment.NewLine + ((FileStream)pSR.BaseStream).Name, "Open Import Data File");
                    break;
                }

                if (double.TryParse(lArr[0], out lValx) && double.TryParse(lArr[1], out lValy))
                {
                    XSectionStation lstation = new XSectionStation();
                    lstation.x = lValx;
                    lstation.y = lValy;
                    lprofileStations.Add(lstation);
                }
                else
                {
                    //lstation.x = double.NaN;
                    //lstation.y = double.NaN;
                    //bypass this row of data including header lines;
                }
            }

            pSR.Close();
            pSR.Dispose();
            pSR = null;

            if (fileImportOK)
            {
                if (rdoDataElev.Checked)
                    ConvertToDepth(lprofileStations);
                if (rdoImportDataUnitSI.Checked && FTableCalculatorConstants.programunits == 1)
                    ConvertUnit(lprofileStations, FTableCalculatorConstants.UnitSystem.SI, FTableCalculatorConstants.UnitSystem.US);
                else if (rdoImportDataUnitUS.Checked && FTableCalculatorConstants.programunits == 0)
                    ConvertUnit(lprofileStations, FTableCalculatorConstants.UnitSystem.US, FTableCalculatorConstants.UnitSystem.SI);

                if (clsGlobals.gProfileStations != null)
                {
                    clsGlobals.gProfileStations.Clear();
                    clsGlobals.gProfileStations = null;
                }
                clsGlobals.gProfileStations = new ArrayList();
                clsGlobals.gProfileStations.AddRange(lprofileStations);
            }
            return fileImportOK;
        }

        private void ConvertToDepth(ArrayList aProfileStations)
        {
            //Convert to depth
            double minElevation = double.MaxValue;
            foreach (XSectionStation lStation in aProfileStations)
            {
                if (lStation.y < minElevation)
                    minElevation = lStation.y;
            }
            foreach (XSectionStation lStation in aProfileStations)
            {
                lStation.y -= minElevation;
            }
        }

        private void ConvertUnit(ArrayList aProfileStations, FTableCalculatorConstants.UnitSystem aFromUnit, FTableCalculatorConstants.UnitSystem aToUnit)
        {
            if (aFromUnit == aToUnit)
                return;

            double lConvMultiplier = 1;
            if (aFromUnit == FTableCalculatorConstants.UnitSystem.SI &&
                aToUnit == FTableCalculatorConstants.UnitSystem.US)
            {
                lConvMultiplier = 3.28084; // 1 meter = 3.28084 feet
            }
            else if (
              aFromUnit == FTableCalculatorConstants.UnitSystem.US &&
              aToUnit == FTableCalculatorConstants.UnitSystem.SI)
            {
                lConvMultiplier = 0.3048; // 1 foot = 0.3048 meter
            }
            foreach (XSectionStation lStation in aProfileStations)
            {
                lStation.x *= lConvMultiplier;
                lStation.y *= lConvMultiplier;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string lFullPath = Properties.Settings.Default.ChannelProfileFilename;
            string lPath = "";
            string lFilename = "";
            if (!string.IsNullOrEmpty(lFullPath))
            {
                lPath = System.IO.Path.GetDirectoryName(lFullPath);
                lFilename = System.IO.Path.GetFileName(lFullPath);
            }

            OpenFileDialog lFD = new OpenFileDialog();
            lFD.Title = "Select X-section Profile Data File";
            if (System.IO.Directory.Exists(lPath))
                lFD.InitialDirectory = lPath;
            else
                lFD.InitialDirectory = "C:";

            if (!string.IsNullOrEmpty(lFilename))
                lFD.FileName = lFilename;
            else
                lFD.FileName = "Profile";

            //lFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (rdoImportDataExcel.Checked)
            {
                lFD.DefaultExt = "txt";
                lFD.FileName += ".txt";
                lFD.Filter = "Tab-Delimited (*.txt)|*.txt|All files (*.*)|*.*";
            }
            else if (rdoImportDataCSV.Checked)
            {
                lFD.DefaultExt = ".csv";
                lFD.FileName += ".csv";
                lFD.Filter = "Comma-Delimited (*.csv)|*.csv|All files (*.*)|*.*";
            }
            if (lFD.ShowDialog() == DialogResult.OK)
            {
                txtChProfileDatafile.Text = lFD.FileName;
                Properties.Settings.Default.ChannelProfileFilename = lFD.FileName;
                Properties.Settings.Default.Save();
            }
        }
    }

    public class NewProfileEventArgs : EventArgs
    {
        public bool ReadSuccess;
        public int numStations;
    }
}
