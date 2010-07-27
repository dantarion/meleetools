using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class AttributesIndex : Node<FTHeader>, IEnumerable<Attribute>
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
        public uint Count { get { return Parent.AttributesEnd - Parent.AttributesStart; } }
        public override ArraySlice<byte> RawData
        {
            get
            {
                return File.RawData.Slice((int)Parent.AttributesStart, (int)Count);
            }
        }
        public Attribute this[int i]
        {
            get {return new Attribute(this, i, RawData.GetInt32(i)); }
        }

        public IEnumerator<Attribute> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
