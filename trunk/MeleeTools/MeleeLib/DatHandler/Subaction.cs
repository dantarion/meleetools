using System;
using System.Collections.Generic;

namespace MeleeLib.DatHandler
{
    public class Subaction
    {
        public int Index{get;set;}
        public SubactionHeader Header{get;set;}
        public List<ScriptCommand> Commands { get; set; }
        public String Name { get; set; }

    }

}
