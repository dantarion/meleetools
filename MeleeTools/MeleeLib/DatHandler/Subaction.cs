using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public class Subaction : IEnumerable<IFilePiece>, IHasDefinition<SubactionDefinition>, IHasIndex {
        internal Subaction(int index, string name, IList<ScriptCommand> script, SubactionDefinition definition) {
            Index = index;
            Name = name;
            Script = script;
            Definition = definition;
        }

        public SubactionDefinition Definition { get; internal set; }
        public int Index { get; internal set; }
        public string Name { get; internal set; }
        public string ShortName { get { var split = NameSplit; return split != null ? split[3] : null; } }
        public string[] NameSplit { get { return Name != null ? Name.Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries) : null; } }
        public IList<ScriptCommand> Script { get; internal set; }
        public IEnumerator<IFilePiece> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }
    public class SubactionDefinition : IDefinition {
        internal SubactionDefinition(int offset, int size, int stringOffset, int unknown1, int unknown2, int dataOffset, int unknown3, int unknown4) {
            Offset = offset;
            Size = size;
            StringOffset = stringOffset;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
            DataOffset = dataOffset;
            Unknown3 = unknown3;
            Unknown4 = unknown4;
        }

        public int Offset { get; internal set; }
        public int Size { get; internal set; }

        public byte[] RawData {
            get { throw new NotImplementedException(); }
        }

        public int StringOffset { get; internal set; }
        public int Unknown1 { get; internal set;  }
        public int Unknown2 {  get; internal set; }
        public int DataOffset {  get; internal set; }
        public int Unknown3 {  get; internal set; }
        public int Unknown4 { get; internal set; }
    }
}