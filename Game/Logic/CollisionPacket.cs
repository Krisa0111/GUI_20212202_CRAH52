using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    internal struct CollisionPacket
    {
        // ellipsoid radius
        public Vector3 eRadius;

        // move infomration in r3 space
        public Vector3 R3Velocity;
        public Vector3 R3Position;

        // move infomration in ellipsoid space
        public Vector3 velocity;
        public Vector3 normalizedVelocity;
        public Vector3 basePoint;

        public bool foundCollison;
        public double nearestDistance;
        public Vector3 intersectionPoint;
    }
}
