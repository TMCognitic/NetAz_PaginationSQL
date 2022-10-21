namespace Tools
{
    internal class Parameter
    {
        public object? Value { get; internal set; }
        public Direction Direction { get; init; }

        internal Parameter(object value, Direction direction)
        {
            Value = value;
            Direction = direction;
        }
    }
}
