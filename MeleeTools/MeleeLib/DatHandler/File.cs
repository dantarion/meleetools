using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class File : IData {
        public readonly String Filename;
        public Header Header { get { return new Header(this); } }
        public ArraySlice<byte> DataSection { get { return RawData.Slice(Header.Length); } }
        private File() { }
        public File(string filename) {
            Filename = filename;
            var stream = global::System.IO.File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawData = new byte[(int)stream.Length].Slice();
            stream.Read(RawData.Array, 0, RawData.Count);
            stream.Close();
        }
        public FtHeader FtHeader { get { return new FtHeader(this); } }
        public AttributesIndex Attributes { get { return new AttributesIndex(this); } }
        public ArraySlice<byte> RawData { get; private set; }
        public SectionType1Index SectionType1Index { get { return new SectionType1Index(this); } }
        public SectionType2Index SectionType2Index { get { return new SectionType2Index(this); } }
    }
}
