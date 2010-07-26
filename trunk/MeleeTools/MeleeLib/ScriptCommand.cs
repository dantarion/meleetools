using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace MeleeLib
{
    public unsafe class ScriptCommand
    {
        public static ScriptCommand Factory(byte* data)
        {
            switch ((uint)data[0] >> 2)
            {
                case 0x00: return new ScriptCommand(data, "End");
                case 0x01: return new TimerCommand(data, "Synchronous Timer");
                case 0x02: return new TimerCommand(data, "Asynchronous Timer");
                case 0x03: return new StartLoopCommand(data);

                case 0x05: return new PointerCommand(data, "Goto?");

                case 0x07: return new PointerCommand(data, "Subroutine?");

                case 0x0a: return new UnsolvedCommand(data, "Graphic Effect", 0x14);
                case 0x0b: return new HitboxCommand(data);

                case 0x10: return new ScriptCommand(data, "Remove Hitboxes");
                case 0x11: return new UnsolvedCommand(data, "Sound Effect");
                case 0x12: return new UnsolvedCommand(data, "Random Smash SFX");
                case 0x13: return new UnsolvedCommand(data, "Autocancel?");
                case 0x14: return new UnsolvedCommand(data, "Reverse Direction");

                case 0x17: return new ScriptCommand(data, "IASA");
                case 0x19: return new VisibilityCommand(data);
                case 0x1a: return new BodyStateCommand(data, "Body State 1");
                case 0x1b: return new BodyStateCommand(data, "Body State 2");
                case 0x1c: return new PartialBodyStateCommand(data);

                case 0x1f: return new UnsolvedCommand(data, "Model Mod");

                case 0x28: return new UnsolvedCommand(data, 0x8);

                case 0x33: return new UnsolvedCommand(data, "Self-Damage");

                case 0x38: return new UnsolvedCommand(data, "Start Smash Charge");

                case 0x88: return new ThrowCommand(data);
            }
            return new UnsolvedCommand(data);
        }


        protected ScriptCommand(byte* dataptr)
            : this(dataptr, null, 0x4) { }
        protected ScriptCommand(byte* ptr, uint length)
            : this(ptr, null, length) { }
        protected ScriptCommand(byte* dataptr, string name)
            : this(dataptr, name, 0x4) { }
        protected ScriptCommand(byte* dataptr, string name, uint length)
        {
            Data = dataptr;
            Name = name;
            Length = length;
        }

        private string _name;
        [CategoryAttribute("Internal")]
        public string Name
        {
            get { return _name ?? String.Format("!Unknown 0x{0:X2}!", Type); }
            set { _name = value; }
        }

        [CategoryAttribute("Internal")]
        public uint Type { get { return (uint)Data[0] >> 2; } }

        uint _length;
        [CategoryAttribute("Internal")]
        public uint Length
        {
            get { return _length == 0 ? 4 : _length; }
            set { _length = value; }
        }
        protected string[] DisplayParams
        {
            get { return null; }
        }

        [CategoryAttribute("Internal")]
        public string HexString
        {
            get
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Length; i++)
                {
                    sb.AppendFormat("{0:X2} ", Data[i]);
                }
                return sb.ToString();
            }
        }
        protected byte* Data;


    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ParamAttribute : System.Attribute
    {
    }

    public unsafe class UnsolvedCommand : ScriptCommand
    {
        public UnsolvedCommand(byte* ptr)
            : base(ptr)
        {

        }

        public UnsolvedCommand(byte* ptr, string name, uint length)
            : base(ptr, name, length) { }

        public UnsolvedCommand(byte* ptr, string name)
            : base(ptr, name) { }
        public UnsolvedCommand(byte* ptr, uint length) : base(ptr, length) { }
        protected new string[] DisplayParams
        {
            get { return null; }
        }
    }
    public unsafe class BodyStateCommand : ScriptCommand
    {
        public BodyStateCommand(byte* dataptr)
            : base(dataptr) { }

        public BodyStateCommand(byte* dataptr, string name)
            : base(dataptr, name) { }

        public enum BodyTypes
        {
            Normal = 0x0,
            Invulnerable = 0x1,
            Intangible = 0x2
        }
        protected new string[] DisplayParams
        {
            get { return new[] { BodyType.ToString() }; }
        }

        public BodyTypes BodyType
        {
            get { return (BodyTypes)(*(Data + 3) & 0x3); }
        }
    }
    public unsafe class Unknown28Command : ScriptCommand
    {
        public Unknown28Command(byte* dataptr)
            : base(dataptr)
        {
        }
        public UInt32 Flags { get { return *(buint*)(Data) & 0x3FFF; } }

        protected new string[] DisplayParams
        {
            get { return new[] { Convert.ToString(Flags, 2).PadLeft(16, '0') }; }
        }
    }
    public unsafe class PartialBodyStateCommand : BodyStateCommand
    {
        public PartialBodyStateCommand(byte* dataptr)
            : base(dataptr)
        {
        }
        protected new string[] DisplayParams
        {
            get
            {
                return new[] { Bone.ToString(), BodyType.ToString() };
            }
        }
        public ushort Bone
        {
            get { return (ushort)(*(bushort*)(Data) & 0x7F); }
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
            get { return *(bushort*)(Data + 2); }
        }
        protected new string[] DisplayParams
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

        public TimerCommand(byte* ptr, string name)
            : base(ptr, name)
        {
        }

        protected new string[] DisplayParams
        {
            get { return new[] { Frames.ToString() }; }
        }

        [CategoryAttribute("Timer Params")]
        public ushort Frames
        {
            get { return *(bushort*)(Data + 2); }
        }
    }
    public unsafe class HitboxCommand : CollisionCommand
    {
        public HitboxCommand(byte* ptr)
            : base(ptr)
        {

        }

        private string _name;
        public new string Name { get { return _name ?? "Hitbox"; } set { _name = value; } }
        public enum HurtboxInteractionFlags
        {
            NoClank, SomeClank, MoreClank, AllClank
        }
        public int ID
        {
            get { return *(bushort*)(Data) >> 7 & 0x7; }
        }
        public int UnknownR
        {
            get { return ((Data[1] & 0x7B) >> 2); }
        }
        [CategoryAttribute("Stats")]
        public int BoneID
        {
            get { return *(bushort*)(Data + 1) >> 3 & 0x7F; }
        }
        public int Unknown0
        {
            get { return ((Data[2] & 0x07) >> 1); }
        }
        [CategoryAttribute("Position")]
        public int Size
        {
            get { return *(bushort*)(Data + 4) >> 0 & 0xFFFF; }
        }
        [CategoryAttribute("Position")]
        public int ZOffset
        {
            get { return *(bshort*)(Data + 6); }
        }
        [CategoryAttribute("Position")]
        public int YOffset
        {
            get { return *(bshort*)(Data + 8); }
        }
        [CategoryAttribute("Position")]
        public int XOffset
        {
            get { return *(bshort*)(Data + 10); }
        }
        public int UnknownQ
        {
            get { return (Data[15]) >> 2 & 0x7; }
        }
        [CategoryAttribute("Flags")]
        public HurtboxInteractionFlags HurtboxInteraction
        {
            get { return (HurtboxInteractionFlags)((Data[15]) >> 0 & 0x3); }
        }
        [CategoryAttribute("Stats")]
        public int BaseKnockback
        {
            get { return *(bushort*)(Data + 16) >> 7 & 0xFFFF; }
        }
        public int UnknownV
        {
            get { return (Data[17]) >> 1 & 0x1; }
        }
        [CategoryAttribute("Stats")]
        public int ShieldDamage
        {
            get { return *(bushort*)(Data + 17) >> 2 & 0x3F; }
        }
        [CategoryAttribute("Stats")]
        public int SFX
        {
            get { return *(bushort*)(Data + 18) >> 2 & 0xFF; }
        }
        [CategoryAttribute("Flags")]
        public bool HitsGround
        {
            get { return Convert.ToBoolean((Data[19]) >> 1 & 0x1); }
        }
        [CategoryAttribute("Flags")]
        public bool HitsAir
        {
            get { return Convert.ToBoolean((Data[19]) >> 0 & 0x1); }
        }

        protected new string[] DisplayParams
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
            get { return (VisibilityConstant)((Data[3]) >> 0 & 0x1); }
        }

        protected new string[] DisplayParams
        {
            get { return new[] { Visibility.ToString() }; }
        }
    }
    public unsafe abstract class CollisionCommand : ScriptCommand
    {
        protected CollisionCommand(byte* dataptr)
            : base(dataptr, 0x14){}
        protected CollisionCommand(byte* dataptr, uint length) : base(dataptr,length){}

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
            get { return *(bushort*)(Data + 2) >> 0 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public int KnockbackGrowth
        {
            get { return *(bushort*)(Data + 13) >> 6 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public int WeightDependantKnockback
        {
            get { return *(bushort*)(Data + 14) >> 5 & 0x1FF; }
        }
        [CategoryAttribute("Stats")]
        public ElementType Element
        {
            get { return (ElementType)(*(bushort*)(Data + 17) >> 2 & 0x1F); }
        }
        [CategoryAttribute("Stats")]
        public int Angle
        {
            get { return *(bushort*)(Data + 12) >> 7 & 0xFFFF; }
        }
    }
    public unsafe class ThrowCommand : CollisionCommand
    {
        public ThrowCommand(byte* dataptr)
            : base(dataptr,0xc){}
        public enum ThrowTypes
        {
            Throw = 0x00,
            Release = 0x01,
        }
        public ThrowTypes ThrowType
        {
            get { return (ThrowTypes)(*(bushort*)(Data) >> 7 & 0x1); }
        }

        protected new string[] DisplayParams
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

        public StartLoopCommand(byte* dataptr, string name)
            : base(dataptr, name)
        {
        }

        protected new string[] DisplayParams
        {
            get { return new[] { Iterations.ToString() }; }
        }

        [CategoryAttribute("Loop Params")]
        public ushort Iterations
        {
            get { return *(bushort*)(Data + 2); }
        }
    }
    public unsafe class PointerCommand : ScriptCommand
    {

        public PointerCommand(byte* dataptr)
            : base(dataptr, 0x8)
        {
        }

        public PointerCommand(byte* dataptr, string name)
            : base(dataptr, name, 0x8)
        {
        }

        protected new string[] DisplayParams
        {
            get { return new[] { String.Format("@{0:X8}", Pointer) }; }
        }
        [CategoryAttribute("Pointer Params")]
        public uint Pointer
        {
            get { return (*(uint*)(Data + 4)).Reverse(); }
        }
    }
    public interface IBrawlCommand
    {
        uint CommandID { get; }
        byte[] ParameterData { get; }
    }
    public class IncompatibleCommandException : ArgumentException { }
}