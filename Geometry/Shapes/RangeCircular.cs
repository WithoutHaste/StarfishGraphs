using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	public class RangeCircular : Range
	{
		public readonly int CircularModulus;

		public override double Span {
			get {
				if(Start == End)
					return CircularModulus;
				if(Start < End)
					return End - Start;
				return CircularModulus - Start + End;
			}
		}

		public override double Middle {
			get {
				if(Start == End)
					return CircularModulus / 2;
				if(Start < End)
					return Start + ((End - Start) / 2);
				return Mod(Start + (Span / 2));
			}
		}

		public RangeCircular(double s, double e, int mod) : base(Mod(s, mod), Mod(e, mod))
		{
			if(mod <= 0)
				throw new ArgumentException("RangeCircular.Modulus must be greater than 0.");
			CircularModulus = mod;
		}

		public static RangeCircular Centered(double center, double span, int mod)
		{
			return new RangeCircular(center - (span / 2), center + (span / 2), mod);
		}

		public bool Overlaps(RangeCircular b)
		{
			return (this.Overlaps(b.Start) || this.Overlaps(b.End) || b.Overlaps(this.Start) || b.Overlaps(this.End));
		}

		public override bool Overlaps(double b)
		{
			b = Mod(b);
			if(Start == End)
			{
				return true;
			}
			if(Start <= End)
			{
				return (Start <= b && End >= b);
			}
			return ((Start <= b && b <= CircularModulus) || (0 <= b && End >= b));
		}

		/// <summary>
		/// Returns a range that covers all the area both A and B cover, including any gap in between.
		/// If the ranges overlap, there is no gap filled in.
		/// Gaps are covered from direction A to B.
		/// </summary>
		public static RangeCircular operator +(RangeCircular a, RangeCircular b)
		{
			if(a.CircularModulus != b.CircularModulus)
				throw new ArgumentException("RangeCirculars with different Modulus values cannot be combined.");
			return new RangeCircular(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End), a.CircularModulus);
		}

		public double Mod(double number)
		{
			return Mod(number, CircularModulus);
		}

		/// <summary>
		/// Returns number modulus m. Ensures a positive result.
		/// </summary>
		public static double Mod(double number, int m)
		{
			if(m <= 0)
				throw new ArgumentException("RangeCircular.Mod requires a positive, non-zero M.");
			while(number < 0)
				number += m;
			return number % m;
		}
	}
}
