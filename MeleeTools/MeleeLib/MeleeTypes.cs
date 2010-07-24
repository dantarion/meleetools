using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace MeleeLib
{
    public unsafe struct DatHeader
    {
        public uint Filesize { get { return filesize; } set { filesize = value; } }
        public uint Datasize { get { return datasize; } set { datasize = value; } }
        public uint OffsetCount { get { return offsetcount; } set { offsetcount = value; } }
        public uint SectionType1Count { get { return sectiontype1count; } set { sectiontype1count = value; } }
        public uint SectionType2Count { get { return sectiontype2count; } set { sectiontype2count = value; } }
        public uint Unknown1 { get { return unk1; } set { unk1 = value; } }
        public uint Unknown2 { get { return unk2; } set { unk2 = value; } }

        private buint filesize;
        private buint datasize;
        private buint offsetcount;
        private buint sectiontype1count;
        private buint sectiontype2count;
        private fixed byte version[4];
        private buint unk1;    
        private buint unk2;
    }
    public struct SectionHeader
    {
        public uint StringOffset { get { return stringoffset; } set { stringoffset = value; } }
        public uint DataOffset { get { return dataoffset; } set { dataoffset = value; } }
        
        private buint dataoffset;
        private buint stringoffset;
    }
    public unsafe struct FTHeader
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
