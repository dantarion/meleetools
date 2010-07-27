using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Header : Node<File>
    {
        #region TODO
        public readonly FTHeader FTHeader;
        public readonly List<Attribute> Attributes;
        public readonly List<Subaction> Subactions;
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
        #endregion
        public const int Length = 0x20;
        private Header() { }
        public Header(File parent)
        {
            if (parent.RawData.Count < Length) throw new IndexOutOfRangeException();
            _parent = parent;
        }
        private readonly File _parent;
        public override File Parent { get { return _parent; } }
        public override File File
        {
            get { return Parent; }
        }
        public uint Filesize            { get { return RawData.GetUInt32(0x00); } }
        public buint Datasize           { get { return RawData.GetUInt32(0x04); } }
        public buint OffsetCount        { get { return RawData.GetUInt32(0x08); } }
        public buint SectionType1Count  { get { return RawData.GetUInt32(0x0C); } }
        public buint SectionType2Count  { get { return RawData.GetUInt32(0x10); } }
        public ArraySlice<byte> Version { get { return RawData.Slice(    0x14, 0x4); } }
        public buint Unknown1           { get { return RawData.GetUInt32(0x18); } }
        public buint Unknown2           { get { return RawData.GetUInt32(0x1C); } }
        public uint StringOffsetBase
        {
            get
            {
                return Datasize
                     + OffsetCount * sizeof(uint)
                     + SectionType1Count * Section1Header.Length
                     + SectionType2Count * Section2Header.Length;
            }
        }



        public override ArraySlice<byte> RawData
        {
            get { return Parent.RawData.Slice(0, Length); }
        }
    }
}