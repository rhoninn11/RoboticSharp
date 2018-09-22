using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App.Matrices;
using RoboticSharp.App;

namespace RoboticSharp.Test
{
    public class VectorTests
    {
        //[Fact]
        //public void VectorSubstract()
        //{
        //    Vector v1 = new Vector();
        //    Vector v2 = new Vector();
        //    Vector v3 = v1 - v2;
        //    Assert.Equal();
        //}
        [Fact]
        public void VectorProduct() //działa tylko dla liczb, dopóki nie obsłużymy odejmowania symboli w całości
        {
            Symbol[] v1Els = new Symbol[] { new Symbol(1), new Symbol(2), new Symbol(3)};
            Symbol[] v2Els = new Symbol[] { new Symbol(0), new Symbol(0), new Symbol(3) };
            Vector v1 = new Vector(v1Els);
            Vector v2 = new Vector(v2Els);
            Vector v3 = v1 * v2;
            Assert.Equal("[6,0000, -3,0000, 0,0000]", v3.ToString());
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
                new object[] {new App.Symbol[] { new App.Symbol(1), new App.Symbol(2), new App.Symbol(0) } , new App.Symbol[] { new App.Symbol("k"), new App.Symbol("0"), new App.Symbol("0") }, "[(1,0000+k), (2,0000+0), (0)]" }
            };
        }
    }
}
