using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType1Index : DataIndex<SectionType1Header> {
        public sealed override File File { get; protected set; }

        public override int Start {
            get { return (int)(File.DataSection.Offset + File.Header.Datasize + File.Header.OffsetCount * 4); }
        }

        public override int End {
            get { return Start + SectionType1Header.Size * Count; }
        }

        public SectionType1Index(File file) {
            File = file;
        }
        #region Not Implemented
        public override bool Remove(SectionType1Header item) {
            throw new NotImplementedException();
        }

        public override int Count { get { return (int)File.Header.SectionType1Count; } }

        public override bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public override int IndexOf(SectionType1Header item) {
            throw new NotImplementedException();
        }

        public override void Insert(int index, SectionType1Header item) {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public override SectionType1Header this[int i] {
            get { return new SectionType1Header(File, i); }
            set { throw new NotImplementedException(); }
        }
        public override void Add(SectionType1Header item) {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool Contains(SectionType1Header item) {
            throw new NotImplementedException();
        }

        public override void CopyTo(SectionType1Header[] array, int arrayIndex) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
