using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECAD.Entities;

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


        private static bool IsPointOnLine(Line line1, Vector3 result) {

            return IsEqual(line1)
                }
            }
        }
    }
}
