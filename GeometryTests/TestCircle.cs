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

		//Resource: https://www.geogebra.org/m/qBfHYSTQ
		//gives points around a unit circle

		[TestMethod]
		public void PointAtDegrees_0()
		{
			//assign
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 0;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(1, 0), result);
		}

		[TestMethod]
		public void PointAtDegrees_30()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 30;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.87, 0.5), result);
		}

		[TestMethod]
		public void PointAtDegrees_45()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 45;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.71, 0.71), result);
		}

		[TestMethod]
		public void PointAtDegrees_60()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 60;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.5, 0.87), result);
		}

		[TestMethod]
		public void PointAtDegrees_90()
		{
			//assign
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 90;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0, 1), result);
		}

		[TestMethod]
		public void PointAtDegrees_120()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 120;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.5, 0.87), result);
		}

		[TestMethod]
		public void PointAtDegrees_135()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 135;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.71, 0.71), result);
		}

		[TestMethod]
		public void PointAtDegrees_150()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 150;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.87, 0.5), result);
		}

		[TestMethod]
		public void PointAtDegrees_180()
		{
			//assign
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 180;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-1, 0), result);
		}

		[TestMethod]
		public void PointAtDegrees_210()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 210;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.87, -0.5), result);
		}

		[TestMethod]
		public void PointAtDegrees_225()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 225;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.71, -0.71), result);
		}

		[TestMethod]
		public void PointAtDegrees_240()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 240;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(-0.5, -0.87), result);
		}

		[TestMethod]
		public void PointAtDegrees_270()
		{
			//assign
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 270;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0, -1), result);
		}

		[TestMethod]
		public void PointAtDegrees_300()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 300;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.5, -0.87), result);
		}

		[TestMethod]
		public void PointAtDegrees_315()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 315;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.71, -0.71), result);
		}

		[TestMethod]
		public void PointAtDegrees_330()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.01;
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 330;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(0.87, -0.5), result);
		}

		[TestMethod]
		public void PointAtDegrees_360()
		{
			//assign
			Circle a = new Circle(new Point(0, 0), 1);
			double degrees = 360;
			//act
			Point result = a.PointAtDegrees(degrees);
			//assert
			Assert.AreEqual(new Point(1, 0), result);
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
