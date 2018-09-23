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

        [Theory]
        [MemberData(nameof(SymbolSubstractionTheoryData))]
        public void SymbolSubstractionTest(Symbol s1, Symbol s2, string expected)
        {
            Symbol result = s1 - s2;
            Assert.Equal(expected, result.ToString());
        }

        public static IEnumerable<object[]> SymbolSubstractionTheoryData()
        {
            return new List<object[]>()
            {
                new object[]{new Symbol(3), new Symbol(4), "-1,0000"},
                new object[]{new Symbol(2), new Symbol("3"), "-1"}, // czy na razie zostawiamy ten wynik tak jak jest? czyli 2,0000 -3?
                new object[]{new Symbol("a"), new Symbol("c"), "(a-c)"},
                new object[]{new Symbol(2), new Symbol("c"), "(2,0000-c)"},
                new object[]{new Symbol("a"), new Symbol(2), "(-2,0000+a)"}, //to chyba spowodowane sortowaniem
                new object[]{new Symbol("a"), new Symbol("2"), "(-2+a)"},
            };
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
        public void NodeSubSymbolSortingForAditionTest()
        {
            //Given
            var symbolA = new Symbol(new List<Symbol>() { new Symbol("a"), new Symbol(1) }, Symbol.SymbolOperator.times);
            var symbolB = new Symbol(new List<Symbol>() { new Symbol(2), new Symbol("ab") }, Symbol.SymbolOperator.times);
            var symbolC = new Symbol("c");
            var symbolD = new Symbol(12);
            var symbolE = new Symbol(new List<Symbol>() { new Symbol(2), new Symbol("e") }, Symbol.SymbolOperator.plus);


            var resultNode = new Symbol(new List<Symbol>(){
                    new Symbol(14),
                    new Symbol("c"),
                    new Symbol("e"),
                    new Symbol(new List<Symbol>(){
                        new Symbol(1),
                        new Symbol("a")
                        },Symbol.SymbolOperator.times),
                    new Symbol(new List<Symbol>(){
                        new Symbol(2),
                        new Symbol("ab")
                        },Symbol.SymbolOperator.times)
                    }, Symbol.SymbolOperator.plus);

            //When
            Symbol.SortNode(symbolA);
            Symbol.SortNode(symbolB);

            var resultA = symbolA + symbolC + symbolD + symbolB + symbolE;
            var resultB = symbolD + symbolC + symbolA + symbolE + symbolB;
            var resultC = symbolA + symbolB + symbolE + symbolD + symbolC;
            var resultD = symbolA + symbolE + symbolB + symbolD + symbolC;

            //Then
            Assert.True(resultA.Equals(resultNode));
            Assert.True(resultB.Equals(resultNode));
            Assert.True(resultC.Equals(resultNode));
            Assert.True(resultD.Equals(resultNode));
        }

        [Fact]
        public void NodeSubSymbolSortingForMultiplicationTest()
        {
            //Given
            var symbolA = new Symbol(new List<Symbol>() { new Symbol("a"), new Symbol(1) }, Symbol.SymbolOperator.times);
            var symbolB = new Symbol(new List<Symbol>() { new Symbol(2), new Symbol("ab") }, Symbol.SymbolOperator.times);
            var symbolC = new Symbol("c");
            var symbolD = new Symbol(12);
            var symbolE = new Symbol(new List<Symbol>() { new Symbol(2), new Symbol("e") }, Symbol.SymbolOperator.plus);


            var resultNode = new Symbol(new List<Symbol>(){
                    new Symbol(24),
                    new Symbol("a"),
                    new Symbol("ab"),
                    new Symbol("c"),
                    new Symbol(new List<Symbol>(){
                        new Symbol(2),
                        new Symbol("e")
                        },Symbol.SymbolOperator.plus)
                    }, Symbol.SymbolOperator.times);

            //When
            Symbol.SortNode(symbolA);
            Symbol.SortNode(symbolB);

            var resultA = symbolA * symbolC * symbolD * symbolB * symbolE;
            var resultB = symbolD * symbolC * symbolA * symbolE * symbolB;
            var resultC = symbolA * symbolB * symbolE * symbolD * symbolC;
            var resultD = symbolA * symbolE * symbolB * symbolD * symbolC;

            //Then
            Assert.True(resultA.Equals(resultNode));
            Assert.True(resultB.Equals(resultNode));
            Assert.True(resultC.Equals(resultNode));
            Assert.True(resultD.Equals(resultNode));
        }

        [Fact]
        public void RemoveSingularValueFromSymbolNodesAditionTest()
        {
            //Given
            var symbolA = new Symbol(1);
            var symbolB = new Symbol("b");
            var symbolC = new Symbol(-1);

            var resultSymbolA = new Symbol("b");
            var resultSymbolB = new Symbol(0);

            //When
            var resultA = symbolA + symbolB + symbolC;
            var resultB = symbolA + symbolC;

            //Then
            Assert.True(resultA.Equals(resultSymbolA));
            Assert.True(resultB.Equals(resultSymbolB));
        }

        [Fact]
        public void RemoveSingularValueFromSymbolNodesMultiplicationTest()
        {
            //Given
            var symbolA = new Symbol(12);
            var symbolB = new Symbol("b");
            var symbolC = new Symbol(1);
            var symbolD = new Symbol(-1);
            var symbolE = new Symbol("a");

            var resultSymbolA = new Symbol(new List<Symbol>(){
                    new Symbol(12),
                    new Symbol("b")
                }, Symbol.SymbolOperator.times);
            var resultSymbolB = -new Symbol(new List<Symbol>(){
                    new Symbol(12),
                    new Symbol("b")
                }, Symbol.SymbolOperator.times);
            var resultSymbolC = -new Symbol(new List<Symbol>(){
                    new Symbol("a"),
                    new Symbol("b")
                }, Symbol.SymbolOperator.times);

            //When
            var resultA = symbolB * symbolC * symbolA;
            var resultB = symbolB * symbolA * symbolD;
            var resultC = symbolB * symbolE * symbolD;

            //Then
            Assert.True(resultA.Equals(resultSymbolA));
            Assert.True(resultB.Equals(resultSymbolB));
            Assert.True(resultC.Equals(resultSymbolC));
        }

    }
}
