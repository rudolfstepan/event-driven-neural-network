namespace EventNetwork
{
    //class Neuron
    //{
    //    public Func<InputData, bool> Matcher;
    //    public Action<InputData> OnFire;
    //    public List<Neuron> Connections = new();

    //    public void Trigger(InputData data)
    //    {
    //        if (Matcher(data))
    //        {
    //            OnFire(data);
    //            foreach (var n in Connections)
    //                n.Trigger(data);
    //        }
    //    }
    //}


    public class PixelNeuron
    {
        public int X, Y;
        public byte Threshold = 128;
        public Action OnFire;

        public void Evaluate(byte pixelValue)
        {
            if (pixelValue < Threshold)
            {
                OnFire?.Invoke(); // z. B. "Dunkel erkannt"
            }
        }
    }



}
