using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace MeleeLib
{
    public abstract unsafe class ScriptCommand
    {
        public static ScriptCommand Factory(byte* ptr)
        {
            var gc = new GenericCommand(ptr);
            if (gc.Type == 0x1 || gc.Type == 0x2)
                return new TimerCommand(ptr);
            if (gc.Type == 0xb)
                return new HitboxCommand(ptr);
            return new GenericCommand(ptr);
        }
        struct CommandData
        {
            public CommandData(int length, string name)
            {
                this.name = name;
                this.length = length;
            }
            public string name;
            public int length;
        }
        static Dictionary<uint, CommandData> dict;
        static void setupDict()
        {
            if (dict != null)
                return;
            dict = new Dictionary<uint, CommandData>();
            dict[0x00] = new CommandData(0x04, "End");
            dict[0x01] = new CommandData(0x04, "Synch Timer");
            dict[0x02] = new CommandData(0x04, "Asynch Timer");
            dict[0x03] = new CommandData(0x04, "Start Loop");
            dict[0x04] = new CommandData(0x04, "Execute Loop");
            dict[0x05] = new CommandData(0x08, "Unknown");

            dict[0x07] = new CommandData(0x08, "Subroutine?");

            dict[0x0a] = new CommandData(0x14, "GFX Effect");
            dict[0x0b] = new CommandData(0x14, "Hitbox");

            dict[0x10] = new CommandData(0x04, "Remove Hitboxes");
            dict[0x11] = new CommandData(0x0c, "Sound Effect");
            dict[0x12] = new CommandData(0x0c, "Random Smash SFX");
            dict[0x13] = new CommandData(0x04, "Autocancel");

            dict[0x17] = new CommandData(0x04, "IASA");

            dict[0x1a] = new CommandData(0x04, "Body Invincible");
            dict[0x1b] = new CommandData(0x04, "Body Invincible2");
            dict[0x1c] = new CommandData(0x04, "Partial Invincible");

            dict[0x1f] = new CommandData(0x04, "Model Mod");

            dict[0x33] = new CommandData(0x04, "Self-Damage");

            dict[0x36] = new CommandData(0x12, "Unknown");
            dict[0x37] = new CommandData(0x0c, "Unknown");
            dict[0x38] = new CommandData(0x08, "Start Smash Charge");
        }
        static int getLength(uint type)
        {
            setupDict();
            return dict.ContainsKey(type) ? dict[type].length : 4;
        }
        static String getName(uint type)
        {
            setupDict();
            return dict.ContainsKey(type) ? dict[type].name : String.Format("!Unknown 0x{0:X2}!", type); ;
        }
        public ScriptCommand(byte* dataptr)
        {
            data = dataptr;
        }
        [CategoryAttribute("Internal")]
        public uint Type { get { return (uint)data[0] >> 2; } }
        [CategoryAttribute("Internal")]
        public int Length { get { return getLength(Type); } }
        [CategoryAttribute("Internal")]
        public string Name { get { return getName(Type); } }
        [CategoryAttribute("Internal")]
        public string HexString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Length; i++)
                {
                    sb.AppendFormat("{0:X2} ", data[i]);
                }
                return sb.ToString();
            }
        }
        protected byte* data;
    }
    public unsafe class GenericCommand : ScriptCommand
    {
        public GenericCommand(byte* ptr) : base(ptr)
        {
           
        }
    }
    public unsafe class TimerCommand : ScriptCommand
    {
        public TimerCommand(byte* ptr)
            : base(ptr)
        {
           
        }
        public string Name
        {
            get { return base.Name + "[" + Frames + "]"; }
        }
        [CategoryAttribute("Timer Params")]
        public ushort Frames
        {
            get{return *(bushort*)(data+2);}
        }
    }
    public unsafe class HitboxCommand : ScriptCommand
    {
        public HitboxCommand(byte* ptr)
            : base(ptr)
        {

        }
        public enum HIFlags
        {
            NO_CLANK,SOME_CLANK,MORE_CLANK,ALL_CLANK
        }
        public enum HBIFlags
        {
            NONE,AIR_ONLY,GROUND_ONLY,NORMAL
        }
        public enum ElementType
        {
            NORMAL = 0x00,
            FIRE = 0x04,
            ELECTRIC = 0x08,
            SLASH= 0x0C,
            COIN= 0x10,
            ICE = 0x14,
            SLEEP=0x18,
            SLEEP2=0x1C,
            GROUNDED=0x20,
            GROUNDED2=0x24,
            CAPE = 0x28,
            EMPTY=0x2C,
            DISABLED = 0x30,
            SCREW_ATTACK = 0x38,
            POISON_FLOWER=0x3C,
            NOTHING = 0x40
        }
        new public string Name
        {
            get { return base.Name + "[" + ID + "]"; }
        }
        public int ID
        {
            get { return *(bushort*)(data) >> 7 & 0x7; }
        }
        public int UnknownR
        {
            get { return ((data[1] & 0x7B) >> 2); }
        }
        [CategoryAttribute("Stats")]
        public int BoneID
        {
            get { return *(bushort*)(data+1) >> 3 & 0x7F; }
        }
        public int Unknown0
        {
            get { return ((data[2] & 0x07) >> 1); }
        }
        [CategoryAttribute("Stats")]
        public int Damage
        {
            get { return *(bushort*)(data + 2) >> 0 & 0x1FF; }
        }
        [CategoryAttribute("Position")]
        public int Size
        {
            get { return *(bushort*)(data + 4) >> 0 & 0xFFFF; }
        }
        [CategoryAttribute("Position")]
        public int ZOffset
        {
            get { return *(bshort*)(data + 6); }
        }
        [CategoryAttribute("Position")]
        public int YOffset
        {
            get { return *(bshort*)(data + 8); }
        }
        [CategoryAttribute("Position")]
        public int XOffset
        {
            get { return *(bshort*)(data + 10); }
        }
        [CategoryAttribute("Stats")]
        public int Angle
        {
            get { return *(bushort*)(data + 12) >> 7 & 0xFFFF; }
        }
        [CategoryAttribute("Stats")]
        public int KBGrowth
        {
            get { return *(bushort*)(data + 13) >> 6 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public int WeightDependantKB
        {
            get { return *(bushort*)(data + 14) >> 5 & 0x1FF; }
        }
        public int UnknownQ
        {
            get { return (data[15]) >> 2 & 0x7; }
        }
        [CategoryAttribute("Flags")]
        public HIFlags HitboxInteraction
        {
            get { return (HIFlags)((data[15]) >> 0 & 0x3); }
        }
        [CategoryAttribute("Stats")]
        public int BaseKB
        {
            get { return *(bushort*)(data + 16) >> 7 & 0xFFFF; }
        }
        [CategoryAttribute("Stats")]
        public ElementType Element
        {
            get { return (ElementType)(*(bushort*)(data + 17) >> 2 & 0x1F); }
        }
        public int UnknownV
        {
            get { return (data[17]) >> 1 & 0x1; }
        }
        [CategoryAttribute("Stats")]
        public int ShieldDamage
        {
            get { return *(bushort*)(data + 17) >> 2 & 0x3F; }
        }
        [CategoryAttribute("Stats")]
        public int SFX
        {
            get { return *(bushort*)(data + 18) >> 2 & 0xFF; }
        }
        [CategoryAttribute("Flags")]
        public HBIFlags HurtBoxInteraction
        {
            get { return (HBIFlags)((data[19]) >> 0 & 0x3); }
        }
    }
}