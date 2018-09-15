using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticSharp.App.Matrices
{
    class Vector
    {
        public double[] data;
        public double _1;
        public double _2;
        public double _3;

        public Vector(double[] elements)
        {
            data = new double[3];
            for(int i=0; i<3 ;i++)
            {
                data[i] = elements[i];
            }
        }
        /// <summary>
        /// Iloczyn wektorowy dla 3 elementowych wektorów
        /// </summary>
        public static Vector operator *(Vector a, Vector b)
        {
            double[] arr = new double[3];
            arr[0] = a._2 * b._3 - a._3 * b._2;
            arr[1] = (-1) * (a._1 * b._3 - a._3 * b._1);
            arr[2] = a._1 * b._2 - a._2 * b._1;
            return new Vector(arr);
        }
    }
}
