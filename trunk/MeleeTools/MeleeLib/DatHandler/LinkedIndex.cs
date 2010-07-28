using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public abstract class LinkedIndex<T>  : IData, IFilePiece, IEnumerable<T>{
        public abstract File File { get; protected set; }
        public abstract int Start { get; }
        public abstract int End { get; }
        public int Size { get { return End - Start; } }
        public ArraySlice<byte> RawData { get { return File.RawData.Slice(Start, Size); } }
        public abstract T this[int i] { get; }
        public abstract int Count { get; }
        public IEnumerator<T> GetEnumerator() { for (var i = 0; i < Count; i++) yield return this[i]; }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
