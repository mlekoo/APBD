using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace cw2
{
    class OwnComparer : IEqualityComparer<Student>

    {
        public new bool Equals(Student x, Student y)
        {
            return StringComparer
                .InvariantCulture
                .Equals($"{x.Imie} {x.Nazwisko} {x.NumerIndeksu}", $"{y.Imie} {y.Nazwisko} {y.NumerIndeksu}");
                
            throw new NotImplementedException();
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer
                .OrdinalIgnoreCase
                .GetHashCode($"{obj.Imie} {obj.Nazwisko} {obj.NumerIndeksu}");

            throw new NotImplementedException();
        }
    }
}
