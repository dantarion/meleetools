using System;
using System.IO;
using MeleeLib.System;
using MeleeLib.System.Node;

namespace MeleeLib.DatHandler
{
    public class Section1Header : SiblingNode<File, Section1Index>, IData
    {
        public const int Length = 0x8;
        public string Name { get { return Root.DataSection.GetAsciiString((int)(Parent.Parent.StringOffsetBase + StringOffset)); } }
        public readonly int Index;
        public Section1Data Data { get { return new Section1Data(this); } }
        public Section1Header(Section1Index parent, int index)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (parent.Count < index) throw new IndexOutOfRangeException();
            _parent = parent;
            Index = index;
        }
        public uint StringOffset { get { return RawData.GetUInt32(0x04); } }
        public uint DataOffset { get { return RawData.GetUInt32(0x00); } }
        private readonly Section1Index _parent;
        public override Section1Index Parent
        {
            get { return _parent; }
        }

        public override File Root
        {
            get { return Parent.Root; }
        }

        public ArraySlice<byte> RawData
        {
            get
            {
                return Parent.RawData.Slice(Index * 8, Length);
            }
        }

        public override SiblingNode<File,Section1Index> Next
        {
            get { return Index < Parent.Count - 1 ? Parent[Index + 1] : null; }
        }

        public override SiblingNode<File,Section1Index> Previous
        {
            get { return Index > 0 ? Parent[Index - 1] : null; }
        }
    }
}