using System;
using System.Text;

public class MacierzT
{
    Symbol[,] data;
    public MacierzT()
    {
        data = new Symbol[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                data[i,j] = new Symbol(0);
            }
        }
    }
    public MacierzT(Symbol theta_i, Symbol alpha_i_minus_1, Symbol a_i_minus_1, Symbol d_i)
    {
        data = new Symbol[4, 4];

        //wiersz pierwszy
        data[0, 0] = Symbol.Cos(theta_i);
        data[0, 1] = -Symbol.Sin(theta_i);
        data[0, 2] = new Symbol(0);
        data[0, 3] = a_i_minus_1;

        //drugi wiersz
        data[1, 0] = Symbol.Sin(theta_i) * Symbol.Cos(theta_i);
        data[1, 1] = Symbol.Cos(theta_i) * Symbol.Cos(theta_i);
        data[1, 2] = Symbol.Sin(theta_i);
        data[1, 3] = d_i * Symbol.Sin(theta_i);

        //trzeci wiersz
        data[2, 0] = Symbol.Sin(theta_i) * Symbol.Sin(theta_i);
        data[2, 1] = Symbol.Cos(theta_i) * Symbol.Sin(theta_i);
        data[2, 2] = Symbol.Cos(theta_i);
        data[2, 3] = d_i * Symbol.Cos(theta_i);

        //czwarty wiersz
        data[3, 0] = new Symbol(0);;
        data[3, 1] = new Symbol(0);;
        data[3, 2] = new Symbol(0);;
        data[3, 3] = new Symbol(0);;
    }

    public static MacierzT operator *(MacierzT m1, MacierzT m2)
    {
        MacierzT resultMatrix = new MacierzT();
        for (int wiersz = 0; wiersz < 4; wiersz++)
        {
            for (int kolumna = 0; kolumna < 4; kolumna++)
            {
                for (int i=0; i < 4; i++)
                {
                    resultMatrix.data[wiersz, kolumna] += m1.data[wiersz, i]*m2.data[i, kolumna];
                }
            }
        }
        return resultMatrix;

    }

    public override String ToString()
    {
        StringBuilder macierzBuilder = new StringBuilder();

        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[0, 0].ToString(), data[0, 1].ToString(), data[0, 2].ToString(), data[0, 3].ToString()));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[1, 0].ToString(), data[1, 1].ToString(), data[1, 2].ToString(), data[1, 3].ToString()));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[2, 0].ToString(), data[2, 1].ToString(), data[2, 2].ToString(), data[2, 3].ToString()));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[3, 0].ToString(), data[3, 1].ToString(), data[3, 2].ToString(), data[3, 3].ToString()));

        return macierzBuilder.ToString();
    }

}