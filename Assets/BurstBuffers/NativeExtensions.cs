using System;

namespace BurstBuffers
{
    public static unsafe class NativeExtensions
    {
        /// <summary>
        /// Inserts readonly data into a writable buffer box.
        /// Allows for nested readonly structures.
        /// </summary>
        /// <param name="bufferBox">buffer to insert data into</param>
        /// <param name="index">index of buffer to place the readonly data</param>
        /// <param name="data">data array source</param>
        /// <typeparam name="T">type of data to be inserted</typeparam>
        public static void Insert<T>(this BurstArray<BurstArray<T>.Readonly> burstArray, int index, T[] data)
            where T : unmanaged
        {
            if (data == null) throw new NullReferenceException($"'{nameof(data)}' cannot be null");
            
            // check if data is allocated there already, and if so release it
            ref BurstArray<T>.Readonly bufferRef = ref burstArray[index];
            if ((IntPtr) bufferRef.BurstArray != IntPtr.Zero)
                ((INativeResource) (*bufferRef.BurstArray)).ReleaseResource();

            // assign new data to the readonly array
            BurstArray<T>* arrayPointer = BurstArray<T>.Allocate(data);
            bufferRef.BurstArray = arrayPointer;
        }
    }
}