using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfishGeometry.Shapes
{
	public interface IDraw
	{
		double MaxX { get; }
		double MaxY { get; }

		void Paint(Graphics graphics, Pen pen, double unitsToPixels);
	}
}
