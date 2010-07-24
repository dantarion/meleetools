using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib
{

    public struct Attribute
    {
        static Dictionary<int, string> dict;
        static String getName(int offset)
        {
            if (dict == null)
            {
                dict = new Dictionary<int, string>();
                dict[0x000] = "Walk Initial Velocity";
                dict[0x004] = "Walk Acceleration?";
                dict[0x008] = "Walk Maximum Velocity";
                dict[0x00C] = "Walk Animation Speed";
                dict[0x014] = "Same as 0xC?";
                dict[0x018] = "Friction";
                dict[0x01C] = "Dash Initial Velocity";
                dict[0x020] = "StopTurn Initial Velocity";
                dict[0x028] = "Run Initial Velocity?";
                dict[0x030] = "Run Acceleration";
                dict[0x038] = "Jump Startup Lag (Frames)";
                dict[0x03C] = "same as 0x034?";
                dict[0x040] = "Jump V Initial Velocity???";
                dict[0x044] = "same as 0x03C";
                dict[0x048] = "Jump H Initial Velocity???";
                dict[0x04C] = "Hop V Initial Velocity???";
                dict[0x050] = "Air Jump Multiplier";
                dict[0x054] = "Same as 0x04C";
                dict[0x058] = "Number of Jumps";
                dict[0x05C] = "Gravity";
                dict[0x060] = "Terminal Velocity";
                dict[0x064] = "Aerial Mobility";
                dict[0x068] = "Aerial Stopping Mobility";
                dict[0x06C] = "Max Aerial H Velocity";
                dict[0x070] = "same as 0x088?";
                dict[0x074] = "Fast Fall Terminal Velocity";
                dict[0x078] = "0024?";
                dict[0x088] = "Weight";
                dict[0x08C] = "Model Scaling";
                dict[0x090] = "Shield Size";
                dict[0x094] = "Shield Break Initial Velocity";
                dict[0x0A8] = "Ledgejump Horizontal Velocity";
                dict[0x0AC] = "Ledgejump Vertical Velocity";
                dict[0x000] = "Normal Landing Lag";
                dict[0x000] = "N-Air Landing Lag";
                dict[0x0EC] = "F-Air Landing Lag";
                dict[0x0F0] = "B-Air Landing Lag";
                dict[0x0F4] = "D-Air Landing Lag";
                dict[0x0F8] = "U-Air Landing Lag";
                dict[0x160] = "Ice Traction?";
                dict[0x17C] = "Special Jump Action = -1...?";
            }
            return dict.ContainsKey(offset) ? dict[offset] : "";
        }
        public int Offset { get; set; }
        public string Name { get { return getName(Offset); } }
        public object Value { get; set; }


    }
}
