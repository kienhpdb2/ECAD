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
    {
        private List<LwPolylineVertex> vertexes;
        private PolyLineTypeFlags flags;
        private double thickness;

        public LwPolyline()
            :this(new List<LwPolylineVertex>(), false)
        {
        }

        public LwPolyline (List<LwPolylineVertex> vertexes, bool isclosed)
        {
           if(vertexes == null)
                throw new ArgumentNullException(nameof(vertexes));
           this.vertexes = vertexes;
           this.flags = Isclosed?PolyLineTypeFlags.CloseLwPolyline : PolyLineTypeFlags.OpenLwPolyline;
           this.thickness = 0.0;
        }

        public List<LwPolylineVertex> Vertexes
        {
            get { return this.vertexes; }
            set
            {
                if(value == null)
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
            set { this.flags=value?PolyLineTypeFlags.CloseLwPolyline:PolyLineTypeFlags.OpenLwPolyline;}
        }
        public double Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
    }
}
