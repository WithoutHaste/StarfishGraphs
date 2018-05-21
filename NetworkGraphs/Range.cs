using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGraphs
{
	public struct Range
	{
		public float Start;
		public float End;

		public Range(float s, float e)
		{
			Start = s;
			End = e;
		}

		public float Span { get { return End - Start; } }
	}
}
