using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

public class MyPoint
{
    public double X { get; set; }
    public double Y { get; set; }

    public MyPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    public void Move(double dx, double dy)
    {
        X += dx;
        Y += dy;
    }
}

public class Triangle
{
    public MyPoint A { get; set; }
    public MyPoint B { get; set; }
    public MyPoint C { get; set; }

    private static Random rnd = new Random();

    public Triangle()
    {
        A = new MyPoint(rnd.Next(0, 301), rnd.Next(0, 301));
        B = new MyPoint(rnd.Next(0, 301), rnd.Next(0, 301));
        C = new MyPoint(rnd.Next(0, 301), rnd.Next(0, 301));
    }

    public Triangle(MyPoint a, MyPoint b, MyPoint c)
    {
        A = a;
        B = b;
        C = c;
    }

    public void Move(double dx, double dy)
    {
        A.Move(dx, dy);
        B.Move(dx, dy);
        C.Move(dx, dy);
    }
}

public class MyRectangle
{
    public MyPoint LeftTop { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    private static Random rnd = new Random();

    public MyRectangle()
    {
        LeftTop = new MyPoint(rnd.Next(0, 301), rnd.Next(0, 301));
        Width = rnd.Next(30, 101);
        Height = rnd.Next(30, 101);
    }

    public MyRectangle(MyPoint leftTop, double width, double height)
    {
        LeftTop = leftTop;
        Width = width;
        Height = height;
    }

    public void Move(double dx, double dy)
    {
        LeftTop.Move(dx, dy);
    }
}

namespace WpfOOPFigures
{
    public partial class MainWindow : Window
    {
        private Triangle currentTriangle;
        private MyRectangle currentRectangle;

        private Shape currentShape;

        private double prevSliderX = 0;
        private double prevSliderY = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RandomTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvasAndResetSliders();
            currentTriangle = new Triangle(); // случайные точки
            currentRectangle = null;
            currentShape = DrawTriangle(currentTriangle);
        }

        private void RandomRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvasAndResetSliders();
            currentRectangle = new MyRectangle(); // случайный прямоугольник
            currentTriangle = null;
            currentShape = DrawRectangle(currentRectangle);
        }

        private void FixedTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvasAndResetSliders();
            currentTriangle = new Triangle(
                new MyPoint(100, 100),
                new MyPoint(150, 200),
                new MyPoint(250, 120)
            );
            currentRectangle = null;
            currentShape = DrawTriangle(currentTriangle);
        }

        private void FixedRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvasAndResetSliders();
            currentRectangle = new MyRectangle(new MyPoint(100, 100), 120, 60);
            currentTriangle = null;
            currentShape = DrawRectangle(currentRectangle);
        }


        private Polygon DrawTriangle(Triangle triangle)
        {
            var polygon = new Polygon
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Fill = Brushes.LightBlue
            };

            polygon.Points.Add(new System.Windows.Point(triangle.A.X, triangle.A.Y));
            polygon.Points.Add(new System.Windows.Point(triangle.B.X, triangle.B.Y));
            polygon.Points.Add(new System.Windows.Point(triangle.C.X, triangle.C.Y));

            DrawingCanvas.Children.Add(polygon);
            return polygon;
        }

        private   DrawRectangle(MyRectangle rect)
        {
            var rectangle = new Rectangle
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Fill = Brushes.LightCoral,
                Width = rect.Width,
                Height = rect.Height
            };

            Canvas.SetLeft(rectangle, rect.LeftTop.X);
            Canvas.SetTop(rectangle, rect.LeftTop.Y);

            DrawingCanvas.Children.Add(rectangle);
            return rectangle;
        }

        private void ClearCanvasAndResetSliders()
        {
            DrawingCanvas.Children.Clear();
            prevSliderX = 0;
            prevSliderY = 0;
            SliderX.Value = 0;
            SliderY.Value = 0;
            currentShape = null;
        }

        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (currentShape == null) return;
            double dx = SliderX.Value - prevSliderX;
            prevSliderX = SliderX.Value;
            MoveCurrentFigure(dx, 0);
        }

        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (currentShape == null) return;
            double dy = SliderY.Value - prevSliderY;
            prevSliderY = SliderY.Value;
            MoveCurrentFigure(0, dy);
        }

        private void MoveCurrentFigure(double dx, double dy)
        {
            if (currentTriangle != null && currentShape is Polygon poly)
            {
                currentTriangle.Move(dx, dy);

                poly.Points.Clear();
                poly.Points.Add(new System.Windows.Point(currentTriangle.A.X, currentTriangle.A.Y));
                poly.Points.Add(new System.Windows.Point(currentTriangle.B.X, currentTriangle.B.Y));
                poly.Points.Add(new System.Windows.Point(currentTriangle.C.X, currentTriangle.C.Y));
            }

            if (currentRectangle != null && currentShape is Rectangle rect)
            {
                currentRectangle.Move(dx, dy);

                Canvas.SetLeft(rect, currentRectangle.LeftTop.X);
                Canvas.SetTop(rect, currentRectangle.LeftTop.Y);
            }
        }

    }
}