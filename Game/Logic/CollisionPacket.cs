using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    internal class CollisionPacket
    {
        Vector3 eRadius;  // elipsoid radius
        
        Vector3 R3Velocity;
        Vector3 R3Position;

        Vector3 velocity;
        Vector3 normalizedVelocity;
        Vector3 basePoint;

        bool foundCollison;
        double nearestDistance;
        Vector3 intersectionPoint;

        public void CheckTriangle(ref CollisionPacket colPackage, ref Vector3 p1,ref Vector3 p2, ref Vector3 p3)
        {
            //Make the plane containing the triangle
            Plane trinaglePlane = new Plane(ref p1, ref p2, ref p3);

            if (trinaglePlane.IsFrontFacingTo(ref colPackage.normalizedVelocity))
            {
                double t0, t1;
                bool embedInPlane = false;
                double signedDistToTrianglePlane = trinaglePlane.SignedDistanceTo(ref colPackage.basePoint);
                double normalDotVelocity = Vector3.Dot(trinaglePlane.normal,colPackage.velocity);
                if (normalDotVelocity == 0)
                {
                    if (Math.Abs(signedDistToTrianglePlane)>=1)
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
                    // n dot is not 0
                    t0 = (-1-signedDistToTrianglePlane)/normalDotVelocity;
                    t1 = (1-signedDistToTrianglePlane)/normalDotVelocity;
                    if (t0>t1)
                    {
                        double temp = t1;
                        t1 = t0;
                        t0 = temp;
                    }
                    if (t0>1 || t1<0)
                    {
                        //both t values are outside values
                        return;
                    }
                    if (t0<0)
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
                    Vector3 collisionPoint = new Vector3();
                    bool foundCollision = false;
                    double? t = 1;
                    if (!embedInPlane)
                    {
                        Vector3 planeIntersectionPoint = (colPackage.basePoint - trinaglePlane.normal) + ((float)t0*colPackage.velocity);
                        if (Collision.CheckPointInTriangle(ref planeIntersectionPoint,ref p1,ref p2,ref p3))
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
                        double? newT;
                        //Check against points
                        a = velocitySquaredLength;
                        //P1
                        b = 2 * (Vector3.Dot(velocity, (basep - p1)));
                        c = (p1-basep).LengthSquared()-1;
                        if (Collision.GetLowestRoot(a,b,c,t,out newT))
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p1;
                        }
                        //P2
                        b = 2 * (Vector3.Dot(velocity, (basep - p2)));
                        c = (p2 - basep).LengthSquared() - 1;
                        if (Collision.GetLowestRoot(a, b, c, t, out newT))
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p2;
                        }
                        //P3
                        b = 2 * (Vector3.Dot(velocity, (basep - p3)));
                        c = (p3 - basep).LengthSquared() - 1;
                        if (Collision.GetLowestRoot(a, b, c, t, out newT))
                        {
                            t = newT;
                            foundCollision = true;
                            collisionPoint = p3;
                        }
                        //Check against edges
                        Vector3 edge = p2 - p1;
                        Vector3 baseToVertex = p1 - basep;
                        double edgeSquaredLength = edge.LengthSquared();
                        double edgeDotVelocity = Vector3.Dot(edge,velocity);
                        double edgeDotBaseToVertex = Vector3.Dot(edge,baseToVertex);
                        //calculate parameters for equation
                        a = edgeSquaredLength * (-velocitySquaredLength) + edgeDotVelocity * edgeDotVelocity;
                        b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                        c = edgeSquaredLength * (1 - baseToVertex.LengthSquared()) + edgeDotBaseToVertex * edgeDotBaseToVertex;
                        if (Collision.GetLowestRoot(a,b,c,t,out newT))
                        {
                            //check if interection with line segment
                            double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                            if (f>= 0 && f <= 1)
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
                        edgeDotVelocity = Vector3.Dot(edge,velocity);
                        edgeDotBaseToVertex = Vector3.Dot(edge,baseToVertex);

                        a = edgeSquaredLength*(-velocitySquaredLength) + (edgeDotVelocity * edgeDotVelocity);
                        b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                        c = edgeSquaredLength * (1 - baseToVertex.LengthSquared() + edgeDotBaseToVertex * edgeDotBaseToVertex);

                        if (Collision.GetLowestRoot(a,b,c,t,out newT))
                        {
                            double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                            if (f>= 0 && f<= 1)
                            {
                                t = newT;
                                foundCollision=true;
                                collisionPoint = p2 + (float)f * edge;
                            }
                        }
                        //p3 ->p1
                        edge = p1 - p3;
                        baseToVertex = p3 - basep;
                        edgeSquaredLength = edge.LengthSquared();
                        edgeDotVelocity = Vector3.Dot(edge, velocity);
                        edgeDotBaseToVertex = Vector3.Dot(edge, baseToVertex);
                        a = edgeSquaredLength * (-velocitySquaredLength) + (edgeDotVelocity * edgeDotVelocity);
                        b = edgeSquaredLength * (2 * Vector3.Dot(velocity, baseToVertex)) - 2 * edgeDotVelocity * edgeDotBaseToVertex;
                        c = edgeSquaredLength * (1 - baseToVertex.LengthSquared()) + edgeDotBaseToVertex * edgeDotBaseToVertex;
                        if (Collision.GetLowestRoot(a,b,c,t,out newT))
                        {
                            double f = (edgeDotVelocity * (double)newT - edgeDotBaseToVertex) / edgeSquaredLength;
                            if (f>= 0 && f<= 1)
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
                        if(colPackage.foundCollison == false ||
                            distToCollision<colPackage.nearestDistance)
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
}
