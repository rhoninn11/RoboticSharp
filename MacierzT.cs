using System;
using System.Text;

public class MacierzT
{
    double[,] data;
    public MacierzT()
    {
        data = new double[4, 4];
    }
    public MacierzT(double theta_i, double alpha_i_minus_1, double a_i_minus_1, double d_i)
    {
        data = new double[4, 4];

        double theta_radians = ConvertToRadians(theta_i);
        double alpha_radians = ConvertToRadians(alpha_i_minus_1);
        //wiersz pierwszy
        data[0, 0] = Math.Cos(theta_radians);
        data[0, 1] = -Math.Sin(theta_radians);
        data[0, 2] = 0;
        data[0, 3] = a_i_minus_1;

        //drugi wiersz
        data[1, 0] = Math.Sin(theta_radians) * Math.Cos(alpha_radians);
        data[1, 1] = Math.Cos(theta_radians) * Math.Cos(alpha_radians);
        data[1, 2] = Math.Sin(alpha_radians);
        data[1, 3] = d_i * Math.Sin(alpha_radians);

        //trzeci wiersz
        data[2, 0] = Math.Sin(theta_radians) * Math.Sin(alpha_radians);
        data[2, 1] = Math.Cos(theta_radians) * Math.Sin(alpha_radians);
        data[2, 2] = Math.Cos(alpha_radians);
        data[2, 3] = d_i * Math.Cos(alpha_radians);

        //czwarty wiersz
        data[3, 0] = 0;
        data[3, 1] = 0;
        data[3, 2] = 0;
        data[3, 3] = 1;
    }

    public MacierzT TranspozycjaMacierzT()
    {
        MacierzT macierzOdwrotna = new MacierzT();

        for (int wiersz = 0; wiersz < 4; wiersz++)
        {
            for (int kolumna = 0; kolumna < 4; kolumna++)
            {
                macierzOdwrotna.data[kolumna, wiersz] = this.data[wiersz, kolumna];
            }
        }
        return macierzOdwrotna;
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

    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    public override String ToString()
    {
        StringBuilder macierzBuilder = new StringBuilder();

        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[0, 0], data[0, 1], data[0, 2], data[0, 3]));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[1, 0], data[1, 1], data[1, 2], data[1, 3]));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[2, 0], data[2, 1], data[2, 2], data[2, 3]));
        macierzBuilder.Append(String.Format("|{0:N4} {1:N4} {2:N4} {3:N4}|\n", data[3, 0], data[3, 1], data[3, 2], data[3, 3]));

        return macierzBuilder.ToString();
    }

}