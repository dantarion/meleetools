using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeLib.System.Node
{
   public abstract class Node<TRoot>
    {
       public abstract TRoot Root { get; }
    }
}
