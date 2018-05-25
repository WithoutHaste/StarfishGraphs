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
		/// Given a line emerging from the center of a circle, what degrees is the line angle at? 0 degrees is East from center, and increases clockwise.
		/// </summary>
		public static double DegreesOfLine(Point circleCenter, Point lineEnd, CoordinatePlane coordinatePlane)
		{
			//todo: move this into Circle object
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

			double lineLength = circleCenter.Distance(lineEnd);
			double radians = Math.Abs(Math.Asin((lineEnd.Y - circleCenter.Y) / lineLength));
			double degrees = Shapes.Circle.RadiansToDegrees(radians) % 360;
			switch(direction)
			{
				case Direction.SouthEast: return degrees;
				case Direction.SouthWest: return 180 - degrees;
				case Direction.NorthWest: return 180 + degrees;
				case Direction.NorthEast: return 360 - degrees;
			}

			throw new Exception(String.Format("Unsupported direction: {0}.", direction));
		}

		/// <summary>
		/// Calculates point along line AB, starting at A and moving towards B
		/// </summary>
		public static Point PointOnLine(Point a, Point b, double distance)
		{
			double lineLength = a.Distance(b);
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
			double lineLength = a.Distance(b);
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
