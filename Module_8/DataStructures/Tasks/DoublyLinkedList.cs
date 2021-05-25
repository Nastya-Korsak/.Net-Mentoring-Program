using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private readonly DoubleNode<T> _sentinelNode;

        private int _size = 0;

        public DoublyLinkedList()
        {
            _sentinelNode = new DoubleNode<T>(default(T));
            _sentinelNode.PreviosNode = _sentinelNode;
            _sentinelNode.NextNode = _sentinelNode;
        }

        public int Length => _size;

        public void Add(T e)
        {
            var newNode = new DoubleNode<T>(e)
            {
                NextNode = _sentinelNode,
                PreviosNode = _sentinelNode.PreviosNode
            };

            _sentinelNode.PreviosNode.NextNode = newNode;
            _sentinelNode.PreviosNode = newNode;

            _size++;
        }

        public void AddAt(int index, T e)
        {
            if (index > _size || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            var newNode = new DoubleNode<T>(e);
            var node = GetNodeByIndex(index);

            newNode.NextNode = node;
            newNode.PreviosNode = node.PreviosNode;

            node.PreviosNode.NextNode = newNode;
            node.PreviosNode = newNode;

            _size++;
        }

        public T ElementAt(int index)
        {
            if (index > _size - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            return GetNodeByIndex(index).Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DoublyLinkedListEnumerator<T>(_sentinelNode, _size);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(T item)
        {
            var node = _sentinelNode.NextNode;
            var iteration = 0;

            while (!node.Value.Equals(item) && iteration != _size - 1)
            {
                iteration++;
                node = node.NextNode;
            }

            if (node.Value.Equals(item))
            {
                node.PreviosNode.NextNode = node.NextNode;
                node.NextNode.PreviosNode = node.PreviosNode;

                _size--;
            }
        }

        public T RemoveAt(int index)
        {
            if (_size == 0 || index > _size - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            var node = GetNodeByIndex(index);

            node.PreviosNode.NextNode = node.NextNode;
            node.NextNode.PreviosNode = node.PreviosNode;

            _size--;

            return node.Value;
        }

        private DoubleNode<T> GetNodeByIndex(int index)
        {
            int iteration = 0;
            var node = _sentinelNode.NextNode;

            if (_size / 2 >= index)
            {
                while (iteration != index)
                {
                    iteration++;
                    node = node.NextNode;
                }

                return node;
            }
            else
            {
                node = _sentinelNode;
                while (iteration != _size - index)
                {
                    iteration++;
                    node = _sentinelNode.PreviosNode;
                }

                return node;
            }
        }
    }
}
