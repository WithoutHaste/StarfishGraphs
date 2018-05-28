using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using StarfishGeometry;
using StarfishGeometry.Shapes;

namespace GeometryTests
{
	public static class Utilities
	{
		public static string DIAGRAM_FOLDER = "diagrams";
		public static double UNITS_TO_PIXELS = 40;

		//todo: auto center diagram in minimal space with a small margin
		public static void SaveDiagram(StarfishGeometry.Shapes.IDraw[] shapes, string className, [CallerMemberName] string methodName = "unknown")
		{
			double margin = 0.5 * UNITS_TO_PIXELS;
			double width = shapes.Select(s => s.MaxX).Max() * UNITS_TO_PIXELS;
			double height = Math.Abs(shapes.Select(s => s.MaxY).Max() * UNITS_TO_PIXELS);

			Bitmap bitmap = new Bitmap((int)(width + margin), (int)(height + margin));
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				Pen pen = new Pen(Color.Black, 1);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);
				foreach(IDraw shape in shapes)
				{
					shape.Paint(graphics, pen, UNITS_TO_PIXELS);
				}
			}
			bitmap.Save(Path.Combine(DIAGRAM_FOLDER, String.Format("{0}_{1}.png", className, methodName)), ImageFormat.Png);
		}
	}
}
