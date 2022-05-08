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
        private const float gravity = 6.4f;
        private const float jumpForce = 2.6f;
        private bool collided;
        private volatile float finalPosX;
        private volatile bool jump;
        private readonly object playerLock = new object();
        private static Random rnd;
        private System.Media.SoundPlayer jumpsound = new System.Media.SoundPlayer(@"..\..\..\Resources\SoundEffects\jump.wav");

        public PlayerLogic()
        {
            this.player = gameModel.Player;
            collisionPacket = new CollisionPacket();
            collisionPacket.eRadius = new Vector3(0.15f, 0.5f, 0.15f);
        }

        public Vector3 CollideAndSlide(Vector3 vel, /*Vector3 gravity,*/ Vector3 position)
        {
            collided = false;

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

        private void CollideWithEntities(ref IReadOnlyCollection<Entity> entities)
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
                    collided = true;
                    if (entity.Type == EntityType.Obstacle || entity.Type == EntityType.Other)
                    {
                        collisionPacket = temp;
                    }
                    else
                    {
                        // apply powerups / portals

                        //Decrease speed
                        if (entity.Type == EntityType.Decelerator)
                        {
                            player.Distance *= 0.8f;
                        }

                        //Increase speed
                        else if (entity.Type == EntityType.Accelerator)
                        {
                            player.Distance *= 1.2f;
                        }

                        //Blue portal
                        else if (entity.Type == EntityType.BluePortal)
                        {
                            player.Score += player.Position.Z * 0.1f;   //Nem a pozíciót változtatja meg, hanem a Scoret módosítja
                        }

                        //Red portal
                        else if (entity.Type == EntityType.RedPortal)
                        {
                            player.Score -= (player.Position.Z * 1.1f - player.Position.Z); //Nem a pozíciót változtatja meg, hanem a Scoret módosítja
                        }

                        //Plus life
                        else if (entity.Type == EntityType.PlusLife)
                        {
                            if (player.Life < 5)
                            {
                                player.Life++;
                            }
                        }

                        //Random portal
                        else if (entity.Type == EntityType.Random)
                        {
                            rnd = new Random();
                            float r = rnd.Next(0, 1);
                            if (r<0.2)
                            {
                                player.Score += player.Position.Z * 0.1f;
                            }
                            else if(r < 0.4)
                            {
                                player.Score -= (player.Position.Z * 1.1f - player.Position.Z);
                            }
                            else if(r < 0.6)
                            {
                                player.Distance *= 0.8f;
                            }
                            else if (r < 0.8)
                            {
                                player.Distance *= 1.2f;
                            }
                            else
                            {
                                if (player.Life < 5)
                                {
                                    player.Life++;
                                }
                            }
                        }

                        // Skull
                        else if (entity.Type == EntityType.Skull)
                        {
                            player.Life = 0;        //Egyenlőre csak 0 lesz az életeinek a száma
                        }
                        entity.MarkToDelete();
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
            IReadOnlyCollection<Entity> entities = gameModel.Entities;

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
                        jumpsound.Play();
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
            bool grouned = collided;

            lock (playerLock) // lock finalPosX & jump
            {
                // if player was on ground
                if (grouned)
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
                player.Direction.X =
                    (MathF.Abs(finalPosX - player.Position.X) < 0.85f ?
                    MathF.Sin((finalPosX - player.Position.X) * MathF.PI) :
                    MathF.Sign(finalPosX - player.Position.X) * 0.454f) * 3.0f;
            }

            player.Direction.Z = 1;
            player.Direction.Y = 0;


            Vector3 pos = CollideAndSlide(
                (player.Velocity + new Vector3(0, verticalVelocity, 0)) * (float)dt,
                player.Position);

            /*if (pos.Y < collisionPacket.eRadius.Y)
            {
                pos.Y = collisionPacket.eRadius.Y;
            }*/

            player.Position = pos;

            prevPos.Y = 0;
            pos.Y = 0;
            float distanceMoved = Vector3.Distance(prevPos, pos);

            if (distanceMoved < player.Speed * dt * .9)
            {
                Debug.WriteLine("collision");

                foreach (var entity in gameModel.Entities)
                {
                    if (entity.Type == EntityType.Obstacle && Vector3.Distance(player.Position, entity.Position) < 3)
                    {
                        entity.MarkToDelete();
                        player.Life--;
                    }
                }
                
            }

            if (grouned)
            {
                player.CurrentAnimatonStep += distanceMoved * 10f;
                IncreaseSpeed(player, dt);
            }
            else
            {
                player.CurrentAnimatonStep += distanceMoved * 5f;
                IncreaseSpeed(player, dt);
            }

            player.RotationY = MathF.Atan(player.Direction.X / player.Direction.Z) / 2.0f;

            IncreaseSpeed(player, dt);
        }
        private void IncreaseSpeed(Player player, double dt)
        {
            if (player.Distance < 3.0f)
            {
                player.Distance = 16.0f;
            }
            player.Distance += (float)dt;         //Egyszer csak leesik 1 alá ???????????? talán új pálya generálásakor
            player.Speed = (float)Math.Sqrt(player.Distance);
        }
    }
}
