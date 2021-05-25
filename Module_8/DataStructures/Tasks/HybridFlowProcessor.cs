using System;
using Tasks.DoNotChange;

namespace Tasks
{
    public class HybridFlowProcessor<T> : IHybridFlowProcessor<T>
    {
        private readonly DoublyLinkedList<T> _itemsList;

        public HybridFlowProcessor()
        {
            _itemsList = new DoublyLinkedList<T>();
        }

        public T Dequeue()
        {
            if (_itemsList.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return _itemsList.RemoveAt(0);
        }

        public void Enqueue(T item)
        {
            _itemsList.Add(item);
        }

        public T Pop()
        {
            if (_itemsList.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return _itemsList.RemoveAt(_itemsList.Length - 1);
        }

        public void Push(T item)
        {
            _itemsList.Add(item);
        }
    }
}
