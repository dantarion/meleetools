using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class AttributesIndex : IEnumerable<Attribute>, IData, IFilePiece
    {
        public File File { get; private set; }
        public AttributesIndex(File file) { File = file; }
        public uint Size { get { return File.FTHeader.AttributesEnd - File.FTHeader.AttributesStart; } }
        public uint Count { get { return Size / 4; } }
        public ArraySlice<byte> RawData { get { return File.DataSection.Slice((int)File.FTHeader.AttributesStart, (int)Size); } }
        public Attribute this[int i] { get { return new Attribute(File, i); } }

        public IEnumerator<Attribute> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
