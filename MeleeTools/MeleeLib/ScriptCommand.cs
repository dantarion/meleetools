using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using MeleeLib.System;

namespace MeleeLib
{
    public class ScriptCommand
    {
        public static ScriptCommand Factory(ArraySegment<byte> data)
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
                case 0x11: return new UnsolvedCommand(data, "Sound Effect", 0xc);
                case 0x12: return new UnsolvedCommand(data, "Random Smash SFX");
                case 0x13: return new UnsolvedCommand(data, "Autocancel?");
                case 0x14: return new UnsolvedCommand(data, "Reverse Direction");

                case 0x17: return new ScriptCommand(data, "IASA");
                case 0x19: return new VisibilityCommand(data);
                case 0x1a: return new BodyStateCommand(data, "Body State 1");
                case 0x1b: return new BodyStateCommand(data, "Body State 2");
                case 0x1c: return new PartialBodyStateCommand(data);

                case 0x1f: return new UnsolvedCommand(data, "Model Mod");

                case 0x22: return new ThrowCommand(data);

                case 0x33: return new UnsolvedCommand(data, "Self-Damage");

                case 0x38: return new UnsolvedCommand(data, "Start Smash Charge", 0x8);

            }
            return new UnsolvedCommand(data);
        }

        protected const string DisplayFormat = "{0} [{1}]";
        protected const string DisplayDelimiter = " ";

        [CategoryAttribute("Name")]
        public string DisplayName
        {
            get { return DisplayParams == null ? Name : String.Format(DisplayFormat, Name, String.Join(DisplayDelimiter, DisplayParams)); }
        }
        protected ScriptCommand(byte[] data)
            : this(data, null, 0x4) { }
        protected ScriptCommand(byte[] data, uint length)
            : this(data, null, length) { }
        protected ScriptCommand(byte[] data, string name)
            : this(data, name, 0x4) { }
        protected ScriptCommand(ArraySegment<byte> data, string name, uint length)
        {
            Data = new ArraySegment<
            Array.Copy(data.Array, data.Offset, Data, 0, Length);
            Name = name;
            Length = length;
        }

        private string _name;
        [CategoryAttribute("Name")]
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
        virtual protected string[] DisplayParams
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
                    Data.
                    sb.AppendFormat("{0:X2} ", Data[i]);
                }
                return sb.ToString();
            }
        }
        protected ArraySegment<byte> Data;
        

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ParamAttribute : System.Attribute
    {
    }

    public class UnsolvedCommand : ScriptCommand
    {
        public UnsolvedCommand(byte* ptr)
            : base(ptr) { }

        public UnsolvedCommand(byte* ptr, string name, uint length)
            : base(ptr, name, length) { }

        public UnsolvedCommand(byte* ptr, string name)
            : base(ptr, name) { }
        public UnsolvedCommand(byte* ptr, uint length) : base(ptr, length) { }
        protected override string[] DisplayParams
        {
            get { return null; }
        }
    }
    public class BodyStateCommand : ScriptCommand
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
        protected override string[] DisplayParams
        {
            get { return new[] { BodyType.ToString() }; }
        }

        [CategoryAttribute("Parameters")]
        public BodyTypes BodyType
        {
            get { return (BodyTypes)(*(Data + 3) & 0x3); }
        }
    }
    public class PartialBodyStateCommand : BodyStateCommand
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
        [CategoryAttribute("Parameters")]
        public ushort Bone
        {
            get { return (ushort)(*(bushort*)(Data) & 0x7F); }
        }
    }
    public class TimerCommand : ScriptCommand
    {
        public TimerCommand(byte[] data)
            : base(data) { }

        public TimerCommand(byte[] data, string name)
            : base(data, name)
        {
        }

        protected override string[] DisplayParams
        {
            get { return new[] { Frames.ToString() }; }
        }


        [CategoryAttribute("Parameters")]
        public ushort Frames
        {
            get { return *(bushort*)(Data + 2); }
        }
    }
    public class HitboxCommand : CollisionCommand
    {
        public HitboxCommand(byte* ptr)
            : base(ptr, "Hitbox", 0x14) { }

        public enum HurtboxInteractionFlags
        {
            NoClank, SomeClank, MoreClank, AllClank
        }
        [CategoryAttribute("Identifier")]
        public int ID
        {
            get { return *(bushort*)(Data) >> 7 & 0x7; }
        }

        [CategoryAttribute("Unknown")]
        public int UnknownR
        {
            get { return ((Data[1] & 0x7B) >> 2); }
        }
        [CategoryAttribute("Position")]
        public int BoneID
        {
            get { return *(bushort*)(Data + 1) >> 3 & 0x7F; }
        }
        [CategoryAttribute("Unknown")]
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
        [CategoryAttribute("Unknown")]
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
        [CategoryAttribute("Unknown")]
        public int UnknownV
        {
            get { return (Data[17]) >> 1 & 0x1; }
        }
        [CategoryAttribute("Stats")]
        public int ShieldDamage
        {
            get { return *(bushort*)(Data + 17) >> 2 & 0x3F; }
        }
        [CategoryAttribute("Cosmetic")]
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

        protected override string[] DisplayParams
        {
            get { return new string[] { ID.ToString() }; }
        }

        public override int Damage
        {
            get { return *(bushort*)(Data + 2) >> 0 & 0x1FF; }
        }

        public override int KnockbackGrowth
        {
            get { return *(bushort*)(Data + 13) >> 6 & 0x1FF; }
        }

        public override int WeightDependantKnockback
        {
            get { return *(short*)(Data + 14) >> 5 & 0x1FF; }
        }

        public override ElementType Element
        {
            get { return (ElementType)(*(bushort*)(Data + 17) >> 2 & 0x1F); }
        }

        public override int Angle
        {
            get { return *(bushort*)(Data + 12) >> 7 & 0xFFFF; }
        }
    }
    public class VisibilityCommand : ScriptCommand
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
        [CategoryAttribute("Parameters")]
        public VisibilityConstant Visibility
        {
            get { return (VisibilityConstant)((Data[3]) >> 0 & 0x1); }
        }

        protected override string[] DisplayParams
        {
            get { return new[] { Visibility.ToString() }; }
        }
    }
    public abstract class CollisionCommand : ScriptCommand
    {
        protected CollisionCommand(byte* dataptr) : base(dataptr) { }
        protected CollisionCommand(byte* dataptr, uint length) : base(dataptr, length) { }
        protected CollisionCommand(byte* dataptr, string name, uint length) : base(dataptr, name, length) { }

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
        public abstract int Damage { get; }

        [CategoryAttribute("Stats")]
        public abstract int KnockbackGrowth { get; }

        [CategoryAttribute("Stats")]
        public abstract int WeightDependantKnockback { get; }

        [CategoryAttribute("Cosmetic")]
        public abstract ElementType Element { get; }

        [CategoryAttribute("Stats")]
        public abstract int Angle { get; }

    }
    public class ThrowCommand : CollisionCommand
    {
        public ThrowCommand(byte* dataptr)
            : base(dataptr, "Throw", 0xc) { }
        public enum ThrowTypes
        {
            Throw = 0x00,
            Release = 0x01,
        }
        public ThrowTypes ThrowType
        {
            get { return (ThrowTypes)(*(bushort*)(Data) >> 7 & 0x1); }
        }

        protected override string[] DisplayParams
        {
            get { return new string[] { ThrowType.ToString() }; }
        }

        public override int Damage
        {
            get { throw new NotImplementedException(); }
        }

        public override int KnockbackGrowth
        {
            get { throw new NotImplementedException(); }
        }

        public override int WeightDependantKnockback
        {
            get { throw new NotImplementedException(); }
        }

        public override ElementType Element
        {
            get { throw new NotImplementedException(); }
        }

        public override int Angle
        {
            get { throw new NotImplementedException(); }
        }
    }
    public class StartLoopCommand : ScriptCommand
    {
        public StartLoopCommand(byte* dataptr)
            : base(dataptr)
        {
        }

        public StartLoopCommand(byte* dataptr, string name)
            : base(dataptr, name)
        {
        }

        protected override string[] DisplayParams
        {
            get { return new[] { Iterations.ToString() }; }
        }

        [CategoryAttribute("Loop Params")]
        public ushort Iterations
        {
            get { return *(bushort*)(Data + 2); }
        }
    }
    public class PointerCommand : ScriptCommand
    {

        public PointerCommand(byte* dataptr)
            : base(dataptr, 0x8)
        {
        }

        public PointerCommand(byte* dataptr, string name)
            : base(dataptr, name, 0x8)
        {
        }

        protected override string[] DisplayParams
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