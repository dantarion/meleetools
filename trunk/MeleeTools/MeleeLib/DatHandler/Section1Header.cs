using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Section1Header : Node<Header>, IEnumerable<Section1>
    {
        public const int Length = 0x20;
        protected Section1Header(Header parent)
        {
            _parent = parent;
        }
        private readonly Header _parent;



        public uint Count { get { return Parent.SectionType1Count; } }
        public IEnumerator<Section1> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Section1 this[int i]
        {

            get
            {
                if (i > Count) throw new IndexOutOfRangeException();
                return new Section1(Parent, i);
            }
        }

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
            get { throw new NotImplementedException(); }
        }
    }
}
