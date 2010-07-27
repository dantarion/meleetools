using MeleeLib.System;

namespace MeleeLib
{
    public struct SubactionHeader
    {
        public uint StringOffset { get { return stringoffset; } set { stringoffset = value; } }
        public uint ScriptOffset { get { return scriptOffset; } set { scriptOffset = value; } }
        private buint stringoffset;
        private buint unknown1;
        private buint unknown2;
        private buint scriptOffset;
        private buint unknown3;
        private buint unknown4;
    }
}