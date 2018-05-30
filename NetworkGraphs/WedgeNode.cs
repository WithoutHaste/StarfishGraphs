using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarfishGeometry.Shapes;

namespace NetworkGraphs
{
	public class WedgeNode
	{
		private Point nodeCenter; //location of node
        private Wedge nodeWedge; //wedge from parent center to node location
		private WedgeNode parentNode;
		private WedgeUnbound childrenWedge; //space available to layout this node's children

		public int Id;
		public WedgeNode SiblingClockwise;
		public WedgeNode SiblingCounterClockwise;

		public Point Center { get { return nodeCenter; } set { nodeCenter = value; } }
        public Wedge Wedge {  get { return nodeWedge; } set { nodeWedge = value; } }
		public WedgeNode ParentNode { get { return parentNode; } set { parentNode = value; } }
		public WedgeUnbound ChildrenWedge { get { return childrenWedge; } set { childrenWedge = value; } }
		public Point ChildrenWedgeCenter { get { return childrenWedge.Center; } set { childrenWedge = new WedgeUnbound(value, childrenWedge.Start, childrenWedge.End); } }
		public RangeCircular ChildrenWedgeDegrees { get { return childrenWedge.Degrees; } set { childrenWedge = new WedgeUnbound(childrenWedge.Center, value); } }

		public double ChildrenSpan { get { return childrenWedge.Span; } }

		public WedgeNode()
		{
		}
	}
}
