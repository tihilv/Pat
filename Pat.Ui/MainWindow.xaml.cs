﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
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

                var triangulation = SurfaceModel.GetVisualModel(_workflow.CutTriangulatedTopHorizon);
                
                modelVisual.Content = triangulation;

                viewPort.ZoomExtents(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to process request:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
