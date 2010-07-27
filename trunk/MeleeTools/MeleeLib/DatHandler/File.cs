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
        public readonly ArraySlice<byte> RawFile;
        public readonly Header Header;
        public readonly SectionHeader[][] SectionHeaders;
        public readonly FTHeader FTHeader;
        public readonly List<Attribute> Attributes;
        public readonly List<Subaction> Subactions;
        public uint StringOffsetBase { get { return Header.Datasize + Header.OffsetCount * 4 + Header.SectionType1Count * 8 + Header.SectionType2Count * 8; } }
        public File(string filename)
        {
            Filename = filename;
            //Load up file
            var stream = global::System.IO.File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawFile = new byte[(int)stream.Length].Slice();
            //Get the header
            Header = new Header(this);
            //Allocate space for the rest of the file
            SectionHeaders = new SectionHeader[2][];
            SectionHeaders[0] = new SectionHeader[Header.SectionType1Count];
            for (uint i = 0; i < SectionHeaders[0].Length; i++) Section1Entries[i] = new SectionHeader(this, i);

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
                        attribute.Value = (float)*(bzfloat*)cur;
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
