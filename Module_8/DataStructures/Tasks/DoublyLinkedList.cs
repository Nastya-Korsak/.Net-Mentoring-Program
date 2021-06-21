using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private readonly DoubleNode<T> _sentinelNode;

        public DoublyLinkedList()
        {
            _sentinelNode = new DoubleNode<T>(default(T));
            _sentinelNode.PreviosNode = _sentinelNode;
            _sentinelNode.NextNode = _sentinelNode;
        }

        public int Length { get; private set; }

        public void Add(T item)
        {
            var newNode = new DoubleNode<T>(item)
            {
                NextNode = _sentinelNode,
                PreviosNode = _sentinelNode.PreviosNode
            };

            _sentinelNode.PreviosNode.NextNode = newNode;
            _sentinelNode.PreviosNode = newNode;

            Length++;
        }

        public void AddAt(int index, T item)
        {
            if (index > Length || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            var newNode = new DoubleNode<T>(item);
            var node = GetNodeByIndex(index);

            newNode.NextNode = node;
            newNode.PreviosNode = node.PreviosNode;

            node.PreviosNode.NextNode = newNode;
            node.PreviosNode = newNode;

            Length++;
        }

        public T ElementAt(int index)
        {
            if (index > Length - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            return GetNodeByIndex(index).Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DoublyLinkedListEnumerator<T>(_sentinelNode, Length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(T item)
        {
            var node = _sentinelNode.NextNode;
            var iteration = 0;

            while (!node.Value.Equals(item) && iteration != Length - 1)
            {
                iteration++;
                node = node.NextNode;
            }

            if (node.Value.Equals(item))
            {
                node.PreviosNode.NextNode = node.NextNode;
                node.NextNode.PreviosNode = node.PreviosNode;

                Length--;
            }
        }

        public T RemoveAt(int index)
        {
            if (Length == 0 || index > Length - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            var node = GetNodeByIndex(index);

            node.PreviosNode.NextNode = node.NextNode;
            node.NextNode.PreviosNode = node.PreviosNode;

            Length--;

            return node.Value;
        }

        private DoubleNode<T> GetNodeByIndex(int index)
        {
            int iteration = 0;
            var node = _sentinelNode.NextNode;

            if (Length / 2 >= index)
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
                while (iteration != Length - index)
                {
                    iteration++;
                    node = _sentinelNode.PreviosNode;
                }

                return node;
            }
        }
    }
}
