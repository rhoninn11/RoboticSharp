using System;
using System.Collections.Generic;
using System.Text;
//500 lini max
//40 max w funkcji
namespace RoboticSharp.App
{
    public class Symbol
    {
        List<Symbol> subSymbols;
        double numericValue;
        String textValue;

        SymbolOperator operation;
        SymbolType type;
        SymbolPolarity polarity;

        enum SymbolOperator
        {
            none, plus, minus, times, sin, cos
        }

        enum SymbolType
        {
            text, numerical, node
        }

        enum SymbolPolarity
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

        public static Symbol operator +(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.plus;
            
            if (bothAreNumerical(s1, s2))
            {
                symbol.type = SymbolType.numerical;
                symbol.numericValue = s1.numericValue + s2.numericValue;
            }
            else if (oneOfThemIsNode(s1, s2))
            {
                symbol.type = SymbolType.node;
                Symbol noded = getNodedFrom(s1, s2);
                Symbol notNoded = getNotNodedFrom(s1, s2);
                
                if (noded.isOperatorTypeOf(SymbolOperator.plus))
                    symbol.subSymbols = noded.stackSubsymbolsWith(noded);
                else
                    for (int i = 0; i < noded.subSymbols.Count; i++)
                        symbol.subSymbols.Add(noded.subSymbols[i] + notNoded);
            }
            else
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            ClearNode(symbol);
            return symbol;
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
        public bool isNumerical()
        {
            return this.type == SymbolType.numerical;
        }

        public bool isNode()
        {
            return this.type == SymbolType.node;
        }

        public static bool oneOfThemIsNode(Symbol s1, Symbol s2)
        {
            return s1.isNode() && !s2.isNode() || !s1.isNode() && s2.isNode();
        }

        public static bool bothAreNumerical(Symbol s1, Symbol s2)
        {
            return s1.isNumerical() && s2.isNumerical();
        }
        public List<Symbol> stackSubsymbolsWith(Symbol notNoded) // można wymyślić jakąś ładniejszą nazwę która by więcej mówiła o tym
        {
            subSymbols.Add(notNoded);
            return subSymbols;
        }
        bool isOperatorTypeOf(SymbolOperator operat)
        {
            return operation == operat;
        }

        public static Symbol operator *(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.times;
            
            if (bothAreNumerical(s1,s2))
            {
                symbol.type = SymbolType.numerical;
                symbol.numericValue = s1.numericValue * s2.numericValue;
            }

            else if (oneOfThemIsNode(s1,s2))
            {
                Symbol noded = getNodedFrom(s1, s2);
                Symbol notNoded = getNotNodedFrom(s1, s2);
                symbol.type = SymbolType.node;                
                
                if (noded.isOperatorTypeOf(SymbolOperator.times))
                    symbol.subSymbols = noded.stackSubsymbolsWith(noded);
                else //smieszna konstrukcja
                    for (int i = 0; i < noded.subSymbols.Count; i++)
                        symbol.subSymbols.Add(noded.subSymbols[i] * notNoded);
            }
            else
            {
                symbol.type = SymbolType.node;
                symbol.subSymbols.Add(s1);
                symbol.subSymbols.Add(s2);
            }
            ClearNode(symbol);
            return symbol;
        }

        public static Symbol operator -(Symbol s1)
        {
            if (s1.isNumerical())
            {
                s1.numericValue = -s1.numericValue;
                return s1;
            }
            else
            {
                if (s1.isPolarityPositive())
                    s1.polarity = SymbolPolarity.negative;
                else
                    s1.polarity = SymbolPolarity.positive;
                return s1;
            }
        }
        bool isPolarityPositive()
        {
            return polarity == SymbolPolarity.positive;
        }
        bool isPolarityNegative()
        {
            return polarity == SymbolPolarity.negative;
        }

        public static void ClearNode(Symbol symbol)
        {
            if (!symbol.isNode())
                return;

            switch (symbol.operation)
            {
                case SymbolOperator.plus:
                    symbol.subSymbols.RemoveAll(s => s.isNumerical() && s.numericValue == 0);
                    break;
                case SymbolOperator.times:
                    if (symbol.subSymbols.Exists(s => s.isNumerical() && s.numericValue == 0))
                    {
                        symbol.subSymbols.Clear();
                        symbol.type = SymbolType.numerical;
                        symbol.operation = SymbolOperator.none;
                        symbol.polarity = SymbolPolarity.positive;
                        symbol.numericValue = 0;
                    }
                    symbol.subSymbols.RemoveAll(s => s.isNumerical() && s.numericValue == 1);
                    break;
                  
            }
        }

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

        public static Symbol Sin(Symbol value)
        {
            Symbol symbol = new Symbol();
            symbol.type = value.type;

            switch (value.type)
            {
                case SymbolType.text:
                    symbol.textValue = String.Format("S{0}", value.textValue);
                    break;
                case SymbolType.numerical:
                    symbol.numericValue = RoundForTrygonometryBorderValues(Math.Sin((Math.PI / 180) * value.numericValue));
                    break;
            }
            return symbol;
        }

        public static Symbol Cos(Symbol value)
        {
            Symbol symbol = new Symbol();
            symbol.type = value.type;

            switch (value.type)
            {
                case SymbolType.text:
                    symbol.textValue = String.Format("C{0}", value.textValue);
                    break;
                case SymbolType.numerical:
                    symbol.numericValue = RoundForTrygonometryBorderValues(Math.Cos((Math.PI / 180) * value.numericValue));
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
    }
}
