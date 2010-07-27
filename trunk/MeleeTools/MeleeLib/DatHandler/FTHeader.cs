using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class FTHeader : Node<Header>
    {
        public const int Length = 0x60;
        public uint AttributesStart { get { return RawData.GetUInt32(0x00); } }
        public uint AttributesEnd { get { return RawData.GetUInt32(0x04); } }
        public uint Unknown1 { get { return RawData.GetUInt32(0x08); } }
        public uint SubactionStart { get { return RawData.GetUInt32(0x0C); } }
        public uint Unknown2 { get { return RawData.GetUInt32(0x10); } }
        public uint SubactionEnd { get { return RawData.GetUInt32(0x14); } }
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
            get { return Parent.DataSection.Slice((int)Parent.Section1Index[0].DataOffset, Length); }
        }
    }           
}