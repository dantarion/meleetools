using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public abstract class Node<T>
    {
        public abstract T Parent { get; }
        public abstract File File { get; }
        public abstract ArraySlice<byte> RawData { get; }

    }
}
