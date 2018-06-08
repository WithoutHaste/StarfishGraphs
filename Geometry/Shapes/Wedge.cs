using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	/// <summary>
	/// A wedge (aka circular sector) is a slice of a circle.
	/// </summary>
	public class Wedge : WedgeUnbound, IDraw
	{
		public readonly double Radius;

		public Circle Circle { get { return new Circle(Center, Radius); } }
		public Point StartPoint { get { return Circle.PointAtDegrees(Degrees.Start); } }
		public Point EndPoint { get { return Circle.PointAtDegrees(Degrees.End); } }
		/// <summary>
		/// The point at the center of the arc, furthest from the center.
		/// </summary>
		public Point ArcPoint { get { return Circle.PointAtDegrees(Degrees.Middle); } }
		public Point[] FourPoints { get { return new Point[] { Circle.Center, StartPoint, EndPoint, ArcPoint }; } }
		public LineSegment[] LineEdges { get { return new LineSegment[] { new LineSegment(Circle.Center, StartPoint), new LineSegment(Circle.Center, EndPoint) }; } }
		public double MaxX {
			get {
				double maxX = Math.Max(StartPoint.X, EndPoint.X);
				if(Degrees.Overlaps(Circle.MaxXDegrees))
					maxX = Math.Max(maxX, Circle.Center.X + Circle.Radius);
				else
					maxX = Math.Max(maxX, Circle.Center.X);
				return maxX;
			}
		}
		public double MaxY {
			get {
				double maxY = Math.Max(StartPoint.Y, EndPoint.Y);
				if(Degrees.Overlaps(Circle.MaxYDegrees))
					maxY = Math.Max(maxY, Circle.Center.Y + Circle.Radius);
				else
					maxY = Math.Max(maxY, Circle.Center.Y);
				return maxY;
			}
		}

		public Wedge(Circle c, RangeCircular r) : base(c.Center, r)
		{
			Radius = c.Radius;
		}

		public Wedge(Circle c, double rangeStart, double rangeEnd) : base(c.Center, rangeStart, rangeEnd)
		{
			Radius = c.Radius;
		}

		/// <summary>
		/// Any part of this wedge overlaps any part of circle B.
		/// </summary>
		public bool Overlaps(Circle b)
		{
			Wedge a = this;
			Point[] intersections = a.Circle.GetIntersectionPoints(b);
			if(intersections == null)
			{
				if(!a.Circle.ContainsOrIsContained(b))
				{
					return false;
				}
			}

			//line edge overlaps circle
			foreach(LineSegment lineA in this.LineEdges)
			{
				if(b.Overlaps(lineA))
					return true;
			}
			//arc overlaps circle
			if(a.ArcOverlapsArc(new Wedge(b, 0, Circle.DEGREES_IN_CIRCLE)))
				return true;
			//wedge entirely contains circle or circle entirely contains wedge
			if(a.Contains(b))
				return true;
			if(b.Contains(a))
				return true;

			return false;
		}

		/// <summary>
		/// Any part of this wedge overlaps any part of wedge B.
		/// </summary>
		public bool Overlaps(Wedge b)
		{
			Wedge a = this;
			Point[] intersections = a.Circle.GetIntersectionPoints(b.Circle);
			if(intersections == null)
			{
				if(!a.Circle.ContainsOrIsContained(b.Circle))
				{
					return false;
				}
			}

			//line edge overlaps line edge
			foreach(LineSegment lineA in this.LineEdges)
			{
				foreach(LineSegment lineB in b.LineEdges)
				{
					if(lineA.Overlaps(lineB))
						return true;
				}
			}
			//arc overlaps line edge
			foreach(LineSegment lineA in this.LineEdges)
			{
				if(b.ArcOverlaps(lineA))
					return true;
			}
			foreach(LineSegment lineB in b.LineEdges)
			{
				if(a.ArcOverlaps(lineB))
					return true;
			}
			//arc overlaps arc
			if(a.ArcOverlapsArc(b))
				return true;
			//one wedge entirely contains the other (all three points from one wedge inside the other)
			//	there are cases where all three points are inside, but arc protrudes outside - not important here since that still counts as overlapping
			if(Contains(b.Circle.Center))
			{
				if(Contains(b.StartPoint))
				{
					if(Contains(b.EndPoint))
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// The arc is the curved circle segment part of the wedge.
		/// </summary>
		public bool ArcOverlaps(LineSegment line)
		{
			//find intersection points between full circle and line segment
			Point[] fullCircleIntersections = Circle.GetIntersectionPoints(line);
			if(fullCircleIntersections == null)
				return false;
			//find degrees from circle center to intersection points
			//are any of those degrees within the wedge degree range?
			foreach(Point point in fullCircleIntersections)
			{
				double degrees = Circle.DegreesAtPoint(point);
				if(Degrees.Overlaps(degrees))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// The arc is the curved circle segment part of the wedge.
		/// </summary>
		public bool ArcOverlapsArc(Wedge b)
		{
			Wedge a = this;
			Point[] fullCircleIntersections = a.Circle.GetIntersectionPoints(b.Circle);
			if(fullCircleIntersections == null)
				return false;
			foreach(Point point in fullCircleIntersections)
			{
				if(a.Circle.Center == point || a.Degrees.Overlaps(a.Circle.DegreesAtPoint(point)))
				{
					if(b.Circle.Center == point || b.Degrees.Overlaps(b.Circle.DegreesAtPoint(point)))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Circle B lies entirely within this wedge.
		/// </summary>
		public bool Contains(Circle b)
		{
			Point[] tangentPoints = b.GetTangentPoints(this.Circle.Center);
			foreach(Point point in tangentPoints)
			{
				if(!Contains(point))
					return false;
			}
			return true;
		}

		/// <summary>
		/// This wedge contains point B, including point B being on an edge of the wedge.
		/// </summary>
		public bool Contains(Point b)
		{
			double distance = Circle.Center.Distance(b);
			if(distance > Circle.Radius)
				return false;
			double degrees = Circle.DegreesAtPoint(b);
			return Degrees.Overlaps(degrees);
		}

		/// <summary>
		/// Scale wedge down by B amount. Affects length and location measures, but not degrees.
		/// </summary>
		public static Wedge operator /(Wedge a, double b)
		{
			return new Wedge(a.Circle / b, a.Degrees);
		}

		public void Paint(Graphics graphics, Pen pen, double unitsToPixels)
		{
			graphics.DrawArc(pen, 
				(float)((Circle.X - Circle.Radius) * unitsToPixels), 
				(float)((Circle.Y - Circle.Radius) * unitsToPixels), 
				(float)(Circle.Diameter * unitsToPixels), 
				(float)(Circle.Diameter * unitsToPixels), 
				(float)Degrees.Start, 
				(float)Degrees.Span);
			graphics.DrawLine(pen,
				(float)(Circle.Center.X * unitsToPixels),
				(float)(Circle.Center.Y * unitsToPixels),
				(float)(StartPoint.X * unitsToPixels),
				(float)(StartPoint.Y * unitsToPixels)
			);
			graphics.DrawLine(pen,
				(float)(Circle.Center.X * unitsToPixels),
				(float)(Circle.Center.Y * unitsToPixels),
				(float)(EndPoint.X * unitsToPixels),
				(float)(EndPoint.Y * unitsToPixels)
			);
		}

		public override string ToString()
		{
			return String.Format("{0} Degrees:{1}", Circle, Degrees);
		}
	}
}
