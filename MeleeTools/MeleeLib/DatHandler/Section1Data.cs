using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib.System;
using MeleeLib.System.Node;

namespace MeleeLib.DatHandler
{
    public class Section1Data : ChildNode<File, Section1Header>, IData
    {

        public Section1Data(Section1Header parent)
        {
            throw new UnparseableDataException();

            _parent = parent;
        }
        private Section1Data() { }
        private readonly Section1Header _parent;
        public override Section1Header Parent { get { return _parent; } }

        public override File Root { get { return Parent.Root; } }

        public ArraySlice<byte> RawData
        {
            get { throw new UnknownDataLengthException(); }
        }
    }
}
