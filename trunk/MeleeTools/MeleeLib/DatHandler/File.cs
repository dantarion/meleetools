using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MeleeLib.Utility;

//TODO: Before edit functionality is added, everything needs to be initialized to members rather than read directly from the byte array every time.
namespace MeleeLib.DatHandler {
    public class File {
        public readonly String Filename;
        private File() { }
        public File(string filename) {
            Filename = filename;
            var stream = System.IO.File.OpenRead(filename);
            var rawData = new byte[(int)stream.Length].Slice();
            stream.Read(rawData.Array, 0, rawData.Count);
            stream.Close();
            //Initialize Header
            Header = new Header(this, rawData);
            //The offset of everything else is based off the header
            rawData = rawData.Slice(Header.Length);
            SectionType1Index = new SectionType1Index(this, rawData);
            SectionType2Index = new SectionType2Index(this, rawData);
            FtHeader = new FtHeader(this, rawData);
            Attributes = new AttributesIndex(this, rawData);
            SubactionIndex = new SubactionIndex(this, rawData);
        }

        public Header Header {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public FtHeader FtHeader {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public AttributesIndex Attributes {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public SectionType1Index SectionType1Index {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public SectionType2Index SectionType2Index {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public SubactionIndex SubactionIndex {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
