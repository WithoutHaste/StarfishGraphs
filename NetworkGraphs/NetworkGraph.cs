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
using Shapes = StarfishGeometry.Shapes;

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

			dataFilename = "../../../data/2018May_Lia_starting_point_1952.csv";
			saveAsFilename = "auto_starting_point_1952.png";
			data = LoadData_Lia(dataFilename);
			graph = DrawGraph(data, 1952);
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

		private Bitmap DrawGraph(Dictionary<int, List<int>> data, int startNodeId)
		{
			Bitmap bitmap = new Bitmap(3000, 2000);
			List<int> parentIds = new List<int>();
			Dictionary<int, WedgeNode> nodes = new Dictionary<int, WedgeNode>();
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);

				Shapes.Point center = new Shapes.Point(bitmap.Width / 2, bitmap.Height / 2);
				nodes[startNodeId] = new WedgeNode() {
					Center = center,
                    Wedge = new Shapes.Wedge(new Shapes.Circle(center, nodeWidth / 2), 0, 360),
					ChildrenWedge = new Shapes.WedgeUnbound(center, 0, 360)
				};
				parentIds.Add(startNodeId);
				
				int parentIdsIndex = 0;
				while(parentIdsIndex < parentIds.Count)
				{
					int parentId = parentIds[parentIdsIndex];
					if(!data.ContainsKey(parentId))
					{
						parentIdsIndex++;
						continue;
					}
					WedgeNode parentNode = nodes[parentId];

					ChildCalculations calculations;
					int childCount = data[parentId].Where(x => !nodes.ContainsKey(x)).Distinct().Count();
					if(childCount > 2 && parentNode.ParentNode != null)
					{
						//move node out from old parent to make room for new children
						calculations = new ChildCalculations(childCount + 1, 360, nodeWidth);
						Shapes.Point newCenter = Geometry.PointPastLine(parentNode.ParentNode.Center, parentNode.Center, calculations.Radius * 1.2);
						parentNode.Center = newCenter;
						double connectionToParentAtDegrees = Geometry.DegreesOfLine(newCenter, parentNode.ParentNode.Center, Geometry.CoordinatePlane.Screen);
						parentNode.ChildrenWedge = new Shapes.WedgeUnbound(newCenter, connectionToParentAtDegrees + calculations.AngleUnit, 360 - calculations.AngleUnit);
					}
					else
					{
						calculations = new ChildCalculations(childCount, parentNode.ChildrenWedge.Span, nodeWidth);
					}
					while(DoesCollide(parentId, nodes, parentNode.Center, calculations.Radius))
					{
						//move node out from old parent to make room for new children
						Shapes.Point newCenter = Geometry.PointPastLine(parentNode.ParentNode.Center, parentNode.Center, nodeWidth * 1.2);
						parentNode.Center = newCenter;
						parentNode.ChildrenWedgeCenter = newCenter;
					}
					double childAngle = parentNode.ChildrenWedge.Start;
					Shapes.Point childCenter = parentNode.ChildrenWedge.Center;
					foreach(int childId in data[parentId])
					{
						if(nodes.ContainsKey(childId))
							continue;

						Shapes.Point childPoint = new Shapes.Point(childCenter.X + (Math.Cos(Shapes.Circle.DegreesToRadians(childAngle)) * calculations.Radius), childCenter.Y + (Math.Sin(Shapes.Circle.DegreesToRadians(childAngle)) * calculations.Radius));
						nodes[childId] = new WedgeNode() {
							Center = childPoint,
		                    Wedge = new Shapes.Wedge(new Shapes.Circle(parentNode.Center, calculations.Radius), childAngle, childAngle + calculations.AngleUnit),
							ParentNode = parentNode,
							ChildrenWedge = new Shapes.WedgeUnbound(childCenter, Shapes.Range.Centered(childAngle, calculations.ChildAngleSpan))
						};
						parentIds.Add(childId);

						childAngle += calculations.AngleUnit;
					}

					parentIdsIndex++;
				}

				//draw results
				//lines
				Pen pen = new Pen(Color.Black, 1);
				Pen thickPen = new Pen(Color.Black, 4);
				foreach(int fromId in data.Keys)
				{
					if(!nodes.ContainsKey(fromId))
						continue;
					foreach(int toId in data[fromId].Distinct())
					{
						if(!nodes.ContainsKey(toId))
							continue;
						graphics.DrawLine(pen, 
							new System.Drawing.Point((int)nodes[fromId].Center.X, (int)nodes[fromId].Center.Y),
							new System.Drawing.Point((int)nodes[toId].Center.X, (int)nodes[toId].Center.Y)
						);
						
						//Point pointAlongLine = PointAlongLine(nodeLocations[toId], nodeLocations[fromId], nodeWidth * 0.75F);
						//graphics.DrawLine(thickPen, pointAlongLine, nodeLocations[toId]);
					}
				}
				//nodes
				foreach(int id in nodes.Keys)
				{
					DrawNode(graphics, nodes[id].Center, ShortLabel(id));
				}
			}
			return bitmap;
		}

		private void DrawNode(Graphics graphics, Shapes.Point point, string label)
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

		private bool DoesCollide(int parentId, Dictionary<int, WedgeNode> nodes, Shapes.Point center, double radius)
		{
			foreach(int nodeId in nodes.Keys)
			{
				if(nodeId == parentId)
					continue;
				if(DoesCollide(nodes[nodeId], center, radius))
					return true;
			}
			return false;
		}

		private bool DoesCollide(WedgeNode otherNode, Shapes.Point center, double radius)
		{
			return Geometry.WedgesOverlap(otherNode.Wedge, new Shapes.Wedge(new Shapes.Circle(center, radius), 0, 360));
		}
	}
}
