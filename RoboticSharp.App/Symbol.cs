using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//500 lini max
//40 max w funkcji
namespace RoboticSharp.App
{
    public class Symbol : IComparable<Symbol>
    {
        List<Symbol> subSymbols;
        double numericValue;
        String textValue;

        SymbolOperator operation;
        SymbolType type;
        SymbolPolarity polarity;

        public enum SymbolOperator { none, plus, minus, times, sin, cos }
        public enum SymbolType { text, numerical, node }
        public enum SymbolPolarity { positive, negative }

        public Symbol()
        {
            subSymbols = new List<Symbol>();
            operation = SymbolOperator.none;
            polarity = SymbolPolarity.positive;
            type = SymbolType.numerical;
        }
        public Symbol(double value) : this()
        {
            numericValue = value;
            if (value < 0)
                polarity = SymbolPolarity.negative;
        }
        public Symbol(String value) : this()
        {
            type = SymbolType.text;
            textValue = value;
        }
        public Symbol(List<Symbol> subSymbols, SymbolOperator operation) : this()
        {
            this.subSymbols = subSymbols;
            this.operation = operation;
            this.type = SymbolType.node;
        }

        public static Symbol operator +(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.plus;

            if (areBothANumerical(s1, s2))
            {
                symbol.type = SymbolType.numerical;
                symbol.operation = SymbolOperator.none;
                symbol.numericValue = s1.numericValue + s2.numericValue;
                if (symbol.numericValue < 0)
                    symbol.polarity = SymbolPolarity.negative;
            }
            else if (isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                Symbol noded = GetNodedFrom(s1, s2);
                Symbol notNoded = GetNotNodedFrom(s1, s2);

                if (noded.isOperatorTypeOf(SymbolOperator.plus))
                    symbol.subSymbols = noded.StackSubsymbolsWith(notNoded);
                else
                {
                    symbol.subSymbols.Add(s1);
                    symbol.subSymbols.Add(s2);
                }
            }
            else if (areBothANode(s1, s2))
                MergeNodeSymbolsToOne(s1, s2, ref symbol, SymbolOperator.plus);
            else if (!areBothANumerical(s1, s2) && !isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            ClearNode(ref symbol);
            SortNode(symbol);
            return symbol;
        }
        public static Symbol operator *(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.times;

            if (areBothANumerical(s1, s2))
            {
                symbol.type = SymbolType.numerical;
                symbol.operation = SymbolOperator.none;
                symbol.numericValue = s1.numericValue * s2.numericValue;
                if (symbol.numericValue < 0)
                    symbol.polarity = SymbolPolarity.negative;
            }
            else if (isOneOfThemANode(s1, s2))
            {
                Symbol noded = GetNodedFrom(s1, s2);
                Symbol notNoded = GetNotNodedFrom(s1, s2);
                symbol.type = SymbolType.node;

                if (noded.isOperatorTypeOf(SymbolOperator.times))
                    symbol.subSymbols = noded.StackSubsymbolsWith(notNoded);
                else
                    for (int i = 0; i < noded.subSymbols.Count; i++)
                        symbol.subSymbols.Add(noded.subSymbols[i] * notNoded);
            }
            else if (areBothANode(s1, s2))
                MergeNodeSymbolsToOne(s1, s2, ref symbol, SymbolOperator.times);
            else if (!areBothANumerical(s1, s2) && !isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            ClearNode(ref symbol);
            SortNode(symbol);
            return symbol;
        }
        public static Symbol operator -(Symbol s1)
        {
            ChangePolarity(ref s1);
            return s1;
        }
        public static Symbol operator -(Symbol s1, Symbol s2)
        {

            Symbol symbol = s1 + -s2;
            return symbol;
        }

        private static void MergeNodeSymbolsToOne(Symbol s1, Symbol s2, ref Symbol mergedTo, SymbolOperator operation)
        {
            mergedTo.type = SymbolType.node;
            if (s1.isOperatorTypeOf(s2.operation))
            {
                if (s1.isOperatorTypeOf(operation))
                {
                    mergedTo.AbsorbeBranches(s1);
                    mergedTo.AbsorbeBranches(s2);
                }
                else if (!s1.isOperatorTypeOf(operation))
                {
                    mergedTo.subSymbols.Add(s1);
                    mergedTo.subSymbols.Add(s2);
                }
            }
            else if (s1.isOperatorTypeOf(mergedTo.operation))
                mergedTo.subSymbols = s1.StackSubsymbolsWith(s2);
            else if (s2.isOperatorTypeOf(mergedTo.operation))
                mergedTo.subSymbols = s2.StackSubsymbolsWith(s1);
        }
        private void AbsorbeBranches(Symbol toAbsorb) // myślę że to prawie ta sama funkcja co niżej tylko nie jestem w stanie tego ogarnąć teraz 
        {
            if (!toAbsorb.isNode())
                return;

            foreach (var symbol in toAbsorb.subSymbols)
                this.subSymbols.Add(symbol);
        }
        public List<Symbol> StackSubsymbolsWith(Symbol notNoded) // można wymyślić jakąś ładniejszą nazwę która by więcej mówiła o tym
        {
            List<Symbol> stackedSubSymbols = new List<Symbol>();
            foreach (var symbol in this.subSymbols)
                stackedSubSymbols.Add(symbol);
            stackedSubSymbols.Add(notNoded);
            return stackedSubSymbols;
        }

        static Symbol GetNodedFrom(Symbol s1, Symbol s2)
        {
            if (s1.isNode())
                return s1;
            else
                return s2;
        }
        static Symbol GetNotNodedFrom(Symbol s1, Symbol s2)
        {
            if (!s1.isNode())
                return s1;
            else
                return s2;
        }
        public bool isNumerical() { return this.type == SymbolType.numerical; }
        public bool isText() { return this.type == SymbolType.text; }
        public bool isNode() { return this.type == SymbolType.node; }
        bool isOperatorTypeOf(SymbolOperator opwerator) { return operation == opwerator; }
        public static bool isOneOfThemANode(Symbol s1, Symbol s2) { return s1.isNode() && !s2.isNode() || !s1.isNode() && s2.isNode(); }
        public static bool areBothANumerical(Symbol s1, Symbol s2) { return s1.isNumerical() && s2.isNumerical(); }
        public static bool areBothANode(Symbol s1, Symbol s2) { return s1.isNode() && s2.isNode(); }

        private static void ChangePolarity(ref Symbol s1)
        {
            if (s1.isPolarityPositive())
            {
                s1.polarity = SymbolPolarity.negative;
            }
            else if (s1.isPolarityNegative())
            {
                s1.polarity = SymbolPolarity.positive;
            }

            if (s1.isNumerical())
                s1.numericValue = -s1.numericValue;
        }
        private void ChangePolarity()
        {
            var placeholder = this;
            ChangePolarity(ref placeholder);
        }
        bool isPolarityPositive() { return polarity == SymbolPolarity.positive; }
        bool isPolarityNegative() { return polarity == SymbolPolarity.negative; }

        public static void ClearNode(ref Symbol symbol)
        {
            if (!symbol.isNode())
                return;

            NumericalCalculationInsideNode(ref symbol);
            RemoveSingularValuesFromNode(ref symbol);
            NoneOrSingleBranchNodeFix(ref symbol);
            SetNodePolatityBasedOnBranches(ref symbol);
        }
        private static void SetNodePolatityBasedOnBranches(ref Symbol symbol)
        {
            if (!symbol.isNode())
                return;

            if (symbol.isOperatorTypeOf(SymbolOperator.plus))
            {
                if (symbol.subSymbols.All(sym => sym.isPolarityNegative()))
                {
                    symbol.subSymbols.ForEach(sym => sym.ChangePolarity());
                    symbol.ChangePolarity();
                }
            }
            else if (symbol.isOperatorTypeOf(SymbolOperator.times))
            {
                var negativeSymbols = symbol.subSymbols.FindAll(sym => sym.isPolarityNegative());
                negativeSymbols.ForEach(sym => sym.ChangePolarity());
                if (negativeSymbols.Count % 2 == 1)
                    symbol.ChangePolarity();
            }
        }
        public static void NumericalCalculationInsideNode(ref Symbol symbol) // jak masz pomysł na lepszą nazwę to dawaj:D
        {
            var numericalValueList = symbol.subSymbols.FindAll(sym => sym.isNumerical());
            if (numericalValueList.Count == 0 || numericalValueList.Count == 1)
                return;
            else if (numericalValueList.Count > 1)
            {
                Symbol result = new Symbol();
                switch (symbol.operation)
                {
                    case SymbolOperator.plus:
                        result = new Symbol(0);
                        break;
                    case SymbolOperator.times:
                        result = new Symbol(1);
                        break;
                }

                for (int i = 0; i < numericalValueList.Count; i++)
                    switch (symbol.operation)
                    {
                        case SymbolOperator.plus:
                            result += numericalValueList[i];
                            break;
                        case SymbolOperator.times:
                            result *= numericalValueList[i];
                            break;
                    }
                symbol.subSymbols.RemoveAll(sym => sym.isNumerical());
                symbol.subSymbols.Add(result);
            }
        }
        public static void RemoveSingularValuesFromNode(ref Symbol symbol)
        {
            Predicate<Symbol> symbolIsZero = s => s.isNumerical() && s.numericValue == 0;
            Predicate<Symbol> symbolIsOne = s => s.isNumerical() && s.numericValue == 1;
            Predicate<Symbol> symbolIsMinusOne = s => s.isNumerical() && s.numericValue == -1;

            if (symbol.isOperatorTypeOf(SymbolOperator.plus))
                symbol.subSymbols.RemoveAll(symbolIsZero);
            else if (symbol.isOperatorTypeOf(SymbolOperator.times))
            {
                if (symbol.subSymbols.Exists(symbolIsZero))
                {
                    symbol = new Symbol(0);
                    return;
                }

                if (symbol.subSymbols.Exists(symbolIsOne))
                    symbol.subSymbols.RemoveAll(symbolIsOne);

                if (symbol.subSymbols.Exists(symbolIsMinusOne))
                {
                    int countOfMinusOneSymbols = symbol.subSymbols.RemoveAll(symbolIsMinusOne);
                    if (countOfMinusOneSymbols % 2 == 1)
                        ChangePolarity(ref symbol);
                }
            }
        }
        public static void NoneOrSingleBranchNodeFix(ref Symbol symbol)
        {
            if(!symbol.isNode())
                return;

            if (symbol.subSymbols.Count == 1)
                ColapseOneBranchNodeToSymbol(ref symbol);
            else if (symbol.subSymbols.Count == 0)
                ColapseEmptyNodeToSymbol(ref symbol);
        }
        public static void ColapseOneBranchNodeToSymbol(ref Symbol symbol)
        {
            if (!(symbol.subSymbols.Count == 1) || !symbol.isNode())
                return;

            var colapseTo = symbol.subSymbols[0];
            if (areBothHaveTheSamePolarity(symbol, colapseTo))
            {
                if (colapseTo.isPolarityNegative())
                    ChangePolarity(ref colapseTo);
            }
            else if (!areBothHaveTheSamePolarity(symbol, colapseTo))
            {
                if (colapseTo.isPolarityPositive())
                    ChangePolarity(ref colapseTo);
            }
            symbol = colapseTo;
        }
        public static void ColapseEmptyNodeToSymbol(ref Symbol symbol)
        {
            if (!(symbol.subSymbols.Count == 0) || !symbol.isNode())
                return;

            symbol = new Symbol(0);
        }

        public static void SortNode(Symbol symbol) { symbol.subSymbols.Sort(); }

        public override String ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            switch (type)
            {
                case SymbolType.text:
                    if (polarity == SymbolPolarity.negative) stringBuilder.Append("-");
                    stringBuilder.Append(textValue);
                    break;
                case SymbolType.numerical:
                    stringBuilder.Append(string.Format("{0:N4}", numericValue));
                    break;
                case SymbolType.node:
                    if (polarity == SymbolPolarity.negative) stringBuilder.Append("-");
                    stringBuilder.Append("(");

                    int nodeSize = subSymbols.Count;
                    for (int i = 0; i < nodeSize; i++)
                    {
                        stringBuilder.Append(subSymbols[i].ToString());

                        if (i < nodeSize - 1)
                        {
                            switch (operation)
                            {
                                case SymbolOperator.plus:
                                    stringBuilder.Append("+");
                                    break;
                                case SymbolOperator.times:
                                    stringBuilder.Append("*");
                                    break;
                            }
                        }
                    }
                    stringBuilder.Append(")");
                    break;
            }
            return stringBuilder.ToString().Replace("+-", "-");
        }

        public override bool Equals(object obj)
        {
            Symbol comparedSymbol = (Symbol)obj;

            bool symbolCondition = areTheSameSymbols(this, comparedSymbol);
            bool branchesCondition = true;

            if (this.isNode() && areBothHaveTheSameType(this, comparedSymbol))
                branchesCondition = areBothHaveTheSameNodeBranches(this, comparedSymbol);

            return symbolCondition && branchesCondition;
        }
        private bool areTheSameSymbols(Symbol s1, Symbol s2)
        {
            bool isTheSameType = areBothHaveTheSameType(s1, s2);
            bool isNumericalValueTheSame = areBothHaveTheSameNumericalValue(s1, s2);
            bool isTextValueTheSame = areBothHaveTheSameTextValue(s1, s2);
            bool isPolarityTheSame = areBothHaveTheSamePolarity(s1, s2);
            bool isOperationTheSame = areBothHaveTheSameOperation(s1, s2);

            return isNumericalValueTheSame && isTextValueTheSame && isPolarityTheSame && isOperationTheSame;
        }
        private bool areBothHaveTheSameType(Symbol s1, Symbol s2) { return s1.type == s2.type; }
        private static bool areBothHaveTheSameNumericalValue(Symbol s1, Symbol s2) { return s1.numericValue == s2.numericValue; }
        private static bool areBothHaveTheSameTextValue(Symbol s1, Symbol s2) { return s1.textValue == s2.textValue; }
        private static bool areBothHaveTheSamePolarity(Symbol s1, Symbol s2) { return s1.polarity == s2.polarity; }
        private static bool areBothHaveTheSameOperation(Symbol s1, Symbol s2) { return s1.operation == s2.operation; }
        private bool areBothHaveTheSameNodeBranches(Symbol s1, Symbol s2)
        {
            bool isNodesEqual = false;
            if (s1.subSymbols.Count == s2.subSymbols.Count)
                for (int i = 0; i < s1.subSymbols.Count; i++)
                    if (!(isNodesEqual = s1.subSymbols[i].Equals(s2.subSymbols[i])))
                        break;

            return isNodesEqual;
        }

        public static Symbol Sin(Symbol input)
        {
            Symbol symbol = new Symbol();
            symbol.type = input.type;

            switch (symbol.type)
            {
                case SymbolType.text:
                    symbol.textValue = String.Format("S{0}", input.textValue);
                    break;
                case SymbolType.numerical:
                    symbol.numericValue = RoundForTrygonometryBorderValues(Math.Sin((Math.PI / 180) * input.numericValue));
                    break;
            }
            return symbol;
        }
        public static Symbol Cos(Symbol input)
        {
            Symbol symbol = new Symbol();
            symbol.type = input.type;

            switch (symbol.type)
            {
                case SymbolType.text:
                    symbol.textValue = String.Format("C{0}", input.textValue);
                    break;
                case SymbolType.numerical:
                    symbol.numericValue = RoundForTrygonometryBorderValues(Math.Cos((Math.PI / 180) * input.numericValue));
                    break;
            }
            return symbol;
        }
        static double RoundForTrygonometryBorderValues(double value)
        {
            value = value > 0.9999 && value < 1.0001 ? 1 : value;
            value = value > -0.0001 && value < 0.0001 ? 0 : value;
            value = value > -1.0001 && value < -0.9999 ? -1 : value;
            return value;
        }

        public int CompareTo(Symbol other)
        {
            if (this.isNumerical() && !areBothHaveTheSameType(this, other))
                return -1;
            else if (other.isNumerical() && !areBothHaveTheSameType(this, other))
                return 1;
            else if (this.isNumerical() && areBothHaveTheSameType(this, other))
                return this.numericValue.CompareTo(other.numericValue);

            if (this.isText() && !areBothHaveTheSameType(this, other))
                return -1;
            else if (other.isText() && !areBothHaveTheSameType(this, other))
                return 1;
            else if (this.isText() && areBothHaveTheSameType(this, other))
                return this.textValue.CompareTo(other.textValue);

            else if (this.isNode() && areTheSameSymbols(this, other))
                return this.ToString().Length.CompareTo(other.ToString().Length);

            return 0;
        }
    }
}
