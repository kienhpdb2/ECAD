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
using ECAD.Methods;

namespace ECAD
{
    public partial class GraphicsForm : Form
    {
        private PictureBox pictureBoxResistor;
        Random rand = new Random();
        List<PictureBox> items = new List<PictureBox>();
        public GraphicsForm()
        {
            InitializeComponent();
        }

        private PictureBox newPic;
        private void MakePictureBox()
        {
            newPic = new PictureBox();
            newPic.Height = pictureBox1.Height;
            newPic.Width = pictureBox1.Width;
            newPic.Location = pictureBox1.Location;
            newPic.SizeMode = pictureBox1.SizeMode;
            // Đường dẫn tới hình ảnh 
            // string imagePath = "C:\\Users\\dinhk\\Downloads\\360_F_337277306_bOwr2gmisHGZYFQrnYEYNmx2uVP7nTxE.jpg";
            // Hiển thị hình ảnh trong PictureBox
            newPic.Image = pictureBox1.Image;
            ControlExtension.Draggable(newPic, true);
            items.Add(newPic);
            this.Controls.Add(newPic);
            newPic.BringToFront();
        }

        // list
        private List<Entities.Point> points = new List<Entities.Point>();
        private List<Entities.Line> lines = new List<Entities.Line>();
        private List<LwPolyline> polylines = new List<LwPolyline>();
        private LwPolyline tempPolyline = new LwPolyline();

        // Vetor3
        private Vector3 currentPosition;  // float point pare currentPosition
        private Vector3 firstPoint;

        //int
        private int DrawIndex = -1;
        private int Modify1Index = -1;
        private int segmentIndex = -1;
        private int ClickNum = 1;
        int pictureBoxCount = 0;

        //bool
        private bool active_drawing = false;
        private bool active_modify = false;
        private bool active_selection = true;

        private Line line;

        private void drawing_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = PointToCartesian(e.Location);
            label1.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y); // get Loaction
            label2.Text = string.Format("{0,0:F3}, {1,0:F3}", currentPosition.X, currentPosition.Y);
            // làm tròn đến 3 chữ số 
            drawing.Refresh();
        }

        private void CancelAll(int index = 1)
        {
            DrawIndex = -1;
            active_drawing = false;
            ClickNum = 1;
            // LwPolylineCloseStatus(index);
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
        private float Pixel_to_Mn(float pixel)
        {
            return pixel * 25.4f / DPI;
        }

        private void drawing_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // click chuột trái
            {
                if (active_drawing)
                {
                    switch (DrawIndex)
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
            if (points.Count > 0)
            {
                foreach (Entities.Point p in points)
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
            if (polylines.Count > 0)
            {
                foreach (LwPolyline lw in polylines)
                {
                    e.Graphics.DrawLwPolyline(pen, lw);
                }
            }
            //DrawtempPolyline
            if (tempPolyline.Vertexes.Count > 1)
            {

                e.Graphics.DrawLwPolyline(pen, tempPolyline);
            }
            // Draw line extended

            switch (DrawIndex)
            {
                case 1:
                case 2:
                    if ((ClickNum == 2))
                    {
                        Entities.Line line = new Entities.Line(firstPoint, currentPosition);
                        e.Graphics.DrawLine(extpen, line);
                    }
                    break;
            }
            // Test line line intersection

            if (lines.Count > 0)
            {
                foreach (Entities.Line l1 in lines)
                {
                    foreach (Entities.Line l2 in lines)
                    {
                        Vector3 v = Methods.Method.LineLineIntersection(l1, l2);
                        Entities.Point p = new Entities.Point(v);
                        e.Graphics.DrawPoint(new Pen(Color.Red, 0), p);
                    }
                }

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
        //private void LwPolylineCloseStatus (int index)
        //{
        //    List<LwPolylineVertex> vertexes = new List<LwPolylineVertex>();
        //    foreach (LwPolylineVertex lw in tempPolyline.Vertexes)
        //        vertexes.Add(lw);
        //    if (vertexes.Count > 1)
        //    {
        //        switch (index)
        //        {
        //            case 1:
        //                if (vertexes.Count > 2)
        //                    polylines.Add(new LwPolyline(vertexes, true));
        //                else
        //                    polylines.Add(new LwPolyline(vertexes, false));
        //                break;
        //            case 2:
        //                polylines.Add(new LwPolyline(vertexes, false));

        //                break;
        //        }
        //    }
        //    tempPolyline.Vertexes.Clear();
        //}
        private void CancelAll()
        {
            DrawIndex = -1;
            active_drawing = false;
            drawing.Cursor = Cursors.Default;
            ClickNum = 1;
        }

        private void cancelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CancelAll();
        }

        public void rButton_Click(object sender, EventArgs e)
        {
            MakePictureBox();

        }

        private void rotateBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Rotate((float)90);
            pictureBox1.Refresh();

            //if (newPic != null) // Kiểm tra nếu newPic đã được tạo
            //{
            //    // Quay hình ảnh trong newPic 45 độ mỗi khi nhấn nút rotateBtn
            //    newPic.Rotate(45);
            //    newPic.Refresh();
        }
    } 
}

