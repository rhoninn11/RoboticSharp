using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App;

namespace RoboticSharp.Test
{
    public class SymbolTests
    {
        [Fact]
        public void SymbolAdditionTest()
        {
            Symbol s1 = new Symbol(4);
            Symbol s2 = new Symbol(5);
            Symbol result = s1 + s2;
            Assert.Equal("9.0000", result.ToString());
        }
        [Fact]
        public void NodeSymbolAdditionTest()
        {

            var nodeA = new Symbol(new List<Symbol>() { new Symbol("a"), new Symbol(12) }, Symbol.SymbolOperator.plus);
            var symbolB = new Symbol("b");
            var outputSymbol = new Symbol(new List<Symbol>() { new Symbol(12), new Symbol("a"), new Symbol("b") }, Symbol.SymbolOperator.plus);

            var resultaA = nodeA + symbolB;
            var resultaB = symbolB + nodeA;

            Assert.True(resultaA.Equals(outputSymbol));
            Assert.True(resultaB.Equals(outputSymbol));
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
                new object[]{ new Symbol("a"),-new Symbol("a"),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(-1)},Symbol.SymbolOperator.plus),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(1)},Symbol.SymbolOperator.plus),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2)},Symbol.SymbolOperator.plus),new Symbol(new List<Symbol>(){new Symbol(2),new Symbol(1)},Symbol.SymbolOperator.plus),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(2),new Symbol("a")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(1),-new Symbol("a")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),true},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol("a"),new Symbol("b"),new Symbol("c")},Symbol.SymbolOperator.times),false},
                new object[]{ new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),new Symbol(new List<Symbol>(){new Symbol(1),new Symbol(2),new Symbol(3)},Symbol.SymbolOperator.times),true}
            };
        }

        [Fact]
        public void NodeSubSymbolSortingTest()
        {
            //Given
            var nodeA = new Symbol(new List<Symbol>() { new Symbol("a"), new Symbol(1) }, Symbol.SymbolOperator.times);
            var nodeB = new Symbol(new List<Symbol>() { new Symbol(2), new Symbol("ab") }, Symbol.SymbolOperator.times);
            var nodeC = new Symbol("c");
            var nodeD = new Symbol(12);

            var resultNode = new Symbol(new List<Symbol>(){
                    new Symbol(12),
                    new Symbol("c"),
                    new Symbol(new List<Symbol>(){
                        new Symbol("a"),
                        new Symbol(1)
                        },Symbol.SymbolOperator.times),
                    new Symbol(new List<Symbol>(){
                        new Symbol(2),
                        new Symbol("ab")
                        },Symbol.SymbolOperator.times)
                    }, Symbol.SymbolOperator.plus);
            //When
            Symbol.SortNode(nodeA);
            Symbol.SortNode(nodeB);

            var resultA = nodeA + nodeB + nodeC + nodeD;

            //Then
        }

    }
}
