using Game.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Renderer
{
    internal class GameDisplay
    {
        private Shader gameItemShader;
        private Shader playerShader;

        private Camera mainCamera;

        private StaticMesh<VertexTextureUV> squareMesh;
        private StaticMesh<VertexTextureUV> floorMesh;
        private StaticMesh<VertexTextureUV> boxMesh;

        private Texture floorTexture;
        private Texture boxTexture;
        private Texture playerTexture;
        private Vector2 playerAtlasSize;
        private Vector2 playerAtlasPos;

        public GameDisplay(float width, float height)
        {
            GL.Enable(EnableCap.DepthTest);

            gameItemShader = new Shader("Shaders/simplev.glsl", "Shaders/texturef.glsl");
            playerShader = new Shader("Shaders/playerv.glsl", "Shaders/texturef.glsl");
            playerTexture = Texture.LoadFromFile("Images/running3.png");
            floorTexture = Texture.LoadFromFile("Images/textureStone.png");
            boxTexture = Texture.LoadFromFile("Images/metalbox.png");

            mainCamera = new Camera(new Vector3(0.0f, 1.5f, 1.5f), width / height);
            mainCamera.Pitch = -15f;

            squareMesh = new StaticMesh<VertexTextureUV>(generateSquareMesh());

            squareMesh.AttachTexture(playerTexture);

            playerAtlasSize = new Vector2(16, 3);
            playerAtlasPos = new Vector2(1, 1);

            playerShader.Use();
            playerShader.SetVector2("atlasSize", playerAtlasSize);

            floorMesh = new StaticMesh<VertexTextureUV>(generateFloorMesh());

            floorMesh.AttachTexture(floorTexture);

            boxMesh = new StaticMesh<VertexTextureUV>(generateBoxMesh());

            boxMesh.AttachTexture(boxTexture);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        public void Resize(float width, float height)
        {
            mainCamera.AspectRatio = width / height;
        }

        float x = 0;
        public void Render()
        {
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // render floor

            mainCamera.SetMatrices(gameItemShader);

            gameItemShader.Use();
            gameItemShader.SetVector2("uvOffset", new Vector2(0, x+=0.02f));

            floorMesh.Draw(gameItemShader, Matrix4.Identity);

            // render game items

            gameItemShader.SetVector2("uvOffset", new Vector2(0, 0));

            boxMesh.Draw(gameItemShader, Matrix4.Identity * Matrix4.CreateTranslation(new Vector3(1,0.0f,-1)));


            // render player

            GL.Enable(EnableCap.Blend); // enable transparency
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            mainCamera.SetMatrices(playerShader);

            playerShader.Use();
            playerShader.SetVector2("atlasXY", playerAtlasPos);
            playerAtlasPos.X++;
            if (playerAtlasPos.X > playerAtlasSize.X)
            {
                playerAtlasPos.X = 1;
                playerAtlasPos.Y++;
                if (playerAtlasPos.Y > playerAtlasSize.Y) playerAtlasPos.Y = 1;
            }

            squareMesh.Draw(playerShader, Matrix4.Identity);

            GL.Disable(EnableCap.Blend);
        }

        private List<VertexTextureUV> generateSquareMesh()
        {
            List<VertexTextureUV> square = new List<VertexTextureUV>();
            square.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 0)));
            square.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 1)));
            square.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 1)));
            square.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 0)));
            square.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 1)));
            square.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 0)));
            return square;
        }

        private List<VertexTextureUV> generateFloorMesh()
        {
            List<VertexTextureUV> floor = new List<VertexTextureUV>();

            floor.Add(new VertexTextureUV(new Vector3(-1.5f, -0.5f, 1.0f), new Vector3(0, 1, 0), new Vector2(0, 0)));
            floor.Add(new VertexTextureUV(new Vector3(1.5f, -0.5f, -299.0f), new Vector3(0, 1, 0), new Vector2(1, 100)));
            floor.Add(new VertexTextureUV(new Vector3(-1.5f, -0.5f, -299.0f), new Vector3(0, 1, 0), new Vector2(0, 100)));
            floor.Add(new VertexTextureUV(new Vector3(-1.5f, -0.5f, 1.0f), new Vector3(0, 1, 0), new Vector2(0, 0)));
            floor.Add(new VertexTextureUV(new Vector3(1.5f, -0.5f, 1.0f), new Vector3(0, 1, 0), new Vector2(1, 0)));
            floor.Add(new VertexTextureUV(new Vector3(1.5f, -0.5f, -299.0f), new Vector3(0, 0, 0), new Vector2(1, 100)));

            return floor;
        }

        private List<VertexTextureUV> generateBoxMesh()
        {
            List<VertexTextureUV> box = new List<VertexTextureUV>();

            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 0.0f)));

            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)));

            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)));

            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f)));

            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)));

            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)));
            box.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f)));

            return box;
        }
    }
}
