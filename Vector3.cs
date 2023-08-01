using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECAD
{
    public class Vector3
    {
		private double x;
		private double y;
        private double z;

		public Vector3 (double x, double y)
		{
			this.X = x;
			this.Y = y;
			this.Z = 0.0;
		}
		public Vector3 (double x,double y,double z)
			: this(x,y)
		{
			this.Z = z;
		}
        public double  X
		{
			get { return x; }
			set { x = value; }
		}

		public double Y
		{
			get { return y; }
			set { y = value; }
		}
        public double Z
        {
            get { return z; }
            set { z = value; }
        }
        #region Vector Unit
        public static Vector3 Unitx
		{

			get { return new Vector3(1, 0, 0);}
		}

		public static Vector3 Unity
		{ 
			get { return new Vector3(0, 1, 0); }

		}
		public static Vector3 UnitZ {
		get { return new Vector3(0, 0, 1); }

		}
		public static Vector3 NaN
		{
			get { return new Vector3(double.NaN, double.NaN, double.NaN); }
		}		
		#endregion
		public double this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return this.x;
					case 1: return this.y;
					case 2: return this.z;
					default: throw new ArgumentOutOfRangeException(nameof(index));
				}
			}
			set
			{
				switch (index)
				{
					case 0: this.x = value; break;
					case 1: this.y = value; break;
					case 2: this.z = value;	break;
					default: throw new ArgumentOutOfRangeException(nameof(index));
				}
			}
		}
		public System.Drawing.PointF ToPointF
		{
			get
			{
				return new System.Drawing.PointF((float)X, (float)Y);
			}
		}
		public static Vector3 Zero
		{
			get { return new Vector3(0.0, 0.0, 0.0); }
		}
        public double DistanceFrom(Vector3 v)
        {
            double dx = v.X - X;
            double dy = v.Y - Y;
            double dz = v.Z - Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public Vector2 ToVector2
		{
			get { return new Vector2(X, Y);}
		}
		public Vector3 CopyOrMove(Vector3 fromPoint, Vector3 toPoint)
		{
			double dx = toPoint.X - fromPoint.X;
			double dy = toPoint.Y - fromPoint.Y;
			double dz = toPoint.Z - fromPoint.Z;
			return new Vector3(this.x + dx, this.y + dy, this.z + dz);
		}

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", X, Y, Z);
        }
    }

}
