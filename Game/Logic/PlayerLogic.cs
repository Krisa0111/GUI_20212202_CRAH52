using Game.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    internal class PlayerLogic
    {
        CollisionPacket collisionPacket;
        const float unitspermeter = 100.0f;
        int collisionrecursionDpeth = 0;
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        float verticalVelocity;
        const float gravity = 0.3f;
        const float jumpForce = 0.12f;

        public PlayerLogic()
        {
            collisionPacket = new CollisionPacket();
            collisionPacket.eRadius = new Vector3(0.2f, 0.7f, 0.2f);
        }

        public Vector3 CollideAndSlide(Vector3 vel, Vector3 gravity, Vector3 position)
        {
            collisionPacket.R3Position = position;
            collisionPacket.R3Velocity = vel;

            Vector3 espacePosition = collisionPacket.R3Position / collisionPacket.eRadius;
            Vector3 espaceVelocity = collisionPacket.R3Velocity / collisionPacket.eRadius;
            collisionrecursionDpeth = 0;
            Vector3 finalPosition = CollideWithWorld(ref espacePosition, ref espaceVelocity);

            //   GRAVITY PULL COMMENT IF NOT NEEDED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //collisionPacket.R3Position = finalPosition*collisionPacket.eRadius;
            //collisionPacket.R3Velocity = gravity;
            //espaceVelocity = gravity / collisionPacket.eRadius;
            //collisionrecursionDpeth = 0;
            //finalPosition = CollideWithWorld(ref finalPosition, ref espaceVelocity);
            // TO HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            finalPosition *= collisionPacket.eRadius;
            return finalPosition;
        }
        private void CheckCollisionWithEntities(ref IList<Entity> entities)
        {
            foreach (var item in entities)
            {
                for (int i = 0; i < item.Model.Triangles.Length; i++)
                {
                    (Vector3 P1, Vector3 P2, Vector3 P3) = item.Model.Triangles[i];
                    P1 += item.Position;
                    P2 += item.Position;
                    P3 += item.Position;
                    P1 /= collisionPacket.eRadius;
                    P2 /= collisionPacket.eRadius;
                    P3 /= collisionPacket.eRadius;
                    Collision.CheckTriangle(ref collisionPacket, ref P1, ref P2, ref P3);
                }
            }
        }
        private Vector3 CollideWithWorld(ref Vector3 pos, ref Vector3 vel)
        {
            // All hard-coded distances in this function is
            // scaled to fit the setting above..
            //CollisionPacket collisionPacket = new CollisionPacket();
            float unitscale = unitspermeter / 100.0f;
            float veryCloseDistance = 0.005f * unitscale;
            if (collisionrecursionDpeth > 5)
            {
                return pos;
            }
            collisionPacket.velocity = vel;
            collisionPacket.normalizedVelocity = Vector3.Normalize(vel);
            collisionPacket.basePoint = pos;
            collisionPacket.foundCollison = false;
            // check for collision
            IList<Entity> entities = gameModel.Entities;                    // check for errors

            CheckCollisionWithEntities(ref entities);
            if (collisionPacket.foundCollison == false)
            {
                return pos + vel;
            }
            //  COLLISION OCCURED !!!!!!
            Vector3 destinationPoint = pos + vel;
            Vector3 newBasePoint = pos;
            if (collisionPacket.nearestDistance >= veryCloseDistance)
            {
                Vector3 V = Vector3.Normalize(vel) * (float)(collisionPacket.nearestDistance - veryCloseDistance); // LOOK FOR ERRORS
                newBasePoint = collisionPacket.basePoint + V;
                V = Vector3.Normalize(V);
                collisionPacket.intersectionPoint -= veryCloseDistance * V;
            }
            // determine the sliding plane
            Vector3 slidingPlaneOrigin = collisionPacket.intersectionPoint;
            Vector3 slidingPlaneNormal = Vector3.Normalize(newBasePoint - collisionPacket.intersectionPoint);
            Plane slidingplane = new Plane(ref slidingPlaneOrigin, ref slidingPlaneNormal);
            Vector3 newDestinationPoint = destinationPoint - (float)slidingplane.SignedDistanceTo(ref destinationPoint) * slidingPlaneNormal;
            Vector3 newVelocityVector = newDestinationPoint - collisionPacket.intersectionPoint;
            // recurse
            if (newVelocityVector.Length() < veryCloseDistance)
            {
                return newBasePoint;
            }
            collisionrecursionDpeth++;
            return CollideWithWorld(ref newBasePoint, ref newVelocityVector);

        }
        public void Move(double dt)
        {
            //if (collisionPacket.foundCollison)
            //{
            //    verticalVelocity = -gravity * dt.Milliseconds;
            //    if (gameModel.Player.velocity.Y > 0)
            //    {
            //        verticalVelocity = jumpForce;

            //    }
            //}
            //else
            //{
            //    verticalVelocity -= gravity * dt.Milliseconds;

            //}

            Vector3 temp = CollideAndSlide(gameModel.Player.velocity * (float)dt, new Vector3(0, verticalVelocity, 0), gameModel.Player.Position);
            /*if (temp.Y <0)
            {
                temp.Y = 0;
            }*/

            gameModel.Player.Position = temp;

            if (collisionPacket.foundCollison == false)
                gameModel.Player.CurrentAnimatonStep += (float)(collisionPacket.R3Velocity.Length() * 15);

        }
    }
}
