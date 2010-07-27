using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Section1Data : IData, IFilePiece
    {
        public Section1Data(File file) { throw new UnparseableDataException(); }
        private Section1Data() { }
        public ArraySlice<byte> RawData { get { throw new UnknownDataLengthException(); } }
        public File File { get; private set; }
    }
}
