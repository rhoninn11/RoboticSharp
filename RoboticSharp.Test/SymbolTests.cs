using System;
using System.Collections.Generic;
using Xunit;
using RoboticSharp.App;

namespace RoboticSharp.Test
{
    public class SymbolTests
    {
        [Theory]
        [MemberData(nameof(SymbolEquality_Test_Data))]
        public void SymbolEquality_Test(Symbol a, Symbol b, bool c)
        {
            //act
            var resultA = a.Equals(b);
            var resultB = b.Equals(a);

            //assert
            Assert.True(resultA == c);
            Assert.True(resultB == c);
            Assert.True(resultB == resultA);
        }
        public static IEnumerable<object[]> SymbolEquality_Test_Data()
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
        public void NodeSubSymbolSorting_Test_ForAdition()
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
        public void NodeSubSymbolSorting_Test_ForMultiplication()
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
        public void ColapseEmptyNodeToSymbol_Test()
        {
            //Given
            var symbolA = new Symbol(new List<Symbol>() { }, Symbol.SymbolOperator.plus);

            var resultSymbol = new Symbol(0);

            //when
            Symbol.ColapseEmptyNodeToSymbol(ref symbolA);

            //Then
            Assert.True(symbolA.Equals(resultSymbol));
        }
        [Fact]
        public void ColapseOneBranchNodeToSymbol_Test()
        {
            //Given
            var symbolA = new Symbol(new List<Symbol>() { new Symbol(1) }, Symbol.SymbolOperator.plus);
            var symbolB = -new Symbol(new List<Symbol>() { new Symbol(1) }, Symbol.SymbolOperator.plus);
            var symbolC = new Symbol(new List<Symbol>() { -new Symbol(1) }, Symbol.SymbolOperator.plus);
            var symbolD = -new Symbol(new List<Symbol>() { -new Symbol(1) }, Symbol.SymbolOperator.plus);
            var resultSymbolA = new Symbol(1);
            var resultSymbolB = new Symbol(-1);
            var resultSymbolC = new Symbol(-1);
            var resultSymbolD = new Symbol(1);

            //When
            Symbol.ColapseOneBranchNodeToSymbol(ref symbolA);
            Symbol.ColapseOneBranchNodeToSymbol(ref symbolB);
            Symbol.ColapseOneBranchNodeToSymbol(ref symbolC);
            Symbol.ColapseOneBranchNodeToSymbol(ref symbolD);

            //Then
            Assert.True(symbolA.Equals(resultSymbolA));
            Assert.True(symbolB.Equals(resultSymbolB));
            Assert.True(symbolC.Equals(resultSymbolC));
            Assert.True(symbolD.Equals(resultSymbolD));
        }


        [Fact]
        public void SymbolAddition_Test()
        {
            var symbolA = new Symbol(4) + new Symbol(5);
            var resultSymbolA = new Symbol(9);
            Assert.True(symbolA.Equals(resultSymbolA));
        }

        [Theory]
        [MemberData(nameof(SymbolSubstraction_Test_Data))]
        public void SymbolSubstraction_Test(Symbol s1, Symbol s2, Symbol expected)
        {
            var result = s1 - s2;
            Assert.True(result.Equals(expected));
        }
        public static IEnumerable<object[]> SymbolSubstraction_Test_Data()
        {
            return new List<object[]>()
            {
                new object[]{new Symbol(3), new Symbol(4), new Symbol(-1)},
                new object[]{new Symbol(2), new Symbol(3), new Symbol(-1)}, // czy na razie zostawiamy ten wynik tak jak jest? czyli 2,0000 -3?
                new object[]{
                    new Symbol("a"),
                    new Symbol("c"),
                    new Symbol(
                        new List<Symbol>(){
                            new Symbol("a"),
                            -new Symbol("c")
                        },Symbol.SymbolOperator.plus)
                },
                new object[]{
                    new Symbol(2),
                    new Symbol("c"),
                    new Symbol(
                        new List<Symbol>(){
                            new Symbol(2),
                            -new Symbol("c")
                        },Symbol.SymbolOperator.plus)
                },
                new object[]{
                    new Symbol("a"),
                    new Symbol(2),
                    new Symbol(
                        new List<Symbol>(){
                            new Symbol(-2),
                            new Symbol("a")
                        },Symbol.SymbolOperator.plus)
                },
                new object[]{
                    new Symbol("a"),
                    new Symbol(2),
                    new Symbol(
                        new List<Symbol>(){
                            new Symbol(-2),
                            new Symbol("a")
                        },Symbol.SymbolOperator.plus)
                },
                new object[]{
                    new Symbol(1) * new Symbol(3),
                    new Symbol("k") * new Symbol(0),
                    new Symbol(3)
                },
                new object[]{
                    new Symbol(1) * new Symbol(0),
                    new Symbol(2) * new Symbol(3),
                    new Symbol(-6)
                }
            };
        }


