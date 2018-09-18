using System;
using System.Collections.Generic;
using System.Text;
using RoboticSharp.App.Matrices

namespace RoboticSharp.App
{
    class Velocities
    {
        double angularSpeed;
        double linearSpeed;
        Matrix[] rotationMatrices;
        Vector[] jointsAngularSpeeds;
        Vector[] jointsLinearSpeeds;

        public Velocities(int joints)
        {
            jointsAngularSpeeds = new Vector[joints + 1];
            jointsLinearSpeeds = new Vector[joints + 1];
            rotationMatrices = new Matrix[joints + 1];
            double[] baseSpeeds = { 0, 0, 0 };
            jointsAngularSpeeds[0] = new Vector(baseSpeeds);
            jointsLinearSpeeds[0] = new Vector(baseSpeeds);
        }
        // komentarz mały , w charakterze notatki: 
        /* Musimy jakoś wymyślić oznaczenia i je opisać, żebyśmy nie mieli potem momentu zwątpień. Chodzi mi o indeksy górne i dolne które
            tutaj ciężko będzie odwzorować. A jest to ważne bo trzeba wiedzieć czy macierz R jest transponowana czy nie itd. Kolejna sprawa, 
            że trzeba zaimplementować liczenie pochodnych, to może być cieżkie, myślę, że pójdzie na to trochę czasu. Bo trzeba będzie sprawdzać
            czy jest stała czy nie, i jakoś oznaczyć (może jakaś właściwość w klasie symbol w której będziemy zaznaczali stopień pochodnej. 
            Trzeba tą pochodną naprawde grubo przemyśleć zanim zaczniemy to pisać.
             */
        void angularVelocityRecursion()
        {
            //omega[i+1] = R[(i+1)_i] *omega[i] + pochodna(teta[i+1])*z[i+1];
        }
        void linearVelocityRecursion()
        {
            //v[i+1] = R[(i+1)_i] * (v[i] - omega[i] * l[i/i+1] ) + pochodna(d[i+1]*z[i+1] ;
        }
    }
}
