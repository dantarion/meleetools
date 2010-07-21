using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MeleeLib
{
    public class DatFile
    {
        static int HEADER_LENGTH = 0x20;
        public String Filename { get; set; }

        public DatHeader Header { get { return header; } }
        public Dictionary<String, SectionHeader> Section1Entries { get { return section1s; } }
        public Dictionary<String, SectionHeader> Section2Entries { get { return section2s; } }
        public FTHeader FTHeader { get { return ftheader; } }
        public List<object> Attributes { get { return attributes; } }

        private byte[] rawheader = new byte[HEADER_LENGTH];
        private byte[] rawdata;
        private DatHeader header;
        private Dictionary<String, SectionHeader> section1s = new Dictionary<String, SectionHeader>();
        private Dictionary<String, SectionHeader> section2s = new Dictionary<String, SectionHeader>();
        private FTHeader ftheader;
        private List<object> attributes = new List<object>();

        public unsafe DatFile(string filename)
        {

            
            //Load up file
            var stream =  File.OpenRead(filename);

            stream.Read(rawheader, 0,HEADER_LENGTH);
            //Get the header
            fixed (byte* ptr = rawheader)
            {
                header = *(DatHeader*)ptr;
            }
            //Allocate space for the rest of the file
            rawdata = new byte[header.Filesize - HEADER_LENGTH];
            //Read rest of file
            stream.Read(rawdata, 0, rawdata.Count());
            //Compute offset base for the section name strings (They are near the end of the file)
            uint stringoffsetbase = header.Datasize + header.OffsetCount * 4 + header.SectionType1Count * 8 + header.SectionType2Count * 8;
            //Read SectionType1s
            for (int i = 0; i < header.SectionType1Count; i++)
            {
                fixed (byte* ptr = rawdata)
                {
                    SectionHeader section = *(SectionHeader*)(ptr + header.Datasize + header.OffsetCount * 4 + i * 8);
                    string name = new String((sbyte*)ptr + stringoffsetbase + section.StringOffset);
                    section1s[name] = section;
                }
            }
            //Read SectionType2s
            for (int i = 0; i < header.SectionType2Count; i++)
            {
                fixed (byte* ptr = rawdata)
                {
                    SectionHeader section = *(SectionHeader*)(ptr + header.Datasize + header.OffsetCount * 4 + header.SectionType1Count * 8 + i * 8);
                    Console.WriteLine(section.StringOffset);
                    string name = new String((sbyte*)ptr + stringoffsetbase + section.StringOffset);
                    section2s[name] = section;

                }
            }
            //FTHeader
            fixed (byte* ptr = rawdata)
            {
                uint[] INT_ATTRIBUTES = { 0x58, 0xa4, 0x98, 0x16c };
                ftheader = *(FTHeader*)(ptr + section1s.Values.First().DataOffset);
                byte* cur = ftheader.AttributesOffset + ptr;
                byte* end = ftheader.AttributesOffset2 + ptr;
                uint i = 0;
                while (cur < end)
                {
                    if (!INT_ATTRIBUTES.Contains(i))
                        attributes.Add((float)*(bfloat*)cur);
                    else
                        attributes.Add((uint)*(buint*)cur);
                    i += 4;
                    cur += 4;
                }

            }
        }
    }
}
