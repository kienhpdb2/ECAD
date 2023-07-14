using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ECAD.Entities;

namespace ECAD
{
    public partial class GraphicsForm : Form
    {
        public GraphicsForm()
        {
            InitializeComponent();
        }

        // líst

        private List<Entities.Point> points = new List<Entities.Point>();
        private List<Entities.Line> lines = new List<Entities.Line>();
        private List<LwPolyline> polylines = new List<LwPolyline>();
        private LwPolyline tempPolyline = new LwPolyline(); 

            // Vetor3
        private Vector3 currentPosition;  // float point pare currentPosition
        private Vector3 firstPoint;

        //int
        private int DrawIndex = -1;
        private bool active_drawing = false;
        private int ClickNum = 1;
        private Line line;

        private void drawing_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = PointToCartesian(e.Location);
            label1.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y); // get Loaction
            label2.Text = string.Format("{0,0:F3}, {1,0:F3}", currentPosition.X, currentPosition.Y);
                                          // làm tròn đến 3 chữ số 
            drawing.Refresh();
        }

        private void CancelAll(int index =1 )
        {
            DrawIndex = -1;
            active_drawing = false;
            ClickNum = 1;
            LwPolylineCloseStatus(index);
        }

            // Get screen dpi
            private float DPI
        {
            get
            {
                using (var g = CreateGraphics())
                    return g.DpiX;

            }
        }
        //Convert system point to Decarte point
        private Vector3 PointToCartesian(System.Drawing.Point point)  
        {
            return new Vector3(Pixel_to_Mn(point.X), Pixel_to_Mn(drawing.Height - point.Y));
        }
       // Convert pixels to millimeters
       private float Pixel_to_Mn (float pixel)
        {
            return pixel * 25.4f / DPI;
        }

        private void drawing_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (active_drawing)
                {
                    switch(DrawIndex)
                    {
                        case 0: // Point
                            points.Add(new Entities.Point(currentPosition));
                            break;
                        case 1: // Line
                            switch (ClickNum)
                            {
                                case 1:
                                    firstPoint = currentPosition;
                                    points.Add(new Entities.Point(currentPosition));
                                    ClickNum++;
                                    break;
                                case 2:
                                    lines.Add(new Entities.Line(firstPoint, currentPosition));
                                    points.Add(new Entities.Point(currentPosition));
                                    firstPoint = currentPosition;
                                    break;
                            
                            }
                            break;
                        case 2: // LwPolyLine
                            firstPoint = currentPosition;
                            tempPolyline.Vertexes.Add(new LwPolylineVertex(firstPoint.ToVector2));
                            ClickNum = 2;

                            break;
                    }
                    drawing.Refresh();
                }
            }
        }

        private void drawing_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SetParameters(Pixel_to_Mn(drawing.Height));
            Pen pen = new Pen(Color.Blue, 1f);
            Pen extpen = new Pen(Color.Gray, 1f);

            // Draw all points
            if (points.Count > 0 )
            {
                foreach(Entities.Point p in points)
                {
                    e.Graphics.DrawPoint(new Pen(Color.Black, 0.1f), p);
                 
                }
            }
            // Draw all lines
            if (lines.Count > 0)
            {
                foreach (Entities.Line l in lines)
                {
                    e.Graphics.DrawLine(pen, l);
                }
            }
            // Draw all LwPolyline 
            if(polylines.Count > 0)
            {
                foreach(LwPolyline lw in polylines)
                {
                    e.Graphics.DrawLwPolyline (pen, lw);
                }    
            }
            //DrawtempPolyline
            if(tempPolyline.Vertexes.Count > 1) { 
           
                e.Graphics.DrawLwPolyline (pen, tempPolyline);
            }
            // Draw line extended

            switch(DrawIndex)
            {
                case 1:
                case 2: 
                    if((ClickNum == 2))
                    {
                        Entities.Line line = new Entities.Line(firstPoint, currentPosition);
                        e.Graphics.DrawLine(extpen,line);
                    }
                    break;
            }
                   
        }   

        private void pointBtn_Click(object sender, EventArgs e)
        {
            DrawIndex = 0;
            active_drawing = true;
            drawing.Cursor = Cursors.Cross;

        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            DrawIndex = 1;
            active_drawing = true;
            drawing.Cursor = Cursors.Cross;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void closeBoundary_Click(object sender, EventArgs e)
        {
            switch (DrawIndex)
            {
                case 1: //Line
                    break;
                case 2: //LwPolyLine
                    CancelAll(2);
                    break;
            }
        }
        private void LwPolylineCloseStatus (int index)
        {
            List<LwPolylineVertex> vertexes = new List<LwPolylineVertex>();
            foreach (LwPolylineVertex lw in tempPolyline.Vertexes)
                vertexes.Add(lw);
            if (vertexes.Count > 1)
            {
                switch (index)
                {
                    case 1:
                        if (vertexes.Count > 2)
                            polylines.Add(new LwPolyline(vertexes, true));
                        else
                            polylines.Add(new LwPolyline(vertexes, false));
                        break;
                    case 2:
                        polylines.Add(new LwPolyline(vertexes, false));

                        break;
                }
            }
            tempPolyline.Vertexes.Clear();
        }
    }
}
