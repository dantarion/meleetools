using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class Subaction : IData, IFilePiece {
        public const int Size = 0x18;
        public File File { get; private set; }
        public readonly int Index;
        public Subaction(File file, int index) { File = file; Index = index; }
        public int StringOffset { get { return RawData.GetInt32(0x00); } }
        public int Unknown1 { get { return RawData.GetInt32(0x04); } }
        public int Unknown2 { get { return RawData.GetInt32(0x08); } }
        public int ScriptOffset { get { return RawData.GetInt32(0x0C); } }
        public int Unknown3 { get { return RawData.GetInt32(0x10); } }
        public int Unknown4 { get { return RawData.GetInt32(0x14); } }
        public int Offset { get { return Index * Size; } }
        public string Name { get { return StringOffset != 0 ? File.DataSection.GetAsciiString((int)(StringOffset)) : null; } }
        public string ShortName { get { return StringOffset != 0 ? Name.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[3] : null; } }
        public ArraySlice<byte> RawData { get { return File.DataSection.Slice((int)File.FtHeader.SubactionStart + Offset, Size); } }
        public ScriptIndex Scripts { get { return new ScriptIndex(File,this);}}
        //TODO: Commands, Name

    }
}