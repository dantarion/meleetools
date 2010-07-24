using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeleeLib;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
namespace ConsoleApplication1
{
    class Program
    {
        static void prettyPrint(object o)
        {
            foreach (PropertyInfo pi in o.GetType().GetProperties())
            {
                if (pi.Name.Contains("Offset") && !pi.Name.Contains("Count"))
                    Console.WriteLine("{0:6} - @0x{1:X8}", pi.Name, pi.GetValue(o, null));
                else
                    Console.WriteLine("{0:6} - {1}", pi.Name, pi.GetValue(o, null));
            }
        }
        static void Main(string[] args)
        {

        }
    }
}
