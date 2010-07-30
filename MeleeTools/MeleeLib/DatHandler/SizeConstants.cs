using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib.DatHandler {
  public static  class SizeOf {
      public const int
          Offset                 = 0x04,
          FileHeader             = 0x20,
          Attribute              = 0x04,
          FtDefinition           = 0x08,
          SectionType2Definition = 0x08,
          SubactionDefinition    = 0x18
          ;
      }
}
