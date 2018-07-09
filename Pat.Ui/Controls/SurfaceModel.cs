using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Pat.Api.Model;

namespace Pat.Ui.Controls
{
        class SurfaceModel : ModelVisual3D
        {   
        public static SurfaceModel Create(TriangulatedSurface triangulatedSurface)
        {
            Model3DGroup model = new Model3DGroup();
            foreach (var t in triangulatedSurface.Triangles)            
            {
                model.Children.Add(CreateModel(t, Color.FromRgb(100, 0, 0)));
            }

            
            var triangulation = new SurfaceModel();

            triangulation.Visual3DModel = model;

            return triangulation;
        }

        public static Model3D CreateModel(Triangle3D triangle, Color color)
        {
            var points = new Point3DCollection(triangle.Points.Select(p=>new System.Windows.Media.Media3D.Point3D(p.X, p.Y, p.Z)));
            

            var indices = new Int32Collection();
            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
                        
            var geometry = new MeshGeometry3D { Positions = points, TriangleIndices = indices };            
            var material = new MaterialGroup 
            { 
                Children = new MaterialCollection
                {
                    new DiffuseMaterial(new SolidColorBrush(color) { Opacity = 1.00 }),
                    new SpecularMaterial(Brushes.LightYellow, 2.0) 
                } 
            };
            

            return new GeometryModel3D { Geometry = geometry, Material = material, BackMaterial = material };
        }

    }

}
