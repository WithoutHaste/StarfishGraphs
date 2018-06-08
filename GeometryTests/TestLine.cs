using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestLine
	{
		[TestMethod]
		public void Slope()
		{
			//assign
			Geometry.MarginOfError = 0.001;
			Line a = new Line(new Point(0, -4.0/9.0), new Point(-4.0/6.0, 0));
			//act
			double result = a.Slope;
			//assert
			Assert.AreEqual(-2.0/3.0, result);
		}

		[TestMethod]
		public void PerpendicularSlope()
		{
			//assign
			Geometry.MarginOfError = 0.001;
			Line a = new Line(new Point(0, -4.0/9.0), new Point(-4.0/6.0, 0));
			//act
			double result = a.PerpendicularSlope;
			//assert
			Assert.AreEqual(3.0/2.0, result);
		}

		[TestMethod]
		public void GetPerpendicularIntersect()
		{
			//assign
			Geometry.MarginOfError = 0.001;
			Line a = new Line(new Point(0, 15.0/6.0), new Point(15.0/8.0, 0));
			Point c = new Point(3, -4);
			//act
			Point result = a.GetPerpendicularIntersect(c);
			//assert
			Assert.IsTrue(Geometry.WithinMarginOfError(21.0/5.0, result.X));
			Assert.IsTrue(Geometry.WithinMarginOfError(-31.0/10.0, result.Y));
		}
	}
}
