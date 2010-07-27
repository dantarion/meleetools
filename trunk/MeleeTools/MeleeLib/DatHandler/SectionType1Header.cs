using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType1Header : IData, IFilePiece {
        public const int Length = 0x8;
        public readonly int Index;
        public SectionType1Data Data { get { return new SectionType1Data(File); } }
        public string Name { get { return File.DataSection.GetAsciiString((int)(File.Header.StringOffsetBase + StringOffset)); } }
        public File File { get; private set; }
        private SectionType1Header() {}
        public SectionType1Header(File file, int index) {
            if (file.SectionType1Index.Count < index) throw new IndexOutOfRangeException();
            File = file;
            Index = index;
        }
        public uint StringOffset { get { return RawData.GetUInt32(0x04); } }
        public uint DataOffset { get { return RawData.GetUInt32(0x00); } }
        public ArraySlice<byte> RawData { get { return File.SectionType1Index.RawData.Slice(Index * 8, Length); } }
    }
}