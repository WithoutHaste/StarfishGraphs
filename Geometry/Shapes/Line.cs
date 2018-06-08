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

		/// <summary>
		/// Slope assumes direction from A to B.
		/// </summary>
		public double Slope { get { return ((B.Y - A.Y) / (B.X - A.X)); } }
		public double PerpendicularSlope { get { return -1 * (1 / Slope); } }
		public double YIntercept { get { return A.Y - (Slope * A.X); } }
		public bool IsVertical { get { return (A.X == B.X); } }
		public bool IsHorizontal { get { return (A.Y == B.Y); } }

		public Line(Point a, Point b)
		{
			if(a == b)
				throw new ArgumentException("Points A and B cannot be the same.");
			A = a;
			B = b;
			IsDirected = false;
		}

		public Line(Point a, Point b, bool isDirected)
		{
			if(a == b)
				throw new ArgumentException("Points A and B cannot be the same.");
			A = a;
			B = b;
			IsDirected = isDirected;
		}

		public LineSegment ToLineSegment()
		{
			return new LineSegment(A, B, IsDirected);
		}
		
		//todo: verify that all line operations take vertical and horizontal lines into account

		/// <summary>
		/// Get the point where a perpendicular line passing through point C intersects this line.
		/// </summary>
		public Point GetPerpendicularIntersect(Point c)
		{
			if(IsVertical)
			{
				return new Point(this.A.X, c.Y);
			}
			if(IsHorizontal)
			{
				return new Point(c.X, this.A.Y);
			}
			double cSlope = PerpendicularSlope;
			double cYIntercept = c.Y - (cSlope * c.X);
			double x = (cYIntercept - this.YIntercept) / (this.Slope - cSlope);
			double y = (this.Slope * x) + this.YIntercept;
			return new Point(x, y);
		}

		/// <summary>
		/// Scale line down by B amount. Affects length and location measures.
		/// </summary>
		public static Line operator /(Line a, double b)
		{
			return new Line(a.A / b, a.B / b);
		}

		public override string ToString()
		{
			return String.Format("{0} to {1}", A, B);
		}
	}
}
