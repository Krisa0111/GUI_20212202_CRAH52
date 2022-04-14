using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    class Collision
    {
        public bool CheckPointInTriangle(ref Vector3 point, ref Vector3 pa, ref Vector3 pb,ref Vector3 pc)
        {
           
            // Lets define some local variables, we can change these
            // without affecting the references passed in

            Vector3 p = point;
            Vector3 a = pa;
            Vector3 b = pb;
            Vector3 c = pc;

            // Move the triangle so that the point becomes the 
            // triangles origin
            a -= p;
            b -= p;
            c -= p;

            // The point should be moved too, so they are both
            // relative, but because we don't use p in the
            // equation anymore, we don't need it!
            // p -= p;

            // Compute the normal vectors for triangles:
            // u = normal of PBC
            // v = normal of PCA
            // w = normal of PAB

            Vector3 u = Vector3.Cross(b, c);
            Vector3 v = Vector3.Cross(c, a);
            Vector3 w = Vector3.Cross(a, b);

            // Test to see if the normals are facing 
            // the same direction, return false if not
            if (Vector3.Dot(u, v) < 0f)
            {
                return false;
            }
            if (Vector3.Dot(u, w) < 0.0f)
            {
                return false;
            }

            // All normals facing the same way, return true
            return true;
        }
        public bool GetLowestRoot(double a,double b, double c, double MaxR, out double? root)
        {
            double determinant = b * b - 4 * a * c;
            if (determinant >0)
            {
                double sqrtD = Math.Sqrt(determinant);
                double r1 = (-b - sqrtD) / (2 * a);
                double r2 = (-b + sqrtD) / (2 * a);
                if (r1>r2)
                {
                    double temp = r2;
                    r2 = r1;
                    r1 = temp;
                }
                if (r1>0 && r1<MaxR)
                {
                    root = r1;
                    return true;
                }
                if (r2>0 && r2<MaxR)
                {
                    root = r2;
                    return true;
                }
                // no valid solution
                root = null;
                return false;

            }
            else
            {
                // no valid solution
                root = null;
                return false;
            }
        }
    
    }
}
