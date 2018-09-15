﻿using System;
using RoboticSharp.App.Matrices;

namespace RoboticSharp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //test konstrukcyjny
            MacierzT t = new MacierzT(new Symbol(90), new Symbol(0), new Symbol(0), new Symbol(0));
            Console.WriteLine(t.ToString());

            //test mnożenia
            MacierzT t1 = new MacierzT(new Symbol("q1"), new Symbol(90), new Symbol(0), new Symbol(0));
            MacierzT t2 = new MacierzT(new Symbol("q2"), new Symbol(90), new Symbol(0), new Symbol(0));
            MacierzT t3 = t1.TranspozycjaMacierzT();

            Console.WriteLine("===================");
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t3.ToString());
            Console.WriteLine(t2.ToString());
            Console.WriteLine("===================");
            MacierzT t12 = t1 * t2;
            Console.WriteLine(t12.ToString());
            Console.ReadKey();
        }
    }
}