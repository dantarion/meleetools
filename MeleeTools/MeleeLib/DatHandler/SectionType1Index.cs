using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType1Index : IData, IFilePiece, IEnumerable {
        public File File { get; private set; }
        public SectionType1Index(File file) {
            File = file;
        }
        public uint Count { get { return File.Header.SectionType1Count; } }
        public IEnumerator<SectionType1Header> GetEnumerator() { for (var i = 0; i < Count; i++) yield return this[i]; }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public SectionType1Header this[int i] {
            get {
                if (i > Count) throw new IndexOutOfRangeException();
                return new SectionType1Header(File, i);
            }
        }
        public ArraySlice<byte> RawData {
            get { return File.DataSection.Slice((int)File.Header.Datasize + (int)File.Header.OffsetCount * 4, (int)Count * 8); }
        }
    }
}
