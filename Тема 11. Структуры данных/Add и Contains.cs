using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> where T : IComparable
    {
        private T value;
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
                if (parentNode.value.CompareTo(key) > 0)
                {
                    if (parentNode.left != null)
                        parentNode = parentNode.left;
                    else
                    {
                        parentNode.left = new BinaryTree<T> { value = key, isInitialized = true };
                        break;
                    }
                }
                else
                {
                    if (parentNode.right != null)
                        parentNode = parentNode.right;
                    else
                    {
                        parentNode.right = new BinaryTree<T> { value = key, isInitialized = true };
                        break;
                    }
                }
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
    }
}