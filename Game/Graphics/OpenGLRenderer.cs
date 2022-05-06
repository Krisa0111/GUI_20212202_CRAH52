using Game.Graphics.OpenGL;
using Game.ResourceLoader;
using Game.ViewModel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class OpenGLRenderer : IDisposable, IRenderer
    {

        private readonly Camera camera;

        public ICamera Camera
        {
            get
            {
                return camera;
            }
        }

        private Shader shader;
        private Shader normalShader;
        private Shader fboShader;

        private FrameBuffer multisapleFBO;
        private FrameBuffer intermediateFBO;

        private const int MAX_POINT_LIGHTS = 16;
        private DirectionalLight directionalLight;
        private PointLight[] pointLights;

        private Dictionary<string, Mesh> models;
        private Dictionary<string, Texture> textures;
        private VertexBufferLayout vertexBufferLayout;

        public bool ShowWireframe { get; set; } = false;
        public bool ShowNormals { get; set; } = false;
        public bool ShowColliders { get; set; } = false;


        public IDirectionalLight DirectionalLight { get => directionalLight; }
        public IPointLight[] PointLights { get => pointLights; }

        public OpenGLRenderer(int width, int height)
        {
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.Multisample);

            // Load shaders
            shader = new Shader("Shaders/default_vert.glsl", "Shaders/default_frag.glsl", "Shaders/default_geom.glsl");
            normalShader = new Shader("Shaders/default_vert.glsl", "Shaders/normals_frag.glsl", "Shaders/normals_geom.glsl");
            fboShader = new Shader("Shaders/fbo_vert.glsl", "Shaders/fbo_frag.glsl");

            // setup framebuffers
            multisapleFBO = new FrameBuffer(width, height, 4);
            intermediateFBO = new FrameBuffer(width, height);

            shader.Use();

            // setup lights
            pointLights = new PointLight[MAX_POINT_LIGHTS];
            for (int i = 0; i < pointLights.Length; i++)
            {
                pointLights[i] = new PointLight(Vector3.Zero, 1.0f, 0.09f, 0.032f, Vector3.One * 0.05f, Vector3.One * 0.5f, Vector3.One * 1.0f);
            }
            directionalLight = new DirectionalLight(-Vector3.UnitY, Vector3.One * 0.05f, Vector3.One * 0.1f, Vector3.One * 0.2f);
            directionalLight.SetUniforms(shader);

            // setup material
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            //shader.SetInt("material.normal", 2);
            shader.SetFloat("material.shininess", 32.0f);

            // setup camera
            camera = new Camera(Vector3.Zero, width / (float)height);
            camera.SetMatrices(shader);

            // default layout for 3d models
            vertexBufferLayout = new VertexBufferLayout();
            vertexBufferLayout.Push(VertexElement.Position, VertexType.Float, 3);
            vertexBufferLayout.Push(VertexElement.Normal, VertexType.Float, 3);
            vertexBufferLayout.Push(VertexElement.TexCoord, VertexType.Float, 2);

            models = new Dictionary<string, Mesh>();
            textures = new Dictionary<string, Texture>();

        }

        public OpenGLRenderer() : this(0, 0)
        {

        }

        public void BeginFrame()
        {
            multisapleFBO.Bind();
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            if (ShowWireframe) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            shader.Use();
            shader.SetVector3("viewPos", camera.Position);

            camera.SetMatrices(shader);

            directionalLight.SetUniforms(shader);
            for (int i = 0; i < pointLights.Length; i++)
            {
                pointLights[i].SetUniforms(shader, i);
            }

            if (ShowNormals) camera.SetMatrices(normalShader);
        }

        public void EndFrame()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            multisapleFBO.BlitTo(intermediateFBO);
            multisapleFBO.Unbind();
            intermediateFBO.Unbind();

            intermediateFBO.Draw(fboShader);
        }

        public void Render(IReadOnlyCollection<Entity> entities)
        {
            if (ShowColliders)
            {
                RenderEntitiesCollider(shader, entities);
            }
            else
            {
                RenderEntities(shader, entities);

                if (ShowNormals)
                {
                    RenderEntities(normalShader, entities);
                }
            }

        }

        public void Render(Entity entity)
        {
            if (ShowColliders)
            {
                RenderEntityCollider(shader, entity);
            }
            else
            {
                RenderEntity(shader, entity);
                if (ShowNormals)
                {
                    RenderEntity(normalShader, entity);
                }
            }
        }

        public void RenderMultible(Model model, Vector3 startPosition, Vector3 offset, int instances)
        {
            Mesh mesh = GetMesh(model);

            var transformationMatrix = Matrix4.Identity * Matrix4.CreateTranslation(startPosition);

            mesh.DrawMultible(shader, transformationMatrix, instances, offset);
            if (ShowNormals)
            {
                mesh.DrawMultible(normalShader, transformationMatrix, instances, offset);
            }
        }

        private void RenderEntities(Shader shader, IReadOnlyCollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                RenderEntity(shader, entity);
            }
        }

        private void RenderEntity(Shader shader, Entity entity)
        {
            Mesh mesh = GetMesh(entity.Model);

            Vector3 pos = new()
            {
                X = entity.Position.X,
                Y = entity.Position.Y,
                Z = entity.Position.Z
            };

            var transformationMatrix = Matrix4.Identity * Matrix4.CreateRotationY(entity.RotationY) * Matrix4.CreateTranslation(pos);
            mesh.Draw(shader, transformationMatrix);
        }

        private void RenderEntitiesCollider(Shader shader, IReadOnlyCollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                RenderEntityCollider(shader, entity);
            }
        }

        private void RenderEntityCollider(Shader shader, Entity entity)
        {
            Mesh mesh = GetMesh(entity.ColliderModel);

            Vector3 pos = new()
            {
                X = entity.Position.X,
                Y = entity.Position.Y,
                Z = entity.Position.Z
            };

            var transformationMatrix = Matrix4.Identity * Matrix4.CreateTranslation(pos);
            mesh.Draw(shader, transformationMatrix);
        }

        private Mesh GetMesh(Model model)
        {
            if (!models.TryGetValue(model.Name, out Mesh mesh))
            {
                var textureImage = model.Material.texture;
                var specularMapImage = model.Material.specularMap;
                var normalMapImage = model.Material.normalMap;
                Texture texture = textureImage is null ? null : GetTexture(textureImage);
                Texture specularMap = specularMapImage is null ? null : GetTexture(specularMapImage);
                Texture normalMap = normalMapImage is null ? null : GetTexture(normalMapImage);
                var material = new OpenGL.Material(texture, specularMap, normalMap);
                mesh = new Mesh(model.Vertices, model.Indices, vertexBufferLayout, material);
                models.Add(model.Name, mesh);
            }
            return mesh;
        }

        private Texture GetTexture(Image image)
        {
            if (!textures.TryGetValue(image.Name, out Texture texture))
            {
                texture = Texture.LoadFromImage(image.Bitmap);
                textures.Add(image.Name, texture);
                image.Dispose();
            }
            return texture;
        }

        public void Resize(int width, int height, int defaultFbo)
        {
            camera.AspectRatio = width / (float)height;
            multisapleFBO.Resize(width, height, defaultFbo);
            intermediateFBO.Resize(width, height, defaultFbo);
        }

        public void Dispose()
        {
            foreach (var item in models)
            {
                item.Value.Dispose();
            }
            foreach (var item in textures)
            {
                item.Value.Dispose();
            }
        }
    }
}
