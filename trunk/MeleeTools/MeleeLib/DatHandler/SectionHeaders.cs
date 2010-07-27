using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    class SectionHeaders : Node<Header>
    {
        public SectionHeader this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        public override Header Parent
        {
            get { throw new NotImplementedException(); }
        }

        public override File File
        {
            get { throw new NotImplementedException(); }
        }

        public override ArraySlice<byte> RawData
        {
            get { throw new NotImplementedException(); }
        }
    }
}
