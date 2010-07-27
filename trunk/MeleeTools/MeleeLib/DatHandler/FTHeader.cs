using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class FTHeader : Node<Header>
    {
        public const int Length = 0x60;
        public buint AttriibutesStart { get { return RawData.GetUInt32(0x00); } }
        public buint AttributesEnd { get { return RawData.GetUInt32(0x04); } }
        public buint Unknown1 { get { return RawData.GetUInt32(0x08); } }
        public buint SubactionStart { get { return RawData.GetUInt32(0x0C); } }
        public buint Unknown2 { get { return RawData.GetUInt32(0x10); } }
        public buint SubactionEnd { get { return RawData.GetUInt32(0x14); } }
        public AttributesIndex Attributes { get { return new AttributesIndex(this); } }
        public ArraySlice<byte> Values { get { return RawData.Slice(0x18, 18);  } }

        public FTHeader(Header parent)
        {
            _parent = parent;
        }

        private readonly Header _parent;

        public override Header Parent
        {
            get { return _parent; }
        }

        public override File File
        {
            get { return Parent.File; }
        }

        public override ArraySlice<byte> RawData
        {
            get { return File.RawData.Slice((int)Parent.Section1Index[0].DataOffset, Length); }
        }
    }
}