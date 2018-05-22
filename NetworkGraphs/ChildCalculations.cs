using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGraphs
{
	public class ChildCalculations
	{
		private double angleUnit;
		private double arcLength;
		private double radius;
		private double childAngleSpan;

		public double AngleUnit { get { return angleUnit; } }
		public double Radius { get { return radius; } }
		public double ChildAngleSpan { get { return childAngleSpan; } }

		/// <summary>
		/// Determine how to layout children at equal distance around parent, within a certain arc of the circle
		/// </summary>
		public ChildCalculations(int childCount, double angleSpan, double nodeWidth)
		{
			angleUnit = angleSpan / childCount;

			arcLength = (childCount * nodeWidth) * 1.2F;

			radius = (arcLength / (2 * Math.PI)) * (360 / angleSpan);
			radius = Math.Max(nodeWidth * 2, radius);

			childAngleSpan = (nodeWidth / (2 * Math.PI * radius)) * 360;
		}
	}
}
