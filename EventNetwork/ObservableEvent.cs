namespace EventNetwork
{
    public abstract class ObservableEvent<T>
    {
        private readonly List<Action<T>> _subscribers = new();
        private readonly object _lock = new();

        public void Subscribe(Action<T> handler)
        {
            lock (_lock)
            {
                if (!_subscribers.Contains(handler))
                    _subscribers.Add(handler);
            }
        }

        public void Unsubscribe(Action<T> handler)
        {
            lock (_lock)
            {
                _subscribers.Remove(handler);
            }
        }

        public IReadOnlyList<Action<T>> GetSubscribers()
        {
            lock (_lock)
            {
                return _subscribers.ToList(); // defensive copy
            }
        }

        public void Publish(T data)
        {
            foreach (var handler in GetSubscribers())
            {
                try
                {
                    handler(data);
                }
                catch (Exception ex)
                {
                    OnHandlerException(handler, ex);
                }
            }
        }

        public void PublishParallel(T data)
        {
            Parallel.ForEach(GetSubscribers(), handler =>
            {
                try
                {
                    handler(data);
                }
                catch (Exception ex)
                {
                    OnHandlerException(handler, ex);
                }
            });
        }

        protected virtual void OnHandlerException(Action<T> handler, Exception ex)
        {
            Console.WriteLine($"Exception in handler {handler.Method.Name}: {ex.Message}");
            // overrideable
        }
    }

}
