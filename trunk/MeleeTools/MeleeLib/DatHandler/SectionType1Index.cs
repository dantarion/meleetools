using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType1Index : DataIndex<SectionType1Header> {
        public sealed override File File { get; protected set; }

        public override int Start {
            get { return (int) (File.DataSection.Offset+ File.Header.Datasize + File.Header.OffsetCount * 4); }
        }

        public override int End {
            get { return Start + SectionType1Header.Size*Count; }
        }

        public SectionType1Index(File file) {
            File = file;
        }
        public override int Count { get { return (int)File.Header.SectionType1Count; } }
        public override SectionType1Header this[int i] { get { return new SectionType1Header(File, i); } }
    }
}
