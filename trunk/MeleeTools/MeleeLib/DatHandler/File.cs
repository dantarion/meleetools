using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class File : IData
    {
        public readonly String Filename;
        public Header Header { get { return new Header(this); } }
        public ArraySlice<byte> DataSection { get { return RawData.Slice(Header.Length); } }
        public File(string filename)
        {
            Filename = filename;
            var stream = global::System.IO.File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawData = new byte[(int)stream.Length].Slice();
            stream.Read(RawData.Array, 0, RawData.Count);
            stream.Close();
        }
        
#region Convenience Properties
        public AttributesIndex Attributes { get { return Header.FTHeader.Attributes; } }
#endregion

        public ArraySlice<byte> RawData { get; private set; }
    }
}
