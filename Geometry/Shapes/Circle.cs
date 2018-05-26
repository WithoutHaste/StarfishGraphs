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
		public static double RADIANS_90DEGREES = Math.PI / 2;
		public static double RADIANS_180DEGREES = Math.PI;
		public static double RADIANS_270DEGREES = 3 * Math.PI / 2;
		public static double RADIANS_360DEGREES = 2 * Math.PI;

		public readonly double X;
		public readonly double Y;
		public readonly double Radius;
		public readonly Geometry.CoordinatePlane CoordinatePlane;

		public Point Center { get { return new Point(X, Y); } }
		public double Diameter { get { return 2 * Radius; } }
		public double MaxX { get { return X + Radius; } }
		public double MaxY { get { return Y + Radius; } }

		public double MaxXDegrees {
			get {
				switch(CoordinatePlane)
				{
					case Geometry.CoordinatePlane.Screen: return 0;
					case Geometry.CoordinatePlane.Paper: return 0;
					default: throw new Exception("Coordinate plane not supported.");
				}
			}
		}
		public double MaxYDegrees {
			get {
				switch(CoordinatePlane)
				{
					case Geometry.CoordinatePlane.Screen: return 90;
					case Geometry.CoordinatePlane.Paper: return 270;
					default: throw new Exception("Coordinate plane not supported.");
				}
			}
		}

		public Circle(double x, double y, double radius)
		{
			X = x;
			Y = y;
			Radius = radius;
			CoordinatePlane = Geometry.CoordinatePlane.Screen;
		}

		public Circle(Point center, double radius)
		{
			X = center.X;
			Y = center.Y;
			Radius = radius;
			CoordinatePlane = Geometry.CoordinatePlane.Screen;
		}

		public Circle(double x, double y, double radius, Geometry.CoordinatePlane coordinatePlane)
		{
			X = x;
			Y = y;
			Radius = radius;
			CoordinatePlane = coordinatePlane;
		}

		public Circle(Point center, double radius, Geometry.CoordinatePlane coordinatePlane)
		{
			X = center.X;
			Y = center.Y;
			Radius = radius;
			CoordinatePlane = coordinatePlane;
		}

		/// <summary>
		/// Returns null (no intersection), an array of length 1, or an array of length 2.
		/// </summary>
		public Point[] GetIntersectionPoints(Circle b)
		{
			//following the method in math/intersectionCircleCircle.png

			Circle a = this;
			if(a.CoordinatePlane != b.CoordinatePlane)
				throw new Exception("Both circles in operation must have same coordinate plane.");

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
				deltaX = -1 * Math.Cos(radians - RADIANS_90DEGREES) * Radius;
				deltaY = Math.Sin(radians - RADIANS_90DEGREES) * Radius;
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
				deltaX = Math.Cos(radians) * Radius;
				deltaY = Math.Sin(radians) * Radius;
			}

			switch(CoordinatePlane)
			{
				case Geometry.CoordinatePlane.Screen: return new Point(Center.X + deltaX, Center.Y + deltaY);
				case Geometry.CoordinatePlane.Paper: return new Point(Center.X + deltaX, Center.Y - deltaY);
				default: throw new Exception("Coordinate plane not supported.");
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
		/// Returns null (no intercepts), or array of length 1 or 2.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public Point[] GetIntersectionPoints(LineSegment line)
		{
			//line does not intersect if perpendicular line from circle-center to line is longer than circle-radius
			Point perpendicularToCenter = line.GetLine().GetPerpendicularIntersect(Center);
			if(perpendicularToCenter.Distance(Center) > Radius)
				return null;

			//line: y = mx + b
			//circle: r^2 = x^2 + y^2
			//circle: y = sqrt(r^2 + x^2)
			//line: sqrt(r^2 + x^2) = mx + b
			//line: r^2 + x^2 = (mx + b)^2 = m^2x^2 + 2mbx + b^2
			//0 = (m^2 - 1)x^2 + 2mbx + (r^2 - b^2)

			//quadratic equation: given 0 = ax^2 + bx + c, then x = (-b +- sqrt(b^2 - 4ac)) / 2a
			//therefore x = (-2mb +- sqrt((2mb)^2 - 4(m^2 - 1)(r^2 - b^2)) / 2(m^2 - 1)
			double x1 = (-2 * line.Slope * line.YIntercept + Math.Sqrt(Math.Pow(2 * line.Slope * line.YIntercept, 2) - 4 * (Math.Pow(line.Slope, 2) - 1) * (Math.Pow(Radius, 2) - Math.Pow(line.YIntercept, 2))) / (2 * (Math.Pow(line.Slope, 2) - 1)));
			double x2 = (-2 * line.Slope * line.YIntercept - Math.Sqrt(Math.Pow(2 * line.Slope * line.YIntercept, 2) - 4 * (Math.Pow(line.Slope, 2) - 1) * (Math.Pow(Radius, 2) - Math.Pow(line.YIntercept, 2))) / (2 * (Math.Pow(line.Slope, 2) - 1)));
			double y1 = line.Slope * x1 + line.YIntercept;
			double y2 = line.Slope * x2 + line.YIntercept;
			Point point1 = new Point(x1, y1);
			Point point2 = new Point(x2, y2);
			List<Point> result = new List<Point>();
			if(line.Overlaps(point1))
				result.Add(point1);
			if(line.Overlaps(point2) && point1 != point2)
				result.Add(point2);
			if(result.Count == 0)
				return null;
			return result.ToArray();
		}

		public static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / Math.PI;
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
				360); 
		}
	}
}
