using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public class FtDefinition : IDefinition {
        private FtDefinition() { }
        internal FtDefinition(int offset, int size, int dataOffset, int stringOffset) {
            Offset = offset;
            Size = size;
            DataOffset = dataOffset;
            StringOffset = stringOffset;
        }
        public int DataOffset { get; internal set; }
        public int StringOffset { get; internal set; }
        public int Offset { get; internal set; }
        public int Size { get; internal set; }

        public byte[] RawData {
            get { throw new NotImplementedException(); }
        }
    }
    public class FtHeader : IHeader {
        private FtHeader() { }
        internal FtHeader(ArraySlice<byte> rawData) {
            Debug.Assert(rawData.Count == 0x60);
        }

        public FtHeader(int offset, int size,
                        int attributesStart, int attributesEnd,
                        int unknown1, int subactionStart,
                        int unknown2, int subactionEnd, byte[] values) {
            Offset = offset;
            Size = size;
            AttributesStart = attributesStart;
            AttributesEnd = attributesEnd;
            Unknown1 = unknown1;
            SubactionStart = subactionStart;
            Unknown2 = unknown2;
            SubactionEnd = subactionEnd;
            Values = values;
        }

        public int AttributesStart { get; internal set; }
        public int Unknown1 { get; internal set; }
        public int AttributesEnd { get; internal set; }
        public int SubactionStart { get; internal set; }
        public int Unknown2 { get; internal set; }
        public int SubactionEnd { get; internal set; }
        public byte[] Values { get; internal set; }
        public int Offset { get; internal set; }
        public int Size { get; internal set; }

        public byte[] RawData {
            get { throw new NotImplementedException(); }
        }
    }
    public class FtData : ISection<FtDefinition, FtHeader>, IEnumerable<IFilePiece> {
        private FtData() { }
        internal FtData( FtHeader header,
                        FtDefinition definition, String name,
                        IList<Attribute> attributes, IList<Subaction> subactions) {
            Header = header;
            Definition = definition;
            Name = name;
            Attributes = attributes;
            Subactions = subactions;
        }

        public FtHeader Header { get; internal set; }
        public FtDefinition Definition { get; internal set; }
        public string Name { get; internal set; }
        public IList<Attribute> Attributes { get; internal set; }
        public IList<Subaction> Subactions { get; internal set; }
        public IEnumerator<IFilePiece> GetEnumerator() { throw new NotImplementedException(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}