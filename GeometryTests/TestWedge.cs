using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	[TestClass]
	public class TestWedge
	{
		[TestMethod]
		public void OverlapsWedge_LineEdge_AdjacentExternal_CircleInCircle_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 260, 280);
			LineSegment aEdge = new LineSegment(a.Circle.Center, a.EndPoint);
			Point centerB =  Geometry.PointOnLine(a.Circle.Center, a.EndPoint, a.Circle.Radius * 0.25);
			Circle circleB = new Circle(centerB, a.Circle.Radius * 0.5);
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint);
			Wedge b = new Wedge(circleB, startDegreesB, startDegreesB + 10);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_LineEdge_AdjacentExternal_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 260, 280);
			LineSegment aEdge = new LineSegment(a.Circle.Center, a.EndPoint);
			Point centerB =  Geometry.PointOnLine(a.Circle.Center, a.EndPoint, a.Circle.Radius * 0.25);
			Circle circleB = new Circle(centerB, a.Circle.Radius * 1.5);
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint);
			Wedge b = new Wedge(circleB, startDegreesB, startDegreesB + 10);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_LineEdge_AdjacentExternal_Almost_False()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 260, 280);
			LineSegment aEdge = new LineSegment(a.Circle.Center, a.EndPoint);
			Point centerB =  Geometry.PointOnLine(a.Circle.Center, a.EndPoint, a.Circle.Radius * 0.25);
			centerB = new Point(centerB.X + 0.05, centerB.Y);
			Circle circleB = new Circle(centerB, a.Circle.Radius * 0.5);
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint) + 2;
			Wedge b = new Wedge(circleB, startDegreesB, startDegreesB + 10);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcAndLineEdge_CircleInCircle_SmallBelow_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(1.8, 2.3), 0.75), 190, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcAndLineEdge_CircleInCircle_SmallAbove_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(1.8, 1.5), 0.75), 190, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcAndLineEdge_SmallBelow_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(3.2, 1.5), 2.5), 190, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcAndLineEdge_SmallAbove_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(2.7, 3.2), 2.5), 190, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_LineEdge_CrossWise_CircleInCircle_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(1.8, 2.3), 0.75), 260, 280);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_LineEdge_CrossWise_CircleInCircle_Almost_False()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(2.2, 2.3), 0.75), 260, 280);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void OverlapsWedge_LineEdge_CrossWise_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 190, 230);
			Wedge b = new Wedge(new Circle(new Point(1, 2.3), 2), 265, 275);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcOverLineEdge_CircleInCircle_FromOutside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 100, 145);
			Wedge b = new Wedge(new Circle(new Point(1.8, 3.3), 0.5), 10, 170);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcOverLineEdge_CircleInCircle_FromInside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 100, 145);
			Wedge b = new Wedge(new Circle(new Point(2.1, 4.1), 0.5), 180, 300);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcOverLineEdge_FromOutside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 100, 145);
			Wedge b = new Wedge(new Circle(new Point(5.5, 5), 3), 160, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_Center_CircleInCircle_Inside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 0, 45);
			Wedge b = new Wedge(new Circle(new Point(3, 3), 1.5), 30, 35);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_Center_CircleInCircle_Outside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 0, 45);
			Wedge b = new Wedge(new Circle(new Point(3, 3), 3), 60, 90);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_CenterToEdge_CircleInCircle_Outside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			LineSegment edgeA = a.LineEdges[1];
			Point centerB = Geometry.PointOnLine(edgeA.A, edgeA.B, a.Circle.Radius * 0.25);
			Wedge b = new Wedge(new Circle(centerB, 0.5), 90, 120);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_CenterToEdge_CircleInCircle_Inside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			LineSegment edgeA = a.LineEdges[1];
			Point centerB = Geometry.PointOnLine(edgeA.A, edgeA.B, a.Circle.Radius * 0.25);
			Wedge b = new Wedge(new Circle(centerB, 0.5), 300, 330);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_CenterToEdge_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			LineSegment edgeA = a.LineEdges[1];
			Point centerB = Geometry.PointOnLine(edgeA.A, edgeA.B, a.Circle.Radius * 0.25);
			Wedge b = new Wedge(new Circle(centerB, 3), 90, 120);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_EntirelyInside_CircleInCircle_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			Wedge b = new Wedge(new Circle(new Point(4, 3), 0.5), 300, 330);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_EntirelyInside_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), 250, 290);
			Wedge b = new Wedge(new Circle(new Point(3, 0.25), 1), 80, 100);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcToArc_OneIntersect_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(1, 1), 1), -30, 30);
			Wedge b = new Wedge(new Circle(new Point(3, 1), 1), 100, 200);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_ArcToArc_TwoIntersects_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(1, 1), 1), -30, 30);
			Wedge b = new Wedge(new Circle(new Point(2.9, 1), 1), 100, 230);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_CenterToArc_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(1, 1), 1), -30, 30);
			Wedge b = new Wedge(new Circle(new Point(2, 1), 1), -30, 30);
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsWedge_OneIsFullCircle_A_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(1342.5, 867.2), 187.338), 0, 360) / Utilities.UNITS_TO_PIXELS;
			Wedge b = new Wedge(new Circle(new Point(1170.45, 1133.45), 225.53), 631, 648) / Utilities.UNITS_TO_PIXELS;
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsCircle_A_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Circle a = new Circle(new Point(1342.5, 867.2), 187.338) / Utilities.UNITS_TO_PIXELS;
			Wedge b = new Wedge(new Circle(new Point(1170.45, 1133.45), 225.53), 631, 648) / Utilities.UNITS_TO_PIXELS;
			//account
			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = b.Overlaps(a);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void OverlapsCircle_Temp()
		{
			//assign
			Wedge a = new Wedge(new Circle(new Point(550.83386399939, 0), 235.084533229311), 73.6363636363638, 90.0000000000002) / Utilities.UNITS_TO_PIXELS;
			Circle b = new Circle(new Point(444.902178042324, 244.587042149464), 149.14086054459278) / Utilities.UNITS_TO_PIXELS;
			//account
//			Utilities.SaveDiagram(new IDraw[] { a, b }, nameof(TestWedge));
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void DegreesContains_RangeCrosses360_Below360_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			//act
			bool result = a.Degrees.Overlaps(355);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void DegreesContains_RangeCrosses360_Above0_True()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			//act
			bool result = a.Degrees.Overlaps(10);
			//assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void DegreesContains_RangeCrosses360_Below360_False()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			//act
			bool result = a.Degrees.Overlaps(-50);
			//assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void DegreesContains_RangeCrosses360_Above0_False()
		{
			//assign
			StarfishGeometry.Geometry.MarginOfError = 0.000001;
			Wedge a = new Wedge(new Circle(new Point(3, 3), 3), -45, 45);
			//act
			bool result = a.Degrees.Overlaps(55);
			//assert
			Assert.IsFalse(result);
		}
	}
}
