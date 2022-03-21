using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPainterNs
{
    class Node
    {
        PointClass startPoint;
        List<PointClass> endPoints;
        public List<bool> outGoingLinks;

        public Node()
        {
            endPoints = new List<PointClass>();
            outGoingLinks = new List<bool>();
        }
        public Node(PointClass startPoint)
        {
            this.startPoint = startPoint;
            endPoints = new List<PointClass>();
            outGoingLinks = new List<bool>();
        }
        public PointClass StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                startPoint = value;
            }
        }
        public PointClass this[int index]
        {
            get { return endPoints[index]; }
            set { endPoints[index] = value; }
        }
        public int Count
        {
            get
            {
                return endPoints.Count;
            }
        }
        public void reverseLink(int index)
        {
            outGoingLinks[index] = !outGoingLinks[index];
        }
        public void Remove(PointClass point)
        {
            outGoingLinks.RemoveAt(endPoints.IndexOf(point));
            endPoints.Remove(point);
        }
        public void Remove(System.Drawing.Point point)
        {
            for (int i = 0; i < endPoints.Count; i++)
            {
                if (endPoints[i].Equals(point))
                {
                    endPoints.Remove(endPoints[i]);
                    outGoingLinks.RemoveAt(i);
                    break;
                }
            }
        }
        public static Node operator +(Node node, PointClass point)
        {
            node.endPoints.Add(point);
            node.outGoingLinks.Add(true);
            return node;
        }

    }
}
