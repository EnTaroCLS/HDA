using System;
using System.Text;

namespace HDA.DataStructure
{
    internal class HLinkedList<T>
    {
        private class Node
        {
            public T _e;
            public Node _next;

            public Node(T e, Node next)
            {
                _e = e;
                _next = next;
            }

            public Node(T e) : this(e, null) { }
            public Node() : this(default, null) { }

            public override String ToString()
            {
                return _e.ToString();
            }
        }

        private Node _dummyHead;
        private int _count;

        public int Count { get => _count; private set => _count = value; }
        public bool IsEmpty { get => _count == 0; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");

                Node cur = _dummyHead._next;
                for (int i = 0; i < index; i++)
                    cur = cur._next;
                return cur._e;
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");

                Node cur = _dummyHead._next;
                for (int i = 0; i < index; i++)
                    cur = cur._next;
                cur._e = value;
            }
        }

        public HLinkedList()
        {
            _dummyHead = new Node();
            _count = 0;
        }

        public void Add(int index, T e)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");

            Node prev = _dummyHead;
            for (int i = 0; i < index; i++)
                prev = prev._next;

            prev._next = new Node(e, prev._next);
            _count++;
        }

        // 在链表头添加新的元素e
        public void AddFirst(T e)
        {
            Add(0, e);
        }

        // 在链表末尾添加新的元素e
        public void AddLast(T e)
        {
            Add(_count, e);
        }

        // 查找链表中是否有元素e
        public bool Contains(T e)
        {
            Node cur = _dummyHead._next;
            while (cur != null)
            {
                if (cur._e.Equals(e))
                    return true;
                cur = cur._next;
            }
            return false;
        }

        public T Remove(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException("index");

            Node prev = _dummyHead;
            for (int i = 0; i < index; i++)
                prev = prev._next;

            Node retNode = prev._next;
            prev._next = retNode._next;
            retNode._next = null;
            _count--;

            return retNode._e;
        }

        // 从链表中删除第一个元素, 返回删除的元素
        public T RemoveFirst()
        {
            return Remove(0);
        }

        // 从链表中删除最后一个元素, 返回删除的元素
        public T RemoveLast()
        {
            return Remove(_count - 1);
        }

        // 从链表中删除元素e
        public void RemoveElement(T e)
        {
            Node prev = _dummyHead;
            while (prev._next != null)
            {
                if (prev._next._e.Equals(e))
                    break;
                prev = prev._next;
            }

            if (prev._next != null)
            {
                Node delNode = prev._next;
                prev._next = delNode._next;
                delNode._next = null;
                _count--;
            }
        }

        public void Clear()
        {
            _dummyHead = new Node();
            _count = 0;
        }

        public override String ToString()
        {
            StringBuilder res = new StringBuilder();

            Node cur = _dummyHead._next;
            while (cur != null)
            {
                res.Append(cur + " -> ");
                cur = cur._next;
            }
            res.Append("NULL");

            return res.ToString();
        }
    }
}
