using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphPainterNs;

namespace DotNetKP
{
    public partial class UserDialog : Form
    {
        Graph graph;
        Graphics g;
        GraphPainter graphPainter;
        int clickSensetivity = 10;
        bool alreadyDrawed = false; 
        public UserDialog()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();

            // обработка нажатий на линии графа для и разворота
            Click += (param1, param2) => {
                try
                {
                    Point closestLink = getClosestLink();
                    graph[closestLink.X].reverseLink(closestLink.Y);
                    submit(sender, e);
                    var cycles = graph.getCycles();
                    textBox3.Text = "";
                    for (int i = 0; i < cycles.Count; i++)
                    {
                        for (int j = 0; j < cycles[i].Count; j++)
                        {
                            if (j != cycles[i].Count - 1)
                                textBox3.Text += cycles[i][j] + " - ";
                            else
                                textBox3.Text += cycles[i][j];

                        }
                        textBox3.Text += "\r\n";
                        }
                }
                catch { };
                
            };

            // изменение курсора при наведении на линии графа
            timer1.Tick += (param1, param2) =>
            {
                try
                {
                    getClosestLink();
                    Cursor.Current = Cursors.Hand;
                }
                catch
                {
                    Cursor.Current = Cursors.Arrow;
                }
            };
        }
        private void drawGraph(PaintEventArgs e)
        {
            try
            {
                Random random = new Random((int)System.DateTime.Now.Ticks);
                string textFromTextBox = textBox2.Text;
                int[] degrees = Regex.Split(textFromTextBox, "\\W+").Select(int.Parse).ToArray();

                if (!alreadyDrawed)
                    graph = new Graph(degrees, checkBox1.Checked);

                clearScreen(new object(),new EventArgs());
                if (graph.isReal(degrees, checkBox1.Checked) == 0)
                {
                    MessageBox.Show("Граф с нечетной суммой степеней вершин не существует!");
                    return;
                };
               
                if (graph.isReal(degrees, checkBox1.Checked) == -1)
                {
                    MessageBox.Show("Заданы параметры мультиграфа, но вы хотите построить простой граф!");
                    return;
                };

                graphPainter = new GraphPainter(graph);
                graphPainter.drawGraph(e);
                alreadyDrawed = true;
                
            }
            catch (FormatException) { };
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            alreadyDrawed = false;
        }
        private void submit(object sender, EventArgs e)
        {
            drawGraph(new PaintEventArgs(g, new Rectangle()));
        }
        private void clearScreen(object sender, EventArgs e)
        {
            g.Clear(g.GetNearestColor(BackColor));
            alreadyDrawed = false;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private double FindDistance(PointClass pt, PointClass p1, PointClass p2)
        {
            PointClass closest = new PointClass(0, 0);
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // Это точка не отрезка.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Вычислим t, который минимизирует расстояние.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            // Посмотрим, представляет ли это один из сегментов
            // конечные точки или точка в середине.
            if (t < 0)
            {
                closest = new PointClass(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointClass(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointClass((int)(p1.X + t * dx), (int)(p1.Y + t * dy));
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }
        private Point getClosestLink()
        {
            if (graph == null || graph.Count() == 0) throw new Exception("Graph is not filled");

            // Переменная с локацией места клика курсором
            PointClass clkPnt = new PointClass(
                Cursor.Position.X - this.Location.X, 
                Cursor.Position.Y - this.Location.Y - 35);

            int index = -1,
                jindex = -1;
            double distance,
                   smallestDistance = 9999;
            for (int i = 0; i < graph.Count(); i++)
            {
                for (int j = 0; j < graph[i].Count; j++)
                {
                    PointClass stPnt = graph[i].StartPoint;
                    PointClass endPnt = graph[i][j];

                    // Вычисление дистанции от клика до линий графа по формуле
                    distance = FindDistance(clkPnt, stPnt, endPnt);
                    if ((int)distance <= clickSensetivity && distance < smallestDistance)
                    {
                        index = i;
                        jindex = j;
                        smallestDistance = distance;
                    }
                }
            }
            if (index == -1 && jindex == -1) 
                throw new Exception("The click is too far");

            return new Point(index, jindex);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
