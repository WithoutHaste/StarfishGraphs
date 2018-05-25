using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	/// <summary>
	/// Line of infinite length passing between points A and B.
	/// </summary>
	public class Line : Shape
	{
		public readonly Point A;
		public readonly Point B;
		/// <summary>
		/// When directed, the direction is A to B.
		/// </summary>
		public readonly bool IsDirected;

		public Line(Point a, Point b)
		{
			A = a;
			B = b;
			IsDirected = false;
		}

		public Line(Point a, Point b, bool isDirected)
		{
			A = a;
			B = b;
			IsDirected = isDirected;
		}

		/// <summary>
		/// Slope assumes direction from A to B.
		/// </summary>
		public double Slope { get { return ((B.Y - A.Y) / (B.X - A.X)); } }
		public double PerpendicularSlope { get { return -1 * (1 / Slope); } }
		public double YIntercept { get { return A.Y - (Slope * A.X); } }

		public LineSegment GetLineSegment()
		{
			return new LineSegment(A, B, IsDirected);
		}

		/// <summary>
		/// Get the point where a perpendicular line passing through point C intersects this line.
		/// </summary>
		public Point GetPerpendicularIntersect(Point c)
		{
			double cSlope = PerpendicularSlope;
			double cYIntercept = c.Y - (cSlope * c.X);
			double x = (cYIntercept - this.YIntercept) / (this.Slope - cSlope);
			double y = (this.Slope * x) - this.YIntercept;
			return new Point(x, y);
		}
	}
}
