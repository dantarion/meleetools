using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SubactionIndex : IData, IFilePiece {
        public File File { get; private set; }
        public FTHeader FTHeader { get { return File.FTHeader; } }
        public SubactionIndex(File file) { File = file; }
        public uint Size { get { return FTHeader.SubactionEnd - FTHeader.SubactionStart; } }
        public ArraySlice<byte> RawData { get { return File.RawData.Slice((int)FTHeader.SubactionStart, (int)Size); } }
    }
}
