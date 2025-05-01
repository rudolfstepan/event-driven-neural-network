using System.Drawing;

namespace EventNetwork
{
    public class TrainingExample
    {
        public int[,] Grid;
        public string Label;
    }

    public class FeatureVector
    {
        public double Density;
        public double HorizontalSymmetry;
        public double VerticalSymmetry;
        public double EdgeRatio;
        public string Label;
    }

    public static class ShapeRecognizer
    {
        public static bool IsPoint(int[,] grid, int x, int y) => grid[x, y] == 1;

        public static int[,] LoadImageAsBinaryGrid(string path, int threshold = 128)
        {
            Bitmap bmp = new Bitmap(path);
            int[,] grid = new int[bmp.Height, bmp.Width];

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color color = bmp.GetPixel(x, y);
                    int brightness = (color.R + color.G + color.B) / 3;
                    grid[y, x] = brightness < threshold ? 1 : 0;
                }
            }
            return grid;
        }

        public static void PrintBinaryGrid(int[,] grid)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Console.Write(grid[y, x] == 1 ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        public static FeatureVector ExtractFeatures(int[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            int pixelCount = height * width;

            int filled = 0;
            int edgeCount = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[y, x] == 1)
                    {
                        filled++;
                        if (IsEdge(grid, x, y)) edgeCount++;
                    }
                }
            }

            double density = (double)filled / pixelCount;
            double hSym = CompareSymmetry(grid, horizontal: true);
            double vSym = CompareSymmetry(grid, horizontal: false);
            double edgeRatio = edgeCount == 0 ? 0 : (double)edgeCount / filled;

            return new FeatureVector
            {
                Density = density,
                HorizontalSymmetry = hSym,
                VerticalSymmetry = vSym,
                EdgeRatio = edgeRatio
            };
        }

        private static bool IsEdge(int[,] grid, int x, int y)
        {
            int h = grid.GetLength(0);
            int w = grid.GetLength(1);
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < w && ny >= 0 && ny < h && (dx != 0 || dy != 0))
                    {
                        if (grid[ny, nx] == 0) return true;
                    }
                }
            }
            return false;
        }

        private static double CompareSymmetry(int[,] grid, bool horizontal)
        {
            int h = grid.GetLength(0);
            int w = grid.GetLength(1);
            double matches = 0;
            double total = 0;

            if (horizontal)
            {
                for (int y = 0; y < h / 2; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (grid[y, x] == grid[h - y - 1, x]) matches++;
                        total++;
                    }
                }
            }
            else
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w / 2; x++)
                    {
                        if (grid[y, x] == grid[y, w - x - 1]) matches++;
                        total++;
                    }
                }
            }
            return total == 0 ? 0 : matches / total;
        }

        public static string ClassifyByNearestNeighbor(FeatureVector input, Dictionary<string, List<FeatureVector>> learned)
        {
            string bestLabel = null;
            double bestDistance = double.MaxValue;

            foreach (var kvp in learned)
            {
                foreach (var vec in kvp.Value)
                {
                    double d = Distance(input, vec);
                    if (d < bestDistance)
                    {
                        bestDistance = d;
                        bestLabel = kvp.Key;
                    }
                }
            }
            return bestLabel ?? "Unknown";
        }

        private static double Distance(FeatureVector a, FeatureVector b)
        {
            return Math.Sqrt(
                Math.Pow(a.Density - b.Density, 2) +
                Math.Pow(a.HorizontalSymmetry - b.HorizontalSymmetry, 2) +
                Math.Pow(a.VerticalSymmetry - b.VerticalSymmetry, 2) +
                Math.Pow(a.EdgeRatio - b.EdgeRatio, 2)
            );
        }

        public static string ClassifyWithAugmentedInput(int[,] originalGrid, Dictionary<string, List<FeatureVector>> learned)
        {
            string bestLabel = "Unknown";
            double bestScore = double.MaxValue;

            var angles = new[] { 0, 45, 90, 135 };
            foreach (var angle in angles)
            {
                var rotated = RotateGrid(originalGrid, angle);
                var vector = ExtractFeatures(rotated);
                var label = ClassifyByNearestNeighbor(vector, learned);
                var score = learned[label].Select(vec => Distance(vector, vec)).Min();

                if (score < bestScore)
                {
                    bestScore = score;
                    bestLabel = label;
                }
            }

            return bestLabel;
        }

        public static int[,] RotateGrid(int[,] grid, float angle)
        {
            int w = grid.GetLength(1);
            int h = grid.GetLength(0);
            using Bitmap bmp = new Bitmap(w, h);
            using Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (grid[y, x] == 1)
                        bmp.SetPixel(x, y, Color.Black);
                }
            }

            Bitmap rotatedBmp = new Bitmap(w, h);
            using (Graphics gr = Graphics.FromImage(rotatedBmp))
            {
                gr.Clear(Color.White);
                gr.TranslateTransform(w / 2f, h / 2f);
                gr.RotateTransform(angle);
                gr.TranslateTransform(-w / 2f, -h / 2f);
                gr.DrawImage(bmp, new Point(0, 0));
            }

            int[,] result = new int[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color pixel = rotatedBmp.GetPixel(x, y);
                    result[y, x] = pixel.R < 128 ? 1 : 0;
                }
            }
            return result;
        }

        // Trainingsdaten automatisch laden und in learnedShapes speichern
        public static Dictionary<string, List<FeatureVector>> LoadTrainingData(string folderPath, int threshold = 128)
        {
            var learnedShapes = new Dictionary<string, List<FeatureVector>>();

            foreach (var file in Directory.GetFiles(folderPath, "*.jpg"))
            {
                try
                {
                    string label = Path.GetFileNameWithoutExtension(file).Split('_')[0];
                    var grid = LoadImageAsBinaryGrid(file, threshold);
                    var vector = ExtractFeatures(grid);
                    vector.Label = label;

                    if (!learnedShapes.ContainsKey(label))
                        learnedShapes[label] = new List<FeatureVector>();

                    learnedShapes[label].Add(vector);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Laden von {file}: {ex.Message}");
                }
            }

            Console.WriteLine("Trainingsdaten geladen:");
            foreach (var kvp in learnedShapes)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value.Count} Beispiele");
            }

            return learnedShapes;
        }
    }
}
