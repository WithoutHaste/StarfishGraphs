using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry
{
    public static class Geometry
    {
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
			double lengthRatio = distance / lineLength;
			double x = ((1 - lengthRatio) * a.X) + (lengthRatio * b.X);
			double y = ((1 - lengthRatio) * a.Y) + (lengthRatio * b.Y);
			return new Point(x, y);
		}
	}
}
