using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Pat.BusinessLogic;
using Pat.Ui.Controls;

namespace Pat.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DefaultPatWorkflow _workflow;
        public MainWindow()
        {
            InitializeComponent();

            _workflow = new DefaultPatWorkflow(Path.Combine(Path.GetDirectoryName(typeof(MainWindow).Assembly.Location)));

            DataContext = _workflow;

            // Workaround for WPG issue
            this.CommandBindings.Add(new CommandBinding((ICommand) PropertyEditorCommands.ShowDialogEditor, OnShowDialogEditor, CanExecute));

            var trackball = new Wpf3DTools.Trackball();
            trackball.EventSource = background;
            viewport.Camera.Transform = trackball.Transform;
            light.Transform = trackball.RotateTransform;

        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnShowDialogEditor(object sender, ExecutedRoutedEventArgs e)
        {
            PropertyItemValue parameter = e.Parameter as PropertyItemValue;
            parameter?.ParentProperty?.Editor?.ShowDialog(parameter, (IInputElement) this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _workflow.Calculate();

                viewport.Children.Clear();;

                var triangulation = SurfaceModel.Create(_workflow.CutTriangulatedTopHorizon);
                viewport.Children.Add(triangulation);

                var points = _workflow.CutTriangulatedTopHorizon.Triangles.SelectMany(p => p.Points).ToArray();

                var cameraX = points.Select(p => p.X).Average();
                var cameraY = points.Select(p => p.Y).Min();
                var cameraZ = points.Select(p => p.Z).Average();

                var centerX = points.Select(p => p.X).Max();
                var centerY = points.Select(p => p.Y).Average();
                var centerZ = points.Select(p => p.Z).Average();
                
                double fieldOfViewInRadians = camera.FieldOfView * (Math.PI / 180.0);
                camera.Position = new Point3D(cameraX, cameraY, cameraZ);

                var centralPoint = new Point3D(centerX, centerY, centerZ);
                camera.LookDirection = centralPoint - camera.Position;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to process request:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
