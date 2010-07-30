using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace MeleeLib.Utility {
    public class ArraySlice<T> : IEnumerable<T> {
        public ArraySlice(T[] array, int offset, int count) {
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

        public T this[int i] {
            get { return Array[i + Offset]; }
            set { Array[i + Offset] = value; }
        }
        public T[] Array { get; private set; }

        public bool Contains(T item) { return global::System.Array.IndexOf(Array, item) >= 0; }

        public void CopyTo(T[] array, int arrayIndex = 0) { System.Array.Copy(Array, Offset, array, arrayIndex, Count); }
        public int IndexOf(T value, int startIndex = 0) {
            return global::System.Array.IndexOf(Array, value, Offset + startIndex, Count - startIndex) - Offset;
        }
        public int Count { get; private set; }

        public bool IsReadOnly { get { return Array.IsReadOnly; } }

        public int Offset { get; private set; }

        public IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public ArraySlice<T> Slice(int offset = 0) {
            return Slice(offset, Count - offset);
        }
        public ArraySlice<T> Slice(int offset, int count) {
            return new ArraySlice<T>(Array, Offset + offset, count);
        }
    }

    public static class ArraySliceExtension {
        public static ArraySlice<T> Slice<T>(this T[] array, int offset=0) {
            return Slice(array, offset, array.Length - offset);
        }
        public static ArraySlice<T> Slice<T>(this T[] array, int offset, int count) {
            return new ArraySlice<T>(array, offset, count);
        }

    }
    public static class ByteArraySLiceExtension {
        public static Int16 GetInt16(this ArraySlice<byte> arraySlice, int offset = 0, bool bigEndian = false) {
            var value = BitConverter.ToInt16(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        public static UInt16 GetUInt16(this ArraySlice<byte> arraySlice, int offset = 0, bool bigEndian = false) {
            var value = BitConverter.ToUInt16(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        public static Int32 GetInt32(this ArraySlice<byte> arraySlice, int offset = 0, bool bigEndian = false) {
            var value = BitConverter.ToInt32(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        
        public static UInt32 GetUInt32(this ArraySlice<byte> arraySlice, int offset = 0, bool bigEndian = false) {
            var value = BitConverter.ToUInt32(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        public static Single GetSingle(this ArraySlice<byte> arraySlice, int offset = 0, bool bigEndian = false) {
            var value = BitConverter.ToSingle(arraySlice.Array, arraySlice.Offset + offset);
            return BitConverter.IsLittleEndian && bigEndian ? value : value.Reverse();
        }
        public static string GetAsciiString(this ArraySlice<byte> arraySlice, int offset = 0) {
            var endIndex = arraySlice.IndexOf(0, offset);
            var newSlice = arraySlice.Slice(offset, endIndex - offset);
            return Encoding.UTF7.GetString((newSlice).ToArray());
        }
        public static bool GetBoolean(this ArraySlice<byte> arraySlice, int offset=0, int bitOffset = 0) {
            return Convert.ToBoolean(arraySlice[offset] >> bitOffset);
        }
    }
}

