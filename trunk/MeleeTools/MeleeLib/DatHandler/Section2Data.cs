using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class Section2Data : IData, IFilePiece {
        public Section2Data(File file) { throw new UnparseableDataException(); }
        private Section2Data() { }
        public ArraySlice<byte> RawData { get { throw new UnknownDataLengthException(); } }
        public File File { get; private set; }
    }
}
