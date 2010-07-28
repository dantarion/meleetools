using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType2Index : DataIndex<SectionType2Header> {
        public sealed override File File { get; protected set; }

        public override int Start {
            get { throw new NotImplementedException(); }
        }

        public override int End {
            get { throw new NotImplementedException(); }
        }

        public SectionType2Index(File file) {
            File = file;
        }
        public override int Count { get { return (int)File.Header.SectionType2Count; } }
        public override SectionType2Header this[int i] {
            get {
                if (i > Count) throw new IndexOutOfRangeException();
                return new SectionType2Header(File, i);
            }
        }
    }
}
