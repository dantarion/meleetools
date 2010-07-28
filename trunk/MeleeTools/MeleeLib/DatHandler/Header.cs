using System;
using System.Diagnostics;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public class Header : IFilePiece {
        public const int Length = 0x20;
        private Header() { }
        public Header(File file, ArraySlice<byte> rawData) {
            File              = file;
            rawData           = rawData.Slice(0, Length);
            Filesize          = rawData.GetInt32(0x00);
            Datasize          = rawData.GetInt32(0x04);
            OffsetCount       = rawData.GetInt32(0x08);
            SectionType1Count = rawData.GetInt32(0x0C);
            SectionType2Count = rawData.GetInt32(0x10);
            Version           = rawData.   Slice(0x14, 0x4).ToArray();
            Unknown1          = rawData.GetInt32(0x18);
        }

        public int Filesize {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Datasize {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int OffsetCount {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int SectionType1Count {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int SectionType2Count {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public byte[] Version {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Unknown1 {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Unknown2 {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int StringOffsetBase {
            get {
                return Datasize
                       + OffsetCount * sizeof(int)
                       + SectionType1Count * SectionType1Header.Size
                       + SectionType2Count * SectionType2Header.Length;
            }
        }

        public File File {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
