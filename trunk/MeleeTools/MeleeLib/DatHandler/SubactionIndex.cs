using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SubactionIndex : LinkedIndex<SubactionHeader> {
        public override sealed File File { get; protected set; }
        public override int Start { get { return (int)File.FtHeader.SubactionStart; } }
        public override int End { get { return (int)File.FtHeader.SubactionEnd; } }

        public override SubactionHeader this[int i] {
            get { throw new NotImplementedException(); }
        }

        public override int Count {
            get { throw new NotImplementedException(); }
        }

        public SubactionIndex(File file) { File = file; }
    }
}