        [Theory]
        [MemberData(nameof(SymbolMultiplication_Test_Data))]
        public void SymbolMultiplication_Test(Symbol s1, Symbol s2, Symbol result)
        {
            Symbol s3 = s1 * s2;
            Assert.True(s3.Equals(result));
        }
        public static IEnumerable<object[]> SymbolMultiplication_Test_Data()
        {
            return new List<object[]>()
            {
                new object[]{
                    new Symbol(2),
                    new Symbol(3),
                    new Symbol(6)
                },
                new object[]{
                    new Symbol("k"),
                    new Symbol(4),
                    new Symbol(new List<Symbol>{
                        new Symbol(4),
                        new Symbol("k")
                    },Symbol.SymbolOperator.times)
                },
                new object[]{
                    new Symbol("k"),
                    new Symbol(0),
                    new Symbol(0)
                },
                new object[]{
                    new Symbol(new List<Symbol>{
                        new Symbol(2),
                        new Symbol(3)
                    },Symbol.SymbolOperator.times),
                    new Symbol(2),
                    new Symbol(12)
                },
                new object[]{
                    new Symbol(new List<Symbol>{
                        new Symbol(2),
                        new Symbol(4)
                    },Symbol.SymbolOperator.times),
                    new Symbol(2),
                    new Symbol(16)
                },
                new object[]{
                    new Symbol(new List<Symbol>{
                        new Symbol(2),
                        new Symbol(4)
                    },Symbol.SymbolOperator.times),
                    new Symbol("a"),
                    new Symbol(new List<Symbol>{
                        new Symbol(8),
                        new Symbol("a")
                    },Symbol.SymbolOperator.times),
                },
                new object[]{
                    new Symbol(new List<Symbol>{
                        new Symbol("a"),
                        new Symbol(4)
                    },Symbol.SymbolOperator.times),
                    new Symbol(new List<Symbol>{
                        new Symbol("b"),
                        new Symbol(3)
                    },Symbol.SymbolOperator.times),
                    new Symbol(new List<Symbol>{
                        new Symbol(12),
                        new Symbol("a"),
                        new Symbol("b")
                    },Symbol.SymbolOperator.times),
                }
            };
        }

        [Fact]
        public void RemoveSingularValuesFromNode_Test()
        {
            //Given
            var symbolA = new Symbol(new List<Symbol> { new Symbol(1), new Symbol(0) }, Symbol.SymbolOperator.plus);
            var symbolB = new Symbol(new List<Symbol> { new Symbol(0), new Symbol("a") }, Symbol.SymbolOperator.times);
            var symbolC = new Symbol(new List<Symbol> { new Symbol(1), new Symbol("a") }, Symbol.SymbolOperator.times);
            var symbolD = new Symbol(new List<Symbol> { new Symbol(-1), new Symbol("a") }, Symbol.SymbolOperator.times);
            var symbolE = new Symbol(new List<Symbol> { new Symbol(-1), new Symbol(-1), new Symbol("a") }, Symbol.SymbolOperator.times);

            var resultSymbolA = new Symbol(new List<Symbol> { new Symbol(1), }, Symbol.SymbolOperator.plus);
            var resultSymbolB = new Symbol(0);
            var resultSymbolC = new Symbol("a");
            var resultSymbolD = -new Symbol("a");
            var resultSymbolE = new Symbol("a");

            //When
            Symbol.RemoveSingularValuesFromNode(ref symbolA);
            Symbol.RemoveSingularValuesFromNode(ref symbolB);
            Symbol.RemoveSingularValuesFromNode(ref symbolC);
            Symbol.NoneOrSingleBranchNodeFix(ref symbolC);
            Symbol.RemoveSingularValuesFromNode(ref symbolD);
            Symbol.NoneOrSingleBranchNodeFix(ref symbolD);
            Symbol.RemoveSingularValuesFromNode(ref symbolE);
            Symbol.NoneOrSingleBranchNodeFix(ref symbolE);

            //Then
            Assert.True(symbolA.Equals(resultSymbolA));
            Assert.True(symbolB.Equals(resultSymbolB));
            Assert.True(symbolC.Equals(resultSymbolC));
            Assert.True(symbolD.Equals(resultSymbolD));
            Assert.True(symbolE.Equals(resultSymbolE));
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
