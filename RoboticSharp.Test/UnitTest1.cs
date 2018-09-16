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

        [Fact]
        public void SymbolAdditionTest()
        {
            Symbol s1 = new Symbol(4);
            Symbol s2 = new Symbol(5);
            Symbol result = s1 + s2;
            Assert.Equal("9,0000", result.ToString());
        }

        [Theory]
        [MemberData(nameof(SymbolEqualityTheoryData))]
        public void SymbolEqualityTheory(Symbol a, Symbol b, bool c)
        {
            //act
            var resultA = a.Equals(b);
            var resultB = b.Equals(a);

            //assert
            Assert.True(resultA == c);
            Assert.True(resultB == c);
            Assert.True(resultB == resultA);
        }


        public static IEnumerable<object[]> SymbolEqualityTheoryData()
        {
            return new List<object[]>(){
                new object[]{ new Symbol(1),new Symbol(1),true},
                new object[]{ new Symbol(1),new Symbol(2),false},
                new object[]{ new Symbol(1),new Symbol("a"),false},
                new object[]{ new Symbol("a"),new Symbol(1),false},
                new object[]{ new Symbol("a"),new Symbol("a"),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(2),new Symbol(1)},Symbol.SymbolOperator.plus),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(2),new Symbol("a")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),true}
            };
        }

    }
}
