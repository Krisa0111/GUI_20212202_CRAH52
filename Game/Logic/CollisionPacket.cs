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
        public Vector3 eRadius = Vector3.One; // elipsoid radius

        public Vector3 R3Velocity;
        public Vector3 R3Position;

        public Vector3 velocity;
        public Vector3 normalizedVelocity;
        public Vector3 basePoint;

        public bool foundCollison;
        public double nearestDistance;
        public Vector3 intersectionPoint;

        


    }
}
