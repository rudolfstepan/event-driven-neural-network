using System.Drawing;
using System.Drawing.Drawing2D;

namespace EventNetwork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //GenerateAugmentedTrainingImages(baseSize: 64, outputDir: "D:\\Development\\EventNetwork\\testImages");

            //return;

            var learnedShapes = ShapeRecognizer.LoadTrainingData("D:\\Development\\EventNetwork\\testImages");  // Beispielhafte Funktion wie zuvor erklärt
            Console.WriteLine("Anzahl gelernter Formen: " + learnedShapes.Count);

            var input = ShapeRecognizer.LoadImageAsBinaryGrid("D:\\Development\\EventNetwork\\test.jpg");

            var testVec = ShapeRecognizer.ExtractFeatures(input);
            Console.WriteLine($"Test-Vektor: Dens={testVec.Density:F2}, HSym={testVec.HorizontalSymmetry:F2}, VSym={testVec.VerticalSymmetry:F2}, Edge={testVec.EdgeRatio:F2}");

            string prediction = ShapeRecognizer.ClassifyWithAugmentedInput(input, learnedShapes);

            Console.WriteLine("Erkanntes Muster: " + prediction);
         
        }

        // AUTOMATISCHE GENERIERUNG VON BILDERN
        public static void GenerateTrainingImages(string outputDir, int size = 64)
        {
            Directory.CreateDirectory(outputDir);
            using var blackPen = new Pen(Color.Black, 2);

            var shapes = new[] { "Circle", "Square", "Line", "Triangle" };
            foreach (var shape in shapes)
            {
                for (int i = 0; i < 5; i++) // 5 Varianten
                {
                    Bitmap bmp = new Bitmap(size, size);
                    using Graphics g = Graphics.FromImage(bmp);
                    g.Clear(Color.White);

                    var rnd = new Random(i);
                    int offset = rnd.Next(5, 15);
                    int len = size - 2 * offset;
                    int angle = rnd.Next(0, 360);

                    switch (shape)
                    {
                        case "Circle":
                            g.DrawEllipse(blackPen, offset, offset, len, len);
                            break;
                        case "Square":
                            g.DrawRectangle(blackPen, offset, offset, len, len);
                            break;
                        case "Line":
                            g.DrawLine(blackPen, offset, offset, size - offset, size - offset);
                            break;
                        case "Triangle":
                            Point[] tri = {
                            new Point(size / 2, offset),
                            new Point(offset, size - offset),
                            new Point(size - offset, size - offset)
                        };
                            g.DrawPolygon(blackPen, tri);
                            break;
                    }

                    bmp.Save(Path.Combine(outputDir, $"{shape}_{i}.jpg"));
                }
            }
        }


        // ERWEITERTE GENERIERUNG VON TRAININGSBILDERN MIT ROTATION UND SKALIERUNG UND FÜLLUNG
        public static void GenerateAugmentedTrainingImages(string outputDir, int baseSize = 64)
        {
            Directory.CreateDirectory(outputDir);
            var shapes = new[] { "Circle", "Square", "Line", "Triangle" };
            var rotations = new[] { 0, 45, 90, 135 };
            var scales = new[] { 0.6f, 0.8f, 1.0f };

            using var blackPen = new Pen(Color.Black, 2);
            var blackBrush = Brushes.Black;

            foreach (var shape in shapes)
            {
                foreach (var angle in rotations)
                {
                    foreach (var scale in scales)
                    {
                        Bitmap bmp = new Bitmap(baseSize, baseSize);
                        using Graphics g = Graphics.FromImage(bmp);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.Clear(Color.White);

                        int offset = 5;
                        int len = (int)((baseSize - 2 * offset) * scale);

                        var center = new PointF(baseSize / 2f, baseSize / 2f);
                        g.TranslateTransform(center.X, center.Y);
                        g.RotateTransform(angle);
                        g.TranslateTransform(-center.X, -center.Y);

                        switch (shape)
                        {
                            case "Circle":
                                g.FillEllipse(blackBrush, center.X - len / 2, center.Y - len / 2, len, len);
                                break;
                            case "Square":
                                g.FillRectangle(blackBrush, center.X - len / 2, center.Y - len / 2, len, len);
                                break;
                            case "Line":
                                g.DrawLine(blackPen,
                                    center.X - len / 2, center.Y,
                                    center.X + len / 2, center.Y);
                                break;
                            case "Triangle":
                                PointF[] tri =
                                {
                                new PointF(center.X, center.Y - len / 2),
                                new PointF(center.X - len / 2, center.Y + len / 2),
                                new PointF(center.X + len / 2, center.Y + len / 2)
                            };
                                g.FillPolygon(blackBrush, tri);
                                break;
                        }

                        g.ResetTransform();
                        string filename = $"{shape}_rot{angle}_scale{(int)(scale * 100)}.jpg";
                        bmp.Save(Path.Combine(outputDir, filename));
                    }
                }
            }
        }

        // UNIT TEST METHOD
        public static void RunShapeRecognizerTests()
        {
            int[,] horizontalLine = new int[5, 5];
            horizontalLine[2, 1] = 1;
            horizontalLine[2, 2] = 1;
            horizontalLine[2, 3] = 1;

            int[,] verticalLine = new int[5, 5];
            verticalLine[1, 2] = 1;
            verticalLine[2, 2] = 1;
            verticalLine[3, 2] = 1;

            int[,] diagonalLine = new int[5, 5];
            diagonalLine[0, 0] = 1;
            diagonalLine[1, 1] = 1;
            diagonalLine[2, 2] = 1;

            int[,] rectangle = new int[5, 5];
            for (int i = 1; i <= 3; i++)
            {
                rectangle[1, i] = 1;
                rectangle[3, i] = 1;
            }
            rectangle[2, 1] = 1;
            rectangle[2, 3] = 1;

            Console.WriteLine("Horizontal Line: " + StaticShapeRecognizer.IsHorizontalLine(horizontalLine, 2));
            Console.WriteLine("Vertical Line: " + StaticShapeRecognizer.IsVerticalLine(verticalLine, 2));
            Console.WriteLine("Diagonal Line: " + StaticShapeRecognizer.IsDiagonalLine(diagonalLine));
            Console.WriteLine("Rectangle: " + StaticShapeRecognizer.IsRectangle(rectangle, 1, 1, 3, 3));
            Console.WriteLine("Square: " + StaticShapeRecognizer.IsSquare(rectangle, 1, 1, 3));
        }
    }
}
