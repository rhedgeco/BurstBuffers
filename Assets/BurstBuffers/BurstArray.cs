using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace BurstBuffers
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct BurstArray<T> : INativeResource where T : unmanaged
    {
        private T* _pointer;
        private long _nativeSize;

        void* INativeResource.Pointer => _pointer;
        long INativeResource.NativeSize => _nativeSize;

        /// <summary>
        /// The length of the underlying buffer
        /// </summary>
        public int Length { get; private set; }
        
        /// <summary>
        /// Checks if the native data is allocated
        /// </summary>
        public bool Allocated => (IntPtr) _pointer != IntPtr.Zero;

        internal static BurstArray<T>* Allocate(int length)
        {
            BurstArray<T>* ptr = NativeUtils.AllocateItemSpace<BurstArray<T>>(false);
            ptr->_pointer = NativeUtils.CreateNativeBuffer<T>(length);
            ptr->_nativeSize = length * sizeof(T);
            ptr->Length = length;
            return ptr;
        }
        
        internal static BurstArray<T>* Allocate(T[] data)
        {
            BurstArray<T>* ptr = NativeUtils.AllocateItemSpace<BurstArray<T>>(false);
            ptr->_pointer = NativeUtils.CopyToNativeBuffer(data);
            ptr->_nativeSize = data.Length * sizeof(T);
            ptr->Length = data.Length;
            return ptr;
        }

        /// <summary>
        /// Gets a reference to data stored at given index
        /// </summary>
        /// <param name="index">index of data</param>
        /// <exception cref="IndexOutOfRangeException">The index is out of range of buffer size</exception>
        /// <exception cref="ObjectDisposedException">The native buffer has been disposed</exception>
        public ref T this[int index]
        {
            get
            {
                if (!Allocated) throw new ObjectDisposedException($"NativeBuffer<{typeof(T)}>");
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException($"index '{index}' is out of range for range [0..{Length}]");
                return ref *(T*) ((long) _pointer + index * sizeof(T));
            }
        }

        /// <summary>
        /// Clears the data from the buffer, and disposes native children if any.
        /// </summary>
        public void Clear()
        {
            if (!Allocated) return;
            TryReleaseNativeChildren();
            UnsafeUtility.MemClear(_pointer, _nativeSize);
        }

        void INativeResource.ReleaseResource()
        {
            if (!Allocated) return;
            TryReleaseNativeChildren();
            _pointer = (T*) IntPtr.Zero;
            _nativeSize = 0;
            Length = 0;
        }

        private void TryReleaseNativeChildren()
        {
            if (!typeof(T).IsAssignableFrom(typeof(INativeResource))) return;
            foreach (ref T item in GetRefEnumerator()) ((INativeResource)item).ReleaseResource();
        }
    }
}