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
        public FTableCalculator.ChannelType CurrentChannelType;

        //Circular channel
        public double _diam;
        public double _diameter;

        public double _width;
        public double _depth;
        public double _sideslope;

        public Color _color;

        public BMPSketch()
        {
            //InitializeComponent();
            CurrentChannelType = FTableCalculator.ChannelType.NONE;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BMPSketch_Paint);
        }

        private void DoPaint(System.Drawing.Graphics g)
        {
            //search Java for paintComponent
            this._color = Color.Red;
            switch (CurrentChannelType)
            {
                case FTableCalculator.ChannelType.CIRCULAR:
                    PaintCircular(g);
                    break;
                case FTableCalculator.ChannelType.RECTANGULAR:
                    PaintRectangle(g);
                    break;
                case FTableCalculator.ChannelType.TRIANGULAR:
                    PaintTriangle(g);
                    break;
                case FTableCalculator.ChannelType.TRAPEZOIDAL:
                    PaintTrape(g);
                    break;
                case FTableCalculator.ChannelType.PARABOLIC:
                    PaintEllipse(g);
                    break;
                case FTableCalculator.ChannelType.NATURAL:
                    PaintNatural(g);
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

        private void PaintRectangle(System.Drawing.Graphics g)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);

            //get suitable rectangle dimensions
            double stepSize = 5;
            double gWidth = this.Width - stepSize;
            double WtoH = _width / _depth;
            double gHeight = gWidth / WtoH;
            do
            {
                if (gHeight >= this.Height)
                {
                    gWidth -= stepSize;
                    gHeight = gWidth / WtoH;
                }
                else
                {
                    break;
                }
            } while (true);

            int xPt, yPt;
            xPt = (this.Width - (int)gWidth) / 2;
            yPt = (this.Height - (int)gHeight) / 2;
            g.DrawRectangle(myPen, xPt, yPt, (int)gWidth, (int)gHeight);

            myPen.Color = Color.Black;
            //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            g.DrawLine(myPen, xPt, yPt, xPt + (int)gWidth, yPt);
            g.DrawLine(myPen, xPt + (int)(gWidth / 2), yPt, xPt + (int)(gWidth /2), yPt + (int)gHeight);

            string sWidth = _width.ToString() + " (Top Width)";
            string sDepth = _depth.ToString() + " (Depth)";
            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 9);
            SolidBrush lblBrush = new SolidBrush(Color.Black);
            g.DrawString(sWidth, lblFont, lblBrush, (float)(xPt + gWidth / 4), (float)(yPt + 3));

            var textSize = TextRenderer.MeasureText(sDepth, lblFont);
            g.TranslateTransform((float)(xPt + gWidth / 2 + textSize.Height + 2), (float)(yPt + 8));
            g.RotateTransform(90f);
            //g.DrawString(sDepth, lblFont, lblBrush, (float)(xPt + gWidth / 2 + 1), (float)(yPt + gHeight / 2));
            g.DrawString(sDepth, lblFont, lblBrush, this.DisplayRectangle);
            g.ResetTransform();
        }

        private void PaintTriangle(System.Drawing.Graphics g)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            //get suitable triangle dimensions
            double stepSize = 5;
            double gHeight = this.Height - stepSize; //start out from height or depth
            double gWidth = gHeight * _sideslope * 2.0;
            do
            {
                if (gWidth > this.Width)
                {
                    gHeight -= stepSize;
                    gWidth = gHeight * _sideslope * 2.0;
                }
                else
                {
                    break;
                }
            } while (true);

            int xPt, yPt;
            xPt = (this.Width - (int)gWidth) / 2;
            yPt = (this.Height - (int)gHeight) / 2;

            float xPtMid = (float)(xPt + gWidth / 2);
            float yPtMid = (float)(yPt + gHeight);

            g.DrawLine(myPen, (float)xPt, (float)yPt, xPtMid, yPtMid);
            g.DrawLine(myPen, (float)(xPt +gWidth), (float)yPt ,xPtMid, yPtMid);

            myPen.Color = Color.Black;
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            //g.DrawLine(myPen, xPt, yPt, xPt + (int)gWidth, yPt);
            g.DrawLine(myPen, xPt + (int)(gWidth / 2), yPt, xPt + (int)(gWidth / 2), yPt + (int)gHeight);

            string sSideSlope = _sideslope.ToString() + " (Side Slope H:V)";
            string sDepth = _depth.ToString() + " (Max. Depth)";
            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 9);
            SolidBrush lblBrush = new SolidBrush(Color.Black);
            g.DrawString(sDepth, lblFont, lblBrush, (float)(xPt + gWidth / 2 + 1), (float)(yPt + gHeight / 2));
            float lRotateAng = (float)(Math.Atan2(1.0d, _sideslope) * (180 / Math.PI));
            g.TranslateTransform((float)(xPt + gWidth / 8 + 1), (float)(yPt + gWidth / 8 / 3 - 1));
            g.RotateTransform(lRotateAng);
            //g.DrawString(sSideSlope, lblFont, lblBrush, (float)(xPt + gWidth / 8), (float)(yPt + gWidth/8/3));
            g.DrawString(sSideSlope, lblFont, lblBrush, this.DisplayRectangle);
            g.ResetTransform();
        }

        private void PaintEllipse(System.Drawing.Graphics g)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);

            //get suitable rectangle dimensions
            double stepSize = 2;
            double gWidth = this.Width - stepSize;
            double WtoH = _width / _depth;
            double gHeight = gWidth / WtoH;
            do
            {
                if (gHeight > (this.Height - stepSize))
                {
                    gWidth -= stepSize;
                    gHeight = gWidth / WtoH;
                }
                else
                {
                    break;
                }
            } while (true);

            double upperLeftPtX = (this.Width - gWidth) / 2;
            double upperLeftPtY = (this.Height - gHeight) / 2;

            double upperRightPtX = upperLeftPtX + gWidth;
            double controlX = upperLeftPtX + (gWidth / 2);
            double controlY = upperLeftPtY + gHeight;

            double lRad = (Math.Pow(gHeight, 2.0) + Math.Pow(gWidth / 2.0, 2.0)) / (2 * gHeight);
            double lDropToChord = lRad - gHeight;
            double lStartAngle = Math.Atan2(lDropToChord, gWidth / 2) * 180 / Math.PI;
            double lSweepAngle = 180 - 2 * lStartAngle;

            //Rectangle lRect = new Rectangle((int)upperLeftPtX, (int)upperLeftPtY, (int)gWidth, (int)gHeight);
            double lNewUpperLeftPtX = (upperLeftPtX + gWidth / 2) - lRad;
            double lNewUpperLeftPtY = upperLeftPtY - lRad - lDropToChord;
            double lNewHeight = gHeight + lRad + lDropToChord;
            Rectangle lRect = new Rectangle((int)lNewUpperLeftPtX, (int)lNewUpperLeftPtY, (int)(lRad * 2), (int)lNewHeight);
            //g.DrawRectangle(myPen, lRect);
            g.DrawArc(myPen, lRect, (float)lStartAngle, (float)lSweepAngle);

            //System.Drawing.Drawing2D.GraphicsPath lArc = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Alternate);
            //lArc.AddArc(lRect, 0, 180);
            //g.DrawPath(myPen, lArc);

            myPen.Color = Color.Black;
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            g.DrawLine(myPen, (int)upperLeftPtX, (int)upperLeftPtY, (int)upperRightPtX, (int)upperLeftPtY);
            g.DrawLine(myPen, (float)controlX, (float)upperLeftPtY, (float)controlX, (float)gHeight);

            String w = _width.ToString() + "(width)";
            int xWidthDrawText = (int)gWidth / 2;
            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 9);
            SolidBrush lblBrush = new SolidBrush(Color.Black);
            //g.DrawString(w, lblFont, lblBrush, upperLeftPtX + xWidthDrawText - 5, upperLeftPtY + 10);
            g.DrawString(w, lblFont, lblBrush, (float)(upperLeftPtX + gWidth / 4), (float)(upperLeftPtY + 5));

            String d = _depth.ToString() + " (depth)";
            //g.DrawString(d, lblFont, lblBrush, xDepth, yDepth);
            var textSize = TextRenderer.MeasureText(d, lblFont);
            g.TranslateTransform((float)(upperLeftPtX + gWidth / 2 + textSize.Height + 2), (float)(upperLeftPtY + 8));
            g.RotateTransform(90f);
            g.DrawString(d, lblFont, lblBrush, this.DisplayRectangle);
            g.ResetTransform();
        }

        private void PaintTrape(System.Drawing.Graphics g)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);

            //get suitable rectangle dimensions
            double stepSize = 5;
            double gWidth = this.Width - stepSize;
            double WtoH = _width / _depth;
            double gHeight = gWidth / WtoH;
            do
            {
                if (gHeight >= this.Height)
                {
                    gWidth -= stepSize;
                    gHeight = gWidth / WtoH;
                }
                else
                {
                    break;
                }
            } while (true);

            int xPt, yPt;
            xPt = (this.Width - (int)gWidth) / 2;
            yPt = (this.Height - (int)gHeight) / 2;
            //g.DrawRectangle(myPen, xPt, yPt, (int)gWidth, (int)gHeight);

            double lBankRun = gHeight * _sideslope;
            double lWidthBottom = gWidth - 2 * lBankRun;

            Point UL = new Point(xPt, yPt);
            Point BL = new Point((int)(xPt + lBankRun), (int)(yPt + gHeight));
            Point BR = new Point((int)(xPt + lBankRun + lWidthBottom), (int)(yPt + gHeight));
            Point UR = new Point((int)(xPt + gWidth), yPt);

            g.DrawLine(myPen, UL, BL);
            g.DrawLine(myPen, BL, BR);
            g.DrawLine(myPen, BR, UR);

            myPen.Color = Color.Black;
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            g.DrawLine(myPen, xPt, yPt, xPt + (int)gWidth, yPt);
            g.DrawLine(myPen, xPt + (int)(gWidth / 2), yPt, xPt + (int)(gWidth / 2), yPt + (int)gHeight);

            string sWidth = _width.ToString() + " (Top Width)";
            string sDepth = _depth.ToString() + " (Depth)";
            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 9);
            SolidBrush lblBrush = new SolidBrush(Color.Black);
            g.DrawString(sWidth, lblFont, lblBrush, (float)(xPt + gWidth / 4), (float)(yPt + 3));

            var textSize = TextRenderer.MeasureText(sDepth, lblFont);
            g.TranslateTransform((float)(xPt + gWidth / 2 + textSize.Height + 2), (float)(yPt + 8));
            g.RotateTransform(90f);
            //g.DrawString(sDepth, lblFont, lblBrush, (float)(xPt + gWidth / 2 + 1), (float)(yPt + gHeight / 2));
            g.DrawString(sDepth, lblFont, lblBrush, this.DisplayRectangle);
            g.ResetTransform();
        }

        private void PaintNatural(System.Drawing.Graphics g)
        {
            /*The following paints a line on the channel panel.  It is just a 
             * random line illustrating an example of a natural channel.*/
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(_color);

            Font lblFont = new Font(System.Drawing.FontFamily.GenericSerif, 9);
            SolidBrush lblBrush = new SolidBrush(Color.Black);

            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            int incrementalX = (this.Width / 10);
            int incrementalY = (this.Height / 10);

            //The points/lines are somewhat randomly picked.  They were coded individually,
            //inspected in the window iteratively, and modified to get the right look for the 
            //natural channel.
            x1 = (int)(incrementalX * .75);
            y1 = (int)((int)(incrementalY * 1));
            x2 = (int)(incrementalX * 1.5);
            y2 = (int)(incrementalY * 7);
            g.DrawLine(myPen, x1, y1, x2, y2);

            g.DrawString("x1,y1: High Point(Left Bank)", lblFont, lblBrush, x1, y1);

            x1 = x2;
            y1 = y2;
            x2 = x2 + (incrementalX);
            y2 = y2 - (int)(incrementalY * 2.5);
            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawString("x2,y2", lblFont, lblBrush, x1, y1 + 10);
            x1 = x2;
            y1 = y2;
            x2 = x2 + (incrementalX);
            y2 = y2 + 5;
            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawString("x3,y3", lblFont, lblBrush, x1, y1 - 2);
            x1 = x2;
            y1 = y2;
            x2 = x2 + incrementalX;
            y2 = y2 + 4 * incrementalY;
            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawString("x4,y4", lblFont, lblBrush, x1, y1);
            x1 = x2;
            y1 = y2;
            x2 = x2 + incrementalX;
            y2 = y2 - 4 * incrementalY;
            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawString("x5,y5: Subtract all elevation values from the lowest to get relative depth above thalweg", lblFont, lblBrush, x1, y1 - 6);
            x1 = x2;
            y1 = y2;
            x2 = x2 + incrementalX;
            y2 = y2 - 17;
            g.DrawLine(myPen, x1, y1, x2, y2);

            g.DrawString("x6,y6",lblFont, lblBrush, x1, y1 - 10);
            x1 = x2;
            y1 = y2;
            x2 = x2 + incrementalX;
            y2 = incrementalY;
            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawString("x7,y7", lblFont, lblBrush, x1, y1 + 10);
            g.DrawString("x8,y8: High Point (Right Bank)",lblFont, lblBrush, x2, y2);
        }

        private void BMPSketch_Paint(object sender, PaintEventArgs e)
        {
            this.DoPaint(e.Graphics);
        }

        private void BMPSketch_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
