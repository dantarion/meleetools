using System;
using System.Collections;
using System.Collections.Generic;
using MeleeLib.System;
using MeleeLib.System.Node;

namespace MeleeLib.DatHandler
{
    public class Section1Index : ChildNode<File, Header>, IEnumerable<Section1Header>, IData
    {
        public Section1Index(Header parent)
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
                return new Section1Header(this, i);
            }
        }

        public override Header Parent
        {
            get { return _parent; }
        }


        public ArraySlice<byte> RawData
        {
            get { return Root.DataSection.Slice((int)Parent.Datasize + (int)Parent.OffsetCount * 4, (int)Count * 8); }
        }
    }
}
