//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright © 2005  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ZedGraph
{
	/// <summary>
	/// The ProbabilityScale class inherits from the <see cref="Scale" /> class, and implements
    /// the features specific to <see cref="AxisType.Probability" />.
	/// </summary>
	/// <remarks>
    /// ProbabilityScale is a non-linear axis in which the values are scaled using a Gaussian
    /// normal distribution function.
	/// </remarks>
	/// 
    /// <author> Mark Gray </author> based on LogScale class by John Champion
	/// <version> $Revision: 1.9 $ $Date: 2006/08/25 05:19:09 $ </version>
	[Serializable]
	public class ProbabilityScale : Scale, ISerializable //, ICloneable
	{
        /// <summary>
        /// Number of standard deviations to display in graph (plus and minus this many are displayed)
        /// </summary>
        public double standardDeviations = 5;

        /// <summary>
        /// Probability axis labeling style (Percent, Fraction, or Return Interval)
        /// </summary>
        public enum ProbabilityLabelStyle
        {
            /// Label using Percent
            Percent = 0,
            /// Label using Fraction (Percent/100)
            Fraction = 1,
            /// Label using Return Interval (100/Percent)
            ReturnInterval = 2,
        }

        /// <summary>
        /// Style to use for labeling this axis
        /// </summary>
        public ProbabilityLabelStyle LabelStyle = ProbabilityLabelStyle.Percent;

        /// <summary>
        /// Percent chance exceeded to label, if there is room
        /// </summary>
        public double[] Percentages = {0.0001, 0.001, 0.01, 0.02, 0.05, 
                                       0.1, 0.2, 0.5, 
                                       1, 2, 5,
                                       10, 20, 25, 30, 40, 50, 60, 70, 75, 80, 90, 
                                       95, 98, 99, 
                                       99.5, 99.8, 99.9, 
                                       99.95, 99.98, 99.99, 99.999, 99.9999};
	#region constructors

		/// <summary>
		/// Default constructor that defines the owner <see cref="Axis" />
		/// (containing object) for this new object.
		/// </summary>
		/// <param name="owner">The owner, or containing object, of this instance</param>
		public ProbabilityScale( Axis owner )
			: base( owner )
		{
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="ProbabilityScale" /> object from which to copy</param>
		/// <param name="owner">The <see cref="Axis" /> object that will own the
		/// new instance of <see cref="ProbabilityScale" /></param>
		public ProbabilityScale( Scale rhs, Axis owner )
			: base( rhs, owner )
		{
		}

		/// <summary>
		/// Create a new clone of the current item, with a new owner assignment
		/// </summary>
		/// <param name="owner">The new <see cref="Axis" /> instance that will be
		/// the owner of the new Scale</param>
		/// <returns>A new <see cref="Scale" /> clone.</returns>
		public override Scale Clone( Axis owner )
		{
			return new ProbabilityScale( this, owner );
		}

	#endregion

	#region properties

		/// <summary>
		/// Return the <see cref="AxisType" /> for this <see cref="Scale" />, which is
		/// <see cref="AxisType.Log" />.
		/// </summary>
		public override AxisType Type
		{
            get { return AxisType.Probability; }
		}

	#endregion

	#region methods

		/// <summary>
		/// Setup some temporary transform values in preparation for rendering the <see cref="Axis"/>.
		/// </summary>
		/// <remarks>
		/// This method is typically called by the parent <see cref="GraphPane"/>
		/// object as part of the <see cref="GraphPane.Draw"/> method.  It is also
		/// called by <see cref="GraphPane.GeneralTransform(double,double,CoordType)"/> and
		/// GraphPane.ReverseTransform
		/// methods to setup for coordinate transformations.
		/// </remarks>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="axis">
		/// The parent <see cref="Axis" /> for this <see cref="Scale" />
		/// </param>
		override public void SetupScaleData( GraphPane pane, Axis axis )
		{
			base.SetupScaleData( pane, axis );

			_minLinTemp = Linearize( _min );
			_maxLinTemp = Linearize( _max );
		}

		/// <summary>
		/// Convert a value to its linear equivalent for this type of scale.
		/// </summary>
		/// <remarks>
        /// 
		/// </remarks>
		/// <param name="val">The value to be converted</param>
		override public double Linearize( double val )
		{
            // gausex returns +/- standardDeviations of val from 0.5
            return (1 - (gausex(val) / standardDeviations)) / 2;
        }

		/// <summary>
		/// Convert a value from its linear equivalent to its actual scale value
		/// for this type of scale.
		/// </summary>
		/// <remarks>
		/// Knowing no direct inverse of gausex, we use successive approximation to locate 
		/// a return value where Linearize(return value) is close enough to val
		/// </remarks>
		/// <param name="val">The value to be converted</param>
		override public double DeLinearize( double val )
		{
            double linearized;
            double guess = 0.5;
            double guesschange = 0.25;
            double closeEnough = Math.Pow(10, -(standardDeviations + 2));
            while (guesschange > closeEnough)
            {
                linearized = Linearize(guess);
                if (val > linearized)
                    guess += guesschange;
                else
                    guess -= guesschange;
                guesschange /= 2;
            }
            return guess;
		}

		/// <summary>
		/// Determine the value for any major tic.
		/// </summary>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double)
		/// </param>
		/// <param name="tic">
		/// The major tic number (0 = first major tic).
		/// </param>
		/// <returns>
		/// The specified major tic value (floating point double).
		/// </returns>
        override internal double CalcMajorTicValue(double baseVal, double tic)
        {
            int ticInt = (int)tic;
            if (ticInt >= 0 && ticInt < Percentages.Length)
                return Linearize(Percentages[ticInt] / 100.0);
            else
                return 0;
        }

        //internal double Percentage(int index)
        //{
        //    if (Exceedance) 
        //    {
        //        return Percentages[index];
        //    } else {
        //        return Percentages[Percentages.Length - 1 - index]; 
        //    }
        //}

		/// <summary>
		/// Determine the value for any minor tic.
		/// </summary>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double).  This tic value is the base
		/// reference for all tics (including minor ones).
		/// </param>
		/// <param name="iTic">
		/// The major tic number (0 = first major tic).
		/// </param>
		/// <returns>
		/// The specified minor tic value (floating point double).
		/// </returns>
        override internal double CalcMinorTicValue(double baseVal, int iTic)
        {
            return baseVal;
        }

		/// <summary>
		/// Internal routine to determine the ordinals of the first minor tic mark
		/// </summary>
		/// <param name="baseVal">
		/// The value of the first major tic for the axis.
		/// </param>
		/// <returns>
		/// The ordinal position of the first minor tic, relative to the first major tic.
		/// This value can be negative (e.g., -3 means the first minor tic is 3 minor step
		/// increments before the first major tic.
		/// </returns>
        override internal int CalcMinorStart(double baseVal)
        {
            return 0;
        }

		/// <summary>
		/// Select a reasonable axis scale
		/// </summary>
		/// <remarks>
		/// This method only applies to <see cref="AxisType.Probability"/> type axes, and it
		/// is called by the general <see cref="PickScale"/> method.  
        /// This method honors the <see cref="Scale.MinAuto"/> and <see cref="Scale.MaxAuto"/>.
		/// <para>On Exit:</para>
		/// <para><see cref="Scale.Min"/> is set to scale minimum (if <see cref="Scale.MinAuto"/> = true)</para>
		/// <para><see cref="Scale.Max"/> is set to scale maximum (if <see cref="Scale.MaxAuto"/> = true)</para>
		/// <para><see cref="Scale.Mag"/> is set to a magnitude multiplier according to the data</para>
		/// <para><see cref="Scale.Format"/> is set to the display format for the values (this controls the
		/// number of decimal places, whether there are thousands separators, currency types, etc.)</para>
		/// </remarks>
		/// <param name="pane">A reference to the <see cref="GraphPane"/> object
		/// associated with this <see cref="Axis"/></param>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <seealso cref="PickScale"/>
		/// <seealso cref="AxisType.Probability"/>
        override public void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            if ( _minAuto ) _min = 0;
            if ( _maxAuto ) _max = 1;
        }

        /// <summary>
        /// Determine the value for the first major tic.
        /// </summary>
        /// <remarks>
        /// This is done by finding the first possible value that is an integral multiple of
        /// the step size, taking into account the date/time units if appropriate.
        /// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
        /// and other axis format settings.
        /// </remarks>
        /// <returns>
        /// First major tic value (floating point double).
        /// </returns>
        override internal double CalcBaseTic()
        {
            return Percentages[0] / 100;
        }

        /// <summary>
        /// Internal routine to determine the ordinals of the first and last major axis label.
        /// </summary>
        /// <returns>
        /// This is the total number of major tics for this axis.
        /// </returns>
        override internal int CalcNumTics()
        {
            return Percentages.Length;
        }

        /// <summary>
        /// Draw the value labels, tic marks, and grid lines as
        /// required for this <see cref="Axis"/>.
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="pane">
        /// A reference to the <see cref="GraphPane"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="baseVal">
        /// The first major tic value for the axis
        /// </param>
        /// <param name="nTics">
        /// The total number of major tics for the axis
        /// </param>
        /// <param name="topPix">
        /// The pixel location of the far side of the ChartRect from this axis.
        /// This value is the ChartRect.Height for the XAxis, or the ChartRect.Width
        /// for the YAxis and Y2Axis.
        /// </param>
        /// <param name="shift">The number of pixels to shift this axis, based on the
        /// value of <see cref="Axis.Cross"/>.  A positive value is into the ChartRect relative to
        /// the default axis position.</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="GraphPane"/> object using the
        /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        internal override void DrawLabels(Graphics g, GraphPane pane, double baseVal, int nTics,
                        float topPix, float shift, float scaleFactor)
        {
            int lNtics = Percentages.Length;

            MajorTic tic = _ownerAxis._majorTic;
            //			MajorGrid grid = _ownerAxis._majorGrid;

            double dVal;
            float pixVal;
            float scaledTic = tic.ScaledTic(scaleFactor);

            using (Pen ticPen = tic.GetPen(pane, scaleFactor))
            //			using ( Pen gridPen = grid.GetPen( pane, scaleFactor ) )
            {
                // get the Y position of the center of the axis labels
                // (the axis itself is referenced at zero)
                SizeF maxLabelSize = GetScaleMaxSpace(g, pane, scaleFactor, true);
                float charHeight = _fontSpec.GetHeight(scaleFactor);
                float maxSpace = maxLabelSize.Height;

                float edgeTolerance = Default.EdgeTolerance * scaleFactor;
                double rangeTol = (_maxLinTemp - _minLinTemp) * 0.001;

                int firstTic = 0;

                // save the position of the previous tic
                float lastPixVal = -10000;

                // loop for each major tic
                for (int i = firstTic; i < lNtics + firstTic; i++)
                {
                    dVal = CalcMajorTicValue(baseVal, i);
                    double linVal = dVal; // Linearize(dVal);

                    // If we're before the start of the scale, just go to the next tic
                    if (linVal < _minLinTemp)
                        continue;
                    // if we've already past the end of the scale, then we're done
                    if (linVal > _maxLinTemp + rangeTol)
                        break;

                    // convert the value to a pixel position
                    pixVal = LocalTransform(linVal);

                    tic.Draw(g, pane, ticPen, pixVal, topPix, shift, scaledTic);

                    // draw the grid
                    //					grid.Draw( g, gridPen, pixVal2, topPix );

                    bool isMaxValueAtMaxPix = ((_ownerAxis is XAxis || _ownerAxis is Y2Axis) &&
                                                            !IsReverse) ||
                                                (_ownerAxis is Y2Axis && IsReverse);

                    bool isSkipZone = (((_isSkipFirstLabel && isMaxValueAtMaxPix) ||
                                            (_isSkipLastLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal < edgeTolerance) ||
                                        (((_isSkipLastLabel && isMaxValueAtMaxPix) ||
                                            (_isSkipFirstLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal > _maxPix - _minPix - edgeTolerance);

                    bool isSkipCross = _isSkipCrossLabel && !_ownerAxis._crossAuto &&
                                    Math.Abs(_ownerAxis._cross - dVal) < rangeTol * 10.0;

                    isSkipZone = isSkipZone || isSkipCross;

                    if (_isVisible && !isSkipZone)
                    {
                        // For exponential scales, just skip any label that would overlap with the previous one
                        // This is because exponential scales have varying label spacing
                        if (IsPreventLabelOverlap &&
                                Math.Abs(pixVal - lastPixVal) < maxLabelSize.Width)
                            continue;

                        DrawLabel(g, pane, i, dVal, pixVal, shift, maxSpace, scaledTic, charHeight, scaleFactor);

                        lastPixVal = pixVal;
                    }
                }
            }
        }

        /// <summary>
        /// Computes number of standard deviations from 0.5
        /// </summary>
        /// <param name="exprob">A probability between zero and one</param>
        /// <returns>number of standard deviations that the given probability is from 0.5</returns>
        public double gausex(double exprob)
        {
            //GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
            //   GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
            //rev 8/96 by PRH for VB
            //rev 11/2006 by MHG for c#
            double c0 = 2.515517;
            double c1 = 0.802853;
            double c2 = 0.010328;
            double d1 = 1.432788;
            double d2 = 0.189269;
            double d3 = 0.001308;
            double pr, rtmp, p, t, numerat, Denom;

            try
            {
                p = exprob;
                if (p >= 1)
                    rtmp = -standardDeviations; //set to minimum
                else if (p <= 0)
                    rtmp = standardDeviations;  //set at maximum
                else            //compute value
                {
                    pr = p;
                    if (p > 0.5) pr = 1 - pr;
                    t = Math.Sqrt(-2 * Math.Log(pr));
                    numerat = (c0 + t * (c1 + t * c2));
                    Denom = (1 + t * (d1 + t * (d2 + t * d3)));
                    rtmp = t - numerat / Denom;
                    if (p > 0.5) rtmp = -rtmp;
                }
                if (rtmp > standardDeviations) rtmp = standardDeviations;
                if (rtmp < -standardDeviations) rtmp = -standardDeviations;
                return rtmp;
            }
            catch 
            {
                return 0;
            }
        }

		/// <summary>
		/// Make a value label for an <see cref="AxisType.Log" /> <see cref="Axis" />.
		/// </summary>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="index">
		/// The zero-based, ordinal index of the label to be generated.  For example, a value of 2 would
		/// cause the third value label on the axis to be generated.
		/// </param>
		/// <param name="dVal">
		/// The numeric value associated with the label.  This value is ignored for log (<see cref="Scale.IsLog"/>)
		/// and text (<see cref="Scale.IsText"/>) type axes.
		/// </param>
		/// <returns>The resulting value label as a <see cref="string" /></returns>
        override internal string MakeLabel(GraphPane pane, int index, double dVal)
        {
            if (_format == null)
                _format = Scale.Default.Format;

            double lbl = Percentages[index];

            switch (this.LabelStyle)
            {
                case ProbabilityLabelStyle.Fraction: lbl /= 100; break;
                case ProbabilityLabelStyle.Percent: break;
                case ProbabilityLabelStyle.ReturnInterval: lbl = 100 / lbl; break;
            }

            if (!_formatAuto)
                return lbl.ToString(_format);

            //2/21/08 LCW: modified so that if formatAuto is set, will give variable precision on formatting of labels
            string fmt = "0";
            if (lbl < 0.001 || lbl > 99.999)
                fmt="0.0000";
            else
                if (lbl < 0.01 || lbl > 99.99)
                    fmt="0.000";
                else
                    if (lbl < 0.1 || lbl > 99.9)
                        fmt="0.00";
                    else
                        if (lbl < 1 || lbl > 99)
                            fmt="0.0";
            return lbl.ToString(fmt);
        }

	#endregion

	#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		public const int schema2 = 10;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
		/// </param>
        protected ProbabilityScale(SerializationInfo info, StreamingContext context)
            : base(info, context)
		{
			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch = info.GetInt32( "schema2" );

		}
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );
			info.AddValue( "schema2", schema2 );
		}
	#endregion

	}
}
