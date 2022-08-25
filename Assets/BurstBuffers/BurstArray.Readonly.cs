using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace BurstBuffers
{
    public unsafe partial struct BurstArray<T> where T : unmanaged
    {
        public Readonly GetReadonly()
        {
            EnsureAllocatedAndThrow(this);
            fixed (BurstArray<T>* ptr = &this)
            {
                return new Readonly(ptr);
            }
        }
        
        public struct Readonly
        {
            internal BurstArray<T>* BurstArray;

            public int Length => BurstArray->Length;
            public bool Allocated => !IsNull && BurstArray->Allocated;
            public bool IsNull => (IntPtr) BurstArray == IntPtr.Zero;

            internal Readonly(BurstArray<T>* burstArray) => BurstArray = burstArray;

            public T this[int index] => (*BurstArray)[index];

            public void CopyTo<T2>(BurstArray<T2> destination) where T2 : unmanaged => BurstArray->CopyTo(destination);

            public void CopyTo<T2>(T2[] destination) where T2 : unmanaged => BurstArray->CopyTo(destination);

            public Enumerator GetEnumerator() => BurstArray->GetEnumerator();
        }
    }
}