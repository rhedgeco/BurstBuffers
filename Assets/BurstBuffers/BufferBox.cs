namespace BurstBuffers
{
    public unsafe class BufferBox<T> : NativeBox<BurstArray<T>> where T : unmanaged
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
        /// Clears the data from the buffer, and disposes native children if any.
        /// </summary>
        public void Clear() => Data->Clear();
    }
}