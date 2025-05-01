namespace EventNetwork
{
    public class ReactiveValue<T>
    {
        private T _value;
        private readonly List<Action<T>> _bindings = new();

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    foreach (var b in _bindings)
                        b(_value);
                }
            }
        }

        public void Bind(Action<T> reaction)
        {
            _bindings.Add(reaction);
            reaction(_value); // sofortige Initialreaktion
        }
    }


}
