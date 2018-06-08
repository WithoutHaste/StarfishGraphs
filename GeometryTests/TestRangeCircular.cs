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

		[TestMethod]
		public void Middle_StartEqualToEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(0, 0, 360);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(180, result);
		}

		[TestMethod]
		public void Middle_StartEqualToEnd_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(0, 360, 360);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(180, result);
		}

		[TestMethod]
		public void Middle_StartLessThanEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(45, 55, 360);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(50, result);
		}

		[TestMethod]
		public void Middle_StartGreaterThanEnd()
		{
			//assign
			RangeCircular a = new RangeCircular(350, 10, 360);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Middle_StartGreaterThanEnd_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(-10, 10, 360);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Constructor_ZeroModulus()
		{
			//assign
			int modulus = 0;
			//act
			RangeCircular range = new RangeCircular(0, 0, modulus);
			//expected exception
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Constructor_NegativeModulus()
		{
			//assign
			int modulus = -1;
			//act
			RangeCircular range = new RangeCircular(0, 0, modulus);
			//expected exception
		}

		[TestMethod]
		public void Overlaps_False()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 300, 360);
			RangeCircular b = new RangeCircular(10, 30, 360);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsFalse(resultA);
			Assert.IsFalse(resultB);
		}

		[TestMethod]
		public void Overlaps_Barely_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 360, 360);
			RangeCircular b = new RangeCircular(0, 30, 360);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsTrue(resultA);
			Assert.IsTrue(resultB);
		}

		[TestMethod]
		public void Overlaps_Some_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 370, 360);
			RangeCircular b = new RangeCircular(-5, 30, 360);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsTrue(resultA);
			Assert.IsTrue(resultB);
		}

		[TestMethod]
		public void Overlaps_Completely_Mod()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 450, 360);
			RangeCircular b = new RangeCircular(-5, 5, 360);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsTrue(resultA);
			Assert.IsTrue(resultB);
		}

		[TestMethod]
		public void OperatorPlus_CompleteOverlap()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 450, 360);
			RangeCircular b = new RangeCircular(-5, 5, 360);
			//act
			RangeCircular resultA = a + b;
			RangeCircular resultB = b + a;
			//assert
			Assert.AreEqual(resultA, resultB);
			Assert.AreEqual(a, resultA);
		}

		[TestMethod]
		public void OperatorPlus_PartialOverlap()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 360, 360);
			RangeCircular b = new RangeCircular(-5, 5, 360);
			//act
			RangeCircular resultA = a + b;
			RangeCircular resultB = b + a;
			//assert
			Assert.AreEqual(resultA, resultB);
			Assert.AreEqual(new RangeCircular(270, 5, 360), resultA);
		}

		[TestMethod]
		public void OperatorPlus_NoOverlap()
		{
			//assign
			RangeCircular a = new RangeCircular(270, 280, 360);
			RangeCircular b = new RangeCircular(-5, 5, 360);
			//act
			RangeCircular resultA = a + b;
			RangeCircular resultB = b + a;
			//assert
			Assert.AreNotEqual(resultA, resultB);
			Assert.AreEqual(new RangeCircular(270, 5, 360), resultA);
			Assert.AreEqual(new RangeCircular(-5, 280, 360), resultB);
		}
	}
}
