using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class File
    {
        public readonly String Filename;
        public readonly ArraySlice<byte> RawData;
        public Header Header { get { return new Header(this); }}
        public File(string filename)
        {
            Filename = filename;
            var stream = global::System.IO.File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawData = new byte[(int)stream.Length].Slice();
            stream.Read(RawData.Array, 0, RawData.Count);
            stream.Close();
        }
    }
}
