using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ECAD.Entities;

namespace ECAD
{
    public static class GraphicsExtension
    {
        private static float Height;

        public static void SetParameters(this System.Drawing.Graphics g, float height)
        {
            Height = height;
        }

        public static void SetTransForm (this System.Drawing.Graphics g)
        {
            g.PageUnit = System.Drawing.GraphicsUnit.Millimeter;
            g.TranslateTransform(0, Height);
            g.ScaleTransform(1.0f, -1.0f);
        }

        public static void DrawPoint(this System.Drawing.Graphics g, System.Drawing.Pen pen, Entities.Point point)
        {
            g.SetTransForm();
            System.Drawing.PointF p = point.Position.ToPointF;
            g.DrawEllipse(pen, p.X - 1, p.Y - 1, 2, 2);
            g.ResetTransform();
        }

        public static void DrawLine(this System.Drawing.Graphics g, System.Drawing.Pen pen, Entities.Line line)
        {
            g.SetTransForm();
            g.DrawLine(pen, line.StartPoint.ToPointF, line.EndPoint.ToPointF);
            g.ResetTransform();
        }

        public static void DrawLwPolyline (this Graphics g, Pen pen, LwPolyline polyline)
        {
            PointF[] vertexes = new PointF[polyline.Vertexes.Count];
            if(polyline.Isclosed)
            {
                vertexes = new PointF[polyline.Vertexes.Count + 1];
                vertexes[polyline.Vertexes.Count] = polyline.Vertexes[0].Position.ToPointF;
            }

            for (int i=0; i<polyline.Vertexes.Count; i++)
                vertexes[i] = polyline.Vertexes[i].Position.ToPointF;
            g.SetTransForm();
            g.DrawLines(pen, vertexes);
            g.ResetTransform();
        }
    }
}
