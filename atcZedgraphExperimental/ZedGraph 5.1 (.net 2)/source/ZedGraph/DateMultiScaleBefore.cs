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
	/// The DateMultiScale class inherits from the <see cref="Scale" /> class, and implements
	/// the features specific to <see cref="AxisType.DateMulti" />.
	/// </summary>
	/// <remarks>
	/// DateMultiScale is a cartesian axis with calendar dates or times.  The actual data values should
	/// be created with the <see cref="XDate" /> type, which is directly translatable to a
	/// <see cref="System.Double" /> type for storage in the point value arrays.
	/// </remarks>
	/// 
	/// <author> John Champion, Mark Gray </author>
	/// <version> $Revision: 1.12 $ $Date: 2006/08/25 05:19:09 $ </version>
	[Serializable]
	class DateMultiScale : Scale, ISerializable //, ICloneable
	{
        // Full names of months for labeling when there is room
        private string[] MonthNamesFull = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames; //{"", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
        // Abbreviated names of months for labeling when there is less room
        private string[] MonthNamesAbbreviated = System.Globalization.DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames; //{ "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        // Natural numbers of years or days to label
        private int[] roundNumbers = { 1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000, 2000, 5000, 10000, 20000, 25000, 50000, 100000 };
        // Natural numbers of minutes to label
        private int[] roundMinutes = { 1, 2, 5, 10, 15, 30, 60, 120, 240, 360, 480, 720, 1440 };
        // hours in roundMinutes                             1    2    4    6    8   12    24

	#region constructors

		/// <summary>
		/// Default constructor that defines the owner <see cref="Axis" />
		/// (containing object) for this new object.
		/// </summary>
		/// <param name="owner">The owner, or containing object, of this instance</param>
		public DateMultiScale( Axis owner )
			: base( owner )
		{
            _ownerAxis._majorTic.IsAllTics = false;
            _ownerAxis._minorTic.IsAllTics = false;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="DateScale" /> object from which to copy</param>
		/// <param name="owner">The <see cref="Axis" /> object that will own the
		/// new instance of <see cref="DateScale" /></param>
		public DateMultiScale( Scale rhs, Axis owner )
			: base( rhs, owner )
		{
            _ownerAxis._majorTic.IsAllTics = false;
            _ownerAxis._minorTic.IsAllTics = false;
        }

		/// <summary>
		/// Create a new clone of the current item, with a new owner assignment
		/// </summary>
		/// <param name="owner">The new <see cref="Axis" /> instance that will be
		/// the owner of the new Scale</param>
		/// <returns>A new <see cref="Scale" /> clone.</returns>
		public override Scale Clone( Axis owner )
		{
			return new DateMultiScale( this, owner );
		}

	#endregion

	#region properties

		/// <summary>
		/// Return the <see cref="AxisType" /> for this <see cref="Scale" />, which is
        /// <see cref="AxisType.DateMulti" />.
		/// </summary>
		public override AxisType Type
		{
			get { return AxisType.DateMulti; }
		}

		/// <summary>
		/// Gets or sets the minimum value for this scale.
		/// </summary>
		/// <remarks>
        /// The set property is specifically adapted for <see cref="AxisType.DateMulti" /> scales,
		/// in that it automatically limits the value to the range of valid dates for the
		/// <see cref="XDate" /> struct.
		/// </remarks>
		public override double Min
		{
			get { return _min; }
			set { _min = XDate.MakeValidDate( value ); _minAuto = false; }
		}

		/// <summary>
		/// Gets or sets the maximum value for this scale.
		/// </summary>
		/// <remarks>
        /// The set property is specifically adapted for <see cref="AxisType.DateMulti" /> scales,
		/// in that it automatically limits the value to the range of valid dates for the
		/// <see cref="XDate" /> struct.
		/// </remarks>
		public override double Max
		{
			get { return _max; }
			set { _max = XDate.MakeValidDate( value ); _maxAuto = false; }
		}
	#endregion

#region methods

		/// <summary>
		/// Internal routine to determine the ordinals of the first and last major axis label.
		/// </summary>
		/// <returns>
		/// This is the total number of major tics for this axis.
		/// </returns>
		override internal int CalcNumTics()
		{
			int nTics = 1;

			int year1, year2, month1, month2, day1, day2, hour1, hour2, minute1, minute2;
			double second1, second2;

			XDate.XLDateToCalendarDate( _min, out year1, out month1, out day1,
										out hour1, out minute1, out second1 );
			XDate.XLDateToCalendarDate( _max, out year2, out month2, out day2,
										out hour2, out minute2, out second2 );
			switch ( _majorUnit )
			{
				case DateUnit.Year:
				default:
					nTics = (int) ( ( year2 - year1 ) / _majorStep + 1.001 );
					break;
				case DateUnit.Month:
					nTics = (int) ( ( month2 - month1 + 12.0 * ( year2 - year1 ) ) / _majorStep + 1.001 );
					break;
				case DateUnit.Day:
					nTics = (int) ( ( _max - _min ) / _majorStep + 1.001 );
					break;
				case DateUnit.Hour:
					nTics = (int) ( ( _max - _min ) / ( _majorStep / XDate.HoursPerDay ) + 1.001 );
					break;
				case DateUnit.Minute:
					nTics = (int) ( ( _max - _min ) / ( _majorStep / XDate.MinutesPerDay ) + 1.001 );
					break;
				case DateUnit.Second:
					nTics = (int) ( ( _max - _min ) / ( _majorStep / XDate.SecondsPerDay ) + 1.001 );
					break;
			}

			if ( nTics < 1 )
				nTics = 1;
			else if ( nTics > 1000 )
				nTics = 1000;

			return nTics;
		}

		/// <summary>
		/// Select a reasonable date-time axis scale given a range of data values.
		/// </summary>
		/// <remarks>
        /// This method only applies to <see cref="AxisType.DateMulti"/> type axes, and it
		/// is called by the general <see cref="PickScale"/> method.  The scale range is chosen
		/// based on increments of 1, 2, or 5 (because they are even divisors of 10).
		/// Note that the <see cref="Scale.MajorStep"/> property setting can have multiple unit
		/// types (<see cref="Scale.MajorUnit"/> and <see cref="Scale.MinorUnit" />),
		/// but the <see cref="Scale.Min"/> and
		/// <see cref="Scale.Max"/> units are always days (<see cref="XDate"/>).  This
		/// method honors the <see cref="Scale.MinAuto"/>, <see cref="Scale.MaxAuto"/>,
		/// and <see cref="Scale.MajorStepAuto"/> autorange settings.
		/// In the event that any of the autorange settings are false, the
		/// corresponding <see cref="Scale.Min"/>, <see cref="Scale.Max"/>, or <see cref="Scale.MajorStep"/>
		/// setting is explicitly honored, and the remaining autorange settings (if any) will
		/// be calculated to accomodate the non-autoranged values.  The basic default for
		/// scale selection is defined with
		/// <see cref="Scale.Default.TargetXSteps"/> and <see cref="Scale.Default.TargetYSteps"/>
		/// from the <see cref="Scale.Default"/> default class.
		/// <para>On Exit:</para>
		/// <para><see cref="Scale.Min"/> is set to scale minimum (if <see cref="Scale.MinAuto"/> = true)</para>
		/// <para><see cref="Scale.Max"/> is set to scale maximum (if <see cref="Scale.MaxAuto"/> = true)</para>
		/// <para><see cref="Scale.MajorStep"/> is set to scale step size (if <see cref="Scale.MajorStepAuto"/> = true)</para>
		/// <para><see cref="Scale.MinorStep"/> is set to scale minor step size (if <see cref="Scale.MinorStepAuto"/> = true)</para>
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
		/// <seealso cref="Scale.PickScale"/>
        /// <seealso cref="AxisType.DateMulti"/>
		/// <seealso cref="Scale.MajorUnit"/>
		/// <seealso cref="Scale.MinorUnit"/>
		override public void PickScale( GraphPane pane, Graphics g, float scaleFactor )
		{
			// call the base class first
			base.PickScale( pane, g, scaleFactor );

			// Test for trivial condition of range = 0 and pick a suitable default
			if ( _max - _min < 1.0e-20 )
			{
				if ( _maxAuto )
					_max = _max + 0.2 * ( _max == 0 ? 1.0 : Math.Abs( _max ) );
				if ( _minAuto )
					_min = _min - 0.2 * ( _min == 0 ? 1.0 : Math.Abs( _min ) );
			}

			// Calculate the new step size
			if ( _majorStepAuto )
			{
				double targetSteps = ( _ownerAxis is XAxis ) ? Default.TargetXSteps : Default.TargetYSteps;

				// Calculate the step size based on target steps
				_majorStep = CalcDateStepSize( _max - _min, targetSteps );

				if ( _isPreventLabelOverlap )
				{
					// Calculate the maximum number of labels
					double maxLabels = (double) this.CalcMaxLabels( g, pane, scaleFactor );

					if ( maxLabels < this.CalcNumTics() )
						_majorStep = CalcDateStepSize( _max - _min, maxLabels );
				}
			}

			// Calculate the scale minimum
			if ( _minAuto )
				_min = CalcEvenStepDate( _min, -1 );

			// Calculate the scale maximum
			if ( _maxAuto )
				_max = CalcEvenStepDate( _max, 1 );

			_mag = 0;		// Never use a magnitude shift for date scales
			//this.numDec = 0;		// The number of decimal places to display is not used

		}

		/// <summary>
        /// Calculate a step size for a <see cref="AxisType.DateMulti"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="range">The range of data in units of days</param>
		/// <param name="targetSteps">The desired "typical" number of steps
		/// to divide the range into</param>
		/// <returns>The calculated step size for the specified data range.  Also
		/// calculates and sets the values for <see cref="Scale.MajorUnit"/>,
		/// <see cref="Scale.MinorUnit"/>, <see cref="Scale.MinorStep"/>, and
		/// <see cref="Scale.Format"/></returns>
		protected double CalcDateStepSize( double range, double targetSteps )
		{
			return CalcDateStepSize( range, targetSteps, this );
		}

		/// <summary>
        /// Calculate a step size for a <see cref="AxisType.DateMulti"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="range">The range of data in units of days</param>
		/// <param name="targetSteps">The desired "typical" number of steps
		/// to divide the range into</param>
		/// <param name="scale">
		/// The <see cref="Scale" /> object on which to calculate the Date step size.</param>
		/// <returns>The calculated step size for the specified data range.  Also
		/// calculates and sets the values for <see cref="Scale.MajorUnit"/>,
		/// <see cref="Scale.MinorUnit"/>, <see cref="Scale.MinorStep"/>, and
		/// <see cref="Scale.Format"/></returns>
		internal static double CalcDateStepSize( double range, double targetSteps, Scale scale )
		{
			// Calculate an initial guess at step size
			double tempStep = range / targetSteps;

			if ( range > Default.RangeYearYear )
			{
				scale._majorUnit = DateUnit.Year;
				if ( scale._formatAuto )
					scale._format = Default.FormatYearYear;

				tempStep = Math.Ceiling( tempStep / 365.0 );

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Year;
					if ( tempStep == 1.0 )
						scale._minorStep = 0.25;
					else
						scale._minorStep = Scale.CalcStepSize( tempStep, targetSteps );
				}
			}
			else if ( range > Default.RangeYearMonth )
			{
				scale._majorUnit = DateUnit.Year;
				if ( scale._formatAuto )
					scale._format = Default.FormatYearMonth;
				tempStep = Math.Ceiling( tempStep / 365.0 );

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Month;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale._minorStep = Math.Ceiling( range / ( targetSteps * 3 ) / 30.0 );
					// make sure the minorStep is 1, 2, 3, 6, or 12 months
					if ( scale._minorStep > 6 )
						scale._minorStep = 12;
					else if ( scale._minorStep > 3 )
						scale._minorStep = 6;
				}
			}
			else if ( range > Default.RangeMonthMonth )
			{
				scale._majorUnit = DateUnit.Month;
				if ( scale._formatAuto )
					scale._format = Default.FormatMonthMonth;
				tempStep = Math.Ceiling( tempStep / 30.0 );

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Month;
					scale._minorStep = tempStep * 0.25;
				}
			}
			else if ( range > Default.RangeDayDay )
			{
				scale._majorUnit = DateUnit.Day;
				if ( scale._formatAuto )
					scale._format = Default.FormatDayDay;
				tempStep = Math.Ceiling( tempStep );

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Day;
					scale._minorStep = tempStep * 0.25;
					// make sure the minorStep is 1, 2, 3, 6, or 12 hours
				}
			}
			else if ( range > Default.RangeDayHour )
			{
				scale._majorUnit = DateUnit.Day;
				if ( scale._formatAuto )
					scale._format = Default.FormatDayHour;
				tempStep = Math.Ceiling( tempStep );

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Hour;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale._minorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.HoursPerDay );
					// make sure the minorStep is 1, 2, 3, 6, or 12 hours
					if ( scale._minorStep > 6 )
						scale._minorStep = 12;
					else if ( scale._minorStep > 3 )
						scale._minorStep = 6;
					else
						scale._minorStep = 1;
				}
			}
			else if ( range > Default.RangeHourHour )
			{
				scale._majorUnit = DateUnit.Hour;
				tempStep = Math.Ceiling( tempStep * XDate.HoursPerDay );
				if ( scale._formatAuto )
					scale._format = Default.FormatHourHour;

				if ( tempStep > 12.0 )
					tempStep = 24.0;
				else if ( tempStep > 6.0 )
					tempStep = 12.0;
				else if ( tempStep > 2.0 )
					tempStep = 6.0;
				else if ( tempStep > 1.0 )
					tempStep = 2.0;
				else
					tempStep = 1.0;

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Hour;
					if ( tempStep <= 1.0 )
						scale._minorStep = 0.25;
					else if ( tempStep <= 6.0 )
						scale._minorStep = 1.0;
					else if ( tempStep <= 12.0 )
						scale._minorStep = 2.0;
					else
						scale._minorStep = 4.0;
				}
			}
			else if ( range > Default.RangeHourMinute )
			{
				scale._majorUnit = DateUnit.Hour;
				tempStep = Math.Ceiling( tempStep * XDate.HoursPerDay );

				if ( scale._formatAuto )
					scale._format = Default.FormatHourMinute;

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Minute;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale._minorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.MinutesPerDay );
					// make sure the minorStep is 1, 5, 15, or 30 minutes
					if ( scale._minorStep > 15.0 )
						scale._minorStep = 30.0;
					else if ( scale._minorStep > 5.0 )
						scale._minorStep = 15.0;
					else if ( scale._minorStep > 1.0 )
						scale._minorStep = 5.0;
					else
						scale._minorStep = 1.0;
				}
			}
			else if ( range > Default.RangeMinuteMinute )
			{
				scale._majorUnit = DateUnit.Minute;
				if ( scale._formatAuto )
					scale._format = Default.FormatMinuteMinute;

				tempStep = Math.Ceiling( tempStep * XDate.MinutesPerDay );
				// make sure the minute step size is 1, 5, 15, or 30 minutes
				if ( tempStep > 15.0 )
					tempStep = 30.0;
				else if ( tempStep > 5.0 )
					tempStep = 15.0;
				else if ( tempStep > 1.0 )
					tempStep = 5.0;
				else
					tempStep = 1.0;

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Minute;
					if ( tempStep <= 1.0 )
						scale._minorStep = 0.25;
					else if ( tempStep <= 5.0 )
						scale._minorStep = 1.0;
					else
						scale._minorStep = 5.0;
				}
			}
			else if ( range > Default.RangeMinuteSecond )
			{
				scale._majorUnit = DateUnit.Minute;
				tempStep = Math.Ceiling( tempStep * XDate.MinutesPerDay );

				if ( scale._formatAuto )
					scale._format = Default.FormatMinuteSecond;

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Second;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale._minorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.SecondsPerDay );
					// make sure the minorStep is 1, 5, 15, or 30 seconds
					if ( scale._minorStep > 15.0 )
						scale._minorStep = 30.0;
					else if ( scale._minorStep > 5.0 )
						scale._minorStep = 15.0;
					else if ( scale._minorStep > 1.0 )
						scale._minorStep = 5.0;
					else
						scale._minorStep = 1.0;
				}
			}
			else // SecondSecond
			{
				scale._majorUnit = DateUnit.Second;
				if ( scale._formatAuto )
					scale._format = Default.FormatSecondSecond;

				tempStep = Math.Ceiling( tempStep * XDate.SecondsPerDay );
				// make sure the second step size is 1, 5, 15, or 30 seconds
				if ( tempStep > 15.0 )
					tempStep = 30.0;
				else if ( tempStep > 5.0 )
					tempStep = 15.0;
				else if ( tempStep > 1.0 )
					tempStep = 5.0;
				else
					tempStep = 1.0;

				if ( scale._minorStepAuto )
				{
					scale._minorUnit = DateUnit.Second;
					if ( tempStep <= 1.0 )
						scale._minorStep = 0.25;
					else if ( tempStep <= 5.0 )
						scale._minorStep = 1.0;
					else
						scale._minorStep = 5.0;
				}
			}

			return tempStep;
		}

		/// <summary>
		/// Calculate a date that is close to the specified date and an
		/// even multiple of the selected
        /// <see cref="Scale.MajorUnit"/> for a <see cref="AxisType.DateMulti"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="date">The date which the calculation should be close to</param>
		/// <param name="direction">The desired direction for the date to take.
		/// 1 indicates the result date should be greater than the specified
		/// date parameter.  -1 indicates the other direction.</param>
		/// <returns>The calculated date</returns>
		protected double CalcEvenStepDate( double date, int direction )
		{
			int year, month, day, hour, minute, second;

			XDate.XLDateToCalendarDate( date, out year, out month, out day,
										out hour, out minute, out second );

			// If the direction is -1, then it is sufficient to go to the beginning of
			// the current time period, .e.g., for 15-May-95, and monthly steps, we
			// can just back up to 1-May-95
			if ( direction < 0 )
				direction = 0;

			switch ( _majorUnit )
			{
				case DateUnit.Year:
				default:
					// If the date is already an exact year, then don't step to the next year
					if ( direction == 1 && month == 1 && day == 1 && hour == 0
						&& minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year + direction, 1, 1,
														0, 0, 0 );
				case DateUnit.Month:
					// If the date is already an exact month, then don't step to the next month
					if ( direction == 1 && day == 1 && hour == 0
						&& minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month + direction, 1,
												0, 0, 0 );
				case DateUnit.Day:
					// If the date is already an exact Day, then don't step to the next day
					if ( direction == 1 && hour == 0 && minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month,
											day + direction, 0, 0, 0 );
				case DateUnit.Hour:
					// If the date is already an exact hour, then don't step to the next hour
					if ( direction == 1 && minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month, day,
													hour + direction, 0, 0 );
				case DateUnit.Minute:
					// If the date is already an exact minute, then don't step to the next minute
					if ( direction == 1 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month, day, hour,
													minute + direction, 0 );
				case DateUnit.Second:
					return XDate.CalendarDateToXLDate( year, month, day, hour,
													minute, second + direction );

			}
		}

		/// <summary>
        /// Make a value label for an <see cref="AxisType.DateMulti" /> <see cref="Axis" />.
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
		override internal string MakeLabel( GraphPane pane, int index, double dVal )
		{
			if ( _format == null )
				_format = Scale.Default.Format;

			return XDate.ToString( dVal, _format );
		}

		/// <summary>
		/// Gets the major unit multiplier for this scale type, if any.
		/// </summary>
		/// <remarks>The major unit multiplier will correct the units of
		/// <see cref="Scale.MajorStep" /> to match the units of <see cref="Scale.Min" />
		/// and <see cref="Scale.Max" />.  This reflects the setting of
		/// <see cref="Scale.MajorUnit" />.
		/// </remarks>
		override internal double MajorUnitMultiplier
		{
			get { return GetUnitMultiple( _majorUnit ); }
		}

		/// <summary>
		/// Gets the minor unit multiplier for this scale type, if any.
		/// </summary>
		/// <remarks>The minor unit multiplier will correct the units of
		/// <see cref="Scale.MinorStep" /> to match the units of <see cref="Scale.Min" />
		/// and <see cref="Scale.Max" />.  This reflects the setting of
		/// <see cref="Scale.MinorUnit" />.
		/// </remarks>
		override internal double MinorUnitMultiplier
		{
			get { return GetUnitMultiple( _minorUnit ); }
		}

		/// <summary>
		/// Internal routine to calculate a multiplier to the selected unit back to days.
		/// </summary>
		/// <param name="unit">The unit type for which the multiplier is to be
		/// calculated</param>
		/// <returns>
		/// This is ratio of days/selected unit
		/// </returns>
		private double GetUnitMultiple( DateUnit unit )
		{
			switch ( unit )
			{
				case DateUnit.Year:
				default:
					return 365.0;
				case DateUnit.Month:
					return 30.0;
				case DateUnit.Day:
					return 1.0;
				case DateUnit.Hour:
					return 1.0 / XDate.HoursPerDay;
				case DateUnit.Minute:
					return 1.0 / XDate.MinutesPerDay;
				case DateUnit.Second:
					return 1.0 / XDate.SecondsPerDay;
			}
		}

        /// <summary>
        /// Round up to a number in the given array
        /// </summary>
        /// <param name="number">number to round</param>
        /// <param name="roundArray">numbers to round up to</param>
        /// <returns>nearest "round" number that is at least as large as given number</returns>
        private int RoundNumber(int number, int[] roundArray)
        {
            foreach (int i in roundArray)
            {
                if (number <= i)
                {
                    return i;
                }
            }
            return roundArray[roundArray.GetUpperBound(0)];
        }

        private float GetTextVerticalCenter(Graphics g, GraphPane pane,
                float scaledTic, float labelHeight, float shift, float scaleFactor)
        {
            // get the Y position of the center of the axis labels
            // (the axis itself is referenced at zero)
            SizeF maxLabelSize = GetScaleMaxSpace(g, pane, scaleFactor, true);
            float charHeight = _fontSpec.GetHeight(scaleFactor);
            float maxSpace = maxLabelSize.Height;
            float textTop, textVerticalCenter;
            if (_ownerAxis.MajorTic.IsOutside)
                textTop = scaledTic + charHeight * _labelGap;
            else
                textTop = charHeight * _labelGap;

            if (_align == AlignP.Center)
                textVerticalCenter = textTop + maxSpace / 2.0F;
            else if (_align == AlignP.Outside)
                textVerticalCenter = textTop + maxSpace - labelHeight / 2.0F;
            else	// inside
                textVerticalCenter = textTop + labelHeight / 2.0F;

            if (_isLabelsInside)
                textVerticalCenter = shift - textVerticalCenter;
            else
                textVerticalCenter = shift + textVerticalCenter;
            return textVerticalCenter;
        }

        //private sub TruncateInterval(out double dStartInterval,
        //                     out double dEndInterval,
        //                     out bool movedStart,
        //                     out bool movedEnd)
        //{
        //    if (dStartInterval < _minLinTemp)
        //    {
        //        dStartInterval = _minLinTemp;
        //        movedStart = true;
        //    }
        //    else
        //        movedStart = false;

        //    if (dEndInterval > _maxLinTemp)
        //    {
        //        dEndInterval = _maxLinTemp;
        //        movedEnd = true;
        //    }
        //    else
        //        movedEnd = false;
        //}

        private void TruncateIntervalDrawTicGrid(Graphics g, 
                                                 GraphPane pane,
                                                 float topPix,
                                                 MinorTic tic,
                                                 MinorGrid grid,
                                                 float shift,
                                                 float scaleFactor,
                                                 float scaledTic,
                                                 ref double dStartInterval,
                                                 ref double dEndInterval,
                                                 out float pStartInterval,
                                                 out float pEndInterval,
                                                 out float pIntervalWidth)
        {
            Pen ticPen = tic.GetPen(pane, scaleFactor);
            Pen gridPen = grid.GetPen(pane, scaleFactor);

            bool movedStart = false;
            bool movedEnd = false;
            if (dStartInterval < _minLinTemp)
            {
                dStartInterval = _minLinTemp;
                movedStart = true;
            }

            if (dEndInterval > _maxLinTemp)
            {
                dEndInterval = _maxLinTemp;
                movedEnd = true;
            }
            pStartInterval = LocalTransform(dStartInterval);
            pEndInterval = LocalTransform(dEndInterval);
            pIntervalWidth = pEndInterval - pStartInterval;
            if (pIntervalWidth > 0)
            {
                if (!movedStart)
                {
                    tic.Draw(g, pane, ticPen, pStartInterval, topPix, shift, scaledTic);
                    grid.Draw(g, gridPen, pStartInterval, topPix);
                }
                if (!movedEnd && pIntervalWidth > 2)
                {
                    tic.Draw(g, pane, ticPen, pEndInterval, topPix, shift, scaledTic);
                    grid.Draw(g, gridPen, pEndInterval, topPix);
                }
            }
        }

        internal void DrawYearLabels(Graphics g, GraphPane pane,
                    float topPix, float rightPix, float shift, float scaleFactor)
        {
            int year;
            int month;
            int day;

            XDate xStartInterval = new XDate(_minLinTemp);
            XDate xEndInterval;
            string labelText = "2000";
            SizeF labelBox = _fontSpec.BoundingBox(g, labelText, scaleFactor);
            float labelWidth = labelBox.Width + 4;
            float labelWidthHalf = labelWidth / 2;

            MajorGrid grid = _ownerAxis._majorGrid;
            MajorTic tic = _ownerAxis._majorTic;
            float scaledTic = tic.ScaledTic(scaleFactor);
            float textVerticalCenter = GetTextVerticalCenter(g, pane, scaledTic, labelBox.Height, shift, scaleFactor);
            float textHorizontalCenter;

            double dStartInterval, dEndInterval;
            float pStartInterval, pEndInterval, pIntervalWidth;

            xStartInterval.GetDate(out year, out month, out day);
            xStartInterval.SetDate(year, 1, 1);
            xEndInterval = new XDate(year + 1, 1, 1);
            dStartInterval = xStartInterval.XLDate;
            dEndInterval = xEndInterval.XLDate;

            pStartInterval = LocalTransform(dStartInterval);
            pEndInterval = LocalTransform(dEndInterval);
            pIntervalWidth = pEndInterval - pStartInterval;

            int yearsPerLabel = RoundNumber((int)Math.Ceiling(labelWidth / pIntervalWidth), roundNumbers);
            int yearsPerTic = RoundNumber((int)Math.Ceiling(10 / pIntervalWidth), roundNumbers);

            while (dStartInterval < _maxLinTemp)
            {
                bool labelThisYear = ((year / yearsPerLabel) * yearsPerLabel == year);
                if (labelThisYear || ((year / yearsPerTic) * yearsPerTic == year))
                {
                    TruncateIntervalDrawTicGrid(g, pane, topPix, tic, grid, shift, scaleFactor, scaledTic, 
                        ref dStartInterval, ref dEndInterval, out pStartInterval, out pEndInterval, out pIntervalWidth);
                    if (labelThisYear)
                    {
                        textHorizontalCenter = (pStartInterval + pEndInterval) / 2;
                        //if label will not extend beyond left or right edge of this axis, draw it
                        if ((textHorizontalCenter - labelWidthHalf > 0) &&
                            (textHorizontalCenter + labelWidthHalf) < rightPix)
                        {
                            labelText = year.ToString();
                            _fontSpec.Draw(g, pane, labelText,
                                textHorizontalCenter, textVerticalCenter,
                                AlignH.Center, AlignV.Center, scaleFactor);
                        }
                    }
                }
                year += 1;
                xStartInterval.SetDate(year, 1, 1); // = new XDate(endInterval);
                xEndInterval.SetDate(year + 1, 1, 1);
                dStartInterval = xStartInterval.XLDate;
                dEndInterval = xEndInterval.XLDate;
            }
        }

        internal void DrawMonthLabels(Graphics g, GraphPane pane,
                    float topPix, float rightPix, float shift, float scaleFactor, bool includeYear)
        {
            int year;
            int month;
            int day;

            XDate xStartInterval = new XDate(_minLinTemp);
            XDate xEndInterval;
            string labelText = "0";
            SizeF labelBox = _fontSpec.BoundingBox(g, labelText, scaleFactor);
            float ticShift = (includeYear ? (shift + labelBox.Height / 2) : shift);
            float charWidth = labelBox.Width;

            MinorGrid grid = _ownerAxis._minorGrid;
            MinorTic tic = _ownerAxis._minorTic;
            float scaledTic = tic.ScaledTic(scaleFactor);
            float textVerticalCenter = GetTextVerticalCenter(g, pane, scaledTic, labelBox.Height, shift, scaleFactor);

            double dStartInterval, dEndInterval;
            float pStartInterval, pEndInterval, pIntervalWidth;

            SizeF labelSize;

            xStartInterval.GetDate(out year, out month, out day);
            if (day != 1) xStartInterval.SetDate(year, month, 1);
            xEndInterval = new XDate(xStartInterval);
            xEndInterval.AddMonths(1);
            dStartInterval = xStartInterval.XLDate;
            dEndInterval = xEndInterval.XLDate;

            while (dStartInterval < _maxLinTemp)
            {
                TruncateIntervalDrawTicGrid(g, pane, topPix, tic, grid,
                    ticShift, scaleFactor, scaledTic,
                    ref dStartInterval, ref dEndInterval, out pStartInterval, out pEndInterval, out pIntervalWidth);
                // If the width of the interval is at least wide enough for a character,
                // try displaying a month label
                if (pIntervalWidth > charWidth)
                {
                    // First try to fit whole month name
                    labelText = MonthNamesFull[month - 1];
                    if (includeYear) labelText += " " + year;
                    labelSize = _fontSpec.BoundingBox(g, labelText, scaleFactor);
                    if (labelSize.Width >= pIntervalWidth)
                    {
                        // Next try to fit abbreviated month name
                        labelText = MonthNamesAbbreviated[month - 1];
                        if (includeYear) labelText += " " + year;
                        labelSize = _fontSpec.BoundingBox(g, labelText, scaleFactor);
                        if (labelSize.Width >= pIntervalWidth)
                        {
                            // Finally try to fit first letter of month name
                            labelText = labelText.Substring(0, 1);
                            labelSize = _fontSpec.BoundingBox(g, labelText, scaleFactor);
                            if (labelSize.Width >= pIntervalWidth)
                            {
                                labelText = "";
                            }
                        }
                    }
                    if (labelText.Length > 0 )
                        _fontSpec.Draw(g, pane, labelText,
                            (pStartInterval + pEndInterval) / 2, textVerticalCenter,
                            AlignH.Center, AlignV.Center,
                            scaleFactor);
                }
                xStartInterval.AddMonths(1); // = new XDate(endInterval);
                xEndInterval.AddMonths(1);
                dStartInterval = xStartInterval.XLDate;
                dEndInterval = xEndInterval.XLDate;
                xStartInterval.GetDate(out year, out month, out day);
            }
        }

        internal void DrawDayLabels(Graphics g, GraphPane pane,
            float topPix, float rightPix, float shift, float scaleFactor, bool includeMonthYear)
        {
            int year;
            int month;
            int day;
            XDate xStartInterval = new XDate(_minLinTemp);
            XDate xEndInterval;
            string labelText = "28";
            if (includeMonthYear) labelText = "2000 " + MonthNamesAbbreviated[1] + " 28";
            SizeF labelBox = _fontSpec.BoundingBox(g, labelText, scaleFactor);
            float labelWidth = labelBox.Width + 4;
            float labelWidthHalf = labelWidth / 2;
            float ticShift = (includeMonthYear ? (shift + labelBox.Height / 2) : shift);

            MajorGrid grid = _ownerAxis._majorGrid;
            MajorTic tic = _ownerAxis._majorTic;
            float scaledTic = tic.ScaledTic(scaleFactor);
            float textVerticalCenter = GetTextVerticalCenter(g, pane, scaledTic, labelBox.Height, shift, scaleFactor);
            float textHorizontalCenter;

            double dStartInterval, dEndInterval;
            float pStartInterval, pEndInterval, pIntervalWidth;

            xStartInterval.GetDate(out year, out month, out day);
            xStartInterval.SetDate(year, month, 1);
            xEndInterval = new XDate(year, month, 2);
            dStartInterval = xStartInterval.XLDate;
            dEndInterval = xEndInterval.XLDate;

            pStartInterval = LocalTransform(dStartInterval);
            pEndInterval = LocalTransform(dEndInterval);
            pIntervalWidth = pEndInterval - pStartInterval;

            int daysPerLabel = RoundNumber((int)Math.Ceiling(labelWidth / pIntervalWidth), roundNumbers);

            while (dStartInterval < _maxLinTemp)
            {
                TruncateIntervalDrawTicGrid(g, pane, topPix, tic, grid, 
                    ticShift, scaleFactor, scaledTic, 
                    ref dStartInterval, ref dEndInterval, out pStartInterval, out pEndInterval, out pIntervalWidth);
                    
                if (pIntervalWidth > 1)
                {
                    if (((int)(day / daysPerLabel)) * daysPerLabel == day)
                    {
                        textHorizontalCenter = (pStartInterval + pEndInterval) / 2;
                        //if label will not extend beyond left or right edge of this axis, draw it
                        if ((textHorizontalCenter - labelWidthHalf > 0) &&
                            (textHorizontalCenter + labelWidthHalf) < rightPix)
                        {
                            labelText = day.ToString();
                            if (includeMonthYear) labelText = year + " " + MonthNamesAbbreviated[month - 1] + " " + labelText;
                            _fontSpec.Draw(g, pane, labelText,
                                textHorizontalCenter, textVerticalCenter,
                                AlignH.Center, AlignV.Center, scaleFactor);
                        }
                    }
                }
                xStartInterval.AddDays(1);
                xStartInterval.GetDate(out year, out month, out day); // = new XDate(endInterval);
                dStartInterval = xStartInterval.XLDate;
                xEndInterval.AddDays(1);
                dEndInterval = xEndInterval.XLDate;
            }
        }

        internal void DrawHourMinuteLabels(Graphics g, GraphPane pane,
            float topPix, float rightPix, float shift, float scaleFactor)
        {
            XDate xStartInterval = new XDate(_minLinTemp);
            DateTime dtStartInterval = xStartInterval.DateTime;
            DateTime dtEndInterval;
            string labelText = "24:00";
            SizeF labelBox = _fontSpec.BoundingBox(g, labelText, scaleFactor);
            float labelWidth = labelBox.Width + 4;
            float labelWidthHalf = labelWidth / 2;

            MajorGrid grid = _ownerAxis._majorGrid;
            MajorTic tic = _ownerAxis._majorTic;
            float scaledTic = tic.ScaledTic(scaleFactor);
            float textVerticalCenter = GetTextVerticalCenter(g, pane, scaledTic, labelBox.Height, shift, scaleFactor);

            double dStartInterval, dEndInterval;
            float pStartInterval, pEndInterval, pIntervalWidth;

            int year = dtStartInterval.Year;
            int month = dtStartInterval.Month;
            int day = dtStartInterval.Day;
            int hour = dtStartInterval.Hour;
            int minute = dtStartInterval.Minute;
            int second = dtStartInterval.Second;

            dtStartInterval = new DateTime(year, month, day, 0, 0, 0);
            dStartInterval = dtStartInterval.ToOADate();
            pStartInterval = LocalTransform(dStartInterval);

            dtEndInterval = dtStartInterval.AddMinutes(1);
            dEndInterval = dtEndInterval.ToOADate();
            pEndInterval = LocalTransform(dEndInterval);
            pIntervalWidth = pEndInterval - pStartInterval;

            int minutesPerLabel = RoundNumber((int)Math.Ceiling(labelWidth / pIntervalWidth), roundMinutes);
            dtEndInterval = dtStartInterval.AddMinutes(minutesPerLabel);
            dEndInterval = dtEndInterval.ToOADate();

            while (dStartInterval < _maxLinTemp)
            {
                int minutesHours = hour * 60 + minute;
                dtEndInterval = dtStartInterval.AddMinutes(minutesPerLabel);
                dEndInterval = dtEndInterval.ToOADate();

                TruncateIntervalDrawTicGrid(g, pane, topPix, tic, grid, shift, scaleFactor, scaledTic,
                    ref dStartInterval, ref dEndInterval, out pStartInterval, out pEndInterval, out pIntervalWidth);

                //if label will not extend beyond left or right edge of this axis, draw it
                if ((pStartInterval - labelWidthHalf > 0) &&
                        (pStartInterval + labelWidthHalf) < rightPix)
                {
                    labelText = hour.ToString() + ":" + ((minute < 10) ? "0" : "") + minute.ToString();
                    _fontSpec.Draw(g, pane, labelText,
                        pStartInterval, textVerticalCenter,
                        AlignH.Center, AlignV.Center, scaleFactor);
                }               
                dtStartInterval = dtStartInterval.AddMinutes(minutesPerLabel);
                hour = dtStartInterval.Hour;
                minute = dtStartInterval.Minute;
                dStartInterval = dtStartInterval.ToOADate();
            }
        }

        /// <summary>
        /// Draw the scale, including the tic marks, value labels, and grid lines as
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
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="GraphPane"/> object using the
        /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="shiftPos">
        /// The number of pixels to shift to account for non-primary axis position (e.g.,
        /// the second, third, fourth, etc. <see cref="YAxis" /> or <see cref="Y2Axis" />.
        /// </param>
        override internal void Draw(Graphics g, GraphPane pane, float scaleFactor, float shiftPos)
        {
            MajorGrid majorGrid = _ownerAxis._majorGrid;
            MajorTic majorTic = _ownerAxis._majorTic;
            MinorTic minorTic = _ownerAxis._minorTic;
            float charWidth = _fontSpec.GetWidth(g, scaleFactor);

            float rightPix,
                    topPix;

            if (_ownerAxis is XAxis)
            {
                rightPix = pane.Chart._rect.Width;
                topPix = -pane.Chart._rect.Height;
            }
            else
            {
                rightPix = pane.Chart._rect.Height;
                topPix = -pane.Chart._rect.Width;
            }

            // sanity check
            if (_min >= _max)
                return;

            // if the step size is outrageous, then quit
            // (step size not used for log scales)
            if (!IsLog)
            {
                if (_majorStep <= 0 || _minorStep <= 0)
                    return;

                double tMajor = (_max - _min) / (_majorStep * MajorUnitMultiplier);
                double tMinor = (_max - _min) / (_minorStep * MinorUnitMultiplier);

                if (tMajor > 1000 ||
                    ((minorTic.IsOutside || minorTic.IsInside || minorTic.IsOpposite)
                    && tMinor > 5000))
                    return;
            }

            Pen pen = new Pen(_ownerAxis.Color,
                        pane.ScaledPenWidth(majorTic._penWidth, scaleFactor));

            // redraw the axis border
            if (_ownerAxis.IsAxisSegmentVisible)
                g.DrawLine(pen, 0.0F, shiftPos, rightPix, shiftPos);

            // Draw a zero-value line if needed
            if (majorGrid._isZeroLine && _min < 0.0 && _max > 0.0)
            {
                float zeroPix = LocalTransform(0.0);
                g.DrawLine(pen, zeroPix, 0.0F, zeroPix, topPix);
            }

            // draw the time scales that fit best
            double nDays = _maxLinTemp - _minLinTemp;
            bool drawHoursMinutes = (nDays <= 3);
            bool drawDays = (nDays <= 60);
            bool drawMonths = (nDays > 3) && (nDays < 666);
            bool drawYears = (nDays > 60);

            int numLevels = (drawHoursMinutes ? 1 : 0) 
                          + (drawDays ? 1 : 0) 
                          + (drawMonths ? 1 : 0) 
                          + (drawYears ? 1 : 0);

            _ownerAxis.SetMinSpaceBuffer(g, pane, numLevels, false);

            _ownerAxis._majorTic.IsInside = true;
            _ownerAxis._majorTic.IsOpposite = true;
            _ownerAxis._minorTic.IsInside = true;
            _ownerAxis._minorTic.IsOpposite = true;
            if (drawHoursMinutes)
            {
                DrawHourMinuteLabels(g, pane, topPix, rightPix, shiftPos, scaleFactor);
                shiftPos += _fontSpec.GetHeight(scaleFactor) + 2;
            }

            if (drawDays)
            {
                DrawDayLabels(g, pane, topPix, rightPix, shiftPos, scaleFactor, !drawMonths);
                shiftPos += _fontSpec.GetHeight(scaleFactor) + 2;
            }

            if (drawMonths)
            {
                DrawMonthLabels(g, pane, topPix, rightPix, shiftPos, scaleFactor, !drawYears);
                shiftPos += _fontSpec.GetHeight(scaleFactor) + 2;
            }

            if (drawYears)
            {
                DrawYearLabels(g, pane, topPix, rightPix, shiftPos, scaleFactor);
                shiftPos += _fontSpec.GetHeight(scaleFactor) + 2;
            }
            _ownerAxis._majorTic.IsAllTics = false;
            _ownerAxis._minorTic.IsAllTics = false;

            _ownerAxis.DrawTitle(g, pane, shiftPos, scaleFactor);
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
        protected DateMultiScale(SerializationInfo info, StreamingContext context)
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
