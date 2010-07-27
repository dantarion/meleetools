using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Header : Node<File>
    {
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