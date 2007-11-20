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
	/// the features specific to <see cref="AxisType.Log" />.
	/// </summary>
	/// <remarks>
    /// ProbabilityScale is a non-linear axis in which the values are scaled using a Gaussian
    /// normal distribution function.
	/// </remarks>
	/// 
    /// <author> Mark Gray </author> based on LogScale class by John Champion
	/// <version> $Revision: 1.9 $ $Date: 2006/08/25 05:19:09 $ </version>
	[Serializable]
	class ProbabilityScale : Scale, ISerializable //, ICloneable
	{
        /// <summary>
        /// Number of standard deviations to display in graph (plus and minus this many are displayed)
        /// </summary>
        double standardDeviations = 3;

        /// <summary>
        /// Scale factor needed to convert standard deviations into graph scale
        /// </summary>
        double deviationScale = 1.0 / 6.0; // = 1 / (2 * standardDeviations);
        
        /// <summary>
        /// Percent chance exceeded to label, if there is room
        /// </summary>
        //double[] percentages = { 99.9, 99.8, 99.5, 99, 98, 95, 90, 80, 70, 50, 30, 20, 10, 5, 2, 1, 0.5, 0.2, 0.1 };
        double[] percentages = { 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 30, 50, 70, 80, 90, 95, 98, 99, 99.5, 99.8, 99.9 };
        /// <summary>
        /// Return periods to label, if there is room
        /// </summary>
        double[] returnperiods = { 1.001, 1.002, 1.005, 1.01, 1.02, 1.05, 1.1, 1.25, 1.5, 2, 3, 5, 10, 20, 50, 100, 200, 500, 1000 };
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

		/// <summary>
		/// Gets or sets the minimum value for this scale.
		/// </summary>
		/// <remarks>
		/// The set property is specifically adapted for <see cref="AxisType.Log" /> scales,
		/// in that it automatically limits the setting to values greater than zero.
		/// </remarks>
		public override double Min
		{
			get { return _min; }
			set { if ( value > 0 ) _min = value; }
		}

		/// <summary>
		/// Gets or sets the maximum value for this scale.
		/// </summary>
		/// <remarks>
		/// The set property is specifically adapted for <see cref="AxisType.Log" /> scales,
		/// in that it automatically limits the setting to values greater than zero.
		/// <see cref="XDate" /> struct.
		/// </remarks>
		public override double Max
		{
			get { return _max; }
			set { _max = value; }
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
            // gausex returns +/- standard deviations of val from 0.5
            // adding standardDeviations makes the result positive
            // multiplying by deviationScale scales it to fit within zero to one graph scale
            return deviationScale * (1 - (gausex(val) + standardDeviations));
        }

		/// <summary>
		/// Convert a value from its linear equivalent to its actual scale value
		/// for this type of scale.
		/// </summary>
		/// <remarks>
		/// The default behavior is to just return the value unchanged.  However,
		/// for <see cref="AxisType.Log" /> and <see cref="AxisType.Exponent" />,
		/// it returns the anti-log or inverse-power equivalent.
		/// </remarks>
		/// <param name="val">The value to be converted</param>
		override public double DeLinearize( double val )
		{
            double linearized;
            double guess = 0.5;
            double guesschange = 0.25;
            while (guesschange > 0.001)
            {
                linearized = Linearize(guess);
                if (linearized > val)
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
		/// <remarks>
		/// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
		/// and other axis format settings.
		/// </remarks>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double)
		/// </param>
		/// <param name="tic">
		/// The major tic number (0 = first major tic).  For log scales, this is the actual power of 10.
		/// </param>
		/// <returns>
		/// The specified major tic value (floating point double).
		/// </returns>
        override internal double CalcMajorTicValue(double baseVal, double tic)
        {
            int ticInt = (int)tic;
            if (ticInt >= 0 && ticInt < percentages.Length)
                return percentages[ticInt] / 100.0;
            else
                return 0;
        }

		/// <summary>
		/// Determine the value for any minor tic.
		/// </summary>
		/// <remarks>
		/// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
		/// and other axis format settings.
		/// </remarks>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double).  This tic value is the base
		/// reference for all tics (including minor ones).
		/// </param>
		/// <param name="iTic">
		/// The major tic number (0 = first major tic).  For log scales, this is the actual power of 10.
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
		/// Select a reasonable base 10 logarithmic axis scale given a range of data values.
		/// </summary>
		/// <remarks>
		/// This method only applies to <see cref="AxisType.Log"/> type axes, and it
		/// is called by the general <see cref="PickScale"/> method.  The scale range is chosen
		/// based always on powers of 10 (full log cycles).  This
		/// method honors the <see cref="Scale.MinAuto"/>, <see cref="Scale.MaxAuto"/>,
		/// and <see cref="Scale.MajorStepAuto"/> autorange settings.
		/// In the event that any of the autorange settings are false, the
		/// corresponding <see cref="Scale.Min"/>, <see cref="Scale.Max"/>, or <see cref="Scale.MajorStep"/>
		/// setting is explicitly honored, and the remaining autorange settings (if any) will
		/// be calculated to accomodate the non-autoranged values.  For log axes, the MinorStep
		/// value is not used.
		/// <para>On Exit:</para>
		/// <para><see cref="Scale.Min"/> is set to scale minimum (if <see cref="Scale.MinAuto"/> = true)</para>
		/// <para><see cref="Scale.Max"/> is set to scale maximum (if <see cref="Scale.MaxAuto"/> = true)</para>
		/// <para><see cref="Scale.MajorStep"/> is set to scale step size (if <see cref="Scale.MajorStepAuto"/> = true)</para>
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
		/// <seealso cref="AxisType.Log"/>
        override public void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            _mag = 0;
            _min = 0;
            _max = 1;
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
            return percentages[0] / 100;
        }

        /// <summary>
        /// Internal routine to determine the ordinals of the first and last major axis label.
        /// </summary>
        /// <returns>
        /// This is the total number of major tics for this axis.
        /// </returns>
        override internal int CalcNumTics()
        {
            return percentages.Length;
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
            catch (Exception e)
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
            return percentages[index].ToString(_format);
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
