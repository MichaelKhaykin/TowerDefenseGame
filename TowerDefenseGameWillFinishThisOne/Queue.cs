using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Queue<T>
    {
        public T[] array = new T[10];
        int items = 0;

        public void Enqueue(T value)
        {
            if (array.Length == items)
            {
                T[] tempArray = new T[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                {
                    tempArray[i] = array[i];
                }
                array = tempArray;
            }
            array[items] = value;
            items++;
        }

        public T Dequeue(T value)
        {
            items--;
            return array[items];
        }

        public T Peek(T value)
        {
            return array[items];
        }
    }
}
