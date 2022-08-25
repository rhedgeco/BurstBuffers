namespace BurstBuffers
{
    public unsafe partial class BufferBox<T> : NativeBox<BurstArray<T>> where T : unmanaged
    {
        /// <summary>
        /// Creates a buffer of a specified length
        /// </summary>
        /// <param name="length">length of the buffer</param>
        public BufferBox(int length) => Data = BurstArray<T>.Allocate(length);

        /// <summary>
        /// Creates a buffer with data copied from an existing array
        /// </summary>
        /// <param name="data">data to copy</param>
        public BufferBox(T[] data) => Data = BurstArray<T>.Allocate(data);
        
        /// <summary>
        /// Gets the length of the underlying buffer
        /// </summary>
        public int Length
        {
            get
            {
                EnsureAllocatedAndThrow();
                return Data->Length;
            }
        }
        
        /// <summary>
        /// Gets or sets the data located at a given index in the buffer.
        /// Exposed as a copy of the original value.
        /// </summary>
        /// <param name="index">index of the data</param>
        public T this[int index]
        {
            get
            {
                EnsureAllocatedAndThrow();
                return (*Data)[index];
            }

            set
            {
                EnsureAllocatedAndThrow();
                (*Data)[index] = value;
            }
        }

        /// <summary>
        /// Raw copy buffer to a buffer destination.
        /// Destination data type may differ.
        /// </summary>
        /// <param name="destination">Destination buffer</param>
        /// <typeparam name="T2">Destination data type</typeparam>
        public void CopyTo<T2>(BufferBox<T2> destination) where T2 : unmanaged => Data->CopyTo(*destination.Data);
        
        /// <summary>
        /// Raw copy buffer to a managed array destination.
        /// Destination data type may differ.
        /// </summary>
        /// <param name="destination">Destination array</param>
        /// <typeparam name="T2">Destination data type</typeparam>
        public void CopyTo<T2>(T2[] destination) where T2 : unmanaged => Data->CopyTo(destination);
        
        /// <summary>
        /// Raw copy data from a managed array into this buffer.
        /// Source array data type may differ.
        /// </summary>
        /// <param name="source">Source array</param>
        /// <typeparam name="T2">Source data type</typeparam>
        public void CopyFrom<T2>(T2[] source) where T2 : unmanaged => Data->CopyFrom(source);

        /// <summary>
        /// Clears the data from the buffer, and disposes native children if any.
        /// </summary>
        public void Clear() => Data->Clear();
    }
}