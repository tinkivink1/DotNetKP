using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphPainterNs
{
    class GraphPainter
    {
        Graph graph;
        Pen pen;
        Pen pointPen;
        public int _spaceBetweenPoints = 50;
        Random rand = new Random();
        /// <summary>
        /// Конструктор, который задает стандартный стиль отрисовки графа
        /// Черная линия толщиной 2px,
        /// Толщина точек 5px
        /// </summary>
        public GraphPainter()
        {
            graph = new Graph();
            pen = new Pen(Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 2);
            pointPen = new Pen(Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 5);

        }


        public GraphPainter(Graph graph)
        {
            this.graph = new Graph(graph);
            pen = new Pen(Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 2);
            pointPen = new Pen(Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 5);
            
        }
        ///// <summary>
        ///// Конструктор, который задает стиль отрисовки линий графа
        ///// Толщина точек 5px
        ///// </summary>
        ///// <param name="pen">Стиль отрисовки линий графа</param>
        public GraphPainter(Graph graph, Pen pen)
        {
            this.graph = graph;
            this.pen = pen;
            pointPen = pen;
        }
        ///// <summary>
        ///// Конструктор, который задает стиль отрисовки графа
        ///// </summary>
        ///// <param name="pen">Стиль отрисовки линий графа</param>
        ///// <param name="pointPen">Стиль отрисовки точек графа</param>
        public GraphPainter(Graph graph, Pen pen, Pen pointPen)
        {
            this.graph = graph;
            this.pen = pen;
            this.pointPen = pointPen;
        }


        public void drawGraph(PaintEventArgs e)
        {
            int counter;
            for (int i = 0; i < graph.Count(); i++)
            {
                for (int k = 0; k < graph[i].Count; k++)
                {
                    counter = 0;
                    for (int j = 0; j < graph[i].Count; j++)
                    {
                        if (graph[i][k].X == graph[i][j].X && k != j)
                        {
                            counter++;
                            drawArcs(e, graph[i].StartPoint.toStruct(), graph[i][j].toStruct(), 20 * counter);
                            drawArcs(e, graph[i].StartPoint.toStruct(), graph[i][j].toStruct(), 20 * counter);
                            graph[i].Remove(graph[i][k]);
                            graph[i].Remove(graph[i][j-1]);
                            j = 1;
                        }
                    }
                }
            }

            Graph graphToPrint = new Graph(graph.uniqueNodes());
            graphToPrint._degrees = graph._degrees;
            for (int i = 0; i < graph.Count(); i++)
            {
                for (int k = 0; k < graph[i].Count; k++)
                {
                    if (graphToPrint._degrees[i] == 2) drawCircle(e, graphToPrint[i].StartPoint.toStruct());
                    if(graphToPrint[i].outGoingLinks[k])
                        drawArrow(e, graphToPrint[i].StartPoint, graphToPrint[i][k]);
                    else
                        drawArrow(e, graphToPrint[i][k], graphToPrint[i].StartPoint);

                }
            }

            Point temp = new Point();
            for(int i = 0; i < graphToPrint.zeroDegreesCounter; i++) 
            {
                temp = new Point(graphToPrint._degrees.Count() * 100 + i * 50 + 50, rand.Next(0, 400) + 40);
                e.Graphics.DrawEllipse(pointPen, temp.X - 3, temp.Y - 3, 5, 5);
                
            }
        }
        private void drawLine(PaintEventArgs e, Point firstPoint, Point lastPoint)
        {
            e.Graphics.DrawLine(pen, firstPoint.X, firstPoint.Y, lastPoint.X, lastPoint.Y);
            e.Graphics.DrawEllipse(pointPen, firstPoint.X - 3, firstPoint.Y - 3, 5, 5);
            e.Graphics.DrawEllipse(pointPen, lastPoint.X - 3, lastPoint.Y - 3, 5, 5);
        }
        void drawArrow(PaintEventArgs e, PointClass start, PointClass end)
        {
            float x1 = start.X, y1 = start.Y, x2 = end.X, y2 = end.Y;
            // вычисляем угол, под которым стрелка повёрнута против часовой
            // вычисляем длину стрелки
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            angle = angle * 180f / (float)Math.PI; // переводим в градусы
            float len = (float)Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));
            // сохраняем старое состояние Graphics, чтобы потом его восстановить
            GraphicsState state = e.Graphics.Save();
            // трансформация: сдвиг к точке start и поворот на угол
            e.Graphics.TranslateTransform(x1, y1);
            e.Graphics.RotateTransform(angle);
            int arrowLength = 30; // длина окончания стрелки
            int arrowWidth = 5; // ширина окончания стрелки


            // рисуем стрелку в горизонтальном положении
            e.Graphics.DrawLine(pen, 0, 0, len, 0);
            e.Graphics.DrawLine(pen, len, 0, len - arrowLength, arrowWidth);
            e.Graphics.DrawLine(pen, len, 0, len - arrowLength, -arrowWidth);
            // восстанавливаем старое состояние Graphics (убираем наши трансформации)\
            e.Graphics.Restore(state);

            drawPoint(e, start);
            drawPoint(e, end);
            fillPoint(e, start);
            fillPoint(e, end);
        }
        private void drawCircle(PaintEventArgs e, Point point)
        {
            point.X -= 8;
            point.Y -= 8;
            e.Graphics.DrawEllipse(pen, new Rectangle(point, new Size(_spaceBetweenPoints / 2, _spaceBetweenPoints / 2)));
        }
        private void drawArcs(PaintEventArgs e, Point point1, Point point2, int distance)
        {
            e.Graphics.DrawBezier(pen, point1, new Point(Math.Abs(point1.X+point2.X)/2+ distance, Math.Abs(point1.Y + point2.Y)/2 + distance), new Point(Math.Abs(point1.X + point2.X) / 2 + distance, Math.Abs(point1.Y + point2.Y) / 2 + distance), point2);
            distance *= -1;
            e.Graphics.DrawBezier(pen, point1, new Point(Math.Abs(point1.X+point2.X)/2+ distance, Math.Abs(point1.Y + point2.Y)/2 + distance), new Point(Math.Abs(point1.X + point2.X) / 2 + distance, Math.Abs(point1.Y + point2.Y) / 2 + distance), point2);
        }
        private void drawPoint(PaintEventArgs e, PointClass point)
        {
            e.Graphics.DrawEllipse(pen, point.X - 10, point.Y - 10,
                      20, 20);
        }
        private void fillPoint(PaintEventArgs e, PointClass point)
        {
            e.Graphics.FillEllipse(Brushes.White, point.X - 10, point.Y - 10,
                      20, 20);
            e.Graphics.DrawString(point.getNumber.ToString(),
                new Font(FontFamily.GenericSansSerif,12.0F),
                Brushes.Black, point.X - 7, point.Y - 10);
        }
        
    }
}
