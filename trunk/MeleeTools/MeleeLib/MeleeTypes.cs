using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MeleeLib.System;

namespace MeleeLib
{
    public class DatHeader
    {
        public const int HeaderLength = 0x20;
        private readonly ArraySlice<byte> _rawData;
        private DatHeader() { }
        public DatHeader(DatFile file)
        {
            if (file.RawFile.Count < HeaderLength) throw new IndexOutOfRangeException();
            _rawData = file.RawFile.Slice(0, HeaderLength);
        }
        public DatHeader(byte[] fileData) : this(new ArraySlice<byte>(fileData)) { }
        public uint Filesize            { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x00); } }
        public buint Datasize           { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x04); } }
        public buint OffsetCount        { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x08); } }
        public buint SectionType1Count  { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x0C); } }
        public buint SectionType2Count  { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x10); } }
        public ArraySlice<byte> Version { get { return _rawData.Slice(0x14, 0x4); } }                        //0x14
        public buint Unknown1           { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x18); } }
        public buint Unknown2           { get { return BitConverter.ToUInt32(_rawData.Array, _rawData.Offset + 0x1C); } }
    }
    public class SectionHeader
    {
        public buint StringOffset { get { return BitConverter.ToUInt32(_rawData)} }
        public buint DataOffset { get; set; }

    }
    public struct FTHeader
    {
        public uint AttributesOffset { get { return attributesoffset; } }
        public uint AttributesOffset2 { get { return attributesoffset2; } }
        public uint SubactionStart { get { return subactionstart; } }
        public uint SubactionEnd { get { return subactionend; } }
        public buint attributesoffset;
        public buint attributesoffset2;
        private buint unknown1;
        private buint subactionstart;
        private buint unknown2;
        private buint subactionend;
        public fixed uint values[18];
    }
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

    public class Subaction
    {
        public int Index{get;set;}
        public SubactionHeader Header{get;set;}
        public List<ScriptCommand> Commands { get; set; }
        public String Name { get; set; }

    }

}
