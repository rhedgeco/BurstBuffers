namespace BurstBuffers
{
    public unsafe class StructBox<T> : NativeBox<T> where T : unmanaged, INativeResource
    {
        /// <summary>
        /// Creates an struct in native memory with default values
        /// </summary>
        public StructBox() => Data = NativeUtils.AllocateItemSpace<T>();

        /// <summary>
        /// Copies an item into native memory
        /// </summary>
        /// <param name="item">item to copy</param>
        public StructBox(in T item)
        {
            Data = NativeUtils.AllocateItemSpace<T>(false);
            *Data = item;
        }

        /// <summary>
        /// Gets or sets the value from the underlying item.
        /// Item is returned as a copy of the original.
        /// </summary>
        public T Item
        {
            get
            {
                EnsureAllocatedAndThrow();
                return *Data;
            }

            set
            {
                EnsureAllocatedAndThrow();
                *Data = value;
            }
        }
    }
}