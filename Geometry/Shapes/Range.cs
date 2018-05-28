using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	//todo: should I rename Start and End to Min and Max?
	public class Range : Shape
	{
		/// <summary>
		/// Operations do not assume that Start is the minimum value.
		/// </summary>
		public readonly double Start;
		public readonly double End;

		public double Span { get { return End - Start; } }

		public Range(double s, double e)
		{
			Start = s;
			End = e;
		}

		public static Range Centered(double center, double span)
		{
			return new Range(center - (span / 2), center + (span / 2));
		}

		/// <summary>
		/// Returns number modulus m. Ensures a positive result.
		/// </summary>
		public static double Mod(double number, int m)
		{
			if(m <= 0)
				throw new Exception("Requires a positive, non-zero m.");
			while(number < 0)
				number += m;
			return number % m;
		}
	}
}
