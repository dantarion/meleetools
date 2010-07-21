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
            string filename = @"W:\melee_hax\dats\Game & Watch\PlGw.dat";
            DatFile dat = new DatFile(filename);
            //PrettyPrint XD
            Console.WriteLine(dat.Filename);
            prettyPrint(dat.Header);
            Console.WriteLine("\nSection Type 1's");
            foreach (string name in dat.Section1Entries.Keys)
            {
                Console.WriteLine(name);
                prettyPrint(dat.Section1Entries[name]);
            }
            Console.WriteLine("\nSection Type 2's");
            foreach (string name in dat.Section2Entries.Keys)
            {
                Console.WriteLine(name);
                prettyPrint(dat.Section2Entries[name]);
            }
            Console.WriteLine("\nFTHeader");
            prettyPrint(dat.FTHeader);
            Console.WriteLine("\nAttributes");
            for (int i = 0; i < dat.Attributes.Count; i++)
            {
                Console.WriteLine("0x{0:X3} - {1}", i * 4, dat.Attributes[i]);
            }
            Console.ReadKey();
        }
    }
}
