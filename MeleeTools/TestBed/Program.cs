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
        unsafe static void Main(string[] args)
        {
            DatHeader header;
            Dictionary<String,SectionHeader> section1s = new Dictionary<String,SectionHeader>();
            Dictionary<String, SectionHeader> section2s = new Dictionary<String, SectionHeader>();
            FTHeader ftheader;
            List<object> attributes = new List<object>();
            string filename = @"W:\melee_hax\dats\Game & Watch\PlGw.dat";
            //Load up file
            byte[] data = File.ReadAllBytes(filename);
            //Get the header
            fixed (byte* ptr = data)
            {
                header = *(DatHeader*)ptr;
            }
            //Compute offset base for the section name strings (They are near the end of the file)
            uint stringoffsetbase = 0x20 + header.Datasize + header.OffsetCount * 4 + header.SectionType1Count * 8 + header.SectionType2Count * 8;
            //Read SectionType1s
            for (int i = 0; i < header.SectionType1Count; i++)
            {
                fixed (byte* ptr = data)
                {
                    SectionHeader section = *(SectionHeader*)(0x20 + ptr + header.Datasize + header.OffsetCount * 4 + i*8);
                    string name = new String((sbyte*)ptr + stringoffsetbase+ section.StringOffset);
                    section1s[name] = section;     
                }
            }
            //Read SectionType2s
            for (int i = 0; i < header.SectionType2Count; i++)
            {
                fixed (byte* ptr = data)
                {
                    SectionHeader section = *(SectionHeader*)(0x20 + ptr + header.Datasize + header.OffsetCount * 4 + header.SectionType1Count*8 + i*8);
                    Console.WriteLine(section.StringOffset);
                    string name = new String((sbyte*)ptr + stringoffsetbase + section.StringOffset);
                    section2s[name] = section;

                }
            }
            //FTHeader
            fixed (byte* ptr = data)
            {
                uint[] INT_ATTRIBUTES = {0x58,0xa4,0x98,0x16c};
                ftheader = *(FTHeader*)(0x20 + ptr + section1s.Values.First().DataOffset);
                byte* cur = ftheader.AttributesOffset + ptr+0x20;
                byte* end = ftheader.AttributesOffset2 + ptr + 0x20;
                uint i = 0;
                while (cur < end)
                {
                    if(!INT_ATTRIBUTES.Contains(i))
                        attributes.Add((float)*(bfloat*)cur);
                    else
                        attributes.Add((uint)*(buint*)cur);
                    i += 4;
                    cur += 4;
                }

            }

            //PrettyPrint XD
            Console.WriteLine(filename);
            prettyPrint(header);
            Console.WriteLine("\nSection Type 1's");
            foreach (string name in section1s.Keys)
            {
                Console.WriteLine(name);
                prettyPrint(section1s[name]);
            }
            Console.WriteLine("\nSection Type 2's");
            foreach (string name in section2s.Keys)
            {
                Console.WriteLine(name);
                prettyPrint(section2s[name]);
            }
            Console.WriteLine("\nFTHeader");
            prettyPrint(ftheader);
            Console.WriteLine("\nAttributes");
            for (int i = 0; i < attributes.Count; i++)
            {
                Console.WriteLine("0x{0:X3} - {1}", i * 4, attributes[i]);
            }
            Console.ReadKey();
        }
    }
}
