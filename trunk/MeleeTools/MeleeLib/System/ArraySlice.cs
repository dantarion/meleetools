using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace MeleeLib.System
{
    public class ArraySlice<T> : IEnumerable<T>
    {
        public ArraySlice(T[] array, int offset, int count)
        {
            Array = array;
            Offset = offset;
            Count = count;
            if (Offset < 0 || Offset > Array.Length || Count < 1) throw new IndexOutOfRangeException();
        }

        public ArraySlice(T[] array) : this(array, 0, array.Length) { }
        public ArraySlice(ArraySlice<T> arraySlice) : this(arraySlice.Array, arraySlice.Offset, arraySlice.Count) { }
        public ArraySlice(ArraySlice<T> arraySlice, int offset) : this(arraySlice.Array, arraySlice.Offset + offset, arraySlice.Count - offset) { }
        public ArraySlice(ArraySlice<T> arraySlice, int offset, int count) : this(arraySlice.Array, arraySlice.Offset + offset, count) { }
        private ArraySlice() { }

        public T this[int i]
        {
            get { return Array[i + Offset]; }
            set { Array[i + Offset] = value; }
        }
        public T[] Array { get; private set; }

        public bool Contains(T item) { return global::System.Array.IndexOf(Array, item) >= 0; }

        public void CopyTo(T[] array, int arrayIndex = 0) { global::System.Array.Copy(Array, Offset, array, arrayIndex, Count); }
        public T[] ToArray()
        {
            T[] array = new T[Count];
            CopyTo(array);
            return array;
        }
        public int IndexOf(T value)
        {
            return global::System.Array.IndexOf(Array, value, Offset, Count);
        }
        public int Count { get; private set; }

        public bool IsReadOnly { get { return Array.IsReadOnly; } }

        public int Offset { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public ArraySlice<T> Slice()
        {
            return Slice(0);
        }
        public ArraySlice<T> Slice(int offset)
        {
            return Slice(offset, Count - offset);
        }
        public ArraySlice<T> Slice(int offset, int count){
            return new ArraySlice<T>(Array, Offset + offset, count);
        }
    }

    public static class ArraySliceExtension
    {
        public static ArraySlice<T> Slice<T>(this T[] array)
        {
            return Slice(array, 0);
        }
        public static ArraySlice<T> Slice<T>(this T[] array, int offset)
        {
            return Slice(array, offset, array.Length - offset);
        }
        public static ArraySlice<T> Slice<T>(this T[] array, int offset, int count )
        {
            return new ArraySlice<T>(array, offset, count);
        }

    }
    public static class ByteArraySLiceExtension
    {
        public static Int32 GetInt32(this ArraySlice<byte> arraySlice, int offset, bool bigEndian = false)
        {
            var value = BitConverter.ToInt32(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value :  value.Reverse();
        }

        public static UInt32 GetUInt32(this ArraySlice<byte> arraySlice, int offset, bool bigEndian = false)
        {
            var value = BitConverter.ToUInt32(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        public static string GetAsciiString(this ArraySlice<byte> arraySlice, int offset)
        {
            return Encoding.ASCII.GetString(arraySlice.Slice(offset, arraySlice.IndexOf(0)).ToArray());
        }
    }
}

