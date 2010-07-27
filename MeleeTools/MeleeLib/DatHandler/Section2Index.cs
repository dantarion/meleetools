using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class Section2Index : IData, IFilePiece, IEnumerable {
        public File File { get; private set; }
        public Section2Index(File file) {
            File = file;
        }
        public uint Count { get { return File.Header.SectionType2Count; } }
        public IEnumerator<Section2Header> GetEnumerator() { for (var i = 0; i < Count; i++) yield return this[i]; }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public Section2Header this[int i] {
            get {
                if (i > Count) throw new IndexOutOfRangeException();
                return new Section2Header(File, i);
            }
        }
        public ArraySlice<byte> RawData {
            get { return File.DataSection.Slice((int)File.Header.Datasize + (int)File.Header.OffsetCount * 4, (int)Count * 8); }
        }
    }
}
