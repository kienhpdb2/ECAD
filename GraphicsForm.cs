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
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp;
namespace ECAD
{
    public partial class GraphicsForm : Form
    {

        private Circuit ckt = new Circuit();
        public Circuit GetCircuit()
        {
            return ckt;
        }
        List<PictureBox> items = new List<PictureBox>();
        List<TextBox> textures = new List<TextBox>();
        public GraphicsForm()
        {
            InitializeComponent();
        }

        private PictureBox newPic;
        private void MakePictureBox(PictureBox sourcePictureBox)
        {
            PictureBox newPic = new PictureBox();
            newPic.Height = sourcePictureBox.Height;
            newPic.Width = sourcePictureBox.Width;
            newPic.Location = sourcePictureBox.Location;
            newPic.SizeMode = sourcePictureBox.SizeMode;
            newPic.Image = sourcePictureBox.Image;
            ControlExtension.Draggable(newPic, true);
            newPic.MouseClick += NewPic_MouseClick;
            items.Add(newPic);
            this.Controls.Add(newPic);
            newPic.BringToFront();
        }
        private void MakeTextBox(PictureBox sourcePictureBox)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Location = sourcePictureBox.Location; // You might want to adjust the location
            //newTextBox.Size = textBox1.Size;
           // newTextBox.Font = textBox1.Font;
            newTextBox.TextChanged += NewTextBox_TextChanged;
            newTextBox.MouseDoubleClick += NewTextBox_MouseClick;
            this.Controls.Add(newTextBox);
            ControlExtension.Draggable(newTextBox, true);
            //newTextBox.MouseClick += NewTextBox_MouseClick;
            textures.Add(newTextBox);
            newTextBox.BringToFront();
        }
        private void NewTextBox_TextChanged(object sender, EventArgs e)
        {
        }
        // list
        private List<EntityObject> entities;
        private List<Entities.Point> points = new List<Entities.Point>();
        private List<Entities.Line> lines = new List<Entities.Line>();
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

        //private void CancelAll(int index = 1)
        //{
        //    DrawIndex = -1;
        //    active_drawing = false;
        //    ClickNum = 1;
        //    // LwPolylineCloseStatus(index);
        //}

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

            if (e.Button == MouseButtons.Left) // giữ chuột trái
            {

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
            // Draw line extended

            switch (DrawIndex)
            {
                case 1:
                    if ((ClickNum == 2))
                    {
                        Entities.Line line = new Entities.Line(firstPoint, currentPosition);
                        e.Graphics.DrawLine(extpen, line);
                    }
                    break;
            }
            //Test line line intersection

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

        #region Eror
        //private void closeBoundary_Click(object sender, EventArgs e)
        //{
        //    switch (DrawIndex)
        //    {
        //        case 1: 
        //            CancelAll(2);
        //            break;
        //    }
        //}
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
        #endregion
        private void CancelAll()
        {
            DrawIndex = -1;
            active_drawing = false;
            drawing.Cursor = Cursors.Default;
            ClickNum = 1;
        }

        private void cancelToolStripMenuItem1_Click(object sender, EventArgs e) // nút cancel
        {
            CancelAll();
        }

        public void rButton_Click(object sender, EventArgs e)
        {
            PictureBox sourcePictureBox = sender as PictureBox;
            MakePictureBox(sourcePictureBox);
        }
        private void NewPic_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {

                PictureBox temPic = sender as PictureBox;
                items.Remove(temPic);
                this.Controls.Remove(temPic);

            }
            else if (e.Button == MouseButtons.Right)
            {
                MakeTextBox(sender as PictureBox);
            }
        }

        private void NewTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox temText = sender as TextBox;
            textures.Remove(temText);
            this.Controls.Remove(temText);

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
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox sourcePictureBox = sender as PictureBox;
                MakePictureBox(sourcePictureBox);
            }
        }
        private bool IsObjectSelected()
        {
            foreach (EntityObject entity in entities)
            {
                return true;
            }
            return false;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {

        }
        private Pen drawPen = new Pen(Color.Black, 2);
        private int currentLineIndex = -1;



        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox sourcePictureBox = sender as PictureBox;
                MakePictureBox(sourcePictureBox);
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox sourcePictureBox = sender as PictureBox;
                MakePictureBox(sourcePictureBox);
            }
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox sourcePictureBox = sender as PictureBox;
                MakePictureBox(sourcePictureBox);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                VoltageSource V_input = new VoltageSource("V1", "in", "0", Double.Parse(textBox1.Text));
                ckt.Add(V_input);
            }
            catch (System.FormatException err) { ckt.Remove("V1"); }
            catch (System.ArgumentException err) { ckt.Remove("V1"); }
        }

        private void button1_Click(object sender, EventArgs e)
        { // Create a DC simulation that sweeps V1 from -1V to 1V in steps of 100mV
            try
            {
                ckt.Add(new Resistor("R1", "in", "out", Double.Parse(textures[0].Text)));
            }
            catch (System.FormatException err) { ckt.Remove("R1"); }
            catch (System.ArgumentException err) { ckt.Remove("R1"); }
            try
            {
                ckt.Add(new Resistor("R2", "out", "0", Double.Parse(textures[1].Text)));
            }
            catch (System.FormatException err) { ckt.Remove("R2"); }
            catch (System.ArgumentException err) { ckt.Remove("R2"); }
            var dc = new DC("DC 1", "V1", Double.Parse(textBox2.Text), Double.Parse(textBox2.Text), Double.Parse(textBox2.Text));

            // Create exports
            var inputExport = new RealVoltageExport(dc, "in");
            var outputExport = new RealVoltageExport(dc, "out");
            var currentExport = new RealPropertyExport(dc, "V1", "i");
            dc.ExportSimulationData += (s, exportDataEventArgs) =>
            {
                var input = inputExport.Value;
                var output = outputExport.Value;
                var current = currentExport.Value;
                //exportDataEventArgs.GetVoltage("out");
                textBox3.Text = output.ToString();
                textBox4.Text = current.ToString();

            };

            // Run the simulation
            try { dc.Run(ckt); }
            catch (SpiceSharp.BehaviorsNotFoundException err)
            {
                ckt.Add(new VoltageSource("V1", "in", "0", Double.Parse(textBox2.Text)));
                try { ckt.Add(new Resistor("R1", "in", "out", Double.Parse(textures[0].Text))); }
                catch (System.ArgumentException)
                {
                    ckt.Remove("R1");
                    ckt.Add(new Resistor("R1", "in", "out", Double.Parse(textures[0].Text)));
                }
                ckt.Remove("R2");
                ckt.Add(new Resistor("R2", "out", "0", Double.Parse(textures[1].Text)));
                dc.Run(ckt);
            }
        }
        private void Calculate(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
               
                }
            }
        }


    }

