using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PriorityQueue<T>
{
    private List<Pair<int, T>> data;

    public PriorityQueue()
    {
        this.data = new List<Pair<int, T>>();
    }

    public void Enqueue(int priority, T item)
    {
        this.data.Add(new Pair<int, T>(priority, item));
        int childIndex = this.data.Count - 1;
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (this.data[childIndex].First < this.data[parentIndex].First)
            {
                Pair<int, T> tmp = this.data[childIndex];
                this.data[childIndex] = this.data[parentIndex];
                this.data[parentIndex] = tmp;
                childIndex = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    public T Dequeue()
    {
        int lastIndex = this.data.Count - 1;
        Pair<int, T> frontItem = this.data[0];
        this.data[0] = this.data[lastIndex];
        this.data.RemoveAt(lastIndex);

        lastIndex--;

        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            int rightChildIndex = parentIndex * 2 + 2;
            if (leftChildIndex > lastIndex)
            {
                break;
            }
            int minIndex = leftChildIndex;
            if (rightChildIndex <= lastIndex && this.data[rightChildIndex].First < this.data[leftChildIndex].First)
            {
                minIndex = rightChildIndex;
            }
            if (this.data[minIndex].First < this.data[parentIndex].First)
            {
                Pair<int, T> tmp = this.data[parentIndex];
                this.data[parentIndex] = this.data[minIndex];
                this.data[minIndex] = tmp;
                parentIndex = minIndex;
            }
            else
            {
                break;
            }
        }
        return frontItem.Second;
    }

    public int Count()
    {
        return this.data.Count;
    }

    public bool IsEmpty()
    {
        return this.data.Count == 0;
    }

    private class Pair<TFirst, TSecond>
    {
        public Pair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }

        public TFirst First { get; set; }
        public TSecond Second { get; set; }
    }
}
