namespace Tasks
{
    public class DoubleNode<T>
    {
        public DoubleNode(T value)
        {
            Value = value;
        }

        internal T Value { get; set; }

        internal DoubleNode<T> PreviosNode { get; set; }

        internal DoubleNode<T> NextNode { get; set; }
    }
}
