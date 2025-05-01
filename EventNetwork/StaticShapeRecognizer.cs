namespace EventNetwork
{
    // STAGE 2: Linien- und Punkt-Erkennung

    public static class StaticShapeRecognizer
    {
        public static bool IsPoint(int[,] grid, int x, int y)
        {
            return grid[x, y] == 1;
        }

        public static bool IsHorizontalLine(int[,] grid, int row, int minLength = 3)
        {
            int count = 0;
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[row, x] == 1) count++;
                else count = 0;
                if (count >= minLength) return true;
            }
            return false;
        }

        public static bool IsVerticalLine(int[,] grid, int col, int minLength = 3)
        {
            int count = 0;
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                if (grid[y, col] == 1) count++;
                else count = 0;
                if (count >= minLength) return true;
            }
            return false;
        }

        public static bool IsDiagonalLine(int[,] grid, int minLength = 3)
        {
            int size = grid.GetLength(0);
            for (int y = 0; y < size - minLength; y++)
            {
                for (int x = 0; x < size - minLength; x++)
                {
                    int countDown = 0, countUp = 0;
                    for (int d = 0; d < minLength; d++)
                    {
                        if (grid[y + d, x + d] == 1) countDown++;
                        if (grid[y + minLength - d - 1, x + d] == 1) countUp++;
                    }
                    if (countDown >= minLength || countUp >= minLength) return true;
                }
            }
            return false;
        }

        // STAGE 3: Komplexe Formen

        public static bool IsRectangle(int[,] grid, int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                if (grid[startY, x] != 1 || grid[startY + height - 1, x] != 1)
                    return false;
            }
            for (int y = startY; y < startY + height; y++)
            {
                if (grid[y, startX] != 1 || grid[y, startX + width - 1] != 1)
                    return false;
            }
            return true;
        }

        public static bool IsSquare(int[,] grid, int startX, int startY, int size)
        {
            return IsRectangle(grid, startX, startY, size, size);
        }

        public static bool IsTriangle(int[,] grid)
        {
            // Dummy placeholder – you'd use edge detection and corner counting here
            return false;
        }

        public static bool IsCircle(int[,] grid)
        {
            // Dummy placeholder – you'd calculate radius variance around center
            return false;
        }

        public static bool IsRotatedShape(int[,] grid, Func<int[,], bool> shapeFunc)
        {
            int[,] rotated = RotateMatrix(grid);
            return shapeFunc(rotated);
        }

        private static int[,] RotateMatrix(int[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);
            int[,] result = new int[height, width];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    result[j, width - i - 1] = matrix[i, j];
            return result;
        }
    }
}
