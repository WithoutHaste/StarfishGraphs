using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	public struct Range
	{
		public double Start;
		public double End;

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
	}
}
