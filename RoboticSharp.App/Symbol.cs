using System;
using System.Collections.Generic;
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

        public enum SymbolOperator
        {
            none, plus, minus, times, sin, cos
        }

        public enum SymbolType
        {
            text, numerical, node
        }

        public enum SymbolPolarity
        {
            positive, negative
        }

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
            }
            else if (isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                Symbol noded = getNodedFrom(s1, s2);
                Symbol notNoded = getNotNodedFrom(s1, s2);

                if (noded.isOperatorTypeOf(SymbolOperator.plus))
                    symbol.subSymbols = noded.stackSubsymbolsWith(notNoded);
                else
                {
                    symbol.subSymbols.Add(s1);
                    symbol.subSymbols.Add(s2);
                }
            }
            else if (areBothANode(s1, s2))
                mergeNodeSymbolsToOne(s1, s2, ref symbol, SymbolOperator.plus);
            else if (!areBothANumerical(s1, s2) && !isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            clearNode(ref symbol);
            SortNode(symbol);
            return symbol;
        }

        private static void mergeNodeSymbolsToOne(Symbol s1, Symbol s2, ref Symbol symbol, SymbolOperator operation)
        {
            symbol.type = SymbolType.node;
            if (s1.isOperatorTypeOf(s2.operation))
            {
                if (s1.isOperatorTypeOf(operation))
                {
                    symbol.absorbeSubSymbols(s1);
                    symbol.absorbeSubSymbols(s2);
                }
                else if (!s1.isOperatorTypeOf(operation))
                {
                    symbol.subSymbols.Add(s1);
                    symbol.subSymbols.Add(s2);
                }
            }
            else if (s1.isOperatorTypeOf(symbol.operation))
                symbol.subSymbols = s1.stackSubsymbolsWith(s2);
            else if (s2.isOperatorTypeOf(symbol.operation))
                symbol.subSymbols = s2.stackSubsymbolsWith(s1);
        }

        private void absorbeSubSymbols(Symbol s1)
        {
            foreach (var symbol in s1.subSymbols)
            {
                this.subSymbols.Add(symbol);
            }
        }

        public static Symbol operator -(Symbol s1, Symbol s2)
        {
            Symbol symbol = s1 + s2;
            return -symbol;
            // symbol.operation = SymbolOperator.plus;

            // if (areBothANumerical(s1, s2))
            // {
            //     symbol.type = SymbolType.numerical;
            //     symbol.operation = SymbolOperator.none;
            //     symbol.numericValue = s1.numericValue - s2.numericValue;
            // }
            // else if (isOneOfThemANode(s1, s2))
            // {
            //     symbol.type = SymbolType.node;
            //     Symbol noded = getNodedFrom(s1, s2);
            //     Symbol notnoded = getNotNodedFrom(s1, s2);

            //     if (noded.isOperatorTypeOf(SymbolOperator.plus))
            //         symbol.subSymbols = noded.stackSubsymbolsWith(notnoded);
            // }
            // return symbol;
        }
        static Symbol getNodedFrom(Symbol s1, Symbol s2)
        {
            if (s1.isNode())
                return s1;
            else
                return s2;
        }
        static Symbol getNotNodedFrom(Symbol s1, Symbol s2)
        {
            if (!s1.isNode())
                return s1;
            else
                return s2;
        }
        public bool isNumerical() { return this.type == SymbolType.numerical; }
        public bool isText() { return this.type == SymbolType.text; }
        public bool isNode() { return this.type == SymbolType.node; }
        public static bool isOneOfThemANode(Symbol s1, Symbol s2) { return s1.isNode() && !s2.isNode() || !s1.isNode() && s2.isNode(); }
        public static bool areBothANumerical(Symbol s1, Symbol s2) { return s1.isNumerical() && s2.isNumerical(); }
        public static bool areBothANode(Symbol s1, Symbol s2) { return s1.isNode() && s2.isNode(); }
        public List<Symbol> stackSubsymbolsWith(Symbol notNoded) // można wymyślić jakąś ładniejszą nazwę która by więcej mówiła o tym
        {
            List<Symbol> stackedSubSymbols = new List<Symbol>();
            foreach (var symbol in this.subSymbols)
                stackedSubSymbols.Add(symbol);
            stackedSubSymbols.Add(notNoded);
            return stackedSubSymbols;
        }
        bool isOperatorTypeOf(SymbolOperator opwerator) { return operation == opwerator; }
        public static Symbol operator *(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.times;

            if (areBothANumerical(s1, s2))
            {
                symbol.type = SymbolType.numerical;
                symbol.operation = SymbolOperator.none;
                symbol.numericValue = s1.numericValue * s2.numericValue;
            }
            else if (isOneOfThemANode(s1, s2))
            {
                Symbol noded = getNodedFrom(s1, s2);
                Symbol notNoded = getNotNodedFrom(s1, s2);
                symbol.type = SymbolType.node;

                if (noded.isOperatorTypeOf(SymbolOperator.times))
                    symbol.subSymbols = noded.stackSubsymbolsWith(notNoded);
                else
                    for (int i = 0; i < noded.subSymbols.Count; i++)
                        symbol.subSymbols.Add(noded.subSymbols[i] * notNoded);
            }
            else if (areBothANode(s1, s2))
                mergeNodeSymbolsToOne(s1, s2, ref symbol, SymbolOperator.times);
            else if (!areBothANumerical(s1, s2) && !isOneOfThemANode(s1, s2))
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            clearNode(ref symbol);
            SortNode(symbol);
            return symbol;
        }

        public static Symbol operator -(Symbol s1)
        {
            ChangePolarity(ref s1);
            if (s1.isNumerical())
                s1.numericValue = -s1.numericValue;
            return s1;
        }

        private static void ChangePolarity(ref Symbol s1)
        {
            if (s1.isPolarityPositive())
                s1.polarity = SymbolPolarity.negative;
            else if (s1.isPolarityNegative())
                s1.polarity = SymbolPolarity.positive;
        }

        bool isPolarityPositive() { return polarity == SymbolPolarity.positive; }
        bool isPolarityNegative() { return polarity == SymbolPolarity.negative; }

        public static void clearNode(ref Symbol symbol)
        {
            if (!symbol.isNode())
                return;

            numericalCalculationInsideNode(ref symbol);
            removeSingularValuesFromNode(ref symbol);
            NoneOrSingleBranchNodeFix(ref symbol);
        }
        public static void numericalCalculationInsideNode(ref Symbol symbol) // jak masz pomysł na lepszą nazwę to dawaj:D
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
        public static void removeSingularValuesFromNode(ref Symbol symbol)
        {
            Predicate<Symbol> symbolIsZero = s => s.isNumerical() && s.numericValue == 0;
            Predicate<Symbol> symbolIsOne = s => s.isNumerical() && s.numericValue == 1;
            Predicate<Symbol> symbolIsMinusOne = s => s.isNumerical() && s.numericValue == -1;
            switch (symbol.operation)
            {
                case SymbolOperator.plus:
                    symbol.subSymbols.RemoveAll(symbolIsZero);
                    break;
                case SymbolOperator.times:
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
                    break;
            }

        }

        private static Symbol RecoverEmptyNode(ref Symbol symbol)
        {
            switch (symbol.operation)
            {
                case SymbolOperator.plus:
                    symbol = new Symbol(0);
                    break;
                case SymbolOperator.times:
                    symbol.type = SymbolType.numerical;
                    symbol.operation = SymbolOperator.none;
                    if (symbol.isPolarityPositive())
                        symbol.numericValue = 1;
                    else if (symbol.isPolarityPositive())
                        symbol.numericValue = -1;
                    break;
            }

            return symbol;
        }

        public static void NoneOrSingleBranchNodeFix(ref Symbol symbol)
        {
            if (!symbol.isNode())
                return;
            if (symbol.subSymbols.Count == 1)
                symbol = symbol.subSymbols[0];
            else if (symbol.subSymbols.Count == 0)
                RecoverEmptyNode(ref symbol);
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

            bool isTheSameType = areBothHaveTheSameType(this, comparedSymbol);
            bool valueCondition = areBothHaveTheSameValue(this, comparedSymbol);
            bool nodeCondition = true;

            if (this.isNode() && isTheSameType)
                nodeCondition = areNodesEqual(this, comparedSymbol);

            return isTheSameType && valueCondition && nodeCondition;
        }

        private bool areBothHaveTheSameType(Symbol s1, Symbol s2) { return s1.type == s2.type; }

        private bool areBothHaveTheSameValue(Symbol s1, Symbol s2)
        {
            bool isNumericalValueTheSame = s1.numericValue == s2.numericValue;
            bool isTextValueTheSame = s1.textValue == s2.textValue;
            bool isPolarityTheSame = s1.polarity == s2.polarity;
            bool isOperationTheSame = s1.operation == s2.operation;

            return isNumericalValueTheSame && isTextValueTheSame && isPolarityTheSame && isOperationTheSame;
        }

        private bool areNodesEqual(Symbol s1, Symbol s2)
        {
            bool isNodesEqual = false;
            if (!s1.isOperatorTypeOf(s2.operation))
                return false;

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

            else if (this.isNode() && areBothHaveTheSameValue(this, other))
                return this.ToString().Length.CompareTo(other.ToString().Length);

            return 0;
        }
    }
}
