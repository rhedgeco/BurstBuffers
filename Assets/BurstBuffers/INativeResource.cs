namespace BurstBuffers
{
    public unsafe interface INativeResource
    {
        public bool Allocated { get; }
        
        internal void* Pointer { get; }
        internal long NativeSize { get; }

        internal void ReleaseResource();
    }
}