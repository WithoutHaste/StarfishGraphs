using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestRange
	{
		[TestMethod]
		public void Span_StartLessThanEnd()
		{
			//assign
			Range a = new Range(5, 120);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(115, result);
		}

		[TestMethod]
		public void Span_StartGreaterThanEnd()
		{
			//assign
			Range a = new Range(120, 5);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(-115, result);
		}

		[TestMethod]
		public void Span_StartEqualToEnd()
		{
			//assign
			Range a = new Range(120, 120);
			//act
			double result = a.Span;
			//assert
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Middle_StartEqualToEnd()
		{
			//assign
			Range a = new Range(0, 0);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Middle_StartLessThanEnd()
		{
			//assign
			Range a = new Range(45, 55);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(50, result);
		}

		[TestMethod]
		public void Middle_StartGreaterThanEnd()
		{
			//assign
			Range a = new Range(350, 10);
			//act
			double result = a.Middle;
			//assert
			Assert.AreEqual(350 - 170, result);
		}

		[TestMethod]
		public void Overlaps_False()
		{
			//assign
			Range a = new Range(270, 300);
			Range b = new Range(10, 30);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsFalse(resultA);
			Assert.IsFalse(resultB);
		}

		[TestMethod]
		public void Overlaps_Barely()
		{
			//assign
			Range a = new Range(30, 100);
			Range b = new Range(0, 30);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsTrue(resultA);
			Assert.IsTrue(resultB);
		}

		[TestMethod]
		public void Overlaps_Some()
		{
			//assign
			Range a = new Range(20, 100);
			Range b = new Range(-5, 30);
			//act
			bool resultA = a.Overlaps(b);
			bool resultB = b.Overlaps(a);
			//assert
			Assert.IsTrue(resultA);
			Assert.IsTrue(resultB);
		}

		[TestMethod]
		public void Overlaps_Completely()
		{
			//assign
			Range a = new Range(-270, 450);
			Range b = new Range(-5, 5);
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
			Range a = new Range(-270, 450);
			Range b = new Range(-5, 5);
			//act
			Range resultA = a + b;
			Range resultB = b + a;
			//assert
			Assert.AreEqual(resultA, resultB);
			Assert.AreEqual(a, resultA);
		}

		[TestMethod]
		public void OperatorPlus_PartialOverlap()
		{
			//assign
			Range a = new Range(20, 100);
			Range b = new Range(-5, 30);
			//act
			Range resultA = a + b;
			Range resultB = b + a;
			//assert
			Assert.AreEqual(resultA, resultB);
			Assert.AreEqual(new Range(-5, 100), resultA);
		}

		[TestMethod]
		public void OperatorPlus_NoOverlap()
		{
			//assign
			Range a = new Range(270, 300);
			Range b = new Range(10, 30);
			//act
			Range resultA = a + b;
			Range resultB = b + a;
			//assert
			Assert.AreEqual(resultA, resultB);
			Assert.AreEqual(new Range(10, 300), resultA);
		}
	}
}
