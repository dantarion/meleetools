using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeleeLib.System;

namespace MeleeLib
{
    public class DatFile
    {
        public readonly String Filename;
        public readonly ArraySlice<byte> RawFile;
        public readonly ArraySlice<byte> RawData;
        public readonly DatHeader Header;
        public readonly Dictionary<string, SectionHeader> Section1Entries;
        public readonly Dictionary<string, SectionHeader> Section2Entries;
        public readonly FTHeader FTHeader;
        public readonly List<Attribute> Attributes;
        public readonly List<Subaction> Subactions;

        public DatFile(string filename)
        {
            Section2Entries = new Dictionary<String, SectionHeader>();
            Section1Entries = new Dictionary<String, SectionHeader>();
            Attributes = new List<Attribute>();
            Subactions = new List<Subaction>();
            Filename = filename;
            //Load up file
            var stream = File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawFile = new byte[(int)stream.Length].Slice();
            //Get the header
            Header = new DatHeader(this);
            //Allocate space for the rest of the file
            RawData = RawFile.Slice(DatHeader.HeaderLength);
            //Compute offset base for the section name strings (They are near the end of the file)
            uint stringoffsetbase = Header.Datasize + Header.OffsetCount * 4 + Header.SectionType1Count * 8 + Header.SectionType2Count * 8;
            //Read SectionType1s
            for (var i = 0; i < Header.SectionType1Count; i++)
            {
                fixed (byte* ptr = rawdata)
                {
                    SectionHeader section = *(SectionHeader*)(ptr + header.Datasize + header.OffsetCount * 4 + i * 8);
                    string name = new String((sbyte*)ptr + stringoffsetbase + section.StringOffset);
                    Section1Entries[name] = section;
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
                    Section2Entries[name] = section;

                }
            }
            //FTHeader
            fixed (byte* ptr = rawdata)
            {
                int[] INT_ATTRIBUTES = { 0x58, 0xa4, 0x98, 0x16c };
                FTHeader = *(FTHeader*)(ptr + Section1Entries.Values.First().DataOffset);
                byte* cur = FTHeader.AttributesOffset + ptr;
                byte* end = FTHeader.AttributesOffset2 + ptr;
                int i = 0;
                while (cur < end)
                {
                    var attribute = new Attribute();
                    if (!INT_ATTRIBUTES.Contains(i))
                        attribute.Value = (float)*(bfloat*)cur;
                    else
                        attribute.Value = (uint)*(buint*)cur;
                    attribute.Offset = i;
                    Attributes.Add(attribute);
                    i += 4;
                    cur += 4;
                }
            }
            //Subactions
            fixed (byte* ptr = rawdata)
            {
                byte* cur = FTHeader.SubactionStart + ptr;
                byte* end = FTHeader.SubactionEnd + ptr;
                int i = 0;
                while (cur < end)
                {
                    Subaction s = new Subaction();
                    s.Header = *(SubactionHeader*)(cur);
                    string str = new String((sbyte*)ptr + s.Header.StringOffset);
                    if (str.Contains("ACTION_"))
                        str = str.Substring(str.LastIndexOf("ACTION_") + 7).Replace("_figatree", "");
                    if (str == "")
                        str = "[No name]";
                    s.Name = str;
                    s.Index = i;
                    s.Commands = readScript(ptr + s.Header.ScriptOffset);
                    Subactions.Add(s);
                    i += 1;
                    cur += 4 * 6;
                }
            }

        }
        public List<ScriptCommand> readScript(byte* ptr)
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
