using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private T value;
        private int weight = 1;
        private BinaryTree<T> left;
        private BinaryTree<T> right;
        private bool isInitialized;

        public void Add(T key)
        {
            if (!isInitialized)
            {
                value = key;
                isInitialized = true;
                return;
            }

            var parentNode = this;
            AddToNode(parentNode, key);
        }

        private void AddToNode(BinaryTree<T> parentNode, T key)
        {
            while (true)
            {
                parentNode.weight++;

                parentNode = (parentNode.value.CompareTo(key) > 0) 
                    ? parentNode.left ?? (parentNode.left = new BinaryTree<T>() {value = key, isInitialized = true}) 
                    : parentNode.right ?? (parentNode.right = new BinaryTree<T>() {value = key, isInitialized = true});

                if (parentNode.value.Equals(key)) break;
            }
        }

        public bool Contains(T key)
        {
            if (!isInitialized)
                return false;
    
            var parentNodeToAdd = this;
            while (parentNodeToAdd != null)
            {
                var compareResult = parentNodeToAdd.value.CompareTo(key);
                if (compareResult == 0)
                    return true;
                parentNodeToAdd = compareResult > 0 ? parentNodeToAdd.left : parentNodeToAdd.right;
            }
            return false;
        }

        public T this[int i]
        {
            get
            {
                if (i < 0 || i >= weight)
                    throw new ArgumentOutOfRangeException(nameof(i));
        
                var root = this;
                while (true)
                {
                    var currentNodeIndex = (root!.left?.weight ?? 0) + 1;
                    if (i == currentNodeIndex - 1)
                        return root.value;
                    if (i < currentNodeIndex - 1)
                        root = root.left;
                    else
                    {
                        root = root.right;
                        i -= currentNodeIndex;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => GetEnumeratorForNode(this);

        private IEnumerator<T> GetEnumeratorForNode(BinaryTree<T> root)
        {
            if (root is not { isInitialized: true })
                yield break;
            var enumeratorForTreeNode = GetEnumeratorForNode(root.left);
            while (enumeratorForTreeNode.MoveNext())
                yield return enumeratorForTreeNode.Current;
            yield return root.value;
            enumeratorForTreeNode = GetEnumeratorForNode(root.right);
            while (enumeratorForTreeNode.MoveNext())
                yield return enumeratorForTreeNode.Current;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}