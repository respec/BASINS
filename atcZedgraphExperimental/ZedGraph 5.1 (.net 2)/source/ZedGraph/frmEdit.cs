using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZedGraph
{
    /// <summary>
    /// Form for the user to edit a GraphPane
    /// </summary>
    public partial class frmEdit : Form
    {
        /// <summary>
        /// event fired when user presses "Apply" button
        /// </summary>
        public event System.EventHandler Apply;

        private GraphPane pPane;

        /// <summary>
        /// Default constructor of frmEdit
        /// </summary>
        public frmEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show this form for editing the specified GraphPane
        /// </summary>
        /// <param name="aPane">pane to edit</param>
        public void Edit(GraphPane aPane)
        {
            pPane = aPane;
            cboWhichCurve.Items.Clear();
            foreach (CurveItem lCurve in pPane.CurveList)
            {
                cboWhichCurve.Items.Add(lCurve.Label.Text);
            }
            SetControlsFromPane();
            this.Show();
        }

        private void SetControlsFromPane()
        {
            cboWhichAxis.SelectedIndex = 0;
            cboWhichCurve.SelectedIndex = 0;
            SetControlsFromAxis(AxisFromCombo());
            if (pPane.CurveList.Count > 0)
            {
                SetControlsFromCurve(pPane.CurveList[0]);
            }
        }

        private Axis AxisFromCombo()
        {
            switch (cboWhichAxis.SelectedIndex)
            {
                case 0: return pPane.XAxis;
                case 1: return pPane.YAxis;
                case 2: return pPane.Y2Axis;
                default: throw new ApplicationException("Axis type not yet supported: " + cboWhichAxis.Text);
            }
        }

        private void SetControlsFromAxis(Axis aAxis)
        {
            if (aAxis != null)
            {
                txtAxisLabel.Text = aAxis.Title.Text;
                txtAxisDisplayMinimum.Text = aAxis.Scale.Min.ToString();
                txtAxisDisplayMaximum.Text = aAxis.Scale.Max.ToString();
                switch (aAxis.Type)
                {
                    case AxisType.DateMulti:
                        cboAxisType.SelectedIndex = 0;
                        txtAxisDisplayMinimum.Text = XDate.ToString(aAxis.Scale.Min);
                        txtAxisDisplayMaximum.Text = XDate.ToString(aAxis.Scale.Max);
                        break;
                    case AxisType.Linear: cboAxisType.SelectedIndex = 1; break;
                    case AxisType.Log:    cboAxisType.SelectedIndex = 2; break;
                    //case AxisType.Probability: cboAxisType.SelectedIndex = 3; break;              
                }
                chkAxisMajorGridVisible.Checked = aAxis.MajorGrid.IsVisible;
                txtAxisMajorGridColor.BackColor = aAxis.MajorGrid.Color;
            }
        }

        private void SetAxisFromControls(Axis aAxis)
        {
            if (aAxis != null)
            {
                aAxis.Title.Text = txtAxisLabel.Text;
                double lTemp;
                if (double.TryParse(txtAxisDisplayMinimum.Text, out lTemp))
                {
                    aAxis.Scale.Min = lTemp;
                }
                if (double.TryParse(txtAxisDisplayMaximum.Text, out lTemp))
                {
                    aAxis.Scale.Max = lTemp;
                }
                AxisType lNewType = AxisType.Linear;
                switch (cboAxisType.SelectedIndex)
                {
                    case 0: 
                        lNewType = AxisType.DateMulti;
                        //TODO: parse min/max date from textboxes
                        break;
                    case 1: lNewType = AxisType.Linear;    break;
                    case 2: lNewType = AxisType.Log;       break;
                    //case 3: lNewType = AxisType.Probability; break;           
                }
                if (aAxis.Type != lNewType)
                {
                    aAxis.Type = lNewType;
                }
                aAxis.MajorGrid.IsVisible = chkAxisMajorGridVisible.Checked;
                aAxis.MajorGrid.Color = txtAxisMajorGridColor.BackColor;
            }
        }

        private void SetControlsFromCurve(CurveItem aCurve)
        {
            if (aCurve != null)
            {
                txtCurveLabel.Text = aCurve.Label.Text;
                cboCurveAxis.SelectedIndex = (aCurve.IsY2Axis) ? 1 : 0;
                txtCurveColor.BackColor = aCurve.Color;
                if (aCurve is LineItem)
                {
                    lblCurve.Visible = true;
                    txtCurveWidth.Visible = true;

                    txtCurveWidth.Text = ((LineItem)aCurve).Line.Width.ToString();
                }
                else
                {
                    lblCurve.Visible = false;
                    txtCurveWidth.Visible = false;
                }
            }
        }
        private void SetCurveFromControls(CurveItem aCurve)
        {
            if (aCurve != null)
            {
                aCurve.Label.Text = txtCurveLabel.Text;
                if (aCurve.IsY2Axis != (cboCurveAxis.SelectedIndex == 1))
                {
                    //TODO: move curve to other axis
                }
                aCurve.Color = txtCurveColor.BackColor;
                if (aCurve is LineItem)
                {
                    int lWidth;
                    if (int.TryParse(txtCurveWidth.Text, out lWidth)) 
                    {
                        ((LineItem)aCurve).Line.Width = lWidth;
                    }
                }
            }
        }
        private void cboWhichAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlsFromAxis(AxisFromCombo());
        }

        private void cboWhichDataset_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlsFromCurve(pPane.CurveList[cboWhichCurve.SelectedIndex]);
        }
        
        private void btnApply_Click(object sender, EventArgs e)
        {
            SetAxisFromControls(AxisFromCombo());
            if (cboWhichCurve.SelectedIndex >= 0)
            {
                SetCurveFromControls(pPane.CurveList[cboWhichCurve.SelectedIndex]);
            }
            Apply(this, System.EventArgs.Empty);
        }

        private void txtAxisMajorGridColor_Click(object sender, EventArgs e)
        {
            ColorDialog lColorDialog = new ColorDialog();
            lColorDialog.Color = txtAxisMajorGridColor.BackColor;
            if (lColorDialog.ShowDialog() == DialogResult.OK)
            {
                txtAxisMajorGridColor.BackColor = lColorDialog.Color;
            }
        }

        private void txtCurveColor_Click(object sender, EventArgs e)
        {
            ColorDialog lColorDialog = new ColorDialog();
            lColorDialog.Color = txtCurveColor.BackColor;
            if (lColorDialog.ShowDialog() == DialogResult.OK)
            {
                txtCurveColor.BackColor = lColorDialog.Color;
            }
        }
   }
}