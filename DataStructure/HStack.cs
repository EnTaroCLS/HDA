using System;
using System.Text;

namespace HDA.DataStructure
{
    internal class HStack<T>
    {
        private HLinkedList<T> _data;

        public int Count { get => _data.Count; }
        public bool IsEmpty { get => _data.IsEmpty; }

        public HStack()
        {
            _data = new HLinkedList<T>();
        }

        public void Push(T e)
        {
            _data.AddFirst(e);
        }

        public T Pop()
        {
            return _data.RemoveFirst();
        }

        public T Peek()
        {
            return _data[0];
        }

        public void Clear()
        {
            _data.Clear();
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("Stack: top ");
            res.Append(_data);
            return res.ToString();
        }
    }
}
