using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class SectionType2Data : IData, IFilePiece {
        private SectionType2Data() { }
        public SectionType2Data(File file) { throw new UnparseableDataException(); }
        public ArraySlice<byte> RawData { get { throw new UnknownDataLengthException(); } }
        public File File { get; private set; }
    }
}
