using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestDegreesOfLine
	{
		[TestMethod]
		public void OnPaper_A()
		{
			//assign
			Point circleCenter = new Point(0, 0);
			Point lineEnd = new Point(13, 22.5166604984);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd, Geometry.CoordinatePlane.Paper);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(300, degrees, 0.1));
		}

		[TestMethod]
		public void OnScreen_A()
		{
			//assign
			Point circleCenter = new Point(0, 0);
			Point lineEnd = new Point(13, 22.5166604984);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd, Geometry.CoordinatePlane.Screen);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(60, degrees, 0.1));
		}

		private bool IsWithinMarginOfError(double expected, double actual, double marginOfError)
		{
			return (actual >= expected - marginOfError && actual <= expected + marginOfError);
		}
	}
}
