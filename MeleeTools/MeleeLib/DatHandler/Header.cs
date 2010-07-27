using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler {
    public class Header : IData, IFilePiece {
        #region TODO
        //            //Subactions
        //            fixed (byte* ptr = rawdata)
        //            {
        //                byte* cur = FTHeader.SubactionStart + ptr;
        //                byte* end = FTHeader.SubactionEnd + ptr;
        //                int i = 0;
        //                while (cur < end)
        //                {
        //                    Subaction s = new Subaction();
        //                    s.Header = *(SubactionHeader*)(cur);
        //                    string str = new String((sbyte*)ptr + s.Header.StringOffset);
        //                    if (str.Contains("ACTION_"))
        //                        str = str.Substring(str.LastIndexOf("ACTION_") + 7).Replace("_figatree", "");
        //                    if (str == "")
        //                        str = "[No name]";
        //                    s.Name = str;
        //                    s.Index = i;
        //                    s.Commands = readScript(ptr + s.Header.ScriptOffset);
        //                    Subactions.Add(s);
        //                    i += 1;
        //                    cur += 4 * 6;
        //                }
        //            }
        //public List<ScriptCommand> readScript(byte* ptr)
        //        {
        //            var list = new List<ScriptCommand>();
        //            ScriptCommand sc = ScriptCommand.Factory(ptr);
        //            while (sc.Type != 0)
        //            {
        //                list.Add(sc);
        //                ptr += sc.Length;
        //                sc = ScriptCommand.Factory(ptr);
        //            }
        //            return list;
        //        }
        #endregion
        public const int Length = 0x20;
        private Header() { }
        public Header(File file) {
            if (file.RawData.Count < Length) throw new IndexOutOfRangeException();
            File = file;
        }
        public Section1Index Section1Index { get { return new Section1Index(File); } }
        public Section2Index Section2Index { get { return new Section2Index(File); } }
        public File File { get; private set; }
        public uint Filesize { get { return RawData.GetUInt32(0x00); } }
        public uint Datasize { get { return RawData.GetUInt32(0x04); } }
        public uint OffsetCount { get { return RawData.GetUInt32(0x08); } }
        public uint SectionType1Count { get { return RawData.GetUInt32(0x0C); } }
        public uint SectionType2Count { get { return RawData.GetUInt32(0x10); } }
        public ArraySlice<byte> Version { get { return RawData.Slice(0x14, 0x4); } }
        public uint Unknown1 { get { return RawData.GetUInt32(0x18); } }
        public uint Unknown2 { get { return RawData.GetUInt32(0x1C); } }
        public uint StringOffsetBase {
            get {
                return Datasize
                     + OffsetCount * sizeof(uint)
                     + SectionType1Count * Section1Header.Length
                     + SectionType2Count * Section2Header.Length;
            }
        }

        public FTHeader FTHeader { get { return new FTHeader(File); } }
        public ArraySlice<byte> RawData { get { return File.RawData.Slice(0, Length); } }
    }
}