using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	public class Circle : Shape, IDraw
	{
		public static readonly int DEGREES_IN_CIRCLE = 360;
		public static readonly int DEGREES_IN_HALF_CIRCLE = 180;
		public static readonly double RADIANS_90DEGREES = Math.PI / 2;
		public static readonly double RADIANS_180DEGREES = Math.PI;
		public static readonly double RADIANS_270DEGREES = 3 * Math.PI / 2;
		public static readonly double RADIANS_360DEGREES = 2 * Math.PI;

		public readonly double X;
		public readonly double Y;
		public readonly double Radius;

		public Point Center { get { return new Point(X, Y); } }
		public double Diameter { get { return 2 * Radius; } }
		public double MaxX { get { return X + Radius; } }
		public double MaxY { get { return Y + Radius; } }

		public double MaxXDegrees {
			get {
				switch(Geometry.CoordinatePlane)
				{
					case Geometry.CoordinatePlanes.Screen: return 0;
					case Geometry.CoordinatePlanes.Paper: return 0;
					default: throw new NotImplementedException("Coordinate plane not supported.");
				}
			}
		}
		public double MaxYDegrees {
			get {
				switch(Geometry.CoordinatePlane)
				{
					case Geometry.CoordinatePlanes.Screen: return 90;
					case Geometry.CoordinatePlanes.Paper: return 270;
					default: throw new NotImplementedException("Coordinate plane not supported.");
				}
			}
		}

		public Circle(double x, double y, double radius)
		{
			X = x;
			Y = y;
			Radius = radius;
		}

		public Circle(Point center, double radius)
		{
			X = center.X;
			Y = center.Y;
			Radius = radius;
		}

		/// <summary>
		/// Returns null (no intersection), an array of length 1, or an array of length 2.
		/// </summary>
		public Point[] GetIntersectionPoints(Circle b)
		{
			//following the method in math/intersectionCircleCircle.png

			Circle a = this;
			double d = a.Center.Distance(b.Center); //distance between centers
			if(d > a.Radius + b.Radius)
			{
				return null; //circles too far apart to intersect
			}
			if(a.ContainsOrIsContained(b))
			{
				return null; //one circle is wholly inside the other
			}

			//the radical line is the line between the two intersecting points of the circles
			//Point c is the center of the radical line, which is also on the line between the centers
			double dA = (Math.Pow(a.Radius, 2) - Math.Pow(b.Radius, 2) + Math.Pow(d, 2)) / (2 * d); //distance from centerA to pointC
			if(dA == a.Radius)
			{
				return new Point[] { Geometry.PointOnLine(a.Center, b.Center, a.Radius) }; //circles intersect at single point
			}
			Point c = a.Center + dA * (b.Center - a.Center) / d;

			//h is the distance from pointC to either intersection point (the hypotenus of triangle centerA-C-intersection)
			double h = Math.Sqrt(Math.Pow(a.Radius, 2) - Math.Pow(dA, 2));

			return new Point[] {
				new Point(c.X + (h * (b.Y - a.Y) / d), c.Y - h * (b.X - a.X) / d),
				new Point(c.X - (h * (b.Y - a.Y) / d), c.Y + h * (b.X - a.X) / d)
			};
		}

		/// <summary>
		/// Find the two tangent points on the circle that form lines to point B.
		/// </summary>
		public Point[] GetTangentPoints(Point b)
		{
			//point C and D are the tangents
			Circle a = this;
			double distanceAB = a.Center.Distance(b);
			double degreesAB = DegreesAtPoint(b);
			double degreesAB_AC = Math.Cos(Radius / distanceAB);
			Point c = PointAtDegrees(degreesAB + degreesAB_AC);
			Point d = PointAtDegrees(degreesAB - degreesAB_AC);
			return new Point[] { c, d };
		}

		/// <summary>
		/// Any part of this circle overlaps any part of circle B.
		/// </summary>
		public bool Overlaps(Circle b)
		{
			Point[] intersections = this.GetIntersectionPoints(b);
			if(intersections != null)
				return true;
			return this.ContainsOrIsContained(b);
		}

		/// <summary>
		/// Any part of this circle overlaps any part of line segment B.
		/// If line B lies within the circle, that counts as overlapping.
		/// </summary>
		public bool Overlaps(LineSegment b)
		{
			Point[] lineIntersectionPoints = GetIntersectionPoints(b.ToLine());
			if(lineIntersectionPoints == null)
				return false;
			foreach(Point point in lineIntersectionPoints)
			{
				if(b.Overlaps(point))
					return true;
			}
			if(lineIntersectionPoints.Length == 2)
			{
				//if b lies entirely within the circle
				LineSegment intersectionLine = new LineSegment(lineIntersectionPoints[0], lineIntersectionPoints[1]);
				if(intersectionLine.Length > b.Length && intersectionLine.Overlaps(b))
					return true;
			}
			return false;
		}

		/// <summary>
		/// This circle entirely contains circle B, or B entirely contains this circle, or they exactly overlap.
		/// </summary>
		public bool ContainsOrIsContained(Circle b)
		{
			if(this.Radius > b.Radius)
				return this.Contains(b);
			return b.Contains(this);
		}

		/// <summary>
		/// This circle entirely contains circle B, or they exactly overlap.
		/// </summary>
		public bool Contains(Circle b)
		{
			if(b.Radius > this.Radius)
				return false;
			if(b.Radius == this.Radius)
				return (b.Center == this.Center);
			double d = this.Center.Distance(b.Center);
			return (this.Radius >= d + b.Radius);
		}

		/// <summary>
		/// This circle entirely contains wedge B.
		/// </summary>
		public bool Contains(Wedge b)
		{
			foreach(Point point in b.FourPoints)
			{
				if(!Contains(point))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Point B lies within or on this circle.
		/// </summary>
		public bool Contains(Point b)
		{
			return (Center.Distance(b) <= Radius);
		}

		/// <summary>
		/// Return the point on the circle at this radians. 0 radians is East of center, increases clockwise.
		/// </summary>
		public Point PointAtRadians(double radians)
		{
			radians = radians % RADIANS_360DEGREES;
			double deltaX = 0;
			double deltaY = 0;
			if(radians == 0)
			{
				deltaX = Radius;
			}
			else if(radians < RADIANS_90DEGREES)
			{
				deltaX = Math.Cos(radians) * Radius;
				deltaY = Math.Sin(radians) * Radius;
			}
			else if(radians == RADIANS_90DEGREES)
			{
				deltaY = Radius;
			}
			else if(radians < RADIANS_180DEGREES)
			{
				deltaX = -1 * Math.Cos(RADIANS_180DEGREES - radians) * Radius;
				deltaY = Math.Sin(RADIANS_180DEGREES - radians) * Radius;
			}
			else if(radians == RADIANS_180DEGREES)
			{
				deltaX = -1 * Radius;
			}
			else if(radians < RADIANS_270DEGREES)
			{
				deltaX = -1 * Math.Cos(radians - RADIANS_180DEGREES) * Radius;
				deltaY = -1 * Math.Sin(radians - RADIANS_180DEGREES) * Radius;
			}
			else if(radians == RADIANS_270DEGREES)
			{
				deltaY = -1 * Radius;
			}
			else //radians < Math.PI * 2
			{
				deltaX = Math.Cos(RADIANS_360DEGREES - radians) * Radius;
				deltaY = -1 * Math.Sin(RADIANS_360DEGREES - radians) * Radius;
			}

			switch(Geometry.CoordinatePlane)
			{
				case Geometry.CoordinatePlanes.Screen: return new Point(Center.X + deltaX, Center.Y + deltaY);
				case Geometry.CoordinatePlanes.Paper: return new Point(Center.X + deltaX, Center.Y - deltaY);
				default: throw new NotImplementedException("Coordinate plane not supported.");
			}
		}

		/// <summary>
		/// Return the point on the circle at this degree. 0 degrees is East of center, increases clockwise.
		/// </summary>
		public Point PointAtDegrees(double degrees)
		{
			return PointAtRadians(DegreesToRadians(degrees));
		}

		/// <summary>
		/// Given a line from the center of a circle to a point, what degrees is the line angle at? 0 degrees is East from center, and increases clockwise.
		/// </summary>
		public double DegreesAtPoint(Point lineEnd)
		{
			if(Geometry.CoordinatePlane == Geometry.CoordinatePlanes.None)
				throw new ArgumentException("Coordinate plane required.");

			Geometry.Direction direction = Geometry.LineDirection(Center, lineEnd);
			switch(direction)
			{
				case Geometry.Direction.East: return 0;
				case Geometry.Direction.South: return 90;
				case Geometry.Direction.West: return 180;
				case Geometry.Direction.North: return 270;
			}

			double lineLength = Center.Distance(lineEnd);
			double radians = Math.Abs(Math.Asin((lineEnd.Y - Center.Y) / lineLength));
			double degrees = Shapes.Circle.RadiansToDegrees(radians) % DEGREES_IN_CIRCLE;
			switch(direction)
			{
				case Geometry.Direction.SouthEast: return degrees;
				case Geometry.Direction.SouthWest: return DEGREES_IN_HALF_CIRCLE - degrees;
				case Geometry.Direction.NorthWest: return DEGREES_IN_HALF_CIRCLE + degrees;
				case Geometry.Direction.NorthEast: return DEGREES_IN_CIRCLE - degrees;
			}

			throw new NotImplementedException(String.Format("Direction not supported: {0}.", direction));
		}

		/// <summary>
		/// Returns null (no intercepts), or array of length 1 or 2.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public Point[] GetIntersectionPoints(Line line)
		{
			//line does not intersect if perpendicular line from circle-center to line is longer than circle-radius
			Point perpendicularToCenter = line.GetPerpendicularIntersect(Center);
			if(perpendicularToCenter.Distance(Center) > Radius)
				return null;

			//equation of circle with radius r and center (h, k)
			//is (x - h)^2 + (y - k)^2 = r^2

			//line: y = mx + b
			//circle: (x - h)^2 + (y - k)^2 = r^2
			//substitute y: (x - h)^2 + (mx + b - k)^2 = r^2
			//expand: x^2 - 2hx + h^2 + m^2x^2 + 2(b - k)mx + (b - k)^2 - r^2 = 0
			//group: (1 + m^2)x^2 + (-2h + 2(b - k)m)x + (h^2 + (b - k)^2 - r^2) = 0
			//quadratic equation: if 0 = Ax^2 + Bx + C, then x = (-B +- sqrt(B^2 - 4AC)) / 2A
			double A = 1 + Math.Pow(line.Slope, 2);
			double B = (-2 * Center.X) + (2 * (line.YIntercept - Center.Y) * line.Slope);
			double C = Math.Pow(Center.X, 2) + Math.Pow(line.YIntercept - Center.Y, 2) - Math.Pow(Radius, 2);

			double x1 = (-1*B + Math.Sqrt(Math.Pow(B, 2) - (4 * A * C))) / (2 * A);
			double x2 = (-1*B - Math.Sqrt(Math.Pow(B, 2) - (4 * A * C))) / (2 * A);
			double y1 = line.Slope * x1 + line.YIntercept;
			double y2 = line.Slope * x2 + line.YIntercept;
			if(line.IsVertical)
			{
				x1 = line.A.X;
				x2 = line.A.X;
				//must use circle equation instead of line equation to find y's
				y1 = Center.Y + Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x1 - Center.X, 2));
				y2 = Center.Y - Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x1 - Center.X, 2));
			}
			if(line.IsHorizontal)
			{
				y1 = line.A.Y;
				y2 = line.A.Y;
			}
			Point point1 = new Point(x1, y1);
			Point point2 = new Point(x2, y2);
			List<Point> result = new List<Point>() { point1 };
			if(point1 != point2)
				result.Add(point2);
			return result.ToArray();
		}

		public Point[] GetIntersectionPoints(LineSegment lineSegment)
		{
			Point[] lineIntersectionPoints = GetIntersectionPoints(lineSegment.ToLine());
			if(lineIntersectionPoints == null)
				return null;
			List<Point> segmentIntersectionPoints = new List<Point>();
			foreach(Point point in lineIntersectionPoints)
			{
				if(lineSegment.Overlaps(point))
					segmentIntersectionPoints.Add(point);
			}
			if(segmentIntersectionPoints.Count == 0)
				return null;
			return segmentIntersectionPoints.ToArray();
		}

		public static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / Math.PI;
		}

		/// <summary>
		/// Scale circle down by B amount. Affects length and location measures.
		/// </summary>
		public static Circle operator /(Circle a, double b)
		{
			return new Circle(a.Center / b, a.Radius / b);
		}

		public static bool operator ==(Circle a, Circle b)
		{
			return (a.Center == b.Center && a.Radius == b.Radius);
		}

		public static bool operator !=(Circle a, Circle b)
		{
			return (a.Center != b.Center || a.Radius != b.Radius);
		}

		public override bool Equals(Object b)
		{
			if(b != null && b is Circle)
			{
				return (this == (Circle)b);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Center.GetHashCode() ^ Radius.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("C:({0},{1}) R:{2}", X, Y, Radius);
		}

		public void Paint(Graphics graphics, Pen pen, double unitsToPixels)
		{
			graphics.DrawArc(pen, 
				(float)((X - Radius) * unitsToPixels), 
				(float)((Y - Radius) * unitsToPixels), 
				(float)(Diameter * unitsToPixels), 
				(float)(Diameter * unitsToPixels), 
				0, 
				DEGREES_IN_CIRCLE); 
		}
	}
}
