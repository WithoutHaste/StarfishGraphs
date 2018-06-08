using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	/// <summary>
	/// A wedge (aka circular sector) is a slice of a circle.
	/// An unbounded wedge is a slice of circle that extends outward with no limit.
	/// </summary>
	public class WedgeUnbound : Shape
	{
		public readonly Point Center;
		public readonly RangeCircular Degrees;

		public double Span { get { return Degrees.Span; } }
		public double Start { get { return Degrees.Start; } }
		public double End { get { return Degrees.End; } }

		public WedgeUnbound(Point c, RangeCircular r)
		{
			Center = c;
			Degrees = r;
		}

		public WedgeUnbound(Point c, double rangeStart, double rangeEnd)
		{
			Center = c;
			Degrees = new RangeCircular(rangeStart, rangeEnd, Circle.DEGREES_IN_CIRCLE);
		}
	}
}
