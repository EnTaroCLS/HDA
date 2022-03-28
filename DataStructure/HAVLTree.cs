using System;
using System.Collections.Generic;

namespace HDA.DataStructure
{
    internal class HAVLTree<T>
    {
        private class Node
        {
            public T _e;
            public Node _left, _right;
            public int _height;

            public Node(T e)
            {
                _e = e;
                _left = null;
                _right = null;
                _height = 1;
            }
        }

        private Node _root;
        private int _count;

        private Comparer<T> _comparer;
        public Comparer<T> Comparer => _comparer;

        public int Count { get => _count; private set => _count = value; }
        public bool IsEmpty { get => _count == 0;}

        public HAVLTree()
        {
            _root = null;
            _count = 0;
            _comparer = Comparer<T>.Default;
        }

        public HAVLTree(Comparer<T> comparer)
        {
            _root = null;
            _count = 0;
            _comparer = comparer;
        }

        private int GetHeight(Node node)
        {
            if (node == null)
                return 0;
            return node._height;
        }

        private int GetBalanceFactor(Node node)
        {
            if (node == null)
                return 0;
            return GetHeight(node._left) - GetHeight(node._right);
        }

        // 对节点y进行向右旋转操作，返回旋转后新的根节点x
        //        y                              x
        //       / \                           /   \
        //      x   T4     向右旋转 (y)        z     y
        //     / \       - - - - - - - ->    / \   / \
        //    z   T3                       T1  T2 T3 T4
        //   / \
        // T1   T2
        private Node RightRotate(Node y)
        {
            Node x = y._left;
            Node T3 = x._right;

            // 向右旋转过程
            x._right = y;
            y._left = T3;

            // 更新height
            y._height = Math.Max(GetHeight(y._left), GetHeight(y._right)) + 1;
            x._height = Math.Max(GetHeight(x._left), GetHeight(x._right)) + 1;

            return x;
        }

        // 对节点y进行向左旋转操作，返回旋转后新的根节点x
        //    y                             x
        //  /  \                          /   \
        // T1   x      向左旋转 (y)       y     z
        //     / \   - - - - - - - ->   / \   / \
        //   T2  z                     T1 T2 T3 T4
        //      / \
        //     T3 T4
        private Node LeftRotate(Node y)
        {
            Node x = y._right;
            Node T2 = x._left;

            // 向左旋转过程
            x._left = y;
            y._right = T2;

            // 更新height
            y._height = Math.Max(GetHeight(y._left), GetHeight(y._right)) + 1;
            x._height = Math.Max(GetHeight(x._left), GetHeight(x._right)) + 1;

            return x;
        }

        // 添加新的元素e
        public void Add(T e)
        {
            if (e == null)
                throw new ArgumentNullException();
            _root = Add(_root, e);
        }

        // 返回插入新节点后树的根
        private Node Add(Node node, T e)
        {
            if (node == null)
            {
                _count++;
                return new Node(e);
            }

            if (_comparer.Compare(e, node._e) < 0)
                node._left = Add(node._left, e);
            else if (_comparer.Compare(e, node._e) > 0)
                node._right = Add(node._right, e);

            // 更新height
            node._height = 1 + Math.Max(GetHeight(node._left), GetHeight(node._right));
            // 计算平衡因子
            int balanceFactor = GetBalanceFactor(node);

            // LL
            if (balanceFactor > 1 && GetBalanceFactor(node._left) >= 0)
                return RightRotate(node);
            // RR
            if (balanceFactor < -1 && GetBalanceFactor(node._right) <= 0)
                return LeftRotate(node);
            // LR
            if (balanceFactor > 1 && GetBalanceFactor(node._left) < 0)
            {
                node._left = LeftRotate(node._left);
                return RightRotate(node);
            }
            // RL
            if (balanceFactor < -1 && GetBalanceFactor(node._right) > 0)
            {
                node._right = RightRotate(node._right);
                return LeftRotate(node);
            }

            return node;
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

        // 寻找树的最小元素
        public T Min()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Min(_root)._e;
        }

        // 返回以node为根的树的最小值所在的节点
        private Node Min(Node node)
        {
            if (node._left == null)
                return node;
            return Min(node._left);
        }

        // 寻找树的最大元素
        public T Max()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty");

            return Max(_root)._e;
        }

        // 返回以node为根的树的最大值所在的节点
        private Node Max(Node node)
        {
            if (node._right == null)
                return node;

            return Max(node._right);
        }

        // 删除最小值所在节点, 返回最小值
        public T RemoveMin()
        {
            T ret = Min();
            _root = RemoveMin(_root);
            return ret;
        }

        // 删除掉以node为根的树中的最小节点
        // 返回删除节点后新的树的根
        private Node RemoveMin(Node node)
        {
            if (node._left == null)
            {
                Node rightNode = node._right;
                node._right = null;
                _count--;
                return rightNode;
            }

            node._left = RemoveMin(node._left);
            return node;
        }

        // 从树中删除最大值所在节点
        public T RemoveMax()
        {
            T ret = Max();
            _root = RemoveMax(_root);
            return ret;
        }

        // 删除掉以node为根的树中的最大节点
        // 返回删除节点后新的树的根
        private Node RemoveMax(Node node)
        {
            if (node._right == null)
            {
                Node leftNode = node._left;
                node._left = null;
                _count--;
                return leftNode;
            }

            node._right = RemoveMax(node._right);
            return node;
        }

        // 删除元素为e的节点
        public void Remove(T e)
        {
            if (e == null)
                throw new ArgumentNullException();
            _root = Remove(_root, e);
        }

        // 删除掉以node为根的树中值为e的节点, 递归算法
        // 返回删除节点后新的树的根
        private Node Remove(Node node, T e)
        {
            if (node == null)
                return null;

            Node retNode;
            if (_comparer.Compare(e, node._e) < 0)
            {
                node._left = Remove(node._left, e);
                retNode = node;
            }
            else if (_comparer.Compare(e, node._e) > 0)
            {
                node._right = Remove(node._right, e);
                retNode = node;
            }
            else
            {
                // 待删除节点左子树为空的情况
                if (node._left == null)
                {
                    Node rightNode = node._right;
                    node._right = null;
                    _count--;
                    retNode = rightNode;
                }
                // 待删除节点右子树为空的情况
                if (node._right == null)
                {
                    Node leftNode = node._left;
                    node._left = null;
                    _count--;
                    retNode = leftNode;
                }
                // 待删除节点左右子树均不为空的情况
                Node successor = Min(node._right);
                successor._right = Remove(node._right, e);
                successor._left = node._left;

                node._left = node._right = null;

                retNode = successor;
            }

            if (retNode == null)
                return null;

            // 更新height
            retNode._height = 1 + Math.Max(GetHeight(retNode._left), GetHeight(retNode._right));

            // 计算平衡因子
            int balanceFactor = GetBalanceFactor(retNode);

            // 平衡维护
            // LL
            if (balanceFactor > 1 && GetBalanceFactor(retNode._left) >= 0)
                return RightRotate(retNode);

            // RR
            if (balanceFactor < -1 && GetBalanceFactor(retNode._right) <= 0)
                return LeftRotate(retNode);

            // LR
            if (balanceFactor > 1 && GetBalanceFactor(retNode._left) < 0)
            {
                retNode._left = LeftRotate(retNode._left);
                return RightRotate(retNode);
            }

            // RL
            if (balanceFactor < -1 && GetBalanceFactor(retNode._right) > 0)
            {
                retNode._right = RightRotate(retNode._right);
                return LeftRotate(retNode);
            }

            return retNode;
        }
    }
}
