using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SubactionHeader : IData, IFilePiece {
        public const int Size = 0x18;
        public File File { get; private set; }
        public readonly int Index;
        public SubactionHeader(File file, int index) { File = file; Index = index; }
        public uint StringOffset { get { return RawData.GetUInt32(0x00); } }
        public uint Unknown1 { get { return RawData.GetUInt32(0x04); } }
        public uint Unknown2 { get { return RawData.GetUInt32(0x08); } }
        public uint ScriptOffset { get { return RawData.GetUInt32(0x0C); } }
        public uint Unknown3 { get { return RawData.GetUInt32(0x10); } }
        public uint Unknown4 { get { return RawData.GetUInt32(0x14); } }
        public int Offset { get { return Index*Size; } }
        public string Name { get { return File.DataSection.GetAsciiString((int)(StringOffset)); } }
        public ArraySlice<byte> RawData { get { return File.DataSection.Slice((int)File.FtHeader.SubactionStart + Offset, Size); } }
        //TODO: Commands, Name

    }
}