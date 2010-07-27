using System;
using MeleeLib.System;
using MeleeLib.System.Node;

namespace MeleeLib.DatHandler
{
    public class SubactionHeader : ChildNode<File, FTHeader> , IData
    {
        public uint StringOffset { get { return RawData.GetUInt32(0x14); } }
        public uint Unknown1     { get { return RawData.GetUInt32(0x10); } }
        public uint Unknown2     { get { return RawData.GetUInt32(0x0C); } }
        public uint ScriptOffset { get { return RawData.GetUInt32(0x08); } }
        public uint Unknown3     { get { return RawData.GetUInt32(0x04); } }
        public uint Unknown4     { get { return RawData.GetUInt32(0x00); } }
        private readonly FTHeader _parent;

        public SubactionHeader(FTHeader parent)
        {
            _parent = parent;
        }

        public override FTHeader Parent
        {
            get { return _parent; }
        }

        public uint Size { get { return Parent.SubactionEnd - Parent.SubactionStart; } }
        public ArraySlice<byte> RawData
        {
            get { return Root.DataSection.Slice((int)Parent.SubactionStart, (int)Size); }
        }
    }
}