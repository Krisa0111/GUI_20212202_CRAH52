using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class Plane
    {
        float[] equation = new float[4];
        Vector3 origin = new Vector3();
        public Vector3 normal = new Vector3();
        
        public Plane(ref Vector3 origin, ref Vector3 normal)
        {
            this.origin = origin;
            this.normal = normal;
            equation[0] = normal.X;
            equation[1] = normal.Y;
            equation[2] = normal.Z;
            equation[3] = -(
                normal.X * origin.X
                + normal.Y * origin.Y 
                + normal.Z * origin.Z);
        }
        public Plane(ref Vector3 p1, ref Vector3 p2,ref Vector3 p3)
        {
            Vector3.Cross((p2 - p1), (p3 - p1));
            Vector3.Normalize(normal);
            origin = p1;
            equation[0] = normal.X;
            equation[1] = normal.Y;
            equation[2] = normal.Z;
            equation[3] = -(
                normal.X * origin.X
                + normal.Y * origin.Y
                + normal.Z * origin.Z);
        }

        public bool IsFrontFacingTo(ref Vector3 direction)
        {
            double dot = Vector3.Dot(direction,normal);
            return dot <= 0;
        }
        public double SignedDistanceTo(ref Vector3 point)
        {
            return Vector3.Dot(point, normal) + equation[3];
        }
    }
}
