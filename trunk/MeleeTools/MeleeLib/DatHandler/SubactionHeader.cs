using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class SubactionHeader : IData, IFilePiece
    {
        public File File { get; private set; }
#region Convenience Properties
        public FTHeader FTHeader { get { return File.Header.FTHeader; } }
#endregion
        public SubactionHeader(File file) { File = file; }
        public uint StringOffset { get { return RawData.GetUInt32(0x14); } }
        public uint Unknown1     { get { return RawData.GetUInt32(0x10); } }
        public uint Unknown2     { get { return RawData.GetUInt32(0x0C); } }
        public uint ScriptOffset { get { return RawData.GetUInt32(0x08); } }
        public uint Unknown3     { get { return RawData.GetUInt32(0x04); } }
        public uint Unknown4     { get { return RawData.GetUInt32(0x00); } }
        public uint Size { get { return FTHeader.SubactionEnd - FTHeader.SubactionStart; } }
        public ArraySlice<byte> RawData { get { return File.DataSection.Slice((int)FTHeader.SubactionStart, (int)Size); }
        }
    }
}