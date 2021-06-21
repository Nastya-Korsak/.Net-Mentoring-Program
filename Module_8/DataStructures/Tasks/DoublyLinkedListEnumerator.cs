using System;
using System.Collections;
using System.Collections.Generic;

namespace Tasks
{
    public class DoublyLinkedListEnumerator<T> : IEnumerator<T>
    {
        private readonly DoubleNode<T> _sentinelNode;
        private readonly int _size;
        private DoubleNode<T> _node;
        private int _position = -1;

        public DoublyLinkedListEnumerator(DoubleNode<T> sentinelNode, int size)
        {
            _sentinelNode = sentinelNode;
            _node = sentinelNode;
            _size = size;
        }

        public T Current
        {
            get
            {
                if (_position == -1 || _position >= _size)
                {
                    throw new InvalidOperationException();
                }

                return _node.Value;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_position < _size - 1)
            {
                _position++;
                _node = _node.NextNode;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _node = _sentinelNode;
        }

        public void Dispose()
        {
        }
    }
}
