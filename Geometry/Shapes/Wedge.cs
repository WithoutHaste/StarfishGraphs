using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	/// <summary>
	/// A wedge (aka circular sector) is a slice of a circle.
	/// </summary>
	public struct Wedge
	{
		public Circle Circle;
		public Range Degrees;

		public Wedge(Circle c, Range r)
		{
			Circle = c;
			Degrees = r;
		}

		public Wedge(Circle c, double rangeStart, double rangeEnd)
		{
			Circle = c;
			Degrees = new Range(rangeStart, rangeEnd);
		}
	}
}
