namespace _2.ReformatYourOwnCode
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class GenericListReformated<T> : IEnumerable<T>
    {
        private const int InitialCapacity = 16;

        private const string IndexOutOfRangeMessage = "Index is out of range";

        private int capacity;

        private T[] internalArray;

        public GenericListReformated(int capacity = InitialCapacity)
        {
            this.Capacity = capacity;
            this.internalArray = new T[InitialCapacity];
            this.Count = 0;
        }
        
        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value <= 0)
                {
                    value = InitialCapacity;
                }

                this.capacity = value;
            }
        }

        public int Count { get; private set; }
        
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.internalArray[i];
            }
        }

        public void Add(T element)
        {
            if (this.Count >= this.Capacity)
            {
                this.IncreaseCapacity();
            }

            this.internalArray[this.Count] = element;
            this.Count++;
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this.internalArray[i] = default(T);
            }

            this.Count = 0;
        }

        public bool Contains(T element)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.internalArray[i].Equals(element))
                {
                    return true;
                }
            }

            return false;
        }

        public int FindIndex(T element)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.internalArray[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T element)
        {
            if (this.Count == index)
            {
                this.Add(element);
                return;
            }

            this.ValidateIndex(index);

            if (this.Count >= this.Capacity - 1)
            {
                this.IncreaseCapacity();
            }

            for (int i = this.Count; i > index; i--)
            {
                this.internalArray[i] = this.internalArray[i - 1];
            }

            this.internalArray[index] = element;
            this.Count++;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);

            for (int i = index; i < this.Count; i++)
            {
                this.internalArray[i] = this.internalArray[i + 1];
            }

            this.Count--;
            this.internalArray[this.Count] = default(T);
        }

        public override string ToString() => $"[{(this.Count > 0 ? string.Join(", ", this) : "")}]";

        private void IncreaseCapacity()
        {
            this.capacity = this.internalArray.Length * 2;
            var newInternalArray = new T[this.capacity];
            for (int i = 0; i < this.internalArray.Length; i++)
            {
                newInternalArray[i] = this.internalArray[i];
            }

            this.internalArray = newInternalArray;
        }

        private void ValidateIndex(int index)
        {
            if (index >= this.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), IndexOutOfRangeMessage);
            }
        }
    }
}