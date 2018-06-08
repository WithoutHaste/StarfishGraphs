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
		//todo: make sure MarginOfError is applied to all equality in shapes operations, like Circle.ContainsPoint
		//todo: is there reason to allow different margins of error for different shapes? like, they could override the global geometry margin of error if they are set

		/// <summary>
		/// When determining equality, all values have a +- margin of error.
		/// </summary>
		public static double MarginOfError = 0.00000001;

		public static CoordinatePlanes CoordinatePlane = CoordinatePlanes.Screen;

		public enum CoordinatePlanes : int {
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

		public static bool WithinMarginOfError(double a, double b)
		{
			return (a >= b - MarginOfError && a <= b + MarginOfError);
		}

		/// <summary>
		/// Calculates point along line AB, starting at A and moving towards B
		/// </summary>
		public static Point PointOnLine(Point a, Point b, double distance)
		{
			double lineLength = a.Distance(b);
			if(lineLength == 0)
				throw new ArgumentException("Point A and B cannot be the same.");
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
			double lineLength = a.Distance(b);
			return PointOnLine(a, b, lineLength + distance);
		}

		/// <summary>
		/// Given directed line A to B, what direction is it pointing?
		/// North, South, East, and West are precise. The inbetween directions are vague.
		/// </summary>
		public static Direction LineDirection(Point a, Point b)
		{
			if(a == b) 
				return Direction.None;
			switch(CoordinatePlane)
			{
				case CoordinatePlanes.Screen: return LineDirection_Screen(a, b);
				case CoordinatePlanes.Paper: return LineDirection_Paper(a, b);
				default: throw new NotImplementedException("Coordinate plane not supported.");
			}
		}

		private static Direction LineDirection_Screen(Point a, Point b)
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
