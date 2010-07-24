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
        public List<Attribute> Attributes { get { return attributes; } }
        public List<Subaction> Subactions { get { return subactions; } }


        private byte[] rawheader = new byte[HEADER_LENGTH];
        private byte[] rawdata;
        private DatHeader header;
        private Dictionary<String, SectionHeader> section1s = new Dictionary<String, SectionHeader>();
        private Dictionary<String, SectionHeader> section2s = new Dictionary<String, SectionHeader>();
        private FTHeader ftheader;
        private List<Attribute> attributes = new List<Attribute>();
        private List<Subaction> subactions = new List<Subaction>();
        public unsafe DatFile(string filename)
        {

            this.Filename = filename;
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
                int[] INT_ATTRIBUTES = { 0x58, 0xa4, 0x98, 0x16c };
                ftheader = *(FTHeader*)(ptr + section1s.Values.First().DataOffset);
                byte* cur = ftheader.AttributesOffset + ptr;
                byte* end = ftheader.AttributesOffset2 + ptr;
                int i = 0;
                while (cur < end)
                {
                    Attribute attribute = new Attribute();
                    if (!INT_ATTRIBUTES.Contains(i))
                        attribute.Value = (float)*(bfloat*)cur;
                    else
                        attribute.Value = (uint)*(buint*)cur;
                    attribute.Offset = i;
                    attributes.Add(attribute);
                    i += 4;
                    cur += 4;
                }
            }
            //Subactions
            fixed (byte* ptr = rawdata)
            {
                byte* cur = ftheader.SubactionStart + ptr;
                byte* end = ftheader.SubactionEnd + ptr;
                int i = 0;
                while (cur < end)
                {
                    Subaction s = new Subaction();
                    s.Header = *(SubactionHeader*)(cur);
                    string str = new String((sbyte*)ptr + s.Header.StringOffset);
                    if(str.Contains("ACTION_"))
                        str = str.Substring(str.LastIndexOf("ACTION_")+7).Replace("_figatree","");
                    if (str == "")
                        str = "[No name]";
                    s.Name = str;
                    s.Index = i;
                    s.Commands = readScript(ptr + s.Header.ScriptOffset);
                    Subactions.Add(s);                    
                    i += 1;
                    cur += 4*6;
                }
            }

        }
        public unsafe List<ScriptCommand> readScript(byte* ptr)
        {
            var list = new List<ScriptCommand>();
            ScriptCommand sc = ScriptCommand.Factory(ptr);
            while (sc.Type != 0)
            {
                list.Add(sc);
                ptr += sc.Length;
                sc = ScriptCommand.Factory(ptr);
            }
            return list;
        }
    }
}
