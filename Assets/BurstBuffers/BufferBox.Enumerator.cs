using System.Collections;
using System.Collections.Generic;

namespace BurstBuffers
{
    public unsafe partial class BufferBox<T> : IEnumerable<T> where T : unmanaged
    {
        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class Enumerator : IEnumerator<T>
        {
            private BufferBox<T> _bufferBox;
            private long _itemPointer;
            private int _itemIndex;

            public T Current
            {
                get
                {
                    _bufferBox.EnsureAllocatedAndThrow();
                    return *(T*) _itemPointer;
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(BufferBox<T> bufferBox)
            {
                bufferBox.EnsureAllocatedAndThrow();
                _bufferBox = bufferBox;
                Reset();
            }

            public bool MoveNext()
            {
                _bufferBox.EnsureAllocatedAndThrow();
                _itemIndex++;
                if (_itemIndex >= _bufferBox.Length) return false;
                _itemPointer += sizeof(T);
                return true;
            }

            public void Reset()
            {
                _bufferBox.EnsureAllocatedAndThrow();
                _itemPointer = (long) ((INativeResource)(*_bufferBox.Data)).Pointer - sizeof(T);
                _itemIndex = -1;
            }

            public void Dispose()
            {
                // do nothing
            }
        }
    }
}