using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib
{
    class Attributes
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class OffsetAttribute : System.Attribute
        {
        }

    }
}
