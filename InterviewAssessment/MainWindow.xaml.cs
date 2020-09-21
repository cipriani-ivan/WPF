using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using InterviewAssessment.View;
using InterviewAssessment.ViewModel;

namespace InterviewAssessment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;
        private bool _isDragging;
        private Point _clickPosition;
        private TranslateTransform _originTt;
        private TranslateTransform _transform;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            EditorCanvas.SetBinding(ItemsControl.ItemsSourceProperty, _viewModel.EntitiesBinding);

        }

        private void AddEntity_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddEntityDialog();
            popup.ShowDialog();
            if (!string.IsNullOrEmpty(popup.EntityName))
            {
                var randomNrGenerator = new Random();
                _viewModel.AddEntity(popup.EntityName, randomNrGenerator.Next(100), randomNrGenerator.Next(100));
            }
        }

        // The part to drag functionality of the the grid on the canvas is copy paste from the web
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid draggableControl)) return;
            _originTt = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            _isDragging = true;
            _clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            var draggableControl = sender as Grid;
            e.GetPosition(this);

            if (draggableControl != null)
            {
                var entity = draggableControl.DataContext as dynamic;

                _viewModel.MoveEntity((int)entity.id, (double)entity.x, (double)entity.y, _transform);
            }

            draggableControl?.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as Grid;
            if (!_isDragging) return;
            var currentPosition = e.GetPosition(this);
            if (draggableControl == null) return;
            _transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            _transform.X = _originTt.X + (currentPosition.X - _clickPosition.X);
            _transform.Y = _originTt.Y + (currentPosition.Y - _clickPosition.Y);
            draggableControl.RenderTransform = new TranslateTransform(_transform.X, _transform.Y);
        }
    }
}