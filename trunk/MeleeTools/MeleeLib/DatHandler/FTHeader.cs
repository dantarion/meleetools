using MeleeLib.System;

namespace MeleeLib
{
    public struct FTHeader
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
    }
}