﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    // https://www.peroxide.dk/papers/collision/collision.pdf
    class Collision
    {
        private static bool CheckPointInTriangle(ref Vector3 point, ref Vector3 pa, ref Vector3 pb, ref Vector3 pc)
        {
            Vector3 e10 = pb - pa;
            Vector3 e20 = pc - pa;

            float a, b, c, ac_bb;

            a = Vector3.Dot(e10, e10);
            b = Vector3.Dot(e10, e20);
            c = Vector3.Dot(e20, e20);
            ac_bb = (a * c) - (b * b);

            Vector3 vp = new Vector3(point.X - pa.X, point.Y - pa.Y, point.Z - pa.Z);

            float d = Vector3.Dot(vp, e10);
            float e = Vector3.Dot(vp, e20);

            float x = (d * c) - (e * b);
            float y = (e * a) - (d * b);
            float z = x + y - ac_bb;

            uint _x = BitConverter.ToUInt32(BitConverter.GetBytes(x), 0);
            uint _y = BitConverter.ToUInt32(BitConverter.GetBytes(y), 0);
            uint _z = BitConverter.ToUInt32(BitConverter.GetBytes(z), 0);

            return (((_z) & ~((_x) | (_y))) & 0x80000000) != 0;
        }

        private static bool GetLowestRoot(double a, double b, double c, double MaxR, out double root)
        {
            root = 0;
            double determinant = b * b - 4 * a * c;
            if (determinant > 0)
            {
                double sqrtD = Math.Sqrt(determinant);
                double r1 = (-b - sqrtD) / (2 * a);
                double r2 = (-b + sqrtD) / (2 * a);
                if (r1 > r2)
                {
                    double temp = r2;
                    r2 = r1;
                    r1 = temp;
                }
                if (r1 > 0 && r1 < MaxR)
                {
                    root = r1;
                    return true;
                }
                if (r2 > 0 && r2 < MaxR)
                {
                    root = r2;
                    return true;
                }
                // no valid solution
                return false;

            }
            else
            {
                // no valid solution
                return false;
            }
        }

        public static void CheckTriangle(ref CollisionPacket colPackage, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3)
        {
            //Make the plane containing the triangle
            Plane trinaglePlane = new Plane(ref p1, ref p2, ref p3);

            if (trinaglePlane.IsFrontFacingTo(ref colPackage.normalizedVelocity))
            {
                double t0, t1;
                bool embedInPlane = false;
                double signedDistToTrianglePlane = trinaglePlane.SignedDistanceTo(ref colPackage.basePoint);
                double normalDotVelocity = Vector3.Dot(trinaglePlane.normal, colPackage.velocity);
                if (normalDotVelocity == 0)
                {
                    if (Math.Abs(signedDistToTrianglePlane) >= 1)
                    {
                        //Sphere is not embed in plane !!!!!

                        return;
                    }
                    else
                    {
                        //sphere is embed in plane!!
                        embedInPlane = true;
                        t0 = 0;
                        t1 = 1;

                    }
                }
                else
                {
                    // N dot D is not 0
                    t0 = (-1 - signedDistToTrianglePlane) / normalDotVelocity;
                    t1 = (1 - signedDistToTrianglePlane) / normalDotVelocity;
                    if (t0 > t1)
                    {
                        double temp = t1;
                        t1 = t0;
                        t0 = temp;
                    }
                    if (t0 > 1 || t1 < 0)
                    {
                        //both t values are outside values
                        return;
                    }
                    if (t0 < 0)
                    {
                        t0 = 0;
                    }
                    if (t1 < 0)
                    {
                        t1 = 0;
                    }
                    if (t0 > 1)
                    {
                        t0 = 1;
                    }
                    if (t1 > 1)
                    {
                        t1 = 1;
                    }
                }
                Vector3 collisionPoint = new Vector3();
                bool foundCollision = false;
                double t = 1;
                if (!embedInPlane)
                {
                    Vector3 planeIntersectionPoint = (colPackage.basePoint - trinaglePlane.normal) + ((float)t0 * colPackage.velocity);
                    if (CheckPointInTriangle(ref planeIntersectionPoint, ref p1, ref p2, ref p3))
                    {
                        foundCollision = true;
                        t = t0;
                        collisionPoint = planeIntersectionPoint;
                    }
                }
                if (foundCollision == false)
                {
                    Vector3 velocity = colPackage.velocity;
                    Vector3 basep = colPackage.basePoint;
                    double velocitySquaredLength = velocity.LengthSquared();
                    double a, b, c;
                    double newT;
                    //Check against points
                    a = velocitySquaredLength;
                    //P1
                    b = 2 * (Vector3.Dot(velocity, (basep - p1)));
                    c = (p1 - basep).LengthSquared() - 1;
                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        t = newT;
                        foundCollision = true;
                        collisionPoint = p1;
                    }
                    //P2
                    b = 2 * (Vector3.Dot(velocity, (basep - p2)));
                    c = (p2 - basep).LengthSquared() - 1;
                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        t = newT;
                        foundCollision = true;
                        collisionPoint = p2;
                    }
                    //P3
                    b = 2 * (Vector3.Dot(velocity, (basep - p3)));
                    c = (p3 - basep).LengthSquared() - 1;
                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        t = newT;
                        foundCollision = true;
                        collisionPoint = p3;
                    }
                    //Check against edges
                    Vector3 edge = p2 - p1;
                    Vector3 baseToVertex = p1 - basep;
                    double edgeSquaredLength = edge.LengthSquared();
                    double edgeDotVelocity = Vector3.Dot(edge, velocity);
                    double edgeDotBaseToVertex = Vector3.Dot(edge, baseToVertex);
                    //calculate parameters for equation
                    a = edgeSquaredLength * (-velocitySquaredLength) + edgeDotVelocity * edgeDotVelocity;
                    b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                    c = edgeSquaredLength * (1 - baseToVertex.LengthSquared()) + edgeDotBaseToVertex * edgeDotBaseToVertex;
                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        //check if interection with line segment
                        double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                        if (f >= 0 && f <= 1)
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p1 + (float)f * edge;
                        }
                    }

                    //p2->p3
                    edge = p3 - p2;
                    baseToVertex = p2 - basep;
                    edgeSquaredLength = edge.LengthSquared();
                    edgeDotVelocity = Vector3.Dot(edge, velocity);
                    edgeDotBaseToVertex = Vector3.Dot(edge, baseToVertex);

                    a = edgeSquaredLength * (-velocitySquaredLength) + edgeDotVelocity * edgeDotVelocity;
                    b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                    c = edgeSquaredLength * (1 - baseToVertex.LengthSquared()) + edgeDotBaseToVertex * edgeDotBaseToVertex;

                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                        if (f >= 0 && f <= 1)
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p2 + (float)f * edge;
                        }
                    }
                    //p3 ->p1
                    edge = p1 - p3;
                    baseToVertex = p3 - basep;
                    edgeSquaredLength = edge.LengthSquared();
                    edgeDotVelocity = Vector3.Dot(edge, velocity);
                    edgeDotBaseToVertex = Vector3.Dot(edge, baseToVertex);
                    a = edgeSquaredLength * (-velocitySquaredLength) + edgeDotVelocity * edgeDotVelocity;
                    b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                    c = edgeSquaredLength * (1 - baseToVertex.LengthSquared()) + edgeDotBaseToVertex * edgeDotBaseToVertex;
                    if (GetLowestRoot(a, b, c, t, out newT))
                    {
                        double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                        if (f >= 0 && f <= 1)
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p3 + (float)f * edge;
                        }
                    }
                }
                if (foundCollision == true)
                {
                    double distToCollision = (double)t * colPackage.velocity.Length();
                    if (colPackage.foundCollison == false ||
                        distToCollision < colPackage.nearestDistance)
                    {
                        colPackage.nearestDistance = distToCollision;
                        colPackage.intersectionPoint = collisionPoint;
                        colPackage.foundCollison = true;
                    }
                }
            }



        }

    }
}
