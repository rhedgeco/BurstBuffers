namespace BurstBuffers
{
    public unsafe partial struct BurstArray<T> : INativeResource where T : unmanaged
    {
        public Enumerator GetEnumerator()
        {
            fixed (BurstArray<T>* ptr = &this)
            {
                return new Enumerator(ptr);
            }
        }
        
        public RefEnumerator GetRefEnumerator()
        {
            fixed (BurstArray<T>* ptr = &this)
            {
                return new RefEnumerator(ptr);
            }
        }

        /// <summary>
        /// Enumerator for NativeBuffer objects.
        /// Doesn't explicitly inherit IEnumerable to be used in burst.
        /// </summary>
        public struct Enumerator
        {
            // just reuse ref enumerator logic
            private RefEnumerator _refEnumerator;

            internal Enumerator(BurstArray<T>* buffer) => _refEnumerator = new RefEnumerator(buffer);

            public T Current => _refEnumerator.Current;

            public bool MoveNext() => _refEnumerator.MoveNext();

            public void Reset() => _refEnumerator.Reset();

            public Enumerator GetEnumerator() => this;
        }

        /// <summary>
        /// Reference Enumerator for NativeBuffer objects.
        /// Doesn't explicitly inherit IEnumerable to be used in burst.
        /// </summary>
        public struct RefEnumerator
        {
            private BurstArray<T>* _buffer;
            private int _maxLength;
            private long _currentItem;
            private int _currentIndex;

            public ref T Current => ref *(T*) _currentItem;

            internal RefEnumerator(BurstArray<T>* buffer)
            {
                _buffer = buffer;
                _maxLength = buffer->Length;
                _currentIndex = -1;
                _currentItem = (long) _buffer->_pointer;
            }

            public bool MoveNext()
            {
                _currentIndex++;
                if (_currentIndex >= _maxLength) return false;
                _currentItem += sizeof(T);
                return true;
            }

            public void Reset()
            {
                _currentIndex = -1;
                _currentItem = (long) _buffer->_pointer;
            }
            
            public RefEnumerator GetEnumerator() => this;
        }
    }
}