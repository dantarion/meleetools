using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using MeleeLib.DatHandler;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {
    public abstract class ScriptCommand : IFilePiece {
        #region Static
        public const byte NullType = 0;
        internal static ScriptCommand Factory(ArraySlice<byte> data) {
            switch (TypeOf(data)) {
                case 0x00: return new EmptyCommand(data, "End");
                case 0x01: return new TimerCommand(data, "Synchronous Timer");
                case 0x02: return new TimerCommand(data, "Asynchronous Timer");
                case 0x03: return new StartLoopCommand(data);

                case 0x05: return new PointerCommand(data, "Goto?");

                case 0x07: return new PointerCommand(data, "Subroutine?");

                case 0x0A: return new UnsolvedCommand(data, "Graphic Effect", 0x14);
                case 0x0B: return new HitboxCommand(data);

                case 0x10: return new EmptyCommand(data, "Remove Hitboxes");
                case 0x11: return new UnsolvedCommand(data, "Sound Effect", 0xc);
                case 0x12: return new UnsolvedCommand(data, "Random Smash SFX");
                case 0x13: return new UnsolvedCommand(data, "Autocancel?");
                case 0x14: return new UnsolvedCommand(data, "Reverse Direction");

                case 0x17: return new EmptyCommand(data, "IASA");
                case 0x19: return new VisibilityCommand(data);
                case 0x1A: return new BodyStateCommand(data, "Body State 1");
                case 0x1B: return new BodyStateCommand(data, "Body State 2");
                case 0x1C: return new PartialBodyStateCommand(data);

                case 0x1F: return new UnsolvedCommand(data, "Model Mod");

                case 0x22: return new ThrowCommand(data);

                case 0x33: return new UnsolvedCommand(data, "Self-Damage");

                case 0x38: return new UnsolvedCommand(data, "Start Smash Charge", 0x8);

            }
            return new UnsolvedCommand(data);
        }
        protected ScriptCommand(ArraySlice<byte> data, string name = null, int size = 0x04) {
            data = data.Slice(0, size);
            Offset = data.Offset;
            Size = data.Count;
            Type = TypeOf(data);
            Name = name;
        }
        public static byte TypeOf(byte[] data) { return TypeOf(data[0]); }
        public static byte TypeOf(ArraySlice<byte> data) { return TypeOf(data[0]); }
        public static byte TypeOf(byte data) { return (byte)(data >> 2); }
        #endregion
        protected virtual string DisplayFormat { get { return "{0} [{1}]"; } }
        protected virtual string DisplayDelimiter { get { return " "; } }

        [CategoryAttribute("Name")]
        public string DisplayName { get { return DisplayParams == null ? Name : String.Format(DisplayFormat, Name, String.Join(DisplayDelimiter, DisplayParams)); } }

        [CategoryAttribute("Name")]
        public string Name { get; private set; }

        [CategoryAttribute("Internal")]
        public byte Type { get; private set; }

        protected abstract string[] DisplayParams { get; }

        [CategoryAttribute("Internal")]
        public string ToHexString {
            get {
                var sb = new StringBuilder();
                var data = RawData;
                for (int i = 0; i < Size; i++)
                    sb.AppendFormat("{0:X2} ", data[i]);
                return sb.ToString().Trim();
            }
        }

        [CategoryAttribute("Internal")]
        public int Offset { get; internal set; }

        [CategoryAttribute("Internal")]
        public int Size { get; internal set; }

        public abstract byte[] RawData { get; }
    }

    public class UnsolvedCommand : ScriptCommand {
        public class ParameterData : IEnumerable<byte> {
            private readonly ArraySlice<byte> _bytes;
            private ParameterData() { }
            internal ParameterData(ArraySlice<byte> bytes) {
                _bytes = bytes;
                bytes[0] &= 0x3;
            }
            public byte this[int i] {
                get { return _bytes[i]; }
                set {
                    if (value > 7) throw new IndexOutOfRangeException();
                    _bytes[i] = value;
                }
            }
            public int Size { get { return _bytes.Count; } }
            public IEnumerator<byte> GetEnumerator() { return ((IEnumerable<byte>)_bytes).GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
        public UnsolvedCommand(ArraySlice<byte> data, string name = null, int size = 0x04)
            : base(data, name, size) {
            _parameterData = new ParameterData(data);
        }

        protected override string[] DisplayParams {
            get {
                var displayParams = new string[_parameterData.Size];
                for (var i = 0; i < _parameterData.Size; i++)
                    displayParams[i] = String.Format("{0:2X}", _parameterData[i]);
                return displayParams;
            }
        }

        private readonly ParameterData _parameterData;
        public ParameterData Parameter;
        public override byte[] RawData {
            get {
                var parameterData = _parameterData.ToArray();
                parameterData[0] |= (byte)(Type << 2);
                return parameterData;
            }
        }

        //TODO: More helpful analysis properties
    }

    public enum BodyTypes {
        Normal = 0x0,
        Invulnerable = 0x1,
        Intangible = 0x2
    }
    public class BodyStateCommand : ScriptCommand {
        public BodyStateCommand(ArraySlice<byte> data, string name = null)
            : base(data, name) {
            Unknown1 = (byte)(data[0] & 0x3);
            Unknown2 = data[1];
            Unknown3 = data[2];
            BodyType = (BodyTypes)data[3];
        }


        protected override string[] DisplayParams {
            get { return new[] { BodyType.ToString() }; }
        }

        public override byte[] RawData {
            get { return new[] { (byte)(Type & Unknown1), Unknown2, Unknown3, (byte)BodyType }; }
        }


        //These are probably unused
        protected readonly byte Unknown1, Unknown2, Unknown3;
        [CategoryAttribute("Parameters")]
        public BodyTypes BodyType { get; set; }

    }

    public class PartialBodyStateCommand : BodyStateCommand {
        public PartialBodyStateCommand(ArraySlice<byte> data)
            : base(data, "Partial Body State") {
        }

        protected override string[] DisplayParams {
            get {
                return new[] { Bone.ToString(), BodyType.ToString() };
            }
        }
        [CategoryAttribute("Parameters")]
        public byte Bone {
            get { return Unknown2; }
        }
    }
    public class TimerCommand : ScriptCommand {

        public TimerCommand(ArraySlice<byte> data, string name)
            : base(data, name) {
            Frames = data.GetInt32() & 0x3FFF;
        }

        protected override string[] DisplayParams {
            get { return new[] { Frames.ToString() }; }
        }

        public override byte[] RawData {
            get {
                var bytes = BitConverter.GetBytes(Frames);
                bytes[0] |= Type;
                return bytes;
            }
        }

        [CategoryAttribute("Parameters")]
        public int Frames { get; set; }
    }

    public interface ICollisionCommand {

        int Damage { get; } //TODO set

        int KnockbackGrowth { get; } //TODO set

        int WeightDependantKnockback { get; } //TODO set

        int Angle { get; } //TODO set
    }
    public class HitboxCommand : ScriptCommand, ICollisionCommand {
        public enum ElementType {
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
        public enum HurtboxInteractionFlags {
            NoClank, SomeClank, MoreClank, AllClank
        }
        public HitboxCommand(ArraySlice<byte> data, string name = "Hitbox")
            : base(data, null, 0x14) {
            ID = data.GetInt16(0) >> 7 & 0x7;
            UnknownR = ((data[1] & 0x7B) >> 2);
            BoneID = data.GetInt16(1) >> 3 & 0x7F;
            UnknownO = data[2] >> 1 & 0x07;
            HitboxSize = data.GetInt16(4);
            ZOffset = data.GetInt16(6);
            YOffset = data.GetInt16(8);
            XOffset = data.GetInt16(10);
            UnknownQ = (data[15]) >> 2 & 0x7;
            HurtboxInteraction = (HurtboxInteractionFlags)((data[15]) >> 0 & 0x3);
            BaseKnockback = (short)(data.GetInt32(16) >> 7);
            UnknownV = (data[17]) >> 1 & 0x1;
            HitsAir = data.GetBoolean(19);
            Damage = data.GetInt16(2) & 0x1FF;
            KnockbackGrowth = data.GetInt16() >> 6 & 0x1FF;
            WeightDependantKnockback = data.GetInt16(14) >> 5 & 0x1FF;
            SFX = data.GetInt16(17) >> 2 & 0x3F;
            HitsGround = data.GetBoolean(19, 1);
            Element = (ElementType)(data.GetInt16(17) >> 2 & 0x1F);
            Angle = data.GetInt16(12) >> 7 & 0xFFFF;
        }

        public short HitboxSize { get; private set; }

        [CategoryAttribute("Identifier")]
        public int ID { get; private set; }

        [CategoryAttribute("Unknown")]
        public int UnknownR { get; private set; }

        [CategoryAttribute("Position")]
        public int BoneID { get; private set; }

        [CategoryAttribute("Unknown")]
        public int UnknownO { get; private set; }

        [CategoryAttribute("Position")]

        public override byte[] RawData {
            get { throw new NotImplementedException(); }
        }

        [CategoryAttribute("Position")]
        public int ZOffset { get; private set; }

        [CategoryAttribute("Position")]
        public int YOffset { get; private set; }

        [CategoryAttribute("Position")]
        public int XOffset { get; private set; }

        [CategoryAttribute("Unknown")]
        public int UnknownQ { get; private set; }

        [CategoryAttribute("Flags")]
        public HurtboxInteractionFlags HurtboxInteraction { get; private set; }

        [CategoryAttribute("Stats")]
        public short BaseKnockback { get; private set; }

        [CategoryAttribute("Unknown")]
        public int UnknownV { get; private set; }

        [CategoryAttribute("Stats")]
        public int ShieldDamage { get; private set; }

        [CategoryAttribute("Cosmetic")]
        public int SFX { get; private set; }

        [CategoryAttribute("Flags")]
        public bool HitsGround { get; private set; }

        [CategoryAttribute("Flags")]
        public bool HitsAir { get; private set; }

        protected override string[] DisplayParams {
            get { return new string[] { ID.ToString() }; } //TODO
        }

        public int Damage { get; private set; }

        public int KnockbackGrowth { get; private set; }

        public int WeightDependantKnockback { get; private set; }

        public ElementType Element { get; private set; }

        public int Angle { get; private set; }
    }
    public class VisibilityCommand : OneBitCommand {
        public VisibilityCommand(ArraySlice<byte> data)
            : base(data, "Visibility") {
            Visible = data.GetBoolean(3);
        }

        [CategoryAttribute("Parameters")]
        public bool Visible { get; set; }

        protected override string[] DisplayParams {
            get { return new[] { Visible ? "Visible" : "Invisible" }; }
        }

        public override byte[] RawData {
            get { return OneBitRawData(Visible); }
        }
    }
    public class ThrowCommand : ScriptCommand, ICollisionCommand {

        public enum ElementType {
            Normal = 0x0,
            Fire = 0x1,
            Electric = 0x2,
            Ice = 0x5,
            Darkness = 0xD
        }
        public ThrowCommand(ArraySlice<byte> data)
            : base(data, "Throw", 0xC) {
            Damage = data.GetInt16(2) >> 0 & 0x1FF;
            KnockbackGrowth = data.GetInt16(5) >> 6 & 0x1FF;
            Element = (ElementType)(data.GetInt16(9) >> 3 & 0xF);
            Angle = data.GetInt16(4) >> 7 & 0x1FF;
            BaseKnockback = data.GetInt16(8) >> 7 & 0x1FF;
            ThrowType = (ThrowTypes)(data.GetInt16(0) >> 7 & 0x7);
        }

        protected ThrowTypes ThrowType { get; private set; }

        public enum ThrowTypes {
            Throw = 0x00,
            Release = 0x01,
        }
        [CategoryAttribute("Stats")]
        public int Damage { get; private set; }

        [CategoryAttribute("Stats")]
        public int KnockbackGrowth { get; private set; }

        [CategoryAttribute("Stats")]
        public int WeightDependantKnockback { get; private set; }

        [CategoryAttribute("Cosmetic")]
        public ElementType Element { get; private set; }

        [CategoryAttribute("Stats")]
        public int Angle { get; private set; }

        [CategoryAttribute("Stats")]
        public int BaseKnockback { get; private set; }

        protected override string[] DisplayParams { get { return new[] { ThrowType.ToString() }; } } //TODO 

        public override byte[] RawData {
            get { throw new NotImplementedException(); }
        }
    }
    public abstract class OneByteCommand : ScriptCommand {
        protected OneByteCommand(ArraySlice<byte> data, string name = null, int size = 4)
            : base(data, name, size) {

            Unknown1 = (byte)(data[0] & 0x3);
            Unknown2 = data[1];
            Unknown3 = data[2];
        }
        protected readonly byte Unknown1, Unknown2, Unknown3;
    }
    public abstract class OneBitCommand : OneByteCommand {
        protected OneBitCommand(ArraySlice<byte> data, string name = null, int size = 4)
            : base(data, name, size) {
            Unknown4 = (byte)(data[3] & 0xE);
        }
        protected byte[] OneBitRawData(bool bit) {
            return new[] { (byte)(Type | Unknown1), Unknown2, Unknown3, (byte)(Unknown4 | (bit ? 1 : 0)) };
        }
        protected readonly byte Unknown4;
    }
    public class StartLoopCommand : ScriptCommand {
        public StartLoopCommand(ArraySlice<byte> data)
            : base(data, "Start Loop") {

            Iterations = data.GetInt16(2);
        }

        protected override string[] DisplayParams {
            get { return new[] { Iterations.ToString() }; }
        }

        public override byte[] RawData {
            get {
                var bytes = BitConverter.GetBytes(Iterations);
                bytes[0] |= Type;
                return bytes;
            }
        }

        [CategoryAttribute("Parameters")]
        public short Iterations { get; set; }
    }
    public class PointerCommand : ScriptCommand {
        public PointerCommand(ArraySlice<byte> datadata, string name)
            : base(datadata, name, 0x8) {
            Pointer = datadata.GetUInt32() & 0x3FFF;
        }

        protected override string[] DisplayParams {
            get { return new[] { String.Format("@{0:X8}", Pointer) }; }
        }

        public override byte[] RawData {
            get {
                var bytes = BitConverter.GetBytes(Pointer);
                bytes[0] |= Type;
                return bytes;
            }
        }

        [CategoryAttribute("Parameters")]
        public uint Pointer { get; set; }
    }
    public class EmptyCommand : ScriptCommand {
        public EmptyCommand(ArraySlice<byte> data, string name = null, int size = 4)
            : base(data, name, size) {
            _rawData = data;
            _rawData[0] &= 0x3;
        }

        protected override string[] DisplayParams {
            get { return null; }
        }

        private readonly ArraySlice<byte> _rawData;
        public override byte[] RawData { get { return _rawData.ToArray(); } }
    }
    public interface IBrawlCommand {
        uint CommandID { get; }
        ArraySlice<byte> ParameterData { get; }
    }
    public class IncompatibleCommandException : ArgumentException { }

}