using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SubactionIndex : DataIndex<SubactionHeader> {
        public override sealed File File { get; protected set; }
        public override int Start { get { return (int)File.FtHeader.SubactionStart; } }
        public override int End { get { return (int)File.FtHeader.SubactionEnd; } }
        public override SubactionHeader this[int i] {
            get { return new SubactionHeader(File, i); }
        }

        public override int Count { get { return Size / SubactionHeader.Size; } }
        public SubactionIndex(File file) { File = file; }
        private SubactionIndex() { }
    }
}
