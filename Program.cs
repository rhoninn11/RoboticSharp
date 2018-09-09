using System;

namespace RoboticSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //test konstrukcyjny
            MacierzT t = new MacierzT(new Symbol(90), new Symbol(0), new Symbol(0), new Symbol(0));
            Console.WriteLine(t.ToString());

            //test mnożenia
            MacierzT t1 = new MacierzT(new Symbol(90), new Symbol(90), new Symbol(0), new Symbol(0));
            MacierzT t2 = new MacierzT(new Symbol(90), new Symbol(90), new Symbol(0), new Symbol(0));

            Console.WriteLine("===================");
            Console.WriteLine(t1.ToString());
            Console.WriteLine(t2.ToString());
            Console.WriteLine("===================");
            Console.WriteLine((t1 * t2).ToString());

        }
    }
}
