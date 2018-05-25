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
	public class Wedge : Shape, IDraw
	{
		public readonly Circle Circle;
		/// <summary>
		/// 0 degrees is East of center, increases clockwise.
		/// </summary>
		public readonly Range Degrees;

		public Point StartPoint { get { return Circle.PointAtDegrees(Degrees.Start); } }
		public Point EndPoint { get { return Circle.PointAtDegrees(Degrees.End); } }
		public LineSegment[] LineEdges { get { return new LineSegment[] { new LineSegment(Circle.Center, StartPoint), new LineSegment(Circle.Center, EndPoint) }; } }
		public double MaxX {
			get {
				double maxX = Math.Max(StartPoint.X, EndPoint.X);
				if(DegreesContains(Circle.MaxXDegrees))
					maxX = Math.Max(maxX, Circle.Center.X + Circle.Radius);
				else
					maxX = Math.Max(maxX, Circle.Center.X);
				return maxX;
			}
		}
		public double MaxY {
			get {
				double maxY = Math.Max(StartPoint.Y, EndPoint.Y);
				if(DegreesContains(Circle.MaxYDegrees))
					maxY = Math.Max(maxY, Circle.Center.Y + Circle.Radius);
				else
					maxY = Math.Max(maxY, Circle.Center.Y);
				return maxY;
			}
		}

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
			if(Contains(b.Circle.Center) && Contains(b.StartPoint) && Contains(b.EndPoint))
				return true;

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
				double degrees = Geometry.DegreesOfLine(Circle.Center, point, Circle.CoordinatePlane);
				if(DegreesContains(degrees))
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
				double degreesA = Geometry.DegreesOfLine(a.Circle.Center, point, a.Circle.CoordinatePlane);
				if(a.DegreesContains(degreesA))
				{
					double degreesB = Geometry.DegreesOfLine(b.Circle.Center, point, b.Circle.CoordinatePlane);
					if(b.DegreesContains(degreesB))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// This wedge contains point B, including point B being on an edge of the wedge.
		/// </summary>
		public bool Contains(Point b)
		{
			double distance = Circle.Center.Distance(b);
			if(distance > Circle.Radius)
				return false;
			double degrees = Geometry.DegreesOfLine(Circle.Center, b, Circle.CoordinatePlane);
			return DegreesContains(degrees);
		}

		public bool DegreesContains(double degree)
		{
			return (Degrees.Start % 360 <= degree && Degrees.End % 360 >= degree);
		}

		public void Paint(Graphics graphics, Pen pen, double unitsToPixels)
		{
			graphics.DrawArc(pen, 
				(float)((Circle.X - Circle.Radius) * unitsToPixels), 
				(float)((Circle.Y - Circle.Radius) * unitsToPixels), 
				(float)(Circle.Diameter * unitsToPixels), 
				(float)(Circle.Diameter * unitsToPixels), 
				(float)Degrees.Start, 
				(float)Degrees.End);
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
	}
}
