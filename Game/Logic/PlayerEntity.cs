using Game.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    internal class PlayerEntity
    {
        CollisionPacket collisionPacket = new CollisionPacket();
        const float unitspermeter = 100.0f;
        int collisionrecursionDpeth = 0;
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();

        public void CollideAndSlide(ref Vector3 vel,ref Vector3 gravity, Vector3 position)
        {
            collisionPacket.R3Position = position;
            collisionPacket.R3Velocity = vel;

            Vector3 espacePosition = collisionPacket.R3Position / collisionPacket.eRadius;
            Vector3 espaceVelocity = collisionPacket.R3Velocity / collisionPacket.eRadius;
            
            Vector3 finalPosition = CollideWithWorld(ref espacePosition, ref espaceVelocity);

            //   GRAVITY PULL COMMENT IF NOT NEEDED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            collisionPacket.R3Position = finalPosition*collisionPacket.eRadius;
            collisionPacket.R3Velocity = gravity;
            espaceVelocity = gravity / collisionPacket.eRadius;
            collisionrecursionDpeth = 0;
            finalPosition = CollideWithWorld(ref finalPosition, ref espaceVelocity);
            // TO HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            finalPosition = finalPosition * collisionPacket.eRadius;
            //MoveTo(finalPosition); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!***********************************
        }
        private void CheckCollisionWithEntities(ref IList<Entity> entities,ref CollisionPacket collisionPacket)
        {
            foreach (var item in entities)
            {
                for (int i = 0; i < item.Model.Triangles.Length; i++)
                {
                    (Vector3 P1, Vector3 P2, Vector3 P3) triangle = item.Model.Triangles[i];
                    Collision.CheckTriangle(ref collisionPacket, ref triangle.P1, ref triangle.P2, ref triangle.P3);
                }
            }
        }
        private Vector3 CollideWithWorld(ref Vector3 pos, ref Vector3 vel)
        {
            // All hard-coded distances in this function is
            // scaled to fit the setting above..
            float unitscale = unitspermeter / 100.0f;
            float veryCloseDistance = 0.005f * unitscale;
            if (collisionrecursionDpeth >5)
            {
                return pos;
            }
            collisionPacket.velocity = vel;
            collisionPacket.normalizedVelocity = vel;
            Vector3.Normalize(collisionPacket.normalizedVelocity);
            collisionPacket.basePoint = pos;
            collisionPacket.foundCollison = false;
            // check for collision
            IList<Entity> entities = gameModel.Entities;                    // check for errors
            CheckCollisionWithEntities( ref entities, ref collisionPacket);
            if (collisionPacket.foundCollison == false)
            {
                return pos + vel;
            }
            //  COLLISION OCCURED !!!!!!
            Vector3 destinationPoint = pos + vel;
            Vector3 newBasePoint = pos;
            if (collisionPacket.nearestDistance >= veryCloseDistance)
            {
                Vector3 V = vel;
                V = Vector3.Normalize(V) * (float)(collisionPacket.nearestDistance - veryCloseDistance); // LOOK FOR ERRORS
                newBasePoint = collisionPacket.basePoint + V;
                Vector3.Normalize(V);
                collisionPacket.intersectionPoint -= veryCloseDistance * V;
            }
            // determine the sliding plane
            Vector3 slidingPlaneOrigin = collisionPacket.intersectionPoint;
            Vector3 slidingPlaneNormal = newBasePoint - collisionPacket.intersectionPoint;
            Vector3.Normalize(slidingPlaneNormal);
            Plane slidingplane = new Plane(ref slidingPlaneOrigin, ref slidingPlaneNormal);
            Vector3 newDestinationPoint = destinationPoint - (float)slidingplane.SignedDistanceTo(ref destinationPoint) * slidingPlaneNormal;
            Vector3 newVelocityVector = newDestinationPoint - collisionPacket.intersectionPoint;
            // recurse
            if (newVelocityVector.Length() < veryCloseDistance)
            {
                return newBasePoint;
            }
            collisionrecursionDpeth++;
            return CollideWithWorld(ref newBasePoint,ref newVelocityVector);

        }
    }
}
