using System;
using System.Collections.Generic;
using System.Text;

public class Symbol
{
    List<Symbol> symbols;
    double numericValue;
    String textValue;

    SymbolOperator operation;
    SymbolType type;



    public Symbol()
    {
        symbols = new List<Symbol>();

    }

    public Symbol(double value) : base()
    {
        type = SymbolType.numerical;
        numericValue = value;
    }

    public Symbol(String value) : base()
    {
        type = SymbolType.text;
        textValue = value;
    }

    public static Symbol operator +(Symbol s1, Symbol s2)
    {
        Symbol symbol = new Symbol();
        symbol.operation = SymbolOperator.plus;

        if (s1.type == SymbolType.numerical&& s2.type == SymbolType.numerical)
        {
            symbol.type = SymbolType.numerical;
            symbol.numericValue = s1.numericValue + s2.numericValue;
        }

        return symbol;
    }

    public static Symbol operator *(Symbol s1, Symbol s2)
    {
        Symbol symbol = new Symbol();
        symbol.operation = SymbolOperator.plus;

        if (s1.type == SymbolType.numerical && s2.type == SymbolType.numerical)
        {
            symbol.type = SymbolType.numerical;
            symbol.numericValue = s1.numericValue * s2.numericValue;
        }

        return symbol;
    }
    public static Symbol operator -(Symbol s1){
        Symbol symbol = new Symbol();

        if (s1.type == SymbolType.numerical)
        {
            symbol.type = SymbolType.numerical;
            symbol.numericValue = -s1.numericValue;
        }

        return symbol;
    }

    public override String ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        switch (type)
        {
            case SymbolType.text:
                stringBuilder.Append(textValue);
                break;
            case SymbolType.numerical:
                stringBuilder.Append(string.Format("{0:N4}",numericValue));
                break;
        }
        return stringBuilder.ToString();
    }

    public static Symbol Sin(Symbol value){
        Symbol symbol = new Symbol();
        symbol.type = value.type;

        switch(value.type){
            case SymbolType.text:
                symbol.textValue = String.Format("Sin({0})",value.textValue);
                break;
            case SymbolType.numerical:
                symbol.numericValue = Math.Sin((Math.PI / 180) * value.numericValue);
                break;
        }

        return symbol;
    }
    
    public static Symbol Cos(Symbol value){
        Symbol symbol = new Symbol();
        symbol.type = value.type;

        switch(value.type){
            case SymbolType.text:
                symbol.textValue = String.Format("Cos({0})",value.textValue);
                break;
            case SymbolType.numerical:
                symbol.numericValue = Math.Cos((Math.PI / 180) * value.numericValue);
                break;
        }

        return symbol;
    }
}

enum SymbolOperator
{
    plus, minus, times, sin, cos
}

enum SymbolType
{
    text, numerical, node
}
