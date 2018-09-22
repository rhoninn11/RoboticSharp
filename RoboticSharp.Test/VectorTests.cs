using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App.Matrices;

namespace RoboticSharp.Test
{
    public class VectorTests
    {
        [Fact]
        public void VectorAdditionTest()
        {
            App.Symbol[] v1Elements = {new App.Symbol(1), new App.Symbol(2), new App.Symbol(0) };
            App.Symbol[] v2Elements = { new App.Symbol("k"), new App.Symbol("0"), new App.Symbol("0") };
            Vector v1 = new Vector(v1Elements);
            Vector v2 = new Vector(v2Elements);
            Vector v3 = v1 + v2;
            Assert.Equal("[1+k, 2, 0]", v3.ToString());
        }
    }
}
