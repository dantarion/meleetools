﻿using System;
using MeleeLib.Utility;

namespace MeleeLib.DatHandler {

    public class Attribute : IFilePiece, IHasIndex {
        public int Index { get; internal set; }
        private Attribute() { }
        internal Attribute(int index, object value) { Index = index; Value = value; }
        public string Name {
            get {
                switch (Index * 4) {
                    case 0x000: return "Walk Initial Velocity";
                    case 0x004: return "Walk Acceleration?";
                    case 0x008: return "Walk Maximum Velocity";
                    case 0x00C: return "Walk Animation Speed";
                    case 0x014: return "Same as 0xC?";
                    case 0x018: return "Friction";
                    case 0x01C: return "Dash Initial Velocity";
                    case 0x020: return "StopTurn Initial Velocity";
                    case 0x028: return "Run Initial Velocity?";
                    case 0x030: return "Run Acceleration";
                    case 0x038: return "Jump Startup Lag (Frames)";
                    case 0x03C: return "same as 0x034?";
                    case 0x040: return "Jump V Initial Velocity???";
                    case 0x044: return "same as 0x03C";
                    case 0x048: return "Jump H Initial Velocity???";
                    case 0x04C: return "Hop V Initial Velocity???";
                    case 0x050: return "Air Jump Multiplier";
                    case 0x054: return "Same as 0x04C";
                    case 0x058: return "Number of Jumps";
                    case 0x05C: return "Gravity";
                    case 0x060: return "Terminal Velocity";
                    case 0x064: return "Aerial Mobility";
                    case 0x068: return "Aerial Stopping Mobility";
                    case 0x06C: return "Max Aerial H Velocity";
                    case 0x070: return "same as 0x088?";
                    case 0x074: return "Fast Fall Terminal Velocity";
                    case 0x078: return "0024?";
                    case 0x088: return "Weight";
                    case 0x08C: return "Model Scaling";
                    case 0x090: return "Shield Size";
                    case 0x094: return "Shield Break Initial Velocity";
                    case 0x0A8: return "Ledgejump Horizontal Velocity";
                    case 0x0AC: return "Ledgejump Vertical Velocity";
                    case 0x0DC: return "Normal Landing Lag";
                    case 0x0E0: return "N-Air Landing Lag";
                    case 0x0EC: return "F-Air Landing Lag";
                    case 0x0F0: return "B-Air Landing Lag";
                    case 0x0F4: return "D-Air Landing Lag";
                    case 0x0F8: return "U-Air Landing Lag";
                    case 0x160: return "Ice Traction?";
                    case 0x17C: return "Special Jump Action = -1...?";
                }
                return "";
            }
        }
        public Type Type {
            get {
                switch (Index * 4) {
                    case 0x58:
                    case 0xa4:
                    case 0x98:
                    case 0x16c:
                        return typeof(int);
                }
                return typeof(float);
            }
        }

        public object Value { get; internal set; }
        //TODO
        //if (Type == typeof(int)) return RawData.GetInt32();
        //if (Type == typeof(float)) return RawData.GetSingle();
        //throw new ApplicationException(String.Format("Attribute 0x{0:X3} was an invalid type.", Offset));
        public new string ToString {
            get {
                return String.Format("{3} 0x{0:X4} {1,-30} = {2}", Index * 4, Name, Value, Type == typeof(int) ? "*" : " ");
            }
        }
        public int Offset { get; internal set; }
        public int Size { get; internal set; }

        public byte[] RawData {
            get { throw new NotImplementedException(); }
        }
    }
}

