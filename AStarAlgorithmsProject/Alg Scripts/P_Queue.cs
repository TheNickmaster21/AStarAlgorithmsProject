using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    public class P_Queue<T>
    {

        private List<KeyValuePair<T, double>> heap;

        public P_Queue()
        {
            heap = new List<KeyValuePair<T, double>>();
        }

        public void Enqueue(T item, double priority)
        {
            heap.Add(new KeyValuePair<T, double>(item, priority));
            HeapifyUp();
            Print();
        }

        public T Dequeue()
        {
            T top = heap[0].Key;

            heap[0] = heap[Count - 1];
            heap.RemoveAt(Count - 1);

            HeapifyDown();
            Print();
            return top;
        }

        public void Print()
        {
            string s = "";
            foreach (KeyValuePair<T, double> p in heap)
            {
                s += (" " + p.Value);
            }
        }

        private void HeapifyDown()
        {
            int currentIndex = 0;
            int lchild = LeftChild(currentIndex);
            int rchild = RightChild(currentIndex);

            int winner = lchild;

            while (lchild < Count)
            {
                if (rchild < Count && heap[rchild].Value < heap[lchild].Value)
                {
                    winner = rchild;
                }

                if (heap[winner].Value < heap[currentIndex].Value)
                {
                    KeyValuePair<T, double> temp = heap[winner];
                    heap[winner] = heap[currentIndex];
                    heap[currentIndex] = temp;

                    currentIndex = winner;
                    lchild = LeftChild(currentIndex);
                    rchild = RightChild(currentIndex);
                    winner = lchild;
                }
                else
                {
                    break;
                }
            }
        }

        private void HeapifyUp()
        {
            int currentIndex = Count - 1;
            int parentIndex = Parent(currentIndex);

            while (currentIndex != parentIndex)
            {
                if (heap[currentIndex].Value < heap[parentIndex].Value)
                {
                    KeyValuePair<T, double> temp = heap[parentIndex];
                    heap[parentIndex] = heap[currentIndex];
                    heap[currentIndex] = temp;
                }
                else
                {
                    break;
                }

                currentIndex = parentIndex;
                parentIndex = Parent(currentIndex);
            }
        }

        private static int LeftChild(int current)
        {
            return 2 * current + 1;
        }

        private static int RightChild(int current)
        {
            return 2 * current + 2;
        }

        private static int Parent(int current)
        {
            if (current == 0)
            {
                return 0;
            }
            else
            {
                return ((current - 1) / 2);
            }
        }

        public int Count
        { get { return heap.Count; } }
    }
}
