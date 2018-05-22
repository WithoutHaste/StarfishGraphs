using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry
{
	public struct Point
	{
		/// <summary>
		/// When determining equality, X and Y have a +- margin of error
		/// </summary>
		public static double MARGIN_OF_ERROR = 0.00000000001;

		public double X;
		public double Y;

		public Point(double x, double y)
		{
			X = x;
			Y = y;
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
			return a.Equals(b, MARGIN_OF_ERROR);
		}

		public static bool operator !=(Point a, Point b)
		{
			return (! a.Equals(b, MARGIN_OF_ERROR));
		}

		/// <summary>
		/// Determines equality with a custom margin of error
		/// </summary>
		public bool Equals(Point b, double marginOfError)
		{
			if(b.X < this.X - marginOfError || b.X > this.X + marginOfError)
				return false;
			if(b.Y < this.Y - marginOfError || b.Y > this.Y + marginOfError)
				return false;
			return true;
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
	}
}
