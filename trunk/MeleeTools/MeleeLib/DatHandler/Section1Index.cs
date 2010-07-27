using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Section1Index : Node<Header>, IEnumerable<Section1Header>
    {
        protected Section1Index(Header parent)
        {
            _parent = parent;
        }
        private readonly Header _parent;



        public uint Count { get { return Parent.SectionType1Count; } }
        public IEnumerator<Section1Header> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Section1Header this[int i]
        {

            get
            {
                if (i > Count) throw new IndexOutOfRangeException();
                return new Section1Header(Parent, i);
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
