using System;
using System.IO;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Section1Header : Node<Section1Index>
    {
        public const int Length = 0x8;
        public string Name { get { return Parent.Parent.DataSection.GetAsciiString((int) (Parent.Parent.StringOffsetBase + StringOffset)); } }
        private readonly int _index;
        public Section1Data Data { get { return new Section1Data(this); } }
        public Section1Header(Section1Index parent, int index)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (parent.Count < index) throw new IndexOutOfRangeException();
            _parent = parent;
            _index = index;
        }
        public uint StringOffset { get { return RawData.GetUInt32(0x04); } }
        public uint DataOffset   { get { return RawData.GetUInt32(0x00); } }
        private readonly Section1Index _parent;
        public override Section1Index Parent
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
                return Parent.RawData.Slice(_index*8, Length);
            }
        }
    }
}