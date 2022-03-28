using System;

namespace HDA.DataStructure
{
    internal class HTrie
    {
        private class Node
        {
            public bool _isWord;
            public HDictionary<char, Node> _next;

            public Node(bool isWord)
            {
                _isWord = isWord;
                _next = new HDictionary<char, Node>();
            }
            public Node() : this(false) { }
        }

        private Node _root;
        private int _count;

        public int Count => _count;

        public HTrie()
        {
            _root = new Node();
            _count = 0;
        }

        // 添加新的单词
        public void Add(string word)
        {
            Node cur = _root;
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (!cur._next.Contains(c))
                    cur._next.Add(c, new Node());
                cur = cur._next[c];
            }
            if (!cur._isWord)
            {
                cur._isWord = true;
                _count++;
            }
        }

        // 删除单词
        public void Remove(string word)
        {
            Node cur = _root;
            Node last = _root;
            char lastChar = ' ';
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (!cur._next.Contains(c))
                    return;
                if (cur._next.Count > 1 || cur._isWord)
                {
                    last = cur;
                    if (i + 1 < word.Length)
                        lastChar = word[i + 1];
                }
                cur = cur._next[c];
            }
            if (cur._isWord && !cur._next.IsEmpty)
                cur._isWord = false;
            else if (cur._isWord && cur._next.IsEmpty)
                last._next.Remove(lastChar);
        }

        // 查询是否存在单词word
        public bool Contains(string word)
        {
            Node cur = _root;
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (!cur._next.Contains(c))
                    return false;
                cur = cur._next[c];
            }
            return cur._isWord;
        }

        // 查询是否在Trie中存在前缀prefix
        public bool IsPrefix(string prefix)
        {
            Node cur = _root;
            for (int i = 0; i < prefix.Length; i++)
            {
                char c = prefix[i];
                if (!cur._next.Contains(c))
                    return false;
                cur = cur._next[c];
            }
            return true;
        }
    }
}
