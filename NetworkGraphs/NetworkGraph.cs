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

			dataFilename = "../../../data/2018May_Lia_starting_point_1952.csv";
			saveAsFilename = "auto_starting_point_1952_mostconnectedfirst.png";
			data = LoadData_Lia(dataFilename);
			int mostConnectedId = data.OrderByDescending(pair => pair.Value.Distinct().Count()).First().Key;
			graph = DrawGraph(data, mostConnectedId);
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

		private int CountChildrenNotPlaced(int parentId, Dictionary<int, List<int>> data, Dictionary<int, WedgeNode> nodes)
		{
			if(!data.ContainsKey(parentId))
				return 0;
			return data[parentId].Where(x => !nodes.ContainsKey(x)).Distinct().Count();
		}

		private Bitmap DrawGraph(Dictionary<int, List<int>> data, int startNodeId)
		{
			List<int> parentIds = new List<int>();
			Dictionary<int, WedgeNode> nodes = new Dictionary<int, WedgeNode>();

			Geometry.CoordinatePlane = Geometry.CoordinatePlanes.Screen;

			Shapes.Point center = new Shapes.Point(1000, 1000);
			nodes[startNodeId] = new WedgeNode()
			{
				Id = startNodeId,
				Center = center,
				Wedge = new Shapes.Wedge(new Shapes.Circle(center, nodeWidth / 2), 0, 360),
				ChildrenWedge = new Shapes.WedgeUnbound(center, 0, 360)
			};
			parentIds.Add(startNodeId);

			int parentIdsIndex = 0;
			while(parentIdsIndex < parentIds.Count)
			{
//				if(parentIdsIndex > 9)
//					break;
				int parentId = parentIds[parentIdsIndex];
				if(!data.ContainsKey(parentId))
				{
					parentIdsIndex++;
					continue;
				}
				WedgeNode parentNode = nodes[parentId];

				//if a sibling does not have any unplaced children, this node can take up the sibling's childrenwedge space
				if(parentNode.SiblingClockwise != null && CountChildrenNotPlaced(parentNode.SiblingClockwise.Id, data, nodes) == 0)
				{
					parentNode.ChildrenWedgeDegrees += parentNode.SiblingClockwise.ChildrenWedgeDegrees;
				}
				if(parentNode.SiblingCounterClockwise != null && CountChildrenNotPlaced(parentNode.SiblingCounterClockwise.Id, data, nodes) == 0)
				{
					parentNode.ChildrenWedgeDegrees += parentNode.SiblingCounterClockwise.ChildrenWedgeDegrees;
				}

				ChildCalculations calculations;
				int childCount = CountChildrenNotPlaced(parentId, data, nodes);
				if(childCount == 0)
				{
					parentIdsIndex++;
					continue;
				}
				if(childCount > 2 && parentNode.ParentNode != null)
				{
					//move node out from old parent to make room for new children
					calculations = new ChildCalculations(childCount + 1, 360, nodeWidth);
					Shapes.Point newCenter = Geometry.PointPastLine(parentNode.ParentNode.Center, parentNode.Center, calculations.Radius * 1.2);
					parentNode.Center = newCenter;
					double connectionToParentAtDegrees = Geometry.DegreesOfLine(newCenter, parentNode.ParentNode.Center);
					parentNode.ChildrenWedge = new Shapes.WedgeUnbound(newCenter, connectionToParentAtDegrees + calculations.AngleUnit, 360 - calculations.AngleUnit);
				}
				else
				{
					calculations = new ChildCalculations(childCount, parentNode.ChildrenWedge.Span, nodeWidth);
				}
				while(DoesCollide(parentId, nodes, parentNode.Center, calculations.Radius + (nodeWidth / 2)))
				{
					//move node out from old parent to make room for new children
					Shapes.Point newCenter = Geometry.PointPastLine(parentNode.ParentNode.Center, parentNode.Center, nodeWidth * 1.2);
					parentNode.Center = newCenter;
					parentNode.ChildrenWedgeCenter = newCenter;
				}
				double childAngle = parentNode.ChildrenWedge.Start;
				Shapes.Point childCenter = parentNode.ChildrenWedge.Center;
				WedgeNode siblingCounterClockwise = null;
				foreach(int childId in data[parentId])
				{
					if(nodes.ContainsKey(childId))
						continue;

					//todo: if the sibling nodes next to you have no children, you can take up a wider space with your children

					Shapes.Point childPoint = new Shapes.Point(childCenter.X + (Math.Cos(Shapes.Circle.DegreesToRadians(childAngle)) * calculations.Radius), childCenter.Y + (Math.Sin(Shapes.Circle.DegreesToRadians(childAngle)) * calculations.Radius));
					nodes[childId] = new WedgeNode()
					{
						Id = childId,
						Center = childPoint,
						Wedge = new Shapes.Wedge(new Shapes.Circle(parentNode.Center, calculations.Radius + (nodeWidth / 2)), childAngle - (0.5 * calculations.AngleUnit), childAngle + (0.5 * calculations.AngleUnit)),
						ParentNode = parentNode,
						ChildrenWedge = new Shapes.WedgeUnbound(childCenter, Shapes.RangeCircular.Centered(childAngle, calculations.ChildAngleSpan, 360)),
						SiblingCounterClockwise = siblingCounterClockwise
					};
					if(siblingCounterClockwise != null)
						siblingCounterClockwise.SiblingClockwise = nodes[childId];
					siblingCounterClockwise = nodes[childId];
					parentIds.Add(childId);

					childAngle += calculations.AngleUnit;
				}

				parentIdsIndex++;
			}

			//todo: check for lines very closely overlapping each other (see graph 1952, up to parentId 7)

			//adjust graph location as close to origin as possible
			int margin = 20;
			int minX = (int)(nodes.Select(pair => pair.Value.Center.X).Min() - (nodeWidth / 2) - margin);
			int minY = (int)(nodes.Select(pair => pair.Value.Center.Y).Min() - (nodeWidth / 2) - margin);
			Shapes.Point minPoint = new Shapes.Point(minX, minY);
			foreach(WedgeNode node in nodes.Values)
			{
				node.Center -= minPoint;
			}
			int maxX = (int)(nodes.Select(pair => pair.Value.Center.X).Max() + (nodeWidth / 2) + margin);
			int maxY = (int)(nodes.Select(pair => pair.Value.Center.Y).Max() + (nodeWidth / 2) + margin);

			Bitmap bitmap = new Bitmap(maxX, maxY);
			using(Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.White);

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
				//debugging: wedges
				/*
				Pen redPen = new Pen(Color.Red, 1);
				foreach(int id in nodes.Keys)
				{
					nodes[id].Wedge.Paint(graphics, redPen, 1);
				}
				*/
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
			return otherNode.Wedge.Overlaps(new Shapes.Circle(center, radius));
		}
	}
}
