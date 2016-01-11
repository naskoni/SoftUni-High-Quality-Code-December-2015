using System;
using System.Collections;
using System.Collections.Generic;

namespace _2.ReformatYourOwnCode
{
    public class GenericList<T> : IEnumerable<T>
    {
        private const int InitialCapacity = 16;
        private const string IndexOutOfRangeMessage = "Index is out of range";

        private int capacity;
        private int currentIndex;
        private T[] internalArray;

        public GenericList(int capacity = InitialCapacity)
        {
            this.Capacity = capacity;
            this.internalArray = new T[InitialCapacity];
            this.currentIndex = 0;
        }

        public T this[int index]
        {
            get
            {
                this.ValidateIndex(index);
                return this.internalArray[index];
            }

            set
            {
                this.ValidateIndex(index);
                this.internalArray[index] = value;
            }
        }

        public int Capacity
        {
            get { return this.capacity; }

            set
            {
                if (value <= 0)
                {
                    value = InitialCapacity;
                }

                this.capacity = value;
            }
        }

        public int Count
        {
            get { return this.currentIndex; }
        }

        public void Add(T element)
        {
            if (this.currentIndex >= this.Capacity)
            {
                this.IncreaseCapacity();
            }

            this.internalArray[this.currentIndex] = element;
            this.currentIndex++;
        }

        public void Clear()
        {
            for (int i = 0; i < this.currentIndex; i++)
            {
                this.internalArray[i] = default(T);
            }

            this.currentIndex = 0;
        }

        public bool Contains(T element)
        {
            for (int i = 0; i < this.currentIndex; i++)
            {
                if (internalArray[i].Equals(element))
                {
                    return true;
                }
            }

            return false;
        }

        public int FindIndex(T element)
        {
            for (int i = 0; i < this.currentIndex; i++)
            {
                if (internalArray[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T element)
        {
            if (this.currentIndex == index)
            {
                this.Add(element);
                return;
            }

            this.ValidateIndex(index);

            if (this.currentIndex >= this.Capacity - 1)
            {
                this.IncreaseCapacity();
            }

            for (int i = this.currentIndex; i > index; i--)
            {
                this.internalArray[i] = this.internalArray[i - 1];
            }

            this.internalArray[index] = element;
            this.currentIndex++;

        }
        
        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);

            for (int i = index; i < this.currentIndex; i++)
            {
                this.internalArray[i] = this.internalArray[i + 1];
            }

            this.currentIndex--;
            this.internalArray[this.currentIndex] = default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.currentIndex; i++)
            {
                yield return this.internalArray[i];
            }
        }

        public override string ToString() => 
            $"[{(this.Count > 0 ? string.Join(", ", this) : "")}]";
        private void IncreaseCapacity()
        {
            this.capacity = this.internalArray.Length * 2;
            var newInternalArray = new T[this.capacity];
            for (int i = 0; i < this.internalArray.Length; i++)
            {
                newInternalArray[i] = internalArray[i];
            }
            this.internalArray = newInternalArray;
        }
        private void ValidateIndex(int index)
        {
            if (index >= this.currentIndex || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), IndexOutOfRangeMessage);
            }
            ;
        }
    }
}
