﻿using System;
using RoboticSharp.App.Matrices;

namespace RoboticSharp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Symbol s3 = new Symbol(2) * new Symbol("3");
            //test konstrukcyjny
            Matrix t = new Matrix(new Symbol(90), new Symbol(0), new Symbol(0), new Symbol(0));
            Console.WriteLine(t.ToString());

            //test mnożenia
            Matrix t1 = new Matrix(new Symbol("q1"), new Symbol(90), new Symbol(0), new Symbol(0));
            Matrix t2 = new Matrix(new Symbol("q2"), new Symbol(90), new Symbol(0), new Symbol(0));
            Matrix t3 = t1.MatrixTransposition();
            

            Console.WriteLine("===================");
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t3.ToString());
            Console.WriteLine(t2.ToString());
            Console.WriteLine("===================");
            Matrix t12 = t1 * t2;
            Console.WriteLine(t12.ToString());
            Console.ReadKey();
        }
    }
}
