using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType1Index : LinkedIndex<SectionType1Header> {
        public sealed override File File { get; protected set; }

        public override int Start {
            get { throw new NotImplementedException(); }
        }

        public override int End {
            get { throw new NotImplementedException(); }
        }

        public SectionType1Index(File file) {
            File = file;
        }
        public override int Count { get { return (int)File.Header.SectionType1Count; } }
        public override SectionType1Header this[int i] {
            get {
                if (i > Count) throw new IndexOutOfRangeException();
                return new SectionType1Header(File, i);
            }
        }
    }
}
