using System;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public class SubactionIndex : DataIndex<Subaction> {
        public override sealed File File { get; protected set; }
        public override int Start { get { return (int)File.FtHeader.SubactionStart; } }
        public override int End { get { return (int)File.FtHeader.SubactionEnd; } }
        public override int IndexOf(Subaction item) {
            throw new NotImplementedException();
        }

        public override void Insert(int index, Subaction item) {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public override Subaction this[int i] {
            get { return new Subaction(File, i); }
            set { throw new NotImplementedException(); }
        }

        public override void Add(Subaction item) {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool Contains(Subaction item) {
            throw new NotImplementedException();
        }

        public override void CopyTo(Subaction[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public override bool Remove(Subaction item) {
            throw new NotImplementedException();
        }

        public override int Count { get { return Size / Subaction.Size; } }

        public override bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public SubactionIndex(File file, ArraySlice<byte> rawData) { File = file; }
        private SubactionIndex() { }
    }
}
