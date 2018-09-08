using System;

namespace SRDE
{
    class Program
    {
        static void Main(string[] args)
        {
            //test konstrukcyjny
           MacierzT t = new MacierzT(90,0,0,0);
           Console.WriteLine(t.ToString());

           //test mnożenia
           MacierzT t1 = new MacierzT(90,90,0,0);
           MacierzT t2 = new MacierzT(90,90,0,0);
           
           Console.WriteLine("===================");
           Console.WriteLine(t1.ToString());
           Console.WriteLine(t2.ToString());
           Console.WriteLine("===================");
           Console.WriteLine((t1*t2).ToString());

        }
    }
}
