using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestRangeCircular
	{
		[TestMethod]
		public void Span_StartLessThanEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(5, 120, 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(115, result);
		}

		[TestMethod]
		public void Span_StartLessThanEnd_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(360 + 5, 360 + 120, 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(115, result);
		}

		[TestMethod]
		public void Span_StartGreaterThanEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(120, 5, 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(245, result);
		}

		[TestMethod]
		public void Span_StartGreaterThanEnd_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(-1 * (360 - 120), -1 * (360 - 5), 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(245, result);
		}

		[TestMethod]
		public void Span_StartEqualToEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(120, 120, 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(360, result);
		}

		[TestMethod]
		public void Span_StartEqualToEnd_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(0, 360, 360);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(360, result);
		}
	}
}
