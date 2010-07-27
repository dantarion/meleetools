using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class FTHeader : Node<Header>
    {
        public uint AttributesOffset { get { return attributesoffset; } }
        public uint AttributesOffset2 { get { return attributesoffset2; } }
        public uint SubactionStart { get { return subactionstart; } }
        public uint SubactionEnd { get { return subactionend; } }
        public buint attributesoffset;
        public buint attributesoffset2;
        private buint unknown1;
        private buint subactionstart;
        private buint unknown2;
        private buint subactionend;
        public fixed uint values[18];

        public FTHeader(Header parent)
        {
            _parent = parent;
        }

        private readonly Header _parent;

        public override Header Parent
        {
            get { throw new NotImplementedException(); }
        }

        public override File File
        {
            get { throw new NotImplementedException(); }
        }

        public override ArraySlice<byte> RawData
        {
            get { throw new NotImplementedException(); }
        }
    }
}