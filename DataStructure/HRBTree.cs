using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HRBTree<T>
    {
        private const bool RED = true;
        private const bool BLACK = false;

        private class Node
        {
            public T _e;
            public Node _left, _right;
            // true表示红色，false表示黑色
            public bool _color;

            public Node(T e)
            {
                _e = e;
                _left = null;
                _right = null;
                _color = RED;
            }

            public Node(T e, bool color)
            {
                _e = e;
                _left = null;
                _right = null;
                _color = color;
            }
        }

        private Node _root;
        private int _count;

        private Comparer<T> _comparer;

        public int Count { get => _count; private set => _count = value; }
        public bool IsEmpty { get => _count == 0; }

        public HRBTree()
        {
            _root = null;
            _count = 0;
            _comparer = Comparer<T>.Default;
        }

        public HRBTree(Comparer<T> comparer)
        {
            _root = null;
            _count = 0;
            _comparer = comparer;
        }

        private bool IsRed(Node node)
        {
            if (node == null)
                return BLACK;
            return node._color;
        }

        // 是否包含元素e
        public bool Contains(T e)
        {
            if (e == null)
                throw new ArgumentNullException();
            return Contains(_root, e);
        }

        // 以node为根的树中是否包含元素e
        private bool Contains(Node node, T e)
        {
            if (node == null)
                return false;

            if (_comparer.Compare(e, node._e) == 0)
                return true;
            else if (_comparer.Compare(e, node._e) < 0)
                return Contains(node._left, e);
            else
                return Contains(node._right, e);
        }

        // 添加新的元素e
        public void Add(T e)
        {
            _root = Add(_root, e);
            _root._color = BLACK; // 最终根节点为黑色节点
        }

        // 向以node为根的树中插入元素e
        // 返回插入新节点后红黑树的根
        private Node Add(Node node, T e)
        {
            if (e == null)
                throw new ArgumentNullException();

            if (node == null)
            {
                _count++;
                return new Node(e); // 默认插入红色节点
            }

            if (_comparer.Compare(e, node._e) < 0)
                node._left = Add(node._left, e);
            else if (_comparer.Compare(e, node._e) > 0)
                node._right = Add(node._right, e);

            if (IsRed(node._right) && !IsRed(node._left))
                node = LeftRotate(node);
            if (IsRed(node._left) && IsRed(node._left._left))
                node = RightRotate(node);
            if (IsRed(node._left) && IsRed(node._right))
                FlipColors(node);

            return node;
        }

        public void RemoveMin()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = RemoveMin(_root);
            if (!IsEmpty)
                _root._color = BLACK;
        }

        private Node RemoveMin(Node node)
        {
            if (node._left == null)
                return null;

            if (!IsRed(node._left) && !IsRed(node._left._left))
                node = MoveRedLeft(node);

            node._left = RemoveMin(node._left);
            return Balance(node);
        }

        public void RemoveMax()
        {
            if(IsEmpty)
                throw new Exception("Tree is empty");

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = RemoveMax(_root);
            if (!IsEmpty) 
                _root._color = BLACK;
        }

        private Node RemoveMax(Node node)
        {
            if (IsRed(node._left))
                node = RightRotate(node);

            if (node._right == null)
                return null;

            if (!IsRed(node._right) && !IsRed(node._right._left))
                node = MoveRedRight(node);

            node._right = RemoveMax(node._right);

            return Balance(node);
        }

        public void Remove(T e)
        {
            if (e == null)
                throw new ArgumentNullException();
            if (!Contains(e)) return;

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = Remove(_root, e);
            if (!IsEmpty) 
                _root._color = BLACK;
        }

        private Node Remove(Node node, T e)
        {
            if (_comparer.Compare(e, node._e) < 0)
            {
                if (!IsRed(node._left) && !IsRed(node._left._left))
                    node = MoveRedLeft(node);
                node._left = Remove(node._left, e);
            }
            else
            {
                if (IsRed(node._left))
                    node = RightRotate(node);
                if (_comparer.Compare(e, node._e) == 0 && (node._right == null))
                    return null;
                if (!IsRed(node._right) && !IsRed(node._right._left))
                    node = MoveRedRight(node);
                if (_comparer.Compare(e, node._e) == 0)
                {
                    Node x = Min(node._right);
                    node._e = x._e;
                    node._right = RemoveMin(node._right);
                }
                else node._right = Remove(node._right, e);
            }
            return Balance(node);
        }


        //   node                     x
        //  /   \     左旋转         /  \
        // T1   x   --------->   node   T3
        //     / \              /   \
        //    T2 T3            T1   T2
        private Node LeftRotate(Node node)
        {
            Node x = node._right;
            node._right = x._left;
            x._left = node;
            x._color = node._color;
            node._color = RED;

            return x;
        }

        //     node                   x
        //    /   \     右旋转       /  \
        //   x    T2   ------->   y   node
        //  / \                       /  \
        // y  T1                     T1  T2
        private Node RightRotate(Node node)
        {
            Node x = node._left;
            node._left = x._right;
            x._right = node;
            x._color = node._color;
            node._color = RED;

            return x;
        }

        // 颜色翻转
        private void FlipColors(Node node)
        {
            node._color = !node._color;
            node._left._color = !node._left._color;
            node._right._color = !node._right._color;
        }

        private Node MoveRedLeft(Node node)
        {
            FlipColors(node);
            if (IsRed(node._right._left))
            {
                node._right = RightRotate(node._right);
                node = LeftRotate(node);
                FlipColors(node);
            }
            return node;
        }

        private Node MoveRedRight(Node node)
        {
            FlipColors(node);
            if (IsRed(node._left._left))
            {
                node = RightRotate(node);
                FlipColors(node);
            }
            return node;
        }

        private Node Balance(Node node)
        {
            if (IsRed(node._right) && !IsRed(node._left)) 
                node = LeftRotate(node);
            if (IsRed(node._left) && IsRed(node._left._left)) 
                node = RightRotate(node);
            if (IsRed(node._left) && IsRed(node._right)) 
                FlipColors(node);

            return node;
        }

        public T Min()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Min(_root)._e;
        }

        private Node Min(Node node)
        {
            if (node._left == null) 
                return node;
            else return Min(node._left);
        }

        public T Max()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Max(_root)._e;
        }

        private Node Max(Node node)
        {
            if (node._right == null) 
                return node;
            else return Max(node._right);
        }

        // 中序遍历
        public void InOrder(Action<T> action)
        {
            InOrder(_root, action);
        }

        // 中序遍历以node为根的树, 递归算法
        private void InOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;

            InOrder(node._left, action);
            action(node._e);
            InOrder(node._right, action);
        }
    }

    internal class HRBTree<K, V>
    {
        private const bool RED = true;
        private const bool BLACK = false;

        private class Node
        {
            public K _key;
            public V _value;
            public Node _left, _right;
            // true表示红色，false表示黑色
            public bool _color;

            public Node(K key, V value)
            {
                _key = key;
                _value = value;
                _left = null;
                _right = null;
                _color = RED;
            }

            public Node(K key, V value, bool color)
            {
                _key = key;
                _value = value;
                _left = null;
                _right = null;
                _color = color;
            }
        }

        private Node _root;
        private int _count;

        private Comparer<K> _comparer;

        public int Count { get => _count; private set => _count = value; }
        public bool IsEmpty { get => _count == 0; }

        public V this[K key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                return Get(_root, key);
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException();
                if (!Contains(key))
                    return;
                Set(_root, key, value);
            }
        }

        public HRBTree()
        {
            _root = null;
            _count = 0;
        }

        public HRBTree(Comparer<K> comparer)
        {
            _root = null;
            _count = 0;
            _comparer = comparer;
        }

        private bool IsRed(Node node)
        {
            if (node == null)
                return BLACK;
            return node._color;
        }

        private V Get(Node node, K key)
        {
            while (node != null)
            {
                int cmp = _comparer.Compare(key, node._key);
                if (cmp < 0) 
                    node = node._left;
                else if (cmp > 0) 
                    node = node._right;
                else 
                    return node._value;
            }
            return default;
        }

        private void Set(Node node, K key, V value)
        {
            while (node != null)
            {
                int cmp = _comparer.Compare(key, node._key);
                if (cmp < 0)
                    node = node._left;
                else if (cmp > 0)
                    node = node._right;
                else
                {
                    node._value = value;
                    break;
                }
            }
        }

        // 是否包含键key
        public bool Contains(K key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return Contains(_root, key);
        }

        // 以node为根的树中是否包含键key
        private bool Contains(Node node, K key)
        {
            if (node == null)
                return false;

            if (_comparer.Compare(key, node._key) == 0)
                return true;
            else if (_comparer.Compare(key, node._key) < 0)
                return Contains(node._left, key);
            else
                return Contains(node._right, key);
        }

        // 添加新的元素
        public void Add(K key, V value)
        {
            _root = Add(_root, key, value);
            _root._color = BLACK; // 最终根节点为黑色节点
        }

        // 向以node为根的树中插入元素
        // 返回插入新节点后红黑树的根
        private Node Add(Node node, K key, V value)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (node == null)
            {
                _count++;
                return new Node(key, value); // 默认插入红色节点
            }

            if (_comparer.Compare(key, node._key) < 0)
                node._left = Add(node._left, key, value);
            else if (_comparer.Compare(key, node._key) > 0)
                node._right = Add(node._right, key, value);

            if (IsRed(node._right) && !IsRed(node._left))
                node = LeftRotate(node);
            if (IsRed(node._left) && IsRed(node._left._left))
                node = RightRotate(node);
            if (IsRed(node._left) && IsRed(node._right))
                FlipColors(node);

            return node;
        }

        public void RemoveMin()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = RemoveMin(_root);
            if (!IsEmpty)
                _root._color = BLACK;
        }

        private Node RemoveMin(Node node)
        {
            if (node._left == null)
                return null;

            if (!IsRed(node._left) && !IsRed(node._left._left))
                node = MoveRedLeft(node);

            node._left = RemoveMin(node._left);
            return Balance(node);
        }

        public void RemoveMax()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = RemoveMax(_root);
            if (!IsEmpty)
                _root._color = BLACK;
        }

        private Node RemoveMax(Node node)
        {
            if (IsRed(node._left))
                node = RightRotate(node);

            if (node._right == null)
                return null;

            if (!IsRed(node._right) && !IsRed(node._right._left))
                node = MoveRedRight(node);

            node._right = RemoveMax(node._right);

            return Balance(node);
        }

        public void Remove(K key)
        {
            if (key == null)
                throw new ArgumentNullException();
            if (!Contains(key)) 
                return;

            if (!IsRed(_root._left) && !IsRed(_root._right))
                _root._color = RED;

            _root = Remove(_root, key);
            if (!IsEmpty)
                _root._color = BLACK;
        }

        private Node Remove(Node node, K key)
        {
            if (_comparer.Compare(key, node._key) < 0)
            {
                if (!IsRed(node._left) && !IsRed(node._left._left))
                    node = MoveRedLeft(node);
                node._left = Remove(node._left, key);
            }
            else
            {
                if (IsRed(node._left))
                    node = RightRotate(node);
                if (_comparer.Compare(key, node._key) == 0 && (node._right == null))
                    return null;
                if (!IsRed(node._right) && !IsRed(node._right._left))
                    node = MoveRedRight(node);
                if (_comparer.Compare(key, node._key) == 0)
                {
                    Node x = Min(node._right);
                    node._key = x._key;
                    node._value = x._value;
                    node._right = RemoveMin(node._right);
                }
                else node._right = Remove(node._right, key);
            }
            return Balance(node);
        }


        //   node                     x
        //  /   \     左旋转         /  \
        // T1   x   --------->   node   T3
        //     / \              /   \
        //    T2 T3            T1   T2
        private Node LeftRotate(Node node)
        {
            Node x = node._right;
            node._right = x._left;
            x._left = node;
            x._color = node._color;
            node._color = RED;

            return x;
        }

        //     node                   x
        //    /   \     右旋转       /  \
        //   x    T2   ------->   y   node
        //  / \                       /  \
        // y  T1                     T1  T2
        private Node RightRotate(Node node)
        {
            Node x = node._left;
            node._left = x._right;
            x._right = node;
            x._color = node._color;
            node._color = RED;

            return x;
        }

        // 颜色翻转
        private void FlipColors(Node node)
        {
            node._color = !node._color;
            node._left._color = !node._left._color;
            node._right._color = !node._right._color;
        }

        private Node MoveRedLeft(Node node)
        {
            FlipColors(node);
            if (IsRed(node._right._left))
            {
                node._right = RightRotate(node._right);
                node = LeftRotate(node);
                FlipColors(node);
            }
            return node;
        }

        private Node MoveRedRight(Node node)
        {
            FlipColors(node);
            if (IsRed(node._left._left))
            {
                node = RightRotate(node);
                FlipColors(node);
            }
            return node;
        }

        private Node Balance(Node node)
        {
            if (IsRed(node._right) && !IsRed(node._left))
                node = LeftRotate(node);
            if (IsRed(node._left) && IsRed(node._left._left))
                node = RightRotate(node);
            if (IsRed(node._left) && IsRed(node._right))
                FlipColors(node);

            return node;
        }

        public K Min()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Min(_root)._key;
        }

        private Node Min(Node node)
        {
            if (node._left == null)
                return node;
            else return Min(node._left);
        }

        public K Max()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Max(_root)._key;
        }

        private Node Max(Node node)
        {
            if (node._right == null)
                return node;
            else return Max(node._right);
        }

        // 中序遍历
        public void InOrder(Action<K> action)
        {
            InOrder(_root, action);
        }

        // 中序遍历以node为根的树, 递归算法
        private void InOrder(Node node, Action<K> action)
        {
            if (node == null)
                return;

            InOrder(node._left, action);
            action(node._key);
            InOrder(node._right, action);
        }
    }
}
