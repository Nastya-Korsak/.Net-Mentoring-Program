using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    public class DoubleNode<T>
    {
        public DoubleNode(T value)
        {
            Value = value;
            NextNode = null;
            PreviosNode = null;
        }

        public T Value { get; set; }

        public DoubleNode<T> PreviosNode { get; set; }

        public DoubleNode<T> NextNode { get; set; }
    }
}
