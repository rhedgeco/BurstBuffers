using System.Collections;
using System.Collections.Generic;

namespace BurstBuffers
{
    public partial class BufferBox<T> where T : unmanaged
    {
        private Readonly _readonlyCache;

        public Readonly GetReadonly()
        {
            if (_readonlyCache != null) return _readonlyCache;
            _readonlyCache = new Readonly(this);
            return _readonlyCache;
        }
        
        /// <summary>
        /// Provides readonly access to data within a BufferBox
        /// </summary>
        public class Readonly : IEnumerable<T>
        {
            private BufferBox<T> _bufferBox;

            public int Length => _bufferBox.Length;
            public bool Allocated => _bufferBox.Allocated;

            internal Readonly(BufferBox<T> bufferBox) => _bufferBox = bufferBox;

            public T this[int index] => _bufferBox[index];

            public void CopyTo<T2>(BufferBox<T2> destination) where T2 : unmanaged => _bufferBox.CopyTo(destination);
            public void CopyTo<T2>(T2[] destination) where T2 : unmanaged => _bufferBox.CopyTo(destination);

            public ref BurstArray<T> GetUnsafeRef() => ref _bufferBox.GetUnsafeRef();

            public IEnumerator<T> GetEnumerator() => _bufferBox.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}