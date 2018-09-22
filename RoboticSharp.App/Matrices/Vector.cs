using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticSharp.App.Matrices
{
    public class Vector
    {
        public Symbol[] data;
        public Symbol _1 { get { return data[0]; } }
        public Symbol _2 { get { return data[1]; } }
        public Symbol _3 {get {return data[2];} }

        public Vector(Symbol[] elements)
        {
            data = new Symbol[3];
            for (int i = 0; i < 3; i++)
            {
                data[i] = elements[i];
            }
        }
        public Vector()
        {
            data = new Symbol[3];
            for (int x=0; x < data.Length; x++)
                data[x] = new Symbol(0);
        }
        /// <summary>
        /// Iloczyn wektorowy dla 3 elementowych wektorów
        /// </summary>
        public static Vector operator *(Vector a, Vector b)
        {
            Symbol[] arr = new Symbol[3];
            arr[0] = a._2 * b._3 - a._3 * b._2;
            arr[1] = (-1) * (a._1 * b._3 - a._3 * b._1);
            arr[2] = a._1 * b._2 - a._2 * b._1;
            return new Vector(arr);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            Symbol[] arr = new Symbol[3];
            arr[0] = a._1 + b._1;
            arr[1] = a._2 + b._2;
            arr[2] = a._3 + b._3;
            return new Vector(arr);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", _1, _2, _3);
        }
    }
}
