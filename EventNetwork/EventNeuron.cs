namespace EventNetwork
{
    public class EventNeuron<T>
    {
        private readonly List<(Predicate<T> Filter, Action<T> Reaction)> _subscriptions = new();
        private readonly List<EventNeuron<T>> _targets = new();

        public void Subscribe(Predicate<T> condition, Action<T> reaction)
        {
            _subscriptions.Add((condition, reaction));
        }

        public void ConnectTo(EventNeuron<T> target)
        {
            _targets.Add(target);
        }

        public void Trigger(T data)
        {
            foreach (var (filter, reaction) in _subscriptions)
            {
                if (filter(data))
                {
                    reaction(data);

                    // Weiterleitung an verbundene Neuronen
                    foreach (var target in _targets)
                    {
                        target.Trigger(data);
                    }
                }
            }
        }
    }


}
