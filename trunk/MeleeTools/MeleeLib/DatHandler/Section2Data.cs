using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib.System;
using MeleeLib.System.Node;

namespace MeleeLib.DatHandler
{
    public class Section2Data : ChildNode<File, Section2Header>, IData
    {

        public Section2Data(Section2Header parent)
        {
            throw new UnparseableDataException();

            _parent = parent;
        }
        private Section2Data() { }
        private readonly Section2Header _parent;
        public override Section2Header Parent { get { return _parent; } }


        public ArraySlice<byte> RawData
        {
            get { throw new UnknownDataLengthException(); }
        }
    }
}
