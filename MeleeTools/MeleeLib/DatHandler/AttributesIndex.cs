using System;
using System.Diagnostics;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class AttributesIndex : Node<FTHeader>
    {
        private readonly FTHeader _parent;

        public AttributesIndex(FTHeader parent)
        {
            _parent = parent;
        }

        public override FTHeader Parent
        {
            get { return _parent; }
        }

        public override File File
        {
            get { return Parent.File; }
        }
        public uint Count { get { return Parent.AttributesEnd - Parent.AttriibutesStart; } }
        public override ArraySlice<byte> RawData
        {
            get
            {
                return File.RawData.Slice((int)Parent.AttriibutesStart, (int)Count);
            }
        }
        public Attribute this[int i]
        {
            get {return new Attribute(this, i, RawData.GetInt32(i)); }
        }
    }
}
