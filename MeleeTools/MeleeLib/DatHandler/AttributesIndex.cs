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
        public uint Size { get { return Parent.AttributesEnd - Parent.AttributesStart; } }
        public uint Count { get { return Size/4; } }
        public override ArraySlice<byte> RawData
        {
            get
            {
                
                return Parent.Parent.DataSection.Slice((int)Parent.AttributesStart, (int)Size);
            }
        }
        public Attribute this[int i]
        {
            get {return new Attribute(this, i); }
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
