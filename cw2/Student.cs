using System;
using System.Collections.Generic;
using System.Text;

namespace cw2
{

    

    public class Student
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Kierunek { get; set; }
        public string TrybStudiow { get; set; }
        public string NumerIndeksu { get; set; }
        public string DataUrodzenia { get; set; }
        public string Email { get; set; }
        public string ImieMatki { get; set; }
        public string ImieOjca { get; set; }


        override public string ToString() {

            return Imie + " " + Nazwisko + " " + NumerIndeksu;
        }
    }
}
