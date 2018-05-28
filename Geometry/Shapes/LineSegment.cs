using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	//todo: linesegment should probably be parent-object of line
	
	public class LineSegment : Shape, IDraw
	{
		public readonly Point A;
		public readonly Point B;
		/// <summary>
		/// When directed, the direction is A to B.
		/// </summary>
		public readonly bool IsDirected;

		public double MaxX { get { return Math.Max(A.X, B.X); } }
		public double MaxY { get { return Math.Max(A.Y, B.Y); } }

		public LineSegment(Point a, Point b)
		{
			A = a;
			B = b;
			IsDirected = false;
		}

		public LineSegment(Point a, Point b, bool isDirected)
		{
			A = a;
			B = b;
			IsDirected = isDirected;
		}

		/// <summary>
		/// Slope assumes direction from A to B.
		/// </summary>
		public double Slope { get { return ((B.Y - A.Y) / (B.X - A.X)); } }

		public double PerpendicularSlope { get { return -1 * (1 / Slope); } }

		public double YIntercept { get { return A.Y - (Slope * A.X); } }

		public bool IsVertical { get { return (A.X == B.X); } }

		public Line GetLine()
		{
			return new Line(A, B, IsDirected);
		}

		public bool Overlaps(Point c)
		{
			if(IsVertical)
			{
				return (Geometry.WithinMarginOfError(c.X, A.X));
			}

			if(!Geometry.WithinMarginOfError(c.Y, (Slope * c.X) + YIntercept))
			{
				return false;
			}

			return (c.X >= Math.Min(A.X, B.X) && c.X <= Math.Max(A.X, B.X) 
				&& c.Y >= Math.Min(A.Y, B.Y) && c.Y <= Math.Max(A.Y, B.Y)); 
		}

		public bool Overlaps(LineSegment b)
		{
			//line equation: y = mx + b, where m is slope and b is y-intercept
			LineSegment a = this;
			double slopeA = a.Slope; 
			double slopeB = b.Slope;
			if(Geometry.WithinMarginOfError(slopeA, slopeB))
			{
				//parallel lines don't overlap unless they are right on top of each other
				//meaning, one of the points must be on the other line
				return (a.Overlaps(b.A) || a.Overlaps(b.B) || b.Overlaps(a.A) || b.Overlaps(a.B));
			}
			//do lines intercept at a point?
			double x = (b.YIntercept - a.YIntercept) / (a.Slope - b.Slope);
			if(a.IsVertical)
			{
				x = a.A.X;
			}
			else if(b.IsVertical)
			{
				x = b.A.X;
			}
			double y = (a.Slope * x) + a.YIntercept;
			Point interceptPoint = new Point(x, y);
			return (a.Overlaps(interceptPoint) && b.Overlaps(interceptPoint));
		}

		public override string ToString()
		{
			return String.Format("{0}-{1}", A, B);
		}

		public void Paint(Graphics graphics, Pen pen, double unitsToPixels)
		{
			graphics.DrawLine(pen, 
				(float)(A.X * unitsToPixels),
				(float)(A.Y * unitsToPixels),
				(float)(B.X * unitsToPixels),
				(float)(B.Y * unitsToPixels)
			);
		}
	}
}
