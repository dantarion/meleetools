using System;
using MeleeLib;
using MeleeLib.DatHandler;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new File(@"X:\Brawl Hacking\Melee\Backup\Super Smash Bros. Melee - Character Files\1.00\1 - Moveset\Marth\PlMs.dat");
            AttributesIndex attributes = file.Attributes;
            Console.WriteLine(file.SubactionIndex.Count);
            Console.ReadLine();
        }
    }
}
