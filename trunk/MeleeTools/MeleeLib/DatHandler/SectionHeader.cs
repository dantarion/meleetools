using System;
using MeleeLib.DatHandler;
using MeleeLib.System;

namespace MeleeLib
{
    public class SectionHeader : Node<Header>
    {
        public const int Length = 0x8;
        public readonly ArraySlice<byte> RawData;
        public readonly string Name;
        public SectionHeader(Header parent, uint sectionNumber)
        {
            File = file;
            uint offset = _par.Datasize + Parent.OffsetCount*4 + sectionNumber*8;
            RawData = ;
            Name = File.RawFile.GetAsciiString((int) (File.StringOffsetBase + StringOffset));
        }
        public buint StringOffset { get { return RawData.GetUInt32(0x00); } }
        public buint DataOffset   { get { return RawData.GetUInt32(0x04); } }
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
            get
            {
                File.RawFile.Slice((int)offset, Length);
            }
        }
    }
}