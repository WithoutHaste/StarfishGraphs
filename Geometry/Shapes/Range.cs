using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	//todo: should I rename Start and End to Min and Max?
	public class Range : Shape
	{
		/// <summary>
		/// Operations do not assume that Start is the minimum value.
		/// </summary>
		public readonly double Start;
		public readonly double End;

		public virtual double Span { get { return End - Start; } }
		public virtual double Middle { get { return Start + ((End - Start) / 2); } }

		public Range(double s, double e)
		{
			Start = s;
			End = e;
		}

		public static Range Centered(double center, double span)
		{
			return new Range(center - (span / 2), center + (span / 2));
		}

		public virtual bool Overlaps(Range b)
		{
			return (this.Overlaps(b.Start) || this.Overlaps(b.End) || b.Overlaps(this.Start) || b.Overlaps(this.End));
		}

		public virtual bool Overlaps(double b)
		{
			return (Start <= b && End >= b);
		}

		public override string ToString()
		{
			return String.Format("{0}-{1}", Start, End);
		}

		/// <summary>
		/// Returns a range that covers all the area both A and B cover, including any gap in between.
		/// </summary>
		public static Range operator +(Range a, Range b)
		{
			return new Range(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));
		}
	}
}
