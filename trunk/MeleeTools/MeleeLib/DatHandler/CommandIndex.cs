using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib.DatHandler {
    class CommandIndex : DataIndex<ScriptCommand>{
        public sealed override File File { get; protected set; }
        public CommandIndex(File file) { File = file; }

        public override int Start {
            get { throw new NotImplementedException(); }
        }

        public override int End {
            get { throw new NotImplementedException(); }
        }

        public override ScriptCommand this[int i] {
            get { throw new NotImplementedException(); }
        }

        public override int Count {
            get { throw new NotImplementedException(); }
        }
    }
}
