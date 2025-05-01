namespace EventNetwork
{
    public class CentralSignal<T>
    {
        private T _lastValue;
        private readonly List<Action<T>> _listeners = new();

        public void Listen(Action<T> handler)
        {
            _listeners.Add(handler);
        }

        public void Trigger(T value)
        {
            _lastValue = value;
            foreach (var handler in _listeners)
            {
                handler.Invoke(value); // synchron, aber könnte durch Task ersetzt werden
            }
        }

        public T LastValue => _lastValue;
    }

}
