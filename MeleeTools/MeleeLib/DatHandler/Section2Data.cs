using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public class Section2Data : Node<Section2Header>
    {

        public Section2Data(Section2Header parent)
        {
            throw new UnparseableDataException();

            _parent = parent;
        }
        private Section2Data() { }
        private readonly Section2Header _parent;
        public override Section2Header Parent { get { return _parent; } }

        public override File File { get { return Parent.File; } }

        public override ArraySlice<byte> RawData
        {
            get { throw new UnknownDataLengthException(); }
        }
    }
}
