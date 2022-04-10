using Game.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Renderer
{
    internal class GameDisplay
    {
        private FrameBuffer multisapleFbo;
        private FrameBuffer intermediateFBO;

        private Shader shader;
        private Shader normalShader;
        private Shader fboShader;

        private Camera camera;

        private StaticMesh<VertexNT> floorMesh;
        private StaticMesh<VertexNT> tunnelMesh;
        private StaticMesh<VertexNT> pipeMesh;
        private StaticMesh<VertexNT> wireMesh;
        private StaticMesh<VertexNT> lampMesh;
        private StaticMesh<VertexNT> boxMesh;
        private StaticMesh<VertexNT>[] playerMeshes = new StaticMesh<VertexNT>[30];

        private Material floorMaterial;
        private Material concreteMaterial;
        private Material metalMaterial;
        private Material metalBoxMaterial;
        private Material leatherMaterial;

        private DirectionalLight directionalLight;
        private PointLight[] pointLights;

        public bool ShowWireframe { get; set; } = false;
        public bool ShowNormals { get; set; } = false;

        public GameDisplay(int width, int height)
        {
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.Multisample);

            shader = new Shader("Shaders/default_vert.glsl", "Shaders/default_frag.glsl", "Shaders/default_geom.glsl");
            normalShader = new Shader("Shaders/default_vert.glsl", "Shaders/normals_frag.glsl", "Shaders/normals_geom.glsl");
            fboShader = new Shader("Shaders/fbo_vert.glsl", "Shaders/fbo_frag.glsl");

            multisapleFbo = new FrameBuffer(width, height, 4);
            intermediateFBO = new FrameBuffer(width, height);

            floorMaterial = new Material("Images/Metal_Floor_01_basecolor.jpg", "Images/Metal_Floor_01_metallic.jpg");

            concreteMaterial = new Material("Images/Concrete_Base_02_Base_Color.jpg", "Images/Concrete_Base_02_Metallic.jpg");

            metalMaterial = new Material("Images/Metal_Tiles_01_basecolor.jpg", "Images/Metal_Tiles_01_metallic.jpg");

            metalBoxMaterial = new Material("Images/metalbox_diffuse.png", "Images/metalbox_AO.png");

            leatherMaterial = new Material("Images/Leather_Base_01_basecolor.jpg", "Images/Leather_Base_01_metallic.jpg");


            camera = new Camera(new Vector3(0.0f, 1.5f, 1.5f), width / (float)height);
            camera.Pitch = -15;

            floorMesh = new StaticMesh<VertexNT>(generateFloorMesh());
            floorMesh.Material = floorMaterial;

            tunnelMesh = ModelLoader.LoadModel("Models/tunnel.obj").GetMesh();
            tunnelMesh.Material = concreteMaterial;
            pipeMesh = ModelLoader.LoadModel("Models/pipe.obj").GetMesh();
            pipeMesh.Material = metalMaterial;
            wireMesh = ModelLoader.LoadModel("Models/wire.obj").GetMesh();
            wireMesh.Material = leatherMaterial;
            lampMesh = ModelLoader.LoadModel("Models/lamp.obj").GetMesh();
            lampMesh.Material = metalMaterial;

            boxMesh = ModelLoader.LoadModel("Models/box.obj").GetMesh();
            boxMesh.Material = metalBoxMaterial;

            for (int i = 0; i < playerMeshes.Length; i++)
            {
                playerMeshes[i] = ModelLoader.LoadModel("Models/human" + i + ".obj").GetMesh();
                playerMeshes[i].Material = leatherMaterial;
            }
            
            shader.Use();
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            //shader.SetInt("material.normal", 2);
            shader.SetFloat("material.shininess", 32.0f);

            directionalLight = new DirectionalLight(
                new Vector3(0.0f, -0.5f, -1.0f),
                new Vector3(0.05f, 0.03f, 0.03f),
                new Vector3(0.3f, 0.2f, 0.2f),
                new Vector3(0.4f, 0.3f, 0.3f)
                );
            directionalLight.SetUniforms(shader);

            int numLigh = 16;
            pointLights = new PointLight[numLigh];
            for (int i = 0; i < numLigh; i++)
            {
                pointLights[i] = new PointLight(
                    new Vector3(0.0f, 1.95f, -12.0f * i),
                    1.0f, 0.09f, 0.032f,
                    new Vector3(0.05f, 0.05f, 0.05f),
                    new Vector3(0.8f, 0.8f, 0.8f),
                    new Vector3(1.0f, 1.0f, 1.0f)
                );
                pointLights[i].SetUniforms(shader, i);
            }
            shader.SetInt("numOfPointLights", numLigh);
            
        }

        public void Resize(int width, int height, int defaultFbo)
        {
            camera.AspectRatio = width / (float)height;
            multisapleFbo.Resize(width, height, defaultFbo);
            intermediateFBO.Resize(width, height, defaultFbo);
        }

        private void DrawScene(Shader shader)
        {
            floorMesh.Draw(shader, Matrix4.Identity);

            tunnelMesh.Draw(shader, Matrix4.Identity, 32, new Vector3(0, 0, -6));
            pipeMesh.Draw(shader, Matrix4.Identity, 64, new Vector3(0, 0, -2));
            wireMesh.Draw(shader, Matrix4.Identity, 64, new Vector3(0, 0, -2));
            lampMesh.Draw(shader, Matrix4.Identity, 16, new Vector3(0, 0, -12));

            boxMesh.Draw(shader, Matrix4.Identity * Matrix4.CreateTranslation(new Vector3(1, .5f, -10)));

            playerMeshes[a].Draw(shader, Matrix4.Identity * Matrix4.CreateScale(.75f) * Matrix4.CreateTranslation(camera.Position - new Vector3(0.0f, 1.5f, 1.5f)));

        }

        int a = 0;
        public void Render()
        {

            multisapleFbo.Bind();
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            if (ShowWireframe) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            camera.Position += new Vector3(0, 0, -0.03f);
            a = (a + 1) % playerMeshes.Length;

            shader.Use();

            camera.SetMatrices(shader);
            shader.SetVector3("viewPos", camera.Position);

            DrawScene(shader);

            if (ShowNormals) { 
                normalShader.Use();

                camera.SetMatrices(normalShader);

                DrawScene(normalShader);
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            multisapleFbo.BlitTo(intermediateFBO);
            multisapleFbo.Unbind();
            intermediateFBO.Unbind();

            intermediateFBO.Draw(fboShader);
        }

        private List<VertexNT> generateFloorMesh()
        {
            List<VertexNT> floor = new List<VertexNT>();

            floor.Add(new VertexNT(new Vector3(-3f, 0f, 1.0f), new Vector3(0, 1, 0), new Vector2(0, 0)));
            floor.Add(new VertexNT(new Vector3( 3f, 0f, -299.0f), new Vector3(0, 1, 0), new Vector2(1, 100)));
            floor.Add(new VertexNT(new Vector3(-3f, 0f, -299.0f), new Vector3(0, 1, 0), new Vector2(0, 100)));
            floor.Add(new VertexNT(new Vector3(-3f, 0f, 1.0f), new Vector3(0, 1, 0), new Vector2(0, 0)));
            floor.Add(new VertexNT(new Vector3( 3f, 0f, 1.0f), new Vector3(0, 1, 0), new Vector2(1, 0)));
            floor.Add(new VertexNT(new Vector3( 3f, 0f, -299.0f), new Vector3(0, 1, 0), new Vector2(1, 100)));

            return floor;
        }
    }
}
