using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib.DatHandler {
    public class ScriptIndex : DataIndex<ScriptCommand> {
        public sealed override File File { get; protected set; }
        public readonly Subaction Subaction;
        public ScriptIndex(File file, Subaction subaction) { File = file; Subaction = subaction; }

        public override int Start {
            get { return File.DataSection.Offset + Subaction.ScriptOffset; }
        }

        public override int End {
            get {
                throw new NotImplementedException();
            }
        }
        #region Not Implemented
        public override int IndexOf(ScriptCommand item) {
            throw new NotImplementedException();
        }

        public override void Insert(int index, ScriptCommand item) {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public override ScriptCommand this[int i] {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void Add(ScriptCommand item) {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool Contains(ScriptCommand item) {
            throw new NotImplementedException();
        }

        public override void CopyTo(ScriptCommand[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public override bool Remove(ScriptCommand item) {
            throw new NotImplementedException();
        }

        public override int Count {
            get { throw new NotImplementedException(); }
        }

        public override bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
