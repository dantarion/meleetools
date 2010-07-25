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
        public const string DisplayFormat = "{0} [{1}]";
        public const string DisplayParamDelimiter = ", ";
        public static ScriptCommand Factory(byte* ptr)
        {
            var gc = new GenericCommand(ptr);
            switch (gc.Type)
            {
                case 0x01:
                case 0x02:
                    return new TimerCommand(ptr);
                case 0x03:
                    return new StartLoopCommand(ptr);
                case 0x05:
                case 0x07:
                    return new PointerCommand(ptr);
                case 0x0b:
                    return new HitboxCommand(ptr);
                case 0x13:
                case 0x34:
                    return new Generic4ByteCommand(ptr);
                case 0x19:
                    return new VisibilityCommand(ptr);
                case 0x1a:
                case 0x1b:
                    return new BodyStateCommand(ptr);
                case 0x1c:
                    return new PartialBodyStateCommand(ptr);
                case 0x88:
                    return new ThrowCommand(ptr);
            }
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
            dict[0x01] = new CommandData(0x04, "Synchronous Timer ");
            dict[0x02] = new CommandData(0x04, "Asynchronous Timer");
            dict[0x03] = new CommandData(0x04, "Start Loop");
            dict[0x04] = new CommandData(0x04, "Execute Loop");
            dict[0x05] = new CommandData(0x08, "Goto?");

            dict[0x07] = new CommandData(0x08, "Subroutine");

            dict[0x0a] = new CommandData(0x14, "GFX Effect");
            dict[0x0b] = new CommandData(0x14, "Hitbox");

            dict[0x10] = new CommandData(0x04, "Remove Hitboxes");
            dict[0x11] = new CommandData(0x0c, "Sound Effect");
            dict[0x12] = new CommandData(0x04, "Random Smash SFX");
            dict[0x13] = new CommandData(0x04, "Autocancel?");
            dict[0x14] = new CommandData(0x04, "Reverse Direction");

            dict[0x17] = new CommandData(0x04, "IASA");

            dict[0x19] = new CommandData(0x04, "Visibility?");
            dict[0x1a] = new CommandData(0x04, "Body State");
            dict[0x1b] = new CommandData(0x04, "Body State 2");
            dict[0x1c] = new CommandData(0x04, "Partial Body State");

            dict[0x1f] = new CommandData(0x04, "Model Mod");

            dict[0x33] = new CommandData(0x04, "Self-Damage");
            dict[0x34] = new CommandData(0x04, null);

            dict[0x36] = new CommandData(0x12, null);
            dict[0x37] = new CommandData(0x0c, null);
            dict[0x38] = new CommandData(0x08, "Start Smash Charge");

            dict[0x88] = new CommandData(0x88, "Throw");
        }
        static int getLength(uint type)
        {
            setupDict();
            return dict.ContainsKey(type) ? dict[type].length : 4;
        }
        protected string getName(uint type)
        {
            setupDict();
            if (dict.ContainsKey(type))
                if (DisplayParams == null)
                    return dict[type].name;
                else return String.Format(DisplayFormat, dict[type].name ?? String.Format("!Unknown 0x{0:X2}!", type),
                                          String.Join(DisplayParamDelimiter, DisplayParams));
            else return String.Format("!Unknown 0x{0:X2}!", type);

        }
        protected abstract string[] DisplayParams { get; }

        protected ScriptCommand(byte* dataptr)
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
                var sb = new StringBuilder();
                for (int i = 0; i < Length; i++)
                {
                    sb.AppendFormat("{0:X2} ", data[i]);
                }
                return sb.ToString();
            }
        }
        protected byte* data;
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ParamAttribute : System.Attribute
    {
    }

    public unsafe class GenericCommand : ScriptCommand
    {
        public GenericCommand(byte* ptr)
            : base(ptr)
        {

        }

        protected override string[] DisplayParams
        {
            get { return null; }
        }
    }
    public unsafe class BodyStateCommand : ScriptCommand
    {
        public BodyStateCommand(byte* dataptr)
            : base(dataptr)
        {
        }
        public enum BodyTypes
        {
            Normal = 0x0,
            Invulnerable = 0x1,
            Intangible = 0x2
        }
        protected override string[] DisplayParams
        {
            get { return new[] { BodyType.ToString() }; }
        }

        public BodyTypes BodyType
        {
            get { return (BodyTypes)(*(data + 3) & 0x3); }
        }
    }
    public unsafe class PartialBodyStateCommand : BodyStateCommand
    {
        public PartialBodyStateCommand(byte* dataptr)
            : base(dataptr)
        {
        }
        protected override string[] DisplayParams
        {
            get
            {
                return new[] { Bone.ToString(), BodyType.ToString() };
            }
        }
        public ushort Bone
        {
            get { return (ushort) (*(bushort*)(data)& 0x7F) ; }
        }
    }
    public unsafe class Generic4ByteCommand : ScriptCommand
    {
        public Generic4ByteCommand(byte* ptr)
            : base(ptr)
        {
        }
        public ushort Param1
        {
            get { return *(bushort*)(data + 2); }
        }
        protected override string[] DisplayParams
        {
            get { return new[] { Param1.ToString() }; }
        }
    }
    public unsafe class TimerCommand : ScriptCommand
    {
        public TimerCommand(byte* ptr)
            : base(ptr)
        {

        }

        protected override string[] DisplayParams
        {
            get { return new[] { Frames.ToString() }; }
        }

        [CategoryAttribute("Timer Params")]
        public ushort Frames
        {
            get { return *(bushort*)(data + 2); }
        }
    }
    public unsafe class HitboxCommand : CollisionCommand
    {
        public HitboxCommand(byte* ptr)
            : base(ptr)
        {

        }
        public enum HurtboxInteractionFlags
        {
            NoClank, SomeClank, MoreClank, AllClank
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
            get { return *(bushort*)(data + 1) >> 3 & 0x7F; }
        }
        public int Unknown0
        {
            get { return ((data[2] & 0x07) >> 1); }
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
        public int UnknownQ
        {
            get { return (data[15]) >> 2 & 0x7; }
        }
        [CategoryAttribute("Flags")]
        public HurtboxInteractionFlags HurtboxInteraction
        {
            get { return (HurtboxInteractionFlags)((data[15]) >> 0 & 0x3); }
        }
        [CategoryAttribute("Stats")]
        public int BaseKnockback
        {
            get { return *(bushort*)(data + 16) >> 7 & 0xFFFF; }
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
        public bool HitsGround
        {
            get { return Convert.ToBoolean((data[19]) >> 1 & 0x1); }
        }
        [CategoryAttribute("Flags")]
        public bool HitsAir
        {
            get { return Convert.ToBoolean((data[19]) >> 0 & 0x1); }
        }

        protected override unsafe string[] DisplayParams
        {
            get { return new string[] { ID.ToString() }; }
        }

    }
    public unsafe class VisibilityCommand : ScriptCommand
    {
        public VisibilityCommand(byte* dataptr)
            : base(dataptr)
        {
        }
        public enum VisibilityConstant
        {
            Invisible = 0x0,
            Visible = 0x1
        }
        public VisibilityConstant Visibility
        {
            get { return (VisibilityConstant)((data[3]) >> 0 & 0x1); }
        }

        protected override unsafe string[] DisplayParams
        {
            get { return new[] { Visibility.ToString() }; }
        }
    }
    public unsafe abstract class CollisionCommand : ScriptCommand
    {
        protected CollisionCommand(byte* dataptr)
            : base(dataptr)
        {
        }


        public enum ElementType
        {
            Normal = 0x00,
            Fire = 0x04,
            Electric = 0x08,
            Slash = 0x0C,
            Coin = 0x10,
            Ice = 0x14,
            Sleep = 0x18,
            Sleep2 = 0x1C,
            Grounded = 0x20,
            Grounded2 = 0x24,
            Cape = 0x28,
            Empty = 0x2C,
            Disabled = 0x30,
            ScrewAttack = 0x38,
            PoisonFlower = 0x3C,
            Nothing = 0x40
        }
        [CategoryAttribute("Stats")]
        public int Damage
        {
            get { return *(bushort*)(data + 2) >> 0 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public int KnockbackGrowth
        {
            get { return *(bushort*)(data + 13) >> 6 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public int WeightDependantKnockback
        {
            get { return *(bushort*)(data + 14) >> 5 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public ElementType Element
        {
            get { return (ElementType)(*(bushort*)(data + 17) >> 2 & 0x1F); }
        }
        [CategoryAttribute("Stats")]
        public int Angle
        {
            get { return *(bushort*)(data + 12) >> 7 & 0xFFFF; }
        }
    }
    public unsafe class ThrowCommand : CollisionCommand
    {
        public ThrowCommand(byte* dataptr)
            : base(dataptr)
        {
        }
        public enum ThrowTypes
        {
            Throw = 0x00,
            Release = 0x01,
        }
        public ThrowTypes ThrowType
        {
            get { return (ThrowTypes)(*(bushort*)(data) >> 7 & 0x1); }
        }

        protected override unsafe string[] DisplayParams
        {
            get { return new string[] { ThrowType.ToString() }; }
        }
    }
    public unsafe class StartLoopCommand : ScriptCommand
    {
        public StartLoopCommand(byte* dataptr)
            : base(dataptr)
        {
        }

        protected override string[] DisplayParams
        {
            get { return new[] { Iterations.ToString() }; }
        }

        [CategoryAttribute("Loop Params")]
        public ushort Iterations
        {
            get { return *(bushort*)(data + 2); }
        }
    }
    public unsafe class PointerCommand : ScriptCommand
    {

        public PointerCommand(byte* dataptr)
            : base(dataptr)
        {
        }

        protected override string[] DisplayParams
        {
            get { return new[] { String.Format("@{0:X8}", Pointer) }; }
        }
        [CategoryAttribute("Pointer Params")]
        public uint Pointer
        {
            get { return (*(uint*)(data + 4)).Reverse(); }
        }
    }
}