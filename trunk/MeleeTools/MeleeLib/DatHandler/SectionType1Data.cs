using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class SectionType1Data : IData, IFilePiece
    {
        private SectionType1Data() { }
        public SectionType1Data(File file) { throw new UnparseableDataException(); }
        public ArraySlice<byte> RawData { get { throw new UnknownDataLengthException(); } }
        public File File { get; private set; }
    }
}
