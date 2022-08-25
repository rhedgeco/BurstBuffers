using System;

namespace BurstBuffers
{
    /// <summary>
    /// Creates and wraps up a native resource allocation to be used safely.
    /// </summary>
    /// <typeparam name="T">Type of the resource that is held.</typeparam>
    public abstract unsafe class NativeBox<T> : IDisposable where T : unmanaged, INativeResource
    {
        /// <summary>
        /// Raw pointer to the native data allocation
        /// </summary>
        protected internal T* Data;

        /// <summary>
        /// Checks if the underlying pointer is valid, and if the native data is allocated.
        /// </summary>
        public bool Allocated => (IntPtr) Data != IntPtr.Zero && Data->Allocated;

        /// <summary>
        /// Converts the underlying native pointer to an unstable reference.
        /// If this box or native data is disposed this reference will produce undefined behaviour,
        /// and is strictly a direct conversion from pointer to ref. :(
        /// This is necessary for the data to be passed into burst compiled methods without requiring `unsafe`.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">Will throw exception if box is disposed, or native data has been disposed.</exception>
        public ref T GetUnsafeRef()
        {
            EnsureAllocatedAndThrow();
            return ref *Data;
        }

        /// <summary>
        /// Ensures buffer is allocated and throws an exception if not.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Buffer is not allocated.</exception>
        protected internal void EnsureAllocatedAndThrow()
        {
            if (!Allocated) throw new ObjectDisposedException($"{typeof(T)}");
        }

        ~NativeBox()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            if (!Allocated) return;
            Data->ReleaseResource();
            Data = (T*) IntPtr.Zero;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}