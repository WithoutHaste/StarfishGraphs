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
	public struct WedgeUnbound
	{
		public Point Center;
		public Range Degrees;

		public double Span { get { return Degrees.Span; } }
		public double Start { get { return Degrees.Start; } }

		public WedgeUnbound(Point c, Range r)
		{
			Center = c;
			Degrees = r;
		}

		public WedgeUnbound(Point c, double rangeStart, double rangeEnd)
		{
			Center = c;
			Degrees = new Range(rangeStart, rangeEnd);
		}
	}
}
