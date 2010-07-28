using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public class SectionType2Index : DataIndex<SectionType2Header> {
        public sealed override File File { get; protected set; }

        public override int Start {
            get { throw new NotImplementedException(); }
        }

        public override int End {
            get { throw new NotImplementedException(); }
        }

        public SectionType2Index(File file, ArraySlice<byte> rawData) {
            File = file;
        }
        #region
        public override bool Remove(SectionType2Header item) {
            throw new NotImplementedException();
        }

        public override int Count { get { return (int)File.Header.SectionType2Count; } }

        public override bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public override int IndexOf(SectionType2Header item) {
            throw new NotImplementedException();
        }

        public override void Insert(int index, SectionType2Header item) {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public override SectionType2Header this[int i] {
            get {
                if (i > Count) throw new IndexOutOfRangeException();
                return new SectionType2Header(File, i);
            }
            set { throw new NotImplementedException(); }
        }

        public override void Add(SectionType2Header item) {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool Contains(SectionType2Header item) {
            throw new NotImplementedException();
        }

        public override void CopyTo(SectionType2Header[] array, int arrayIndex) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
