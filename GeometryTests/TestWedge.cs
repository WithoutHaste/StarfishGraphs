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
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint, circleB.CoordinatePlane);
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
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint, circleB.CoordinatePlane);
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
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint, circleB.CoordinatePlane) + 2;
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

	}
}
