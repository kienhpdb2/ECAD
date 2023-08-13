using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ECAD.Entities;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp;

namespace ECAD.Methods
{
    public static class Method
    {
        public static Vector3 LineLineIntersection(Entities.Line line1, Entities.Line line2, bool extended = false)
        {
            Vector3 result;
            Vector3 p1 = line1.StartPoint;
            Vector3 p2 = line1.EndPoint;
            Vector3 p3 = line2.StartPoint;
            Vector3 p4 = line2.EndPoint;

            double dx12 = p2.X - p1.X;
            double dy12 = p2.Y - p1.Y;
            double dx34 = p4.X - p3.X;
            double dy34 = p4.Y - p3.Y;

            double denominator = (dy12 * dx34 - dx12 * dy34);
            double k1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;

            if (double.IsInfinity(k1))
            {
                return new Vector3(double.NaN, double.NaN);
            }

            result = new Vector3(p1.X + dx12 * k1, p1.Y + dy12 * k1);

            if (extended) return result;
            else
            {
                if (IsPointOnLine(line1, result) && IsPointOnLine(line2, result)) return result;

                else return new Vector3(double.NaN, double.NaN);
            }
        }


        public static bool IsPointOnLine(Line line1, Vector3 point)

        {

            return IsEqual(line1.Length, line1.StartPoint.DistanceFrom(point) + line1.EndPoint.DistanceFrom(point));

        }

        private static double Epsilon = 1e-12;



        private static bool IsEqual(double d1, double d2)

        {

            return IsEqual(d1, d2, Epsilon);

        }

        private static bool IsEqual(double d1, double d2, double epsilon)

        {

            return IsZero(d1 - d2, epsilon);
        }
        private static bool IsZero(double d, double epsilon)
        {
            return d >= -epsilon && d <= epsilon;
        }
        #region Get segment idex
        //public static int GetSegmentIndex(List<EntityObject> entities, Vector3 mousePosition, PointF[] cursor_rect, out Vector3 PointOnSegment)
        //{
        //    bool flags = false;
        //    Vector3 poSegment = new Vector3(0,0,0);
        //    for (int i=0; i<entities.Count; i++)
        //    {
        //        switch (entities[i].Type)
        //        {
        //            case EntityType.Line:
        //        }

        //    }


        //}
        #endregion
        #region Modify1 Selection
        //public static void Modify1Selection(int modifyIndex, List<EntityObject> entities, Vector3 fromPoint, Vector3 toPoint)
        //{
        //    for (int i = 0; i < entities.Count; i++)
        //    {
        //        if (entities[i].IsSelected)
        //        {
        //            switch (modifyIndex)
        //            {
        //                case 0: // Copy
        //                    entities.Add(entities[i].CopyOrMove(fromPoint, toPoint) as EntityObject);
        //                    break;
        //                case 1: // Move
        //                    entities[i] = entities[i].CopyOrMove(fromPoint, toPoint) as EntityObject;
        //                    entities[i].DeSelect();
        //                    break;
        //            }
        //        }
        //    }
        //}
        #endregion
        public static void Rotate(this PictureBox pictureBox, float angle)
        {
            // Kiểm tra hình ảnh đã tồn tại và góc quay hợp lệ
            if (pictureBox.Image == null || angle % 360 == 0)
            {
                return;
            }

            // Tính toán tâm của PictureBox
            float centerX = pictureBox.Width / 2f;
            float centerY = pictureBox.Height / 2f;

            // Tạo một bitmap mới để vẽ hình ảnh đã quay lên
            Bitmap rotatedImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.Clear(Color.White);

                // Thiết lập các giá trị quay
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(angle);
                g.TranslateTransform(-centerX, -centerY);

                // Vẽ hình ảnh đã quay
                g.DrawImage(pictureBox.Image, new System.Drawing.Point(0, 0));
            }

            // Hiển thị hình ảnh đã quay trong PictureBox
            pictureBox.Image = rotatedImage;
        }
        public static void Delete(List<EntityObject> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].IsSelected)
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
