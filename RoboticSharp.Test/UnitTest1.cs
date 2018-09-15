using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App;

namespace RoboticSharp.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [MemberData(nameof(SymbolEqualityTheoryData))]
        public void SymbolEqualityTheory(Symbol a, Symbol b, bool c){
            //act
            var resultA = a.Equals(b);
            var resultB = b.Equals(a);

            //assert
            Assert.True(resultA == c);
            Assert.True(resultB == c);
            Assert.True(resultB == resultA);
        }


        public static IEnumerable<object[]> SymbolEqualityTheoryData(){
            return new List<object[]>(){
                new object[]{ new Symbol(1),new Symbol(1),true},
                new object[]{ new Symbol(1),new Symbol(2),false},
                new object[]{ new Symbol(1),new Symbol("a"),false},
                new object[]{ new Symbol("a"),new Symbol(1),false},
                new object[]{ new Symbol("a"),new Symbol("a"),true}
            };
        }


    }
}
