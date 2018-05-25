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
		public void OverlapsWedge_LineEdge_AdjacentExternal_True()
		{
			//assign
			Wedge a = new Wedge(new Circle(new Point(2, 2), 2), 260, 280);
			LineSegment aEdge = new LineSegment(a.Circle.Center, a.EndPoint);
			Point centerB =  Geometry.PointOnLine(a.Circle.Center, a.EndPoint, a.Circle.Radius * 0.25);
			Circle circleB = new Circle(centerB, a.Circle.Radius * 0.5);
			double startDegreesB = Geometry.DegreesOfLine(centerB, a.EndPoint, circleB.CoordinatePlane);
			Wedge b = new Wedge(circleB, startDegreesB, startDegreesB + 10);
			//account
			Utilities.SaveDiagram(nameof(TestWedge), nameof(OverlapsWedge_LineEdge_AdjacentExternal_True), new IDraw[] { a, b });
			//act
			bool result = a.Overlaps(b);
			//assert
			Assert.IsTrue(result);
		}

	}
}
