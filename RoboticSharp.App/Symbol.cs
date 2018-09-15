using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticSharp.App
{

    public class Symbol
    {
        List<Symbol> symbols;
        double numericValue;
        String textValue;

        SymbolOperator operation;
        SymbolType type;
        SymbolPolarity polarity;



        public Symbol()
        {
            symbols = new List<Symbol>();
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

            //przypadek numeryczny
            if (s1.type == SymbolType.numerical && s2.type == SymbolType.numerical)
            {
                symbol.type = SymbolType.numerical;
                symbol.numericValue = s1.numericValue + s2.numericValue;
            }
            //przypadek zagnieżdżenia jednego z elementów
            else if ((s1.type == SymbolType.node && s2.type != SymbolType.node) || (s1.type == SymbolType.node && s2.type != SymbolType.node))
            {
                Symbol noded;
                Symbol notNoded;
                symbol.type = SymbolType.node;

                //wybranie nodowanego i nienodowanego elementu
                if (s1.type == SymbolType.node)
                {
                    noded = s1;
                    notNoded = s2;
                }
                else
                {
                    notNoded = s1;
                    noded = s2;
                }

                //sprawdzenie czy operacja nie pokrywa się z operacją węzła
                if (noded.operation == SymbolOperator.plus)
                {
                    foreach (var element in noded.symbols)
                    {
                        symbol.symbols.Add(element);
                    }
                    symbol.symbols.Add(notNoded);
                }
                else
                {
                    for (int i = 0; i < noded.symbols.Count; i++)
                    {
                        symbol.symbols.Add(noded.symbols[i] + notNoded);
                    }
                }

            }
            else
            {
                symbol.type = SymbolType.node;
                symbol.symbols.Add(s1);
                symbol.symbols.Add(s2);
            }

            ClearNode(symbol);
            return symbol;
        }

        public static Symbol operator *(Symbol s1, Symbol s2)
        {
            Symbol symbol = new Symbol();
            symbol.operation = SymbolOperator.times;

            //przypadek numeryczny
            if (s1.type == SymbolType.numerical && s2.type == SymbolType.numerical)
            {
                symbol.type = SymbolType.numerical;
                symbol.numericValue = s1.numericValue * s2.numericValue;
            }
            //przypadek zagnieżdżenia jednego z elementów
            else if ((s1.type == SymbolType.node && s2.type != SymbolType.node) || (s1.type == SymbolType.node && s2.type != SymbolType.node))
            {
                Symbol noded;
                Symbol notNoded;
                symbol.type = SymbolType.node;

                //wybranie nodowanego i nienodowanego elementu
                if (s1.type == SymbolType.node)
                {
                    noded = s1;
                    notNoded = s2;
                }
                else
                {
                    notNoded = s1;
                    noded = s2;
                }

                //sprawdzenie czy operacja nie pokrywa się z operacją węzła
                if (noded.operation == SymbolOperator.times)
                {
                    foreach (var element in noded.symbols)
                    {
                        symbol.symbols.Add(element);
                    }
                    symbol.symbols.Add(notNoded);
                }
                else
                {
                    for (int i = 0; i < noded.symbols.Count; i++)
                    {
                        symbol.symbols.Add(noded.symbols[i] * notNoded);
                    }
                }

            }
            else
            {
                symbol.type = SymbolType.node;
                symbol.symbols.Add(s1);
                symbol.symbols.Add(s2);
            }

            ClearNode(symbol);
            return symbol;
        }

        public static Symbol operator -(Symbol s1)
        {
            Symbol symbol = s1;

            if (symbol.type == SymbolType.numerical)
            {
                symbol.numericValue = -symbol.numericValue;
                return symbol;
            }
            else
            {
                if (symbol.polarity == SymbolPolarity.positive)
                {
                    symbol.polarity = SymbolPolarity.negative;
                }
                else
                {
                    symbol.polarity = SymbolPolarity.positive;
                }
                return symbol;
            }

        }

        public static void ClearNode(Symbol symbol)
        {
            if (symbol.type != SymbolType.node)
                return;

            switch (symbol.operation)
            {
                case SymbolOperator.plus:
                    symbol.symbols.RemoveAll(s => s.type == SymbolType.numerical && s.numericValue == 0);
                    break;
                case SymbolOperator.times:
                    if (symbol.symbols.Exists(s => s.type == SymbolType.numerical && s.numericValue == 0))
                    {
                        symbol.symbols.Clear();
                        symbol.type = SymbolType.numerical;
                        symbol.operation = SymbolOperator.none;
                        symbol.polarity = SymbolPolarity.positive;
                        symbol.numericValue = 0;
                    }

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

                    int nodeSize = symbols.Count;
                    for (int i = 0; i < nodeSize; i++)
                    {
                        stringBuilder.Append(symbols[i].ToString());

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

}
