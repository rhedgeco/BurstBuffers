using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace BurstBuffers
{
    /// <summary>
    /// A collection of utilities for allocating native data
    /// </summary>
    public static unsafe class NativeUtils
    {
        /// <summary>
        /// Allocates space necessary to hold an unmanaged object
        /// </summary>
        /// <param name="clear">clear the allocated memory</param>
        /// <typeparam name="T">Type of data to allocate for</typeparam>
        /// <returns>Pointer to the allocated space</returns>
        public static T* AllocateItemSpace<T>(bool clear = true) where T : unmanaged
        {
            void* ptr = UnsafeUtility.Malloc(sizeof(T), UnsafeUtility.AlignOf<T>(), Allocator.Persistent);
            if (clear) UnsafeUtility.MemClear(ptr, sizeof(T));
            return (T*) ptr;
        }
        
        /// <summary>
        /// Allocates space necessary to hold an array of unmanaged objects
        /// </summary>
        /// <param name="length">length of the array</param>
        /// <param name="clear">clear the allocated memory</param>
        /// <typeparam name="T">Type of data to allocate the array for</typeparam>
        /// <returns>Pointer to the allocated space</returns>
        public static T* CreateNativeBuffer<T>(int length, bool clear = true) where T : unmanaged
        {
            long size = length * sizeof(T);
            void* ptr = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<T>(), Allocator.Persistent);
            if (clear) UnsafeUtility.MemClear(ptr, size);
            return (T*) ptr;
        }

        /// <summary>
        /// Copies the data in a managed array to unmanaged space
        /// </summary>
        /// <param name="data">Array to copy data from</param>
        /// <typeparam name="T">Type of data contained in the array</typeparam>
        /// <returns>Pointer to the allocated space</returns>
        /// <exception cref="NullReferenceException">Throws exception if the managed array is null</exception>
        public static T* CopyToNativeBuffer<T>(in T[] data) where T : unmanaged
        {
            if (data == null) throw new NullReferenceException($"Cannot create buffer, {nameof(data)} is null");
            T* ptr = CreateNativeBuffer<T>(data.Length, false);
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            UnsafeUtility.MemCpy(ptr, (void*) handle.AddrOfPinnedObject(), data.Length * sizeof(T));
            handle.Free();
            return ptr;
        }
    }
}