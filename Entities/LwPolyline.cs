using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECAD.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECAD
{
    public class LwPolyline
        : EntityObject
    {
        private List<LwPolylineVertex> vertexes;
        private PolyLineTypeFlags flags;
        private double thickness;

        public LwPolyline()
            : this(new List<LwPolylineVertex>(), false)
        {
        }

        public LwPolyline(List<LwPolylineVertex> vertexes, bool isclosed)
            : base(EntityType.LwPolyline)
        {
            if (vertexes == null)
                throw new ArgumentNullException(nameof(vertexes));
            this.vertexes = vertexes;
            this.flags = Isclosed ? PolyLineTypeFlags.CloseLwPolyline : PolyLineTypeFlags.OpenLwPolyline;
            this.thickness = 0.0;
        }

        public List<LwPolylineVertex> Vertexes
        {
            get { return this.vertexes; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(vertexes));
                this.vertexes = value;
            }
        }
        internal PolyLineTypeFlags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        public bool Isclosed
        {
            get { return (this.flags & PolyLineTypeFlags.CloseLwPolyline) == PolyLineTypeFlags.CloseLwPolyline; }
            set { this.flags = value ? PolyLineTypeFlags.CloseLwPolyline : PolyLineTypeFlags.OpenLwPolyline; }
        }
        public double Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        public override object CopyOrMove(Vector3 fromPoint, Vector3 toPoint)
        {
            List<LwPolylineVertex> vertex = new List<LwPolylineVertex>();
            foreach (LwPolylineVertex lw in this.vertexes)
            {
                LwPolylineVertex lv = new LwPolylineVertex
                {
                    Position = lw.Position.CopyOrMove(fromPoint.ToVector2, toPoint.ToVector2),
                    Bulge = lw.Bulge
                };
                vertex.Add(lv);
            }
            return new LwPolyline
            {
                Vertexes = vertex,
                Flags = this.flags,
                Thickness = this.thickness,
                IsVisible = this.isVisible
            };
        }
        
        public override object Clone()
        {
            List<LwPolylineVertex> vertexs_copy = new List<LwPolylineVertex>();
            foreach (LwPolylineVertex vertex in this.vertexes)
            {
                vertexs_copy.Add((LwPolylineVertex)vertex.Clone());
            }
            return new LwPolyline
            {
                Vertexes = vertexs_copy,
                Flags = this.flags,
                Thickness = this.thickness,
                // EntityObject properties
                IsVisible = this.isVisible
            };
        }
    }
}
