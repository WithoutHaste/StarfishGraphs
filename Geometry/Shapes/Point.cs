using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	//todo: can user set a geometry-wide margin of error? some of these ops use the margin of error where you can't specify it
	public class Point : Shape, IDraw
	{
		public readonly double X;
		public readonly double Y;

		public double MaxX { get { return X; } }
		public double MaxY { get { return Y; } }

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Distance between this point and point B.
		/// </summary>
		public double Distance(Point b)
		{
			return Math.Sqrt(Math.Pow(b.X - this.X, 2) + Math.Pow(b.Y - this.Y, 2));
		}

		public bool Overlaps(LineSegment line)
		{
			return line.Overlaps(this);
		}

		public static Point operator +(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point operator -(Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}

		public static Point operator *(double a, Point b)
		{
			return new Point(a * b.X, a * b.Y);
		}

		public static Point operator *(Point a, double b)
		{
			return new Point(a.X * b, a.Y * b);
		}

		public static Point operator /(double a, Point b)
		{
			return new Point(a / b.X, a / b.Y);
		}

		public static Point operator /(Point a, double b)
		{
			return new Point(a.X / b, a.Y / b);
		}

		/// <summary>
		/// Greater than/less than is judged along the x-axis first, then the y-axis
		/// </summary>
		public static bool operator <(Point a, Point b)
		{
			return (a.X < b.X || (a.X == b.X && a.Y < b.Y));
		}

		public static bool operator >(Point a, Point b)
		{
			return (a.X > b.X || (a.X == b.X && a.Y > b.Y));
		}

		public static bool operator ==(Point a, Point b)
		{
			return (Geometry.WithinMarginOfError(a.X, b.X) && Geometry.WithinMarginOfError(a.Y, b.Y));
		}

		public static bool operator !=(Point a, Point b)
		{
			return (!Geometry.WithinMarginOfError(a.X, b.X) || !Geometry.WithinMarginOfError(a.Y, b.Y));
		}

		public override bool Equals(Object b)
		{
			if(b != null && b is Point)
			{
				return (this == (Point)b);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0},{1})", X, Y);
		}

		public void Paint(Graphics graphics, Pen pen, double unitsToPixels)
		{
			graphics.DrawArc(pen, 
				(float)((X - 1) * unitsToPixels), 
				(float)((Y - 1) * unitsToPixels), 
				(float)(2 * unitsToPixels), 
				(float)(2 * unitsToPixels), 
				0, 
				360); 
		}
	}
}
