using System.Collections.Generic;

namespace Tasks.DoNotChange
{
    public interface IDoublyLinkedList<T> : IEnumerable<T>
    {
        public int Length { get; }

        void Add(T item);

        void AddAt(int index, T item);

        void Remove(T item);

        T RemoveAt(int index);

        T ElementAt(int index);
    }
}
