
class ChannelPoint //implements Comparable<ChannelPoint>  // <---  note <Pair>
{
	//Double values describing distance from bank and depth
	double _distance = 0.0;
	double _depth = 0.0;


	ChannelPoint( double distance, double depth )
	{
		_distance = distance;
		_depth = depth;
	}
	
	public double getDistance() {return _distance;}
	public void setDistance(double distance){_distance = distance;}
	
	public double getDepth() {return _depth;}
	public void setDepth(double depth){_depth = depth;}

/**
 * Compare two Pairs. Callback for natural order sort.
 * effectively returns a-b;
 * @return +1, a>b, 0 a=b, -1 a<b, case insensitive
 */
	public int compareTo ( ChannelPoint b )   // <--- note Pair, not Object
	{
		if (_distance > b.getDistance())
			return 1;		
		else if (_distance < b.getDistance())
			return -1;
		else
			return 0;
		
		
	} // end compareTo

} // end class Pair


