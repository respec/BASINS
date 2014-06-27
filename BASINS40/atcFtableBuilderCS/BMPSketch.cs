using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace atcFtableBuilder
{
    public partial class BMPSketch : UserControl
    {
        public FTableCalculator.ChannelType CurrentBMPType;

        //Circular channel
        public double _diam;
        public double _diameter;


        public Color _color;

        public BMPSketch()
        {
            //InitializeComponent();
            CurrentBMPType = FTableCalculator.ChannelType.NONE;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BMPSketch_Paint);
        }

        private void DoPaint(System.Drawing.Graphics g)
        {
            switch (CurrentBMPType)
            {
                case FTableCalculator.ChannelType.CIRCULAR:
                    this._color = Color.Red;
                    PaintCircular(g);
                    break;
                case FTableCalculator.ChannelType.RECTANGULAR:
                    break;
                case FTableCalculator.ChannelType.TRAPEZOIDAL:
                    break;
            }
        }

        private void PaintCircular(System.Drawing.Graphics g)
        {
            if (_diameter < 0) return;

            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);

            int width = this.Width;
            int height = this.Height;

            if (width >= height)
            {
                if (height > 20)
                    _diam = height - 10;
                else
                    _diam = height - 10;
            }

            if (height > width)
            {
                if (width > 20)
                    _diam = width - 10;
                else
                    _diam = width;
            }

            int xPt, yPt;
            xPt = (width - (int)_diam) / 2;
            yPt = (height - (int)_diam) / 2;
            g.DrawEllipse(myPen, xPt, yPt, (int)_diam, (int)_diam);

            //g.setColor(Color.black);
            myPen.Color = Color.Black;
            g.DrawLine(myPen, (width / 2) - (int)_diam / 2, height / 2, (width / 2) + ((int)(_diam / 2)), height / 2);

            String diam = _diameter.ToString();
            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 12);
            SolidBrush lblBrush = new SolidBrush(Color.Black);
            float lblx = width / 2 - 5;
            float lbly = height / 2 - 5;
            g.DrawString(diam, lblFont, lblBrush, lblx, lbly);
        }

        private void BMPSketch_Paint(object sender, PaintEventArgs e)
        {
            this.DoPaint(e.Graphics);
        }
    }
}
