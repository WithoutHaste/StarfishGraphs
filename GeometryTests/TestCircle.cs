using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestCircle
	{
		[TestMethod]
		public void GetIntersectioPointsCircle_NoIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-2, 0), 1);
			Circle b = new Circle(new Point(2, 0), 1);
			//act
			Point[] result = a.GetIntersectionPoints(b);
			//assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetIntersectioPointsCircle_SingleIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-1, 0), 1);
			Circle b = new Circle(new Point(1, 0), 1);
			//act
			Point[] result = a.GetIntersectionPoints(b);
			//assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new Point(0, 0), result[0]);
		}

		[TestMethod]
		public void GetIntersectioPointsCircle_SingleIntersection_SameX_DiffRadius()
		{
			//assign
			Circle a = new Circle(new Point(-2, 0), 2);
			Circle b = new Circle(new Point(1, 0), 1);
			//act
			Point[] result = a.GetIntersectionPoints(b);
			//assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new Point(0, 0), result[0]);
		}

		[TestMethod]
		public void GetIntersectioPointsCircle_DoubleIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-0.5, 0), 1);
			Circle b = new Circle(new Point(0.5, 0), 1);
			//act
			Point[] result = a.GetIntersectionPoints(b);
			Point minResult = (result[0] < result[1]) ? result[0] : result[1];
			Point maxResult = (minResult == result[0]) ? result[1] : result[0];
			//assert
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(new Point(0, -0.5 * Math.Sqrt(3)), minResult);
			Assert.AreEqual(new Point(0, 0.5 * Math.Sqrt(3)), maxResult);
		}

		[TestMethod]
		public void DegreesToRadians()
		{
			//assign
			double degreesA = 0;
			double degreesB = 90;
			double degreesC = 180;
			double degreesD = 270;
			double degreesE = 360;
			//act
			double radiansA = Circle.DegreesToRadians(degreesA);
			double radiansB = Circle.DegreesToRadians(degreesB);
			double radiansC = Circle.DegreesToRadians(degreesC);
			double radiansD = Circle.DegreesToRadians(degreesD);
			double radiansE = Circle.DegreesToRadians(degreesE);
			//assert
			Assert.AreEqual(0, radiansA);
			Assert.AreEqual(Math.PI / 2, radiansB);
			Assert.AreEqual(Math.PI, radiansC);
			Assert.AreEqual(Math.PI + (Math.PI / 2), radiansD);
			Assert.AreEqual(2 * Math.PI, radiansE);
		}

		[TestMethod]
		public void RadiansToDegrees()
		{
			//assign
			double radiansA = 0;
			double radiansB = Math.PI / 2;
			double radiansC = Math.PI;
			double radiansD = Math.PI + (Math.PI / 2);
			double radiansE = 2 * Math.PI;
			//act
			double degreesA = Circle.RadiansToDegrees(radiansA);
			double degreesB = Circle.RadiansToDegrees(radiansB);
			double degreesC = Circle.RadiansToDegrees(radiansC);
			double degreesD = Circle.RadiansToDegrees(radiansD);
			double degreesE = Circle.RadiansToDegrees(radiansE);
			//assert
			Assert.AreEqual(0, degreesA);
			Assert.AreEqual(90, degreesB);
			Assert.AreEqual(180, degreesC);
			Assert.AreEqual(270, degreesD);
			Assert.AreEqual(360, degreesE);
		}

	}
}
