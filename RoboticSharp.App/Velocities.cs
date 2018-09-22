using System;
using System.Collections.Generic;
using System.Text;
using RoboticSharp.App.Matrices;

namespace RoboticSharp.App
{

    // Macierze rotacji używane w tej klasie muszą być transponowane przed użyciem, czyli zapis R[0] oznacza macierz R1_0, czyli transponowana R0_1 ............ R[4] == R4_3
    //czyli indeks tablicy/listy jest indeksem dolnym z wzorów ogólnych
    class Velocities
    {
        double angularSpeed;
        double linearSpeed;
        Matrix[] rotationMatrix;
        Vector[] jointsAngularSpeeds;
        Vector[] jointsLinearSpeeds;
        Vector[] transferVectors;
        Vector[] d; //wektory z pochodnymi d na osi Z
        Vector[] teta; //wektor z pocohdnymi tety w osi Z
        int joints;

        public Velocities(int joints)
        {
            this.joints = joints;
            jointsAngularSpeeds = new Vector[joints + 1];
            jointsLinearSpeeds = new Vector[joints + 1];
            transferVectors = new Vector[joints + 1];
            rotationMatrix = new Matrix[joints + 1];
            Symbol[] baseSpeed = { new Symbol(0), new Symbol(0), new Symbol(0) };
            jointsAngularSpeeds[0] = new Vector(baseSpeed);
            jointsLinearSpeeds[0] = new Vector(baseSpeed);
        }

        void angularVelocityRecursion()
        {
            for (int i = 0; i < joints; i++)
            {
                jointsAngularSpeeds[i + 1] = rotationMatrix[i] * jointsAngularSpeeds[i] + teta[i + 1];
                //omega[i+1] = R[i] * omega[i] + teta[i+1];
            }
        }

        void linearVelocityRecursion()
        {
            for (int i = 0; i < joints; i++)
            {
                jointsLinearSpeeds[i + 1] = rotationMatrix[i] * (jointsLinearSpeeds[i] + jointsAngularSpeeds[i] * transferVectors[i + 1]) + d[i + 1];
                //v[i+1] = R[1] * (v[i] + omega[i] * l[i+1]) + d[i+1]';
            }
        }
    }
}
