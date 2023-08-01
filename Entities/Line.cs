using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECAD.Entities
{
    public class Line
		:EntityObject
    {
		private Vector3 startPoint;
		private Vector3 endPoint;
		private double thickness;

		public Line()
			:this(Vector3.Zero, Vector3.Zero)
		{
			
		}
		
		public Line(Vector3 start, Vector3 end)
			:base(EntityType.Line)
		{
			this.StartPoint = start;
			this.EndPoint = end;
			this.Thickness = 0.0;
		}

		public Vector3 EndPoint
        {
			get { return endPoint; }
			set { endPoint = value; }
		}


		public Vector3 StartPoint
        {
			get { return startPoint; }
			set { startPoint = value; }
		}
        public double Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
		public double Length
		{
			get
			{
				double dx=endPoint.X - startPoint.X; 
				double dy=endPoint.Y - startPoint.Y;
				double dz = endPoint.Z - startPoint.Z;
				return Math.Sqrt(dx * dx + dy * dy + dz * dz);
			}
		}
		public override object CopyOrMove(Vector3 fromPoint, Vector3 toPoint)
		{
			Vector3 startpoint = this.startPoint.CopyOrMove(fromPoint, toPoint);
			Vector3 endpoint = this.endPoint.CopyOrMove(fromPoint, toPoint);
			return new Line
			{
				StartPoint = startpoint,
				EndPoint = endpoint,
				Thickness = this.thickness,
				IsVisible = this.isVisible
			};
		}
        public override object Clone()
        {
			return new Line
			{
				StartPoint = this.startPoint,
				EndPoint = this.endPoint,
                Thickness = this.thickness,
                // EntityObject properties
                IsVisible = this.isVisible
            };

        }

    }
}
