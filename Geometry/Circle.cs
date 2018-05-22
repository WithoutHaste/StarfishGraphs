using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry
{
	public struct Circle
	{
		public double X;
		public double Y;
		public double Radius;

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

		public Point Center { get { return new Point(X, Y); } }

		public static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / Math.PI;
		}
	}
}
