using System.Drawing;

namespace EventNetwork
{
    public class VisualNet
    {
        public PixelNeuron[,] Neurons;

        public void LoadImage(Bitmap image)
        {
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    var brightness = image.GetPixel(x, y).R; // Grauwert
                    Neurons[x, y].Evaluate(brightness);
                }
        }
    }


}
