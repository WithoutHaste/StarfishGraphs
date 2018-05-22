using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGraphs
{
	public class NetworkGraph_Lia
	{
		private static float scale = 1;
		private static float nodeWidth = 50 * scale;
		private static Point NULL_POINT = new Point(-50000, -50000);
/*
		static void Main(string[] args)
		{
			new NetworkGraph_Lia();
		}
		*/
		public NetworkGraph_Lia()
		{
			string dataFilename = "../../../data/2018May_Lia_starting_point_3747.csv";
			string saveAsFilename = "starting_point_3747.png";
			Dictionary<int, List<int>> data = LoadData_Lia(dataFilename);
			Bitmap graph = DrawGraph_Lia_3747(data);
			graph.Save(saveAsFilename, ImageFormat.Png);

			dataFilename = "../../../data/2018May_Lia_starting_point_1952.csv";
			saveAsFilename = "starting_point_1952.png";
			data = LoadData_Lia(dataFilename);
			graph = DrawGraph_Lia_1952(data);
			graph.Save(saveAsFilename, ImageFormat.Png);
		}

		private Dictionary<int, List<int>> LoadData_Lia(string filename)
		{
			Dictionary<int, List<int>> data = new Dictionary<int, List<int>>();
			using(StreamReader reader = new StreamReader(filename))
			{
				string line = null;
				while((line = reader.ReadLine()) != null)
				{
					string[] fields = line.Split(',');
					if(fields[0] == "")
						continue;
					int fromId = Int32.Parse(fields[1]);
					int toId = Int32.Parse(fields[2]);
					if(!data.ContainsKey(fromId))
						data[fromId] = new List<int>();
					data[fromId].Add(toId);
				}
			}
			return data;
		}

		private Bitmap DrawGraph_Lia_3747(Dictionary<int, List<int>> data)
		{
			Bitmap bitmap = new Bitmap(2000, 2000);
			List<int> parentIds = new List<int>();
			Dictionary<int, Point> nodeLocations = new Dictionary<int, Point>();
			Dictionary<int, Range> nodeChildAngles = new Dictionary<int, Range>();
			Dictionary<int, Point> nodeChildCenter = new Dictionary<int, Point>();
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);

				Point center = new Point(bitmap.Width / 2, bitmap.Height / 2);
				nodeLocations[3747] = center;
				nodeChildAngles[3747] = new Range(10, 350);
				nodeChildCenter[3747] = nodeLocations[3747];
				parentIds.Add(3747);

				nodeLocations[886] = new Point(center.X - (int)(nodeWidth * 1.5), center.Y);
				nodeChildAngles[886] = new Range(0, 360);
				nodeChildCenter[886] = nodeLocations[886];
				parentIds.Add(886);

				nodeLocations[859] = new Point(center.X - 300, center.Y);
				nodeChildAngles[859] = new Range(0, 360);
				nodeChildCenter[859] = nodeLocations[859];
				parentIds.Add(859);

				nodeLocations[905] = new Point(center.X + 500, center.Y);
				nodeChildAngles[905] = new Range(190, 530);
				nodeChildCenter[905] = nodeLocations[905];
				parentIds.Add(905);

				nodeLocations[862] = new Point(center.X - 250, center.Y - 200);
				nodeChildAngles[862] = new Range(135, 315);
				nodeChildCenter[862] = nodeLocations[862];
				parentIds.Add(862);

				nodeLocations[1939685] = new Point(center.X - 250, center.Y + 400);
				nodeChildAngles[1939685] = new Range(-15, 230);
				nodeChildCenter[1939685] = nodeLocations[1939685];
				parentIds.Add(1939685);

				nodeLocations[2138767] = new Point(nodeLocations[1939685].X - (int)(nodeWidth * 1.5), nodeLocations[1939685].Y);
				nodeChildAngles[2138767] = new Range(0, 360);
				nodeChildCenter[2138767] = nodeLocations[1939685];
				parentIds.Add(2138767);

				int parentIdsIndex = 0;
				while(parentIdsIndex < parentIds.Count)
				{
					int parentId = parentIds[parentIdsIndex];
					if(!data.ContainsKey(parentId))
					{
						parentIdsIndex++;
						continue;
					}

					int childCount = data[parentId].Where(x => !nodeLocations.ContainsKey(x)).Count();
					float childAngleUnit = nodeChildAngles[parentId].Span / childCount;
					float childArcLength = (childCount * nodeWidth) * 1.1F;
					float childRadius = (childArcLength / (2 * (float)Math.PI)) * (360 / nodeChildAngles[parentId].Span);
					float childChildAngleSpan = (nodeWidth / (2 * (float)Math.PI * childRadius)) * 360; //result is in degrees
					float childAngle = nodeChildAngles[parentId].Start;
					Point childCenter = nodeChildCenter[parentId];
					foreach(int childId in data[parentId])
					{
						if(nodeLocations.ContainsKey(childId))
							continue;

						Point childPoint = new Point((int)(childCenter.X + (Math.Cos(Radians(childAngle)) * childRadius)), (int)(childCenter.Y + (Math.Sin(Radians(childAngle)) * childRadius)));
						if(childId == 905)
						{
							childChildAngleSpan = 180;
						}
						nodeLocations[childId] = childPoint;
						nodeChildAngles[childId] = new Range(childAngle - (childChildAngleSpan / 2), childAngle + (childChildAngleSpan / 2));
						nodeChildCenter[childId] = childCenter;
						parentIds.Add(childId);

						childAngle += childAngleUnit;
					}

					parentIdsIndex++;
				}

				//draw results
				//lines
				Pen pen = new Pen(Color.Black, 1);
				Pen thickPen = new Pen(Color.Black, 4);
				Pen redPen = new Pen(Color.Red, 1);
				foreach(int fromId in data.Keys)
				{
					if(!nodeLocations.ContainsKey(fromId))
						continue;
					foreach(int toId in data[fromId])
					{
						if(!nodeLocations.ContainsKey(toId))
							continue;
						graphics.DrawLine(pen, nodeLocations[fromId], nodeLocations[toId]);
						Point pointAlongLine = PointAlongLine(nodeLocations[toId], nodeLocations[fromId], nodeWidth * 0.75F);
						if(pointAlongLine != NULL_POINT)
						{
							graphics.DrawLine(thickPen, pointAlongLine, nodeLocations[toId]);
						}
						/*
						if(fromId == 859 || toId == 859)
						{
							graphics.DrawLine(redPen, nodeLocations[fromId], nodeLocations[toId]);
						}*/
					}
				}
				//nodes
				foreach(int id in nodeLocations.Keys)
				{
					DrawNode(graphics, nodeLocations[id], ShortLabel(id));
				}
			}
			return bitmap;
		}

		private Bitmap DrawGraph_Lia_1952(Dictionary<int, List<int>> data)
		{
			Bitmap bitmap = new Bitmap(3000, 2000);
			List<int> parentIds = new List<int>();
			Dictionary<int, Point> nodeLocations = new Dictionary<int, Point>();
			Dictionary<int, Range> nodeChildAngles = new Dictionary<int, Range>();
			Dictionary<int, Point> nodeChildCenter = new Dictionary<int, Point>();
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);

				Point center = new Point((int)(bitmap.Width * 0.75F), (int)(bitmap.Height * 0.75));
				nodeLocations[1952] = center;
				nodeChildAngles[1952] = new Range(0, 360);
				nodeChildCenter[1952] = nodeLocations[1952];
				parentIds.Add(1952);

				nodeLocations[1951] = new Point(center.X - 600, center.Y);
				nodeChildAngles[1951] = new Range(0, 360);
				nodeChildCenter[1951] = nodeLocations[1951];
				parentIds.Add(1951);

				nodeLocations[1953] = new Point(nodeLocations[1951].X - 300, nodeLocations[1951].Y - 700);
				nodeChildAngles[1953] = new Range(0, 360);
				nodeChildCenter[1953] = nodeLocations[1953];
				parentIds.Add(1953);

				nodeLocations[1956] = new Point(nodeLocations[1951].X - 1000, nodeLocations[1951].Y - 975);
				nodeChildAngles[1956] = new Range(0, 360);
				nodeChildCenter[1956] = nodeLocations[1956];
				parentIds.Add(1956);

				nodeLocations[1959] = new Point(nodeLocations[1951].X - 900, nodeLocations[1951].Y - 375);
				nodeChildAngles[1959] = new Range(90, 180);
				nodeChildCenter[1959] = nodeLocations[1959];
				parentIds.Add(1959);

				nodeLocations[1973] = new Point(nodeLocations[1951].X - (int)(nodeWidth * 1.5), nodeLocations[1951].Y + (int)(nodeWidth * 0.75));
				nodeChildAngles[1973] = new Range(0, 360);
				nodeChildCenter[1973] = nodeLocations[1973];
				parentIds.Add(1973);

				nodeLocations[2021] = new Point(nodeLocations[1973].X - 400, nodeLocations[1973].Y + 200);
				nodeChildAngles[2021] = new Range(0, 360);
				nodeChildCenter[2021] = nodeLocations[2021];
				parentIds.Add(2021);

				nodeLocations[3744] = new Point(center.X + 25, center.Y - 500);
				nodeChildAngles[3744] = new Range(260, 420);
				nodeChildCenter[3744] = nodeLocations[3744];
				parentIds.Add(3744);

				nodeLocations[2029] = new Point(nodeLocations[3744].X, nodeLocations[3744].Y + 200);
				nodeChildAngles[2029] = new Range(0, 360);
				nodeChildCenter[2029] = nodeLocations[2029];
				parentIds.Add(2029);

				nodeLocations[1961] = new Point(nodeLocations[3744].X - 400, nodeLocations[3744].Y - 600);
				nodeChildAngles[1961] = new Range(200, 360);
				nodeChildCenter[1961] = nodeLocations[1961];
				parentIds.Add(1961);

				int parentIdsIndex = 0;
				while(parentIdsIndex < parentIds.Count)
				{
					int parentId = parentIds[parentIdsIndex];
					if(!data.ContainsKey(parentId))
					{
						parentIdsIndex++;
						continue;
					}

					int childCount = data[parentId].Where(x => !nodeLocations.ContainsKey(x)).Distinct().Count();
					float childAngleUnit = nodeChildAngles[parentId].Span / childCount;
					float childArcLength = (childCount * nodeWidth) * 1.2F;
					float childRadius = (childArcLength / (2 * (float)Math.PI)) * (360 / nodeChildAngles[parentId].Span);
					childRadius = Math.Max(nodeWidth * 2, childRadius);
					if(parentId == 1951)
					{
						childRadius = Math.Max(nodeWidth * 6, childRadius);
					}
					float childChildAngleSpan = (nodeWidth / (2 * (float)Math.PI * childRadius)) * 360; //result is in degrees
					float childAngle = nodeChildAngles[parentId].Start;
					Point childCenter = nodeChildCenter[parentId];
					foreach(int childId in data[parentId])
					{
						if(nodeLocations.ContainsKey(childId))
							continue;

						Point childPoint = new Point((int)(childCenter.X + (Math.Cos(Radians(childAngle)) * childRadius)), (int)(childCenter.Y + (Math.Sin(Radians(childAngle)) * childRadius)));
						if(childId == 905)
						{
							childChildAngleSpan = 180;
						}
						nodeLocations[childId] = childPoint;
						nodeChildAngles[childId] = new Range(childAngle - (childChildAngleSpan / 2), childAngle + (childChildAngleSpan / 2));
						nodeChildCenter[childId] = childCenter;
						parentIds.Add(childId);

						childAngle += childAngleUnit;
					}

					parentIdsIndex++;
				}

				//draw results
				//lines
				Pen pen = new Pen(Color.Black, 1);
				Pen thickPen = new Pen(Color.Black, 4);
				foreach(int fromId in data.Keys)
				{
					if(!nodeLocations.ContainsKey(fromId))
						continue;
					foreach(int toId in data[fromId].Distinct())
					{
						if(!nodeLocations.ContainsKey(toId))
							continue;
						graphics.DrawLine(pen, nodeLocations[fromId], nodeLocations[toId]);
						
						if(fromId == 1951 || toId == 1951)
						{
							graphics.DrawLine(new Pen(Color.Green, 2), nodeLocations[fromId], nodeLocations[toId]);
						}
						if(fromId == 1973 || toId == 1973)
						{
							graphics.DrawLine(new Pen(Color.Blue, 2), nodeLocations[fromId], nodeLocations[toId]);
						}
						if(fromId == 1959 || toId == 1959)
						{
							graphics.DrawLine(new Pen(Color.DarkRed, 2), nodeLocations[fromId], nodeLocations[toId]);
						}
						if(fromId == 1961 || toId == 1961)
						{
							graphics.DrawLine(new Pen(Color.Orange, 2), nodeLocations[fromId], nodeLocations[toId]);
						}
						if(fromId == 3744 || toId == 3744)
						{
							graphics.DrawLine(new Pen(Color.Purple, 2), nodeLocations[fromId], nodeLocations[toId]);
						}
						
						Point pointAlongLine = PointAlongLine(nodeLocations[toId], nodeLocations[fromId], nodeWidth * 0.75F);
						if(pointAlongLine != NULL_POINT)
						{
							graphics.DrawLine(thickPen, pointAlongLine, nodeLocations[toId]);
						}
					}
				}
				//nodes
				foreach(int id in nodeLocations.Keys)
				{
					DrawNode(graphics, nodeLocations[id], ShortLabel(id));
				}
			}
			return bitmap;
		}

		private void DrawNode(Graphics graphics, Point point, string label)
		{
			float fontSize = 12 * scale;
			Font font = new Font("Arial", fontSize);
			SizeF labelSize = graphics.MeasureString(label, font);
			float paddedLabelWidth = nodeWidth; //labelSize.Width * 1.2F;

			Pen pen = new Pen(Color.Black, 1);
			Rectangle nodeRectangle = new Rectangle((int)(point.X - (paddedLabelWidth / 2)), (int)(point.Y - (nodeWidth / 2)), (int)paddedLabelWidth, (int)nodeWidth);
			graphics.FillPie(new SolidBrush(Color.White), nodeRectangle, 0, 360);
			graphics.DrawArc(pen, nodeRectangle, 0, 360);

			SolidBrush brush = new SolidBrush(Color.Black);
			graphics.DrawString(label, font, brush, point.X - (labelSize.Width / 2), point.Y - (labelSize.Height / 2));
		}

		private float Radians(float degrees)
		{
			return degrees * (float)Math.PI / 180F;
		}

		private string ShortLabel(int id)
		{
			if(id < 10000)
				return id.ToString();
			string sId = id.ToString();
			return sId[0] + "." + sId.Substring(sId.Length - 3, 3);
//			return id.ToString();
		}

		private Point PointAlongLine(Point a, Point b, float length)
		{
			//float slope = (b.Y - a.Y) / (b.X - a.X);
			float segmentLength = (float)Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
			float lengthRatio = length / segmentLength;

			int x = (int)(((1 - lengthRatio) * a.X) + (lengthRatio * b.X));
			int y = (int)(((1 - lengthRatio) * a.Y) + (lengthRatio * b.Y));
			if(x < NULL_POINT.X || y < NULL_POINT.Y)
				return NULL_POINT;
			return new Point(x, y);
		}
	}
}
