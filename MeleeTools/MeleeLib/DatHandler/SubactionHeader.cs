using System;
using MeleeLib.System;

namespace MeleeLib.DatHandler
{
    public struct SubactionHeader
    {
        public uint StringOffset { get { return stringoffset; } set { stringoffset = value; } }
        public uint ScriptOffset { get { return scriptOffset; } set { scriptOffset = value; } }
        private uint stringoffset;
        private uint unknown1;
        private uint unknown2;
        private uint scriptOffset;
        private uint unknown3;
        private uint unknown4;
    }
}