using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarfishGeometry.Shapes;

namespace StarfishGeometry
{
    public static class Geometry
    {
		public enum CoordinatePlane : int {
			None = 0,
			/// <summary>
			/// Computer screens have (0,0) in the upper-left corner and increase to the right and down.
			/// </summary>
			Screen,
			/// <summary>
			/// Paper graphs have (0,0) in the lower-left corner and increase to the right and up.
			/// </summary>
			Paper
		};
		public enum Direction : int { None = 0, East, SouthEast, South, SouthWest, West, NorthWest, North, NorthEast };

		/// <summary>
		/// Returns null (no intersection), an array of length 1, or an array of length 2
		/// </summary>
		public static Point[] IntersectionBetweenCircles(Circle a, Circle b)
		{
			//following the method in math/intersectionCircleCircle.png

			double d = DistanceBetweenPoints(a.Center, b.Center); //distance between centers
			if(d > a.Radius + b.Radius)
			{
				return null; //circles too far apart to intersect
			}

			//the radical line is the line between the two intersecting points of the circles
			//Point c is the center of the radical line, which is also on the line between the centers
			double dA = (Math.Pow(a.Radius, 2) - Math.Pow(b.Radius, 2) + Math.Pow(d, 2)) / (2 * d); //distance from centerA to pointC
			if(dA == a.Radius)
			{
				return new Point[] { PointOnLine(a.Center, b.Center, a.Radius) }; //circles intersect at single point
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
		/// Given a line emerging from the center of a circle, what degrees is the line angle at? 0 degrees is East from center, and increases clockwise.
		/// </summary>
		public static double DegreesOfLine(Point circleCenter, Point lineEnd, CoordinatePlane coordinatePlane)
		{
			if(coordinatePlane == CoordinatePlane.None)
				throw new Exception("Coordinate plane required");

			Direction direction = LineDirection(circleCenter, lineEnd, coordinatePlane);
			switch(direction)
			{
				case Direction.East: return 0;
				case Direction.South: return 90;
				case Direction.West: return 180;
				case Direction.North: return 270;
			}

			double lineLength = DistanceBetweenPoints(circleCenter, lineEnd);
			double radians = Math.Abs(Math.Asin((lineEnd.Y - circleCenter.Y) / lineLength));
			double degrees = Shapes.Circle.RadiansToDegrees(radians) % 360;
			switch(direction)
			{
				case Direction.SouthEast: return degrees;
				case Direction.SouthWest: return 180 - degrees;
				case Direction.NorthWest: return 180 + degrees;
				case Direction.NorthEast: return 360 - degrees;
			}

			throw new Exception("Unsupported direction.");
		}

		public static double DistanceBetweenPoints(Point a, Point b)
		{
			return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
		}

		/// <summary>
		/// Calculates point along line AB, starting at A and moving towards B
		/// </summary>
		public static Point PointOnLine(Point a, Point b, double distance)
		{
			double lineLength = DistanceBetweenPoints(a, b);
			if(lineLength == 0)
				throw new Exception("Line length must be greater than 0");
			double lengthRatio = distance / lineLength;
			double x = ((1 - lengthRatio) * a.X) + (lengthRatio * b.X);
			double y = ((1 - lengthRatio) * a.Y) + (lengthRatio * b.Y);
			return new Point(x, y);
		}

		/// <summary>
		/// Calculates point along line AB, starting at B and moving away from A
		/// </summary>
		public static Point PointPastLine(Point a, Point b, double distance)
		{
			double lineLength = DistanceBetweenPoints(a, b);
			return PointOnLine(a, b, lineLength + distance);
		}

		/// <summary>
		/// Given directed line A to B, what direction is it pointing?
		/// North, South, East, and West are precise. The inbetween directions are vague.
		/// </summary>
		public static Direction LineDirection(Point a, Point b, CoordinatePlane coordinatePlane)
		{
			if(a == b) 
				return Direction.None;
			switch(coordinatePlane)
			{
				case CoordinatePlane.Screen: return LineDirection_ComputerScreen(a, b);
				case CoordinatePlane.Paper: return LineDirection_Paper(a, b);
				default: throw new Exception("Unsupported coordinate plane.");
			}
		}

		private static Direction LineDirection_ComputerScreen(Point a, Point b)
		{
			if(a.X == b.X)
			{
				return (a.Y < b.Y) ? Direction.South : Direction.North;
			}
			if(a.Y == b.Y)
			{
				return (a.X < b.X) ? Direction.East : Direction.West;
			}
			if(a.X < b.X)
			{
				return (a.Y < b.Y) ? Direction.SouthEast : Direction.NorthEast;
			}
			else
			{
				return (a.Y < b.Y) ? Direction.SouthWest : Direction.NorthWest;
			}
		}

		private static Direction LineDirection_Paper(Point a, Point b)
		{
			if(a.X == b.X)
			{
				return (a.Y < b.Y) ? Direction.North : Direction.South;
			}
			if(a.Y == b.Y)
			{
				return (a.X < b.X) ? Direction.East : Direction.West;
			}
			if(a.X < b.X)
			{
				return (a.Y < b.Y) ? Direction.NorthEast : Direction.SouthEast;
			}
			else
			{
				return (a.Y < b.Y) ? Direction.NorthWest : Direction.SouthWest;
			}
		}
	}
}
