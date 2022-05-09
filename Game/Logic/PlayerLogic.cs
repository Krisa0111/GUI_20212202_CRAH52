using Game.Controller;
using Game.HighScores;
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
using Random = System.Random;

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
        private const float jumpForce = 3.5f;
        private bool collided;
        private bool redPortal;
        private bool bluePortal;
        private volatile float finalPosX;
        private volatile bool jump;
        private readonly object playerLock = new object();
        private static Random rnd = new Random();

        private List<Entity> collidedEntities = new List<Entity>();

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
                temp.foundCollison = false;

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

                        if (entity.Type == EntityType.Obstacle) collidedEntities.Add(entity);
                    }
                    //Blue portal
                    else if (entity.Type == EntityType.BluePortal)
                    {
                        bluePortal = true;
                        player.Score += 10;
                    }
                    //Red portal
                    else if (entity.Type == EntityType.RedPortal)
                    {
                        redPortal = true;
                        player.Score -= 60;
                    }
                    else
                    {
                        // apply powerups

                        //Decrease speed
                        if (entity.Type == EntityType.Decelerator)
                        {
                            player.Distance *= 0.5f;
                            player.Score += 10;
                        }

                        //Increase speed
                        else if (entity.Type == EntityType.Accelerator)
                        {
                            player.Distance *= 1.2f;
                            player.Score -= 60;
                        }

                        //Plus life
                        else if (entity.Type == EntityType.PlusLife)
                        {
                            player.Score += 10;
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
                            if (r < 0.4)
                            {
                                player.Distance *= 0.8f;
                                player.Score += 10;
                            }
                            else if (r < 0.8)
                            {
                                player.Distance *= 1.2f;
                                player.Score -= 60;
                            }
                            else
                            {
                                if (player.Life < 5)
                                {
                                    player.Life++;
                                    player.Score += 10;
                                }
                            }
                        }

                        // Skull
                        else if (entity.Type == EntityType.Skull)
                        {
                            player.Life = 0;        //Egyenlőre csak 0 lesz az életeinek a száma
                            EndOfTheGame(player.Score);
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
            collidedEntities.Clear();
            redPortal = false;
            bluePortal = false;
            
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

            if (bluePortal)
            {
                float jumpDistance = rnd.Next(30, 60);
                pos.Z += jumpDistance;
                foreach (var item in gameModel.Entities)
                {
                    if (item.Type != EntityType.Other && Vector3.Distance(item.Position, pos) < 4)
                        item.MarkToDelete();
                }
            }
            else if (redPortal)
            {
                float jumpDistance = -rnd.Next(10, 40);
                pos.Z += jumpDistance;
                foreach (var item in gameModel.Entities)
                {
                    if (item.Type != EntityType.Other && Vector3.Distance(item.Position, pos) < 2)
                        item.MarkToDelete();
                }
            }

            player.Position = pos;

            prevPos.Y = 0;
            pos.Y = 0;
            float distanceMoved = Vector3.Distance(prevPos, pos);

            if (distanceMoved < player.Speed * dt * .9)
            {
                Debug.WriteLine("collision");

                player.Life--;
                foreach (var item in collidedEntities)
                {
                    item.MarkToDelete();
                }
                if (player.Life == 0)
                {
                    EndOfTheGame(player.Score);
                }

            }

            if (grouned)
            {
                player.CurrentAnimatonStep += distanceMoved * 10f;
            }
            else
            {
                player.CurrentAnimatonStep += distanceMoved * 5f;
            }

            player.RotationY = MathF.Atan(player.Direction.X / player.Direction.Z) / 2.0f;

            player.Distance += distanceMoved;
            player.Speed = MathF.Sqrt(player.Distance + 1000) / 5;
            player.Score += distanceMoved;
        }

        private void EndOfTheGame(float finalScore)
        {
            gameModel.GameOver(finalScore);
            HighScoreManager.EndOfTheGame(finalScore);
        }

        
    }
}
