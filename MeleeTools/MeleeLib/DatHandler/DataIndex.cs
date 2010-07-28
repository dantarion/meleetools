using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public abstract class DataIndex<T>  : IData, IFilePiece, IList<T>{
        public abstract File File { get; protected set; }
        public abstract int Start { get; }
        public abstract int End { get; }
        public int Size { get { return End - Start; } }
        public ArraySlice<byte> RawData { get { return File.RawData.Slice(Start, Size); } }
        public abstract int IndexOf(T item);
        public abstract void Insert(int index, T item);
        public abstract void RemoveAt(int index);
        public abstract T this[int i] { get; set;  }
        public abstract void Add(T item);
        public abstract void Clear();
        public abstract bool Contains(T item);
        public abstract void CopyTo(T[] array, int arrayIndex);
        public abstract bool Remove(T item);
        public abstract int Count { get; }
        public abstract bool IsReadOnly { get; }
        public IEnumerator<T> GetEnumerator() { for (var i = 0; i < Count; i++) yield return this[i]; }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
