using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App.Matrices;
using RoboticSharp.App;

namespace RoboticSharp.Test
{
    public class VectorTests
    {
        [Theory]
        [MemberData(nameof(VectorProductData))]
        public void VectorProduct(Symbol[] v1Els, Symbol[] v2Els, string expected)
        {
            Vector v1 = new Vector(v1Els);
            Vector v2 = new Vector(v2Els);
            Vector v3 = v1 * v2;
            Assert.Equal(expected, v3.ToString());
        }

        public static IEnumerable<object[]> VectorProductData()
        {
            return new List<object[]>()
            {
                new object[]
                {
                    new Symbol[] { new Symbol(1), new Symbol(2), new Symbol(3) },
                    new Symbol[] { new Symbol(0), new Symbol(0), new Symbol(3) },
                    "[6.0000, -3.0000, 0.0000]",
                },
                new object[]
                {
                   new Symbol[] { new Symbol(1), new Symbol(2), new Symbol("k") },
                   new Symbol[] { new Symbol("0"), new Symbol(0), new Symbol("3") },    //drugi element powstalego wektora nie za bardzo się zgadza,
                   "[6.0000, -3.0000, -6.0000]",                                        // trzeci w sumie też, coś może być nie tak z mnożeniem symboli
                }
            };
        }


        [Theory]
        [MemberData(nameof(VectorAdditionData))]
        public void VectorAdditionTest(App.Symbol[] v1Els, App.Symbol[] v2Els, string expected)
        {
            Vector v1 = new Vector(v1Els);
            Vector v2 = new Vector(v2Els);
            Vector v3 = v1 + v2;
            Assert.Equal(expected, v3.ToString());
        }

        public static IEnumerable<object[]> VectorAdditionData()
        {
            return new List<object[]>()
            {
                new object[] {
                    new App.Symbol[] { new App.Symbol(1), new App.Symbol(2), new App.Symbol(0) },
                    new App.Symbol[] { new App.Symbol("k"), new App.Symbol("0"), new App.Symbol("0") },
                    "[(1.0000+k), (2.0000+0), 0]"
                }
            };
        }
    }
}
