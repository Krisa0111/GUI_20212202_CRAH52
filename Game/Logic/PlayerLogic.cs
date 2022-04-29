using Game.Controller;
using Game.ViewModel;
using Game.ViewModel.Entities;
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
        private IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        private Player player;
        private CollisionPacket collisionPacket;
        private const float unitspermeter = 100.0f;
        private int collisionrecursionDpeth = 0;
        private float verticalVelocity;
        private const float gravity = 5.0f;
        private const float jumpForce = 3.0f;
        private volatile float finalPosX;
        private volatile bool jump;
        private readonly object playerLock = new object();

        public PlayerLogic()
        {
            this.player = gameModel.Player;
            collisionPacket = new CollisionPacket();
            collisionPacket.eRadius = new Vector3(0.15f, 0.5f, 0.15f);
        }

        public Vector3 CollideAndSlide(Vector3 vel, /*Vector3 gravity,*/ Vector3 position)
        {
            collisionPacket.R3Position = position;
            collisionPacket.R3Velocity = vel;

            Vector3 espacePosition = collisionPacket.R3Position / collisionPacket.eRadius;
            Vector3 espaceVelocity = collisionPacket.R3Velocity / collisionPacket.eRadius;
            collisionrecursionDpeth = 0;
            Vector3 finalPosition = CollideWithWorld(ref espacePosition, ref espaceVelocity);

            //   GRAVITY PULL COMMENT IF NOT NEEDED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            /*collisionPacket.R3Position = finalPosition * collisionPacket.eRadius;
            collisionPacket.R3Velocity = gravity;
            espaceVelocity = gravity / collisionPacket.eRadius;
            collisionrecursionDpeth = 0;
            finalPosition = CollideWithWorld(ref finalPosition, ref espaceVelocity);*/
            // TO HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            finalPosition *= collisionPacket.eRadius;
            return finalPosition;
        }

        private void CollideWithEntities(ref IList<Entity> entities)
        {
            foreach (var entity in entities)
            {
                CollisionPacket temp = collisionPacket;

                for (int i = 0; i < entity.ColliderModel.Triangles.Length; i++)
                {
                    (Vector3 P1, Vector3 P2, Vector3 P3) = entity.ColliderModel.Triangles[i];
                    P1 += entity.Position;
                    P2 += entity.Position;
                    P3 += entity.Position;
                    P1 /= collisionPacket.eRadius;
                    P2 /= collisionPacket.eRadius;
                    P3 /= collisionPacket.eRadius;
                    Collision.CheckTriangle(ref temp, ref P1, ref P2, ref P3);
                }
                if (temp.foundCollison)
                {
                    if (entity.Type == EntityType.Obstacle)
                    {
                        collisionPacket = temp;
                    }
                    else
                    {
                        // apply powerups / portals

                    }
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
            IList<Entity> entities = gameModel.Entities;

            CollideWithEntities(ref entities);
            if (collisionPacket.foundCollison == false)
            {
                return pos + vel;
            }
            //  COLLISION OCCURED !!!!!!
            Vector3 destinationPoint = pos + vel;
            Vector3 newBasePoint = pos;
            if (collisionPacket.nearestDistance >= veryCloseDistance)
            {
                Vector3 V = Vector3.Normalize(vel) * (float)(collisionPacket.nearestDistance - veryCloseDistance);
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

        public void Move(Directions direction)
        {
            lock (playerLock) // lock finalPosX & jump
            {
                switch (direction)
                {
                    case Directions.Up:
                        jump = true;
                        break;
                    case Directions.Down:
                        break;
                    case Directions.Left:
                        finalPosX++;
                        break;
                    case Directions.Right:
                        finalPosX--;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Update(double dt)
        {
            Vector3 prevPos = player.Position;
            bool onGround = collisionPacket.foundCollison || prevPos.Y <= collisionPacket.eRadius.Y;

            lock (playerLock) // lock finalPosX & jump
            {
                // if player was on ground
                if (onGround)
                {
                    // reset gravity
                    verticalVelocity = -gravity * (float)dt;
                    if (jump == true)
                    {
                        verticalVelocity = jumpForce;
                    }
                }
                else // player in the air
                {
                    verticalVelocity -= gravity * (float)dt;
                }

                jump = false;


                //player.Velocity.X = finalPosX - player.Position.X;
                player.Velocity.X =
                    (MathF.Abs(finalPosX - player.Position.X) < 0.85f ?
                    MathF.Sin((finalPosX - player.Position.X) * MathF.PI) :
                    MathF.Sign(finalPosX - player.Position.X) * 0.454f) * 3.0f;
            }

            player.Velocity.Z = 1;
            player.Velocity.Y = 0;
            player.Velocity = Vector3.Normalize(player.Velocity) * player.Speed;

            Vector3 velocity = player.Velocity;
            velocity.Y = verticalVelocity;
            Vector3 pos = CollideAndSlide(velocity * (float)dt,/* new Vector3(0, verticalVelocity, 0),*/ player.Position);

            if (pos.Y < collisionPacket.eRadius.Y)
            {
                pos.Y = collisionPacket.eRadius.Y;
            }

            float distanceMoved = Vector3.Distance(prevPos, pos);
            float momentum = player.Velocity.Length() * (float)dt;

            if (distanceMoved < momentum * .99f)
            {
                Debug.WriteLine("collision");
            }

            player.Position = pos;

            if (onGround)
                player.CurrentAnimatonStep += distanceMoved * 10f;
            else
                player.CurrentAnimatonStep += distanceMoved * 5f;

            player.RotationY = MathF.Atan(player.Velocity.X / player.Velocity.Z) / 2.0f;
        }
    }
}
