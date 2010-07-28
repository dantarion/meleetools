using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib.Utility {
    interface INode<T> {
        T Next { get; set; }
    }
}
