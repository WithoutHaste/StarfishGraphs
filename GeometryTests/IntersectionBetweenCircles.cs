using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;

namespace GeometryTests
{
	[TestClass]
	public class IntersectionBetweenCircles
	{
		[TestMethod]
		public void NoIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-2, 0), 1);
			Circle b = new Circle(new Point(2, 0), 1);
			//act
			Point[] result = Geometry.IntersectionBetweenCircles(a, b);
			//assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void SingleIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-1, 0), 1);
			Circle b = new Circle(new Point(1, 0), 1);
			//act
			Point[] result = Geometry.IntersectionBetweenCircles(a, b);
			//assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new Point(0, 0), result[0]);
		}

		[TestMethod]
		public void SingleIntersection_SameX_DiffRadius()
		{
			//assign
			Circle a = new Circle(new Point(-2, 0), 2);
			Circle b = new Circle(new Point(1, 0), 1);
			//act
			Point[] result = Geometry.IntersectionBetweenCircles(a, b);
			//assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new Point(0, 0), result[0]);
		}

		[TestMethod]
		public void DoubleIntersection_SameX()
		{
			//assign
			Circle a = new Circle(new Point(-0.5, 0), 1);
			Circle b = new Circle(new Point(0.5, 0), 1);
			//act
			Point[] result = Geometry.IntersectionBetweenCircles(a, b);
			Point minResult = (result[0] < result[1]) ? result[0] : result[1];
			Point maxResult = (minResult == result[0]) ? result[1] : result[0];
			//assert
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(new Point(0, -0.5 * Math.Sqrt(3)), minResult);
			Assert.AreEqual(new Point(0, 0.5 * Math.Sqrt(3)), maxResult);
		}

	}
}
