using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.IO;

namespace Game
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    /// 
    public partial class GameWindow : Window
    {
        DirectionalLight myDirectionalLight;
        Viewport3D myViewport3D;
        public GameWindow()
        {
            InitializeComponent();

            // Declare scene objects.
            myViewport3D = new Viewport3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view
            // of the camera.
            PerspectiveCamera myPCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            myPCamera.Position = new Point3D(0, 2, 0);

            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = new Vector3D(0, 0, 1);

            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = 90;

            // Asign the camera to the viewport
            myViewport3D.Camera = myPCamera;
            // Define the lights cast in the scene. Without light, the 3D object cannot
            // be seen. Note: to illuminate an object from additional directions, create
            // additional lights.
            myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);


            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet
            // is created.
            createFloor();

            createBox(2, 1, 8);
            createBox(0, 1, 24);

            createPortal(-2, 1, 16);

            // Apply the viewport to the page so it will be rendered.
            grid.Children.Add(myViewport3D);
        }

        void createBox(double x, double y, double z)
        {
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            GeometryModel3D myGeometryModel = new GeometryModel3D();
            Model3DGroup myModel3DGroup = new Model3DGroup();
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            myModel3DGroup.Children.Add(myDirectionalLight);

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));

            myNormalCollection.Add(new Vector3D(-1, 0, 0));
            myNormalCollection.Add(new Vector3D(-1, 0, 0));
            myNormalCollection.Add(new Vector3D(-1, 0, 0));
            myNormalCollection.Add(new Vector3D(-1, 0, 0));
            myNormalCollection.Add(new Vector3D(-1, 0, 0));
            myNormalCollection.Add(new Vector3D(-1, 0, 0));

            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myNormalCollection.Add(new Vector3D(1, 0, 0));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D.
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x,  1 + y, -1 + z));
            myPositionCollection.Add(new Point3D( 1 + x,  1 + y, -1 + z));
            myPositionCollection.Add(new Point3D( 1 + x,  1 + y, -1 + z));
            myPositionCollection.Add(new Point3D( 1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));

            myPositionCollection.Add(new Point3D(1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(1 + x,  1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(1 + x,  1 + y,  1 + z));
            myPositionCollection.Add(new Point3D(1 + x,  1 + y,  1 + z));
            myPositionCollection.Add(new Point3D(1 + x, -1 + y,  1 + z));
            myPositionCollection.Add(new Point3D(1 + x, -1 + y, -1 + z));

            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x,  1 + y,  1 + z));
            myPositionCollection.Add(new Point3D(-1 + x,  1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x,  1 + y,  1 + z));
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y,  1 + z));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));

            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));

            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));

            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;


            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(8);
            myTriangleIndicesCollection.Add(9);
            myTriangleIndicesCollection.Add(10);
            myTriangleIndicesCollection.Add(11);
            myTriangleIndicesCollection.Add(12);
            myTriangleIndicesCollection.Add(13);
            myTriangleIndicesCollection.Add(14);
            myTriangleIndicesCollection.Add(15);
            myTriangleIndicesCollection.Add(16);
            myTriangleIndicesCollection.Add(17);
            myTriangleIndicesCollection.Add(18);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a
            // linear gradient covers the surface of the 3D object.

            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", "metalbox.png"))));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(imageBrush);
            myGeometryModel.Material = myMaterial;


            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);


            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;

            //
            myViewport3D.Children.Add(myModelVisual3D);
        }

        void createPortal(double x, double y, double z)
        {
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            GeometryModel3D myGeometryModel = new GeometryModel3D();
            Model3DGroup myModel3DGroup = new Model3DGroup();
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            myModel3DGroup.Children.Add(myDirectionalLight);

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D.
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x, 2 + y, -1 + z));
            myPositionCollection.Add(new Point3D(1 + x, 2 + y, -1 + z));
            myPositionCollection.Add(new Point3D(1 + x, 2 + y, -1 + z));
            myPositionCollection.Add(new Point3D(1 + x, -1 + y, -1 + z));
            myPositionCollection.Add(new Point3D(-1 + x, -1 + y, -1 + z));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));

            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(6);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a
            // linear gradient covers the surface of the 3D object.

            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", "blueportal.png"))));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(imageBrush);
            myGeometryModel.Material = myMaterial;


            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);


            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;

            //
            myViewport3D.Children.Add(myModelVisual3D);
        }


        void createFloor()
        {
            for (int i = 0; i < 20; i++)
            {
                ModelVisual3D myModelVisual3D = new ModelVisual3D();
                GeometryModel3D myGeometryModel = new GeometryModel3D();
                Model3DGroup myModel3DGroup = new Model3DGroup();
                MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

                myModel3DGroup.Children.Add(myDirectionalLight);

                // Create a collection of normal vectors for the MeshGeometry3D.
                Vector3DCollection myNormalCollection = new Vector3DCollection();
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myNormalCollection.Add(new Vector3D(0, 1, 0));
                myMeshGeometry3D.Normals = myNormalCollection;

                // Create a collection of vertex positions for the MeshGeometry3D.
                Point3DCollection myPositionCollection = new Point3DCollection();
                myPositionCollection.Add(new Point3D(-3.0, 0.0, 0 + 4 * i));
                myPositionCollection.Add(new Point3D(-3.0, 0.0, 8 + 4 * i));
                myPositionCollection.Add(new Point3D( 3.0, 0.0, 8 + 4 * i));
                myPositionCollection.Add(new Point3D( 3.0, 0.0, 8 + 4 * i));
                myPositionCollection.Add(new Point3D( 3.0, 0.0, 0 + 4 * i));
                myPositionCollection.Add(new Point3D(-3.0, 0.0, 0 + 4 * i));
                myMeshGeometry3D.Positions = myPositionCollection;

                // Create a collection of texture coordinates for the MeshGeometry3D.
                PointCollection myTextureCoordinatesCollection = new PointCollection();
                myTextureCoordinatesCollection.Add(new Point(1, 1));
                myTextureCoordinatesCollection.Add(new Point(1, 0));
                myTextureCoordinatesCollection.Add(new Point(0, 0));
                myTextureCoordinatesCollection.Add(new Point(0, 0));
                myTextureCoordinatesCollection.Add(new Point(0, 1));
                myTextureCoordinatesCollection.Add(new Point(1, 1));
                myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;


                // Create a collection of triangle indices for the MeshGeometry3D.
                Int32Collection myTriangleIndicesCollection = new Int32Collection();
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(4);
                myTriangleIndicesCollection.Add(5);
                myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

                // Apply the mesh to the geometry model.
                myGeometryModel.Geometry = myMeshGeometry3D;

                // The material specifies the material applied to the 3D object. In this sample a
                // linear gradient covers the surface of the 3D object.

                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", "textureStone.png"))));

                // Define material and apply to the mesh geometries.
                DiffuseMaterial myMaterial = new DiffuseMaterial(imageBrush);
                myGeometryModel.Material = myMaterial;


                // Add the geometry model to the model group.
                myModel3DGroup.Children.Add(myGeometryModel);


                // Add the group of models to the ModelVisual3d.
                myModelVisual3D.Content = myModel3DGroup;

                //
                myViewport3D.Children.Add(myModelVisual3D);
            }
        }

    }
}
