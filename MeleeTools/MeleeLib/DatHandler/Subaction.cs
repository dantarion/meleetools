using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MeleeLib.DatHandler;

namespace MeleeLib
{
    public class Subaction
    {
        public int Index{get;set;}
        public SubactionHeader Header{get;set;}
        public List<ScriptCommand> Commands { get; set; }
        public String Name { get; set; }

    }

}
