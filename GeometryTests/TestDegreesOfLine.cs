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
			Geometry.CoordinatePlane = Geometry.CoordinatePlanes.Paper;
			Point circleCenter = new Point(0, 0);
			Point lineEnd = new Point(13, 22.5166604984);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(300, degrees, 0.1));
		}

		[TestMethod]
		public void OnScreen_A()
		{
			//assign
			Geometry.CoordinatePlane = Geometry.CoordinatePlanes.Screen;
			Point circleCenter = new Point(0, 0);
			Point lineEnd = new Point(13, 22.5166604984);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(60, degrees, 0.1));
		}

		[TestMethod]
		public void OnScreen_B()
		{
			//assign
			Geometry.CoordinatePlane = Geometry.CoordinatePlanes.Screen;
			Point circleCenter = new Point(2019, 866);
			Point lineEnd = new Point(1500, 1000);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(165, degrees, 5));
		}

		[TestMethod]
		public void OnScreen_C()
		{
			//assign
			Geometry.CoordinatePlane = Geometry.CoordinatePlanes.Screen;
			Point circleCenter = new Point(1737, 1730);
			Point lineEnd = new Point(1573, 1227);
			//act
			double degrees = Geometry.DegreesOfLine(circleCenter, lineEnd);
			//assert
			Assert.IsTrue(IsWithinMarginOfError(252, degrees, 5));
		}

		private bool IsWithinMarginOfError(double expected, double actual, double marginOfError)
		{
			return (actual >= expected - marginOfError && actual <= expected + marginOfError);
		}
	}
}
