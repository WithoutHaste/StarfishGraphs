using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarfishGeometry;

namespace NetworkGraphs
{
	public class NetworkGraph
	{
		static void Main(string[] args)
		{
			new NetworkGraph();
		}

		private static float scale = 1;
		private static float nodeWidth = 50 * scale;

		public NetworkGraph()
		{
			string dataFilename = "../../../data/2018May_Lia_starting_point_3747.csv";
			string saveAsFilename = "auto_starting_point_3747.png";
			Dictionary<int, List<int>> data = LoadData_Lia(dataFilename);
			Bitmap graph = DrawGraph(data, 3747);
			graph.Save(saveAsFilename, ImageFormat.Png);

			//dataFilename = "../../../data/2018May_Lia_starting_point_1952.csv";
			//saveAsFilename = "starting_point_1952.png";
			//data = LoadData_Lia(dataFilename);
			//graph = DrawGraph_Lia_1952(data);
			//graph.Save(saveAsFilename, ImageFormat.Png);
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

		private Bitmap DrawGraph(Dictionary<int, List<int>> data, int startNode)
		{
			Bitmap bitmap = new Bitmap(3000, 2000);
			List<int> parentIds = new List<int>();
			Dictionary<int, StarfishGeometry.Point> nodeLocations = new Dictionary<int, StarfishGeometry.Point>();
			Dictionary<int, Range> nodeChildAngles = new Dictionary<int, Range>();
			Dictionary<int, StarfishGeometry.Point> nodeChildCenter = new Dictionary<int, StarfishGeometry.Point>();
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);

				StarfishGeometry.Point center = new StarfishGeometry.Point((int)(bitmap.Width / 2), (int)(bitmap.Height / 2));
				nodeLocations[startNode] = center;
				nodeChildAngles[startNode] = new Range(0, 360);
				nodeChildCenter[startNode] = nodeLocations[startNode];
				parentIds.Add(startNode);
				
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
					StarfishGeometry.Point childCenter = nodeChildCenter[parentId];
					foreach(int childId in data[parentId])
					{
						if(nodeLocations.ContainsKey(childId))
							continue;

						StarfishGeometry.Point childPoint = new StarfishGeometry.Point((int)(childCenter.X + (Math.Cos(StarfishGeometry.Circle.DegreesToRadians(childAngle)) * childRadius)), (int)(childCenter.Y + (Math.Sin(StarfishGeometry.Circle.DegreesToRadians(childAngle)) * childRadius)));
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
						graphics.DrawLine(pen, 
							new System.Drawing.Point((int)nodeLocations[fromId].X, (int)nodeLocations[fromId].Y),
							new System.Drawing.Point((int)nodeLocations[toId].X, (int)nodeLocations[toId].Y)
						);
						
						//Point pointAlongLine = PointAlongLine(nodeLocations[toId], nodeLocations[fromId], nodeWidth * 0.75F);
						//graphics.DrawLine(thickPen, pointAlongLine, nodeLocations[toId]);
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

		private void DrawNode(Graphics graphics, StarfishGeometry.Point point, string label)
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
			graphics.DrawString(label, font, brush, (float)point.X - (labelSize.Width / 2), (float)point.Y - (labelSize.Height / 2));
		}

		private string ShortLabel(int id)
		{
			if(id < 10000)
				return id.ToString();
			string sId = id.ToString();
			//return sId[0] + "." + sId.Substring(sId.Length - 3, 3);
			return id.ToString();
		}

	}
}
