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
        public readonly Header Header;
        public readonly SectionHeader[][] Section1Headers;
        public readonly FTHeader FTHeader;
        public readonly List<Attribute> Attributes;
        public readonly List<Subaction> Subactions;
        public File(string filename)
        {
            Filename = filename;
            var stream = global::System.IO.File.OpenRead(filename);
            if (stream.Length > int.MaxValue) throw new IOException("File too large.");
            RawData = new byte[(int)stream.Length].Slice();
            Header = new Header(this);

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
