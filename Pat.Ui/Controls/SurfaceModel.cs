using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Pat.Api.Model;

namespace Pat.Ui.Controls
{
    internal class SurfaceModel : ModelVisual3D
    {
        private static readonly Color _color = Color.FromRgb(100, 0, 0);

        public static Model3DGroup GetVisualModel(TriangulatedSurface triangulatedSurface)
        {
            var maxZ = triangulatedSurface.Triangles.Any() ? triangulatedSurface.Triangles.SelectMany(t => t.Points).Max(p => p.Z) : 0;

            var material = new MaterialGroup
            {
                Children = new MaterialCollection
                {
                    new DiffuseMaterial(new SolidColorBrush(_color) {Opacity = 1.00}),
                    new SpecularMaterial(Brushes.LightYellow, 2.0)
                }
            };

            Model3DGroup model = new Model3DGroup();
            foreach (var t in triangulatedSurface.Triangles)
                model.Children.Add(CreateTriangleModel(t, material, maxZ));

            return model;
        }

        private static Model3D CreateTriangleModel(Triangle3D triangle, Material material, double maxZ)
        {
            var points = new Point3DCollection(triangle.Points.Select(p => new System.Windows.Media.Media3D.Point3D(p.X, p.Y, maxZ - p.Z)));

            var geometry = new MeshGeometry3D {Positions = points};

            return new GeometryModel3D {Geometry = geometry, Material = material, BackMaterial = material};
        }
    }
}
